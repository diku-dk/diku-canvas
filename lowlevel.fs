module Lowlevel

open System.Runtime.InteropServices
open System
open System.Collections.Generic

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.Drawing
// colors
type Color = SixLabors.ImageSharp.Color

type InternalEvent = internal InternalEvent of SDL.SDL_Event

let fromRgba (red:int) (green:int) (blue:int) (a:int) : Color =
    Color.FromRgba(byte red, byte green, byte blue, byte a)
let fromRgb (red:int) (green:int) (blue:int) : Color =
    Color.FromRgb(byte red, byte green, byte blue)
type image = SixLabors.ImageSharp.Image<Rgba32>
type drawing_context = SixLabors.ImageSharp.Processing.IImageProcessingContext
type drawing_fun = drawing_context -> drawing_context

type Font = SixLabors.Fonts.Font
type FontFamily = SixLabors.Fonts.FontFamily
let systemFontNames: string list = [for i in  SixLabors.Fonts.SystemFonts.Families do yield i.Name]
let getFamily name = SixLabors.Fonts.SystemFonts.Get(name)
let makeFont (fam:FontFamily) (size:float) = fam.CreateFont(float32 size, SixLabors.Fonts.FontStyle.Regular)
let measureText (f:Font) (txt:string) = 
    let rect = SixLabors.Fonts.TextMeasurer.MeasureSize(txt, SixLabors.Fonts.TextOptions(f))
    (float rect.Width,float rect.Height)

type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush

let solidBrush (color:Color) : Tool =
    Brush (Brushes.Solid(color))
let solidPen (color:Color) (width:float) : Tool =
    Pen (Pens.Solid(color, float32 width))

type point = int * int
type pointF = float * float
type Matrix3x2 = System.Numerics.Matrix3x2
type TextOptions = SixLabors.Fonts.TextOptions

let toPointF (x:float, y:float) = PointF(x = float32 x, y = float32 y)
let toVector2 (x:float, y:float) = System.Numerics.Vector2(float32 x, float32 y)

type PathDefinition =
    | EmptyDef
    | ArcTo of float * float * float * bool * bool * pointF
    | CubicBezierTo of pointF * pointF * pointF
    | LineTo of pointF
    | MoveTo of pointF
    | QuadraticBezierTo of pointF * pointF
    | SetTransform of Matrix3x2
    | LocalTransform of Matrix3x2 * PathDefinition
    | StartFigure
    | CloseFigure
    | CloseAllFigures
    | Combine of PathDefinition * PathDefinition

let (<++>) p1 p2 =
    match p1, p2 with
        | EmptyDef, _ -> p2
        | _, EmptyDef -> p1
        | defs -> Combine defs

let construct pd : IPath =
    let initial = Matrix3x2.Identity
    let builder = PathBuilder initial
    let rec loop (builder:PathBuilder) curT = function // tail-recursive traversal
        | [] -> builder
        | cur :: worklist ->
            match cur with
                | EmptyDef ->
                    loop builder curT worklist
                | ArcTo(radiusX, radiusY, rotation, largeArc, sweep, point) ->
                    let builder = builder.ArcTo(float32 radiusX, float32 radiusY,
                                                float32 rotation, largeArc, sweep,
                                                toPointF point)
                    loop builder curT worklist
                | CubicBezierTo(secondControlPoint, thirdControlPoint, point) ->
                    let builder = builder.CubicBezierTo(toVector2 secondControlPoint,
                                                        toVector2 thirdControlPoint,
                                                        toVector2 point)
                    loop builder curT worklist
                | LineTo p ->
                    let builder = builder.LineTo(toPointF p)
                    loop builder curT worklist
                | MoveTo p ->
                    let builder = builder.MoveTo(toPointF p)
                    loop builder curT worklist
                | QuadraticBezierTo(secondControlPoint, point) ->
                    let builder = builder.QuadraticBezierTo(toVector2 secondControlPoint,
                                                            toVector2 point)
                    loop builder curT worklist
                | SetTransform mat ->
                    let builder = builder.SetTransform mat
                    loop builder mat worklist
                | LocalTransform (mat, pd) ->
                    let builder = builder.SetTransform mat
                    loop builder mat (pd :: SetTransform curT :: worklist)
                | StartFigure ->
                    let builder = builder.StartFigure()
                    loop builder curT worklist
                | CloseFigure ->
                    let builder = builder.CloseFigure()
                    loop builder curT worklist
                | CloseAllFigures ->
                    let builder = builder.CloseAllFigures()
                    loop builder curT worklist
                | Combine (p1, p2) ->
                    loop builder curT (p1 :: p2 :: worklist)
    (loop builder initial pd).Build()


type PrimPath =
    | Arc of pointF * float * float * float * float * float
    | CubicBezier of pointF * pointF * pointF * pointF
    | Line of pointF * pointF
    | Lines of pointF list

and PathTree =
    | Empty
    | Prim of Tool * PrimPath
    | PathAdd of PathTree * PathTree
    | Transform of Matrix3x2 * PathTree
    | Text of Tool * string * TextOptions  // FIXME: Maybe we want a `Raw of IPathCollection` constructor instead of the Text constructors
    | TextAlong of Tool * string * TextOptions * PrimPath

let (<+>) p1 p2 =
    match p1, p2 with
        | Empty, _ -> p2
        | _, Empty -> p1
        | _ -> PathAdd (p1, p2)

let pathAdd = (<+>)

let emptyPath = Empty

let pathFromList ps = List.fold pathAdd emptyPath ps

let transform (mat:Matrix3x2) = function
    | Transform (cur, p) ->
        if mat.IsIdentity then Transform(cur, p)
        else let res = mat * cur
             if res.IsIdentity then p
             else Transform(res, p)
    | p ->
        Transform(mat, p)

let rotateDegreeAround (degrees:float) point p =
    let mat = Matrix3x2Extensions.CreateRotationDegrees(float32 degrees, toPointF point)
    transform mat p

let rotateRadiansAround (radians:float) point p =
    let mat = Matrix3x2Extensions.CreateRotation(float32 radians, toPointF point)
    transform mat p

let toILineSegment : PrimPath -> ILineSegment = function
    | Arc(center, rX, rY, rotation, start, sweep) ->
        ArcLineSegment(toPointF center, SizeF(float32 rX, float32 rY), float32 rotation, float32 start, float32 sweep)
    | CubicBezier(start, c1, c2, endPoint) ->
        CubicBezierLineSegment(toPointF start, toPointF c1, toPointF c2, toPointF endPoint, [||])
    | Line(start, endP) ->
        LinearLineSegment(toPointF start, toPointF endP)
    | Lines points ->
        LinearLineSegment(points |> Seq.map toPointF |> Seq.toArray)

let toPath (ilineseg:ILineSegment) : IPath =
    Path [| ilineseg |]

let flatten (p : PathTree) : (Tool*IPathCollection) list =  //FIXME: should maybe return a seq<IPathCollection>
    let rec traverse (acc : (Tool*IPathCollection) list) = function // tail-recursive traversal
        | [] -> List.rev acc
        | cur :: worklist ->
            match cur with
                | Empty ->
                    traverse acc worklist
                | Prim (pen, p) ->
                    let path = PathCollection [| p |> toILineSegment |> toPath |] : IPathCollection
                    let acc =  (pen,path) :: acc
                    traverse acc worklist
                | PathAdd (p1, p2) ->
                    traverse acc (p1 :: p2 :: worklist)
                | Transform (mat, p) ->
                    let transformed = traverse [] [p] |> List.map (fun (pen,p) -> (pen,p.Transform(mat)))
                    let acc = transformed @ acc
                    traverse acc worklist
                | Text (pen, text, options) ->
                    let glyphs = TextBuilder.GenerateGlyphs(text, options)
                    let acc =  (pen,glyphs) :: acc
                    traverse acc worklist
                | TextAlong(pen, text, options, p) ->
                    let path = p |> toILineSegment |> toPath
                    let glyphs = TextBuilder.GenerateGlyphs(text, path, options)
                    let acc =  (pen, glyphs) :: acc
                    traverse acc worklist
    traverse [] [p]

let createText (text, options) =
    TextBuilder.GenerateGlyphs(text, options)

let transformIPathCollection (mat: Matrix3x2, p: IPathCollection) =
    p.Transform(mat)

let drawCollection ((tool,col):Tool * IPathCollection) (ctx:drawing_context) : drawing_context =
    match tool with
        | Pen pen ->
            ctx.Draw(pen, col)
        | Brush brush ->
            ctx.Fill(DrawingOptions(), brush, col)

let drawPathTree (paths:PathTree) (ctx:drawing_context) : drawing_context =
    flatten paths |> List.fold (fun ctx penNCol -> drawCollection penNCol ctx) ctx

let drawToFile width height (filePath:string) (draw:drawing_fun) =
    let img = new Image<Rgba32>(width, height)
    img.Mutate(draw >> ignore)
    img.Save(filePath)

let drawToAnimatedGif width heigth (frameDelay:int) (repeatCount:int) (filePath:string) (drawLst:drawing_fun list) =
    match drawLst with
        draw::rst ->
            let gif = new Image<Rgba32>(width, heigth)
            gif.Mutate(draw >> ignore)
            let gifMetaData = gif.Metadata.GetGifMetadata()
            gifMetaData.RepeatCount <- uint16 repeatCount
            let metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata()
            metadata.FrameDelay <- frameDelay
            List.iter (fun (draw:drawing_fun) -> 
                let frame = new Image<Rgba32>(width, heigth)
                frame.Mutate(draw >> ignore)
                let metadata = frame.Frames.RootFrame.Metadata.GetGifMetadata()
                metadata.FrameDelay <- frameDelay
                gif.Frames.AddFrame(frame.Frames.RootFrame) |> ignore
            ) rst
            gif.SaveAsGif(filePath)
        | _ -> ()

type KeyAction =
    | KeyPress
    | KeyRelease

type KeyboardKey =
    | Unknown // Matches the case of an invalid, non-existent, or unsupported keyboard key.
    | Space // The spacebar key
    | Apostrophe // The apostrophe key '''
    | Comma // The comma key ','
    | Minus // The minus key '-'
    | Plus // The plus key '+'
    | Period // The period/dot key '.'
    | Slash // The slash key '/'
    | Num0 // The 0 key.
    | Num1 // The 1 key.
    | Num2 // The 2 key.
    | Num3 // The 3 key.
    | Num4 // The 4 key.
    | Num5 // The 5 key.
    | Num6 // The 6 key.
    | Num7 // The 7 key.
    | Num8 // The 8 key.
    | Num9 // The 9 key.
    | Semicolon // The semicolon key.
    | Equal // The equal key.
    | A // The A key.
    | B // The B key.
    | C // The C key.
    | D // The D key.
    | E // The E key.
    | F // The F key.
    | G // The G key.
    | H // The H key.
    | I // The I key.
    | J // The J key.
    | K // The K key.
    | L // The L key.
    | M // The M key.
    | N // The N key.
    | O // The O key.
    | P // The P key.
    | Q // The Q key.
    | R // The R key.
    | S // The S key.
    | T // The T key.
    | U // The U key.
    | V // The V key.
    | W // The W key.
    | X // The X key.
    | Y // The Y key.
    | Z // The Z key.
    | LeftBracket // The left bracket(opening bracket) key '['
    | Backslash // The backslash '\'
    | RightBracket // The right bracket(closing bracket) key ']'
    | GraveAccent // The grave accent key '`'
    | AcuteAccent // The acute accent key ("inverted" grave accent) '´'
    | Escape // The escape key.
    | Enter // The enter key.
    | Tab // The tab key.
    | Backspace // The backspace key.
    | Insert // The insert key.
    | Delete // The delete key.
    | Right // The right arrow key.
    | Left // The left arrow key.
    | Down // The down arrow key.
    | Up // The up arrow key.
    | PageUp // The page up key.
    | PageDown // The page down key.
    | Home // The home key.
    | End // The end key.
    | CapsLock // The caps lock key.
    | ScrollLock // The scroll lock key.
    | NumLock // The num lock key.
    | PrintScreen // The print screen key.
    | Pause // The pause key.
    | F1 // The F1 key.
    | F2 // The F2 key.
    | F3 // The F3 key.
    | F4 // The F4 key.
    | F5 // The F5 key.
    | F6 // The F6 key.
    | F7 // The F7 key.
    | F8 // The F8 key.
    | F9 // The F9 key.
    | F10 // The F10 key.
    | F11 // The F11 key.
    | F12 // The F12 key.
    | KeyPad0 // The 0 key on the key pad.
    | KeyPad1 // The 1 key on the key pad.
    | KeyPad2 // The 2 key on the key pad.
    | KeyPad3 // The 3 key on the key pad.
    | KeyPad4 // The 4 key on the key pad.
    | KeyPad5 // The 5 key on the key pad.
    | KeyPad6 // The 6 key on the key pad.
    | KeyPad7 // The 7 key on the key pad.
    | KeyPad8 // The 8 key on the key pad.
    | KeyPad9 // The 9 key on the key pad.
    | KeyPadDecimal // The decimal key on the key pad.
    | KeyPadDivide // The divide key on the key pad.
    | KeyPadMultiply // The multiply key on the key pad.
    | KeyPadSubtract // The subtract key on the key pad.
    | KeyPadAdd // The add key on the key pad.
    | KeyPadEnter // The enter key on the key pad.
    | KeyPadEqual // The equal key on the key pad.
    | LeftShift // The left shift key.
    | LeftControl // The left control key.
    | LeftAlt // The left alt key.
    | LeftSuper // The left super key.
    | RightShift // The right shift key.
    | RightControl // The right control key.
    | RightAlt // The right alt key.
    | RightSuper // The right super key.
    | Menu // The menu key.
    | Diaresis // The Diaresis key '¨'
    | LessThan // The less than sign '<'
    | GreaterThan // The greater than sign '>'
    | FractionOneHalf // The "vulgar fraction one half" key '½'
    | DanishAA // The Danish AA key 'Å'
    | DanishAE // The Danish AE key 'Æ'
    | DanishOE // The Danish OE key 'Ø'

let private toKeyboardKey key =
    match SDL.stringFromKeyboard key with
    | "Space" -> Space
    | "'" -> Apostrophe
    | "," -> Comma
    | "-" -> Minus
    | "+" -> Plus
    | "." -> Period
    | "/" -> Slash
    | "0" -> Num0
    | "1" -> Num1
    | "2" -> Num2
    | "3" -> Num3
    | "4" -> Num4
    | "5" -> Num5
    | "6" -> Num6
    | "7" -> Num7
    | "8" -> Num8
    | "9" -> Num9
    | ";" -> Semicolon
    | "=" -> Equal
    | "A" -> A
    | "B" -> B
    | "C" -> C
    | "D" -> D
    | "E" -> E
    | "F" -> F
    | "G" -> G
    | "H" -> H
    | "I" -> I
    | "J" -> J
    | "K" -> K
    | "L" -> L
    | "M" -> M
    | "N" -> N
    | "O" -> O
    | "P" -> P
    | "Q" -> Q
    | "R" -> R
    | "S" -> S
    | "T" -> T
    | "U" -> U
    | "V" -> V
    | "W" -> W
    | "X" -> X
    | "Y" -> Y
    | "Z" -> Z
    | "[" -> LeftBracket
    | "\\" -> Backslash
    | "]" -> RightBracket
    | "`" -> GraveAccent
    | "´" -> AcuteAccent
    | "Escape" -> Escape
    | "Return" -> Enter // This should probably be Return.
    | "Tab" -> Tab
    | "Backspace" -> Backspace
    | "Insert" -> Insert
    | "Delete" -> Delete
    | "Right" -> Right
    | "Left" -> Left
    | "Down" -> Down
    | "Up" -> Up
    | "PageUp" -> PageUp
    | "PageDown" -> PageDown
    | "Home" -> Home
    | "End" -> End
    | "CapsLock" -> CapsLock
    | "ScrollLock" -> ScrollLock
    | "Numlock" -> NumLock // Has to be lowercase.
    | "PrintScreen" -> PrintScreen
    | "Pause" -> Pause
    | "F1" -> F1
    | "F2" -> F2
    | "F3" -> F3
    | "F4" -> F4
    | "F5" -> F5
    | "F6" -> F6
    | "F7" -> F7
    | "F8" -> F8
    | "F9" -> F9
    | "F10" -> F10
    | "F11" -> F11
    | "F12" -> F12
    | "Keypad 0" -> KeyPad0
    | "Keypad 1" -> KeyPad1
    | "Keypad 2" -> KeyPad2
    | "Keypad 3" -> KeyPad3
    | "Keypad 4" -> KeyPad4
    | "Keypad 5" -> KeyPad5
    | "Keypad 6" -> KeyPad6
    | "Keypad 7" -> KeyPad7
    | "Keypad 8" -> KeyPad8
    | "Keypad 9" -> KeyPad9
    | "Keypad ." -> KeyPadDecimal
    | "Keypad /" -> KeyPadDivide
    | "Keypad *" -> KeyPadMultiply
    | "Keypad -" -> KeyPadSubtract
    | "Keypad +" -> KeyPadAdd
    | "Keypad Enter" -> KeyPadEnter
    | "Keypad =" -> KeyPadEqual
    | "Left Shift" -> LeftShift
    | "Left Ctrl" -> LeftControl
    | "Left Alt" -> LeftAlt
    | "Left GUI" -> LeftSuper
    | "Right Shift" -> RightShift
    | "Right Ctrl" -> RightControl
    | "Right Alt" -> RightAlt
    | "Right GUI" -> RightSuper
    | "Menu" -> Menu // Not sure if this does anything.
    | "¨" -> Diaresis
    | "<" -> LessThan
    | ">" -> GreaterThan
    | "½" -> FractionOneHalf
    | "å" -> DanishAA
    | "æ" -> DanishAE
    | "ø" -> DanishOE
    | _ -> Unknown

let TIMER_EVENT =
    match SDL.SDL_RegisterEvents 1 with
    | UInt32.MaxValue -> failwith "Error: Could not allocate a user-defined Timer Event."
    | x -> x

let timerTickEvent () =
    let mutable ev = SDL.SDL_Event()
    ev.``type`` <- TIMER_EVENT
    SDL.SDL_PushEvent(&ev) |> ignore

type Window(t:string, w:int, h:int) =
    let mutable disposed = false
    static let mutable eventQueues = Map.empty
    let viewWidth, viewHeight = w, h
    let mutable window, renderer, texture, bufferPtr = IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero
    let mutable frameBuffer = Array.create 0 (byte 0)
    let mutable event = SDL.SDL_Event()
    let mutable img = None
    let mutable windowId = 0u
    let windowFlags =
        SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |||
        SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS
    
    do
        let is_initial = 0 = Map.count eventQueues
        if is_initial then
            SDL.SDL_SetMainReady()
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore
            //SDL.SDL_SetHint(SDL.SDL_HINT_QUIT_ON_LAST_WINDOW_CLOSE, "0") |> ignore

        window <- SDL.SDL_CreateWindow(t, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                                viewWidth, viewHeight, windowFlags)
        renderer <- SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |||
                                                SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC)

        texture <- SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_RGBA32, SDL.SDL_TEXTUREACCESS_STREAMING, viewWidth, viewHeight)

        frameBuffer <- Array.create (viewWidth * viewHeight * 4) (byte 0)
        bufferPtr <- IntPtr ((Marshal.UnsafeAddrOfPinnedArrayElement (frameBuffer, 0)).ToPointer ())

        img <- new Image<Rgba32>(w, h, Color.Black) |> Some

        if is_initial then
            SDL.SDL_StartTextInput()
        
        windowId <- SDL.SDL_GetWindowID(window)
        eventQueues <- eventQueues.Add (windowId, Queue())
    
    member this.cleanup () = 
        if not disposed then
            disposed <- true
            this.hideWindow ()
            SDL.SDL_DestroyTexture texture
            //printfn "Texture destroyed"
            SDL.SDL_DestroyRenderer renderer
            //printfn "Render destroyed"
            SDL.SDL_DestroyWindow window
            //printfn "Window destroyed"
            eventQueues <- eventQueues.Remove windowId

            if 0 = Map.count eventQueues then
                SDL.SDL_QuitSubSystem(SDL.SDL_INIT_VIDEO) |> ignore

            window <- IntPtr.Zero
            renderer <- IntPtr.Zero
            texture <- IntPtr.Zero
            bufferPtr <- IntPtr.Zero
            frameBuffer <- Array.create 0 (byte 0)
            event <- SDL.SDL_Event()
            img <- None
            windowId <- 0u
    
    interface IDisposable with
        member self.Dispose() =
            self.cleanup()
            GC.SuppressFinalize(self)
    
    override this.Finalize () =
        this.cleanup()

    member this.render (draw : 's -> drawing_fun, state: 's) =
        Option.map(fun (img': Image<Rgba32>) ->
            img'.Mutate(fun ctx' ->
                    (draw state ctx')
                        .Crop(min viewWidth img'.Width, min viewHeight img'.Height)
                        .Resize(ResizeOptions(Position = AnchorPositionMode.TopLeft,
                                            Size = Size(viewWidth, viewHeight),
                                            Mode = ResizeMode.BoxPad))
                    |> ignore)
            img'.CopyPixelDataTo(frameBuffer)
            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, viewWidth * 4) |> ignore
            SDL.SDL_RenderClear(renderer) |> ignore
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero) |> ignore
            SDL.SDL_RenderPresent(renderer) |> ignore
            img'.Mutate(fun ctx -> ctx.Clear(Color.Black) |> ignore)
        ) img |> ignore
        ()

    member private this.enqueueEvent (event: SDL.SDL_Event)  =
        if event.window.windowID = 0u
        then
            Map.iter (
                fun _ (q: Queue<SDL.SDL_Event>) -> q.Enqueue event
            ) eventQueues
        else
            match Map.tryFind event.window.windowID eventQueues with
            | Some queue -> queue.Enqueue event
            | None -> ()
    
    member private this.assertWindowExists () =
        if Map.containsKey windowId eventQueues |> not then
            failwith "Error: You can not get events from a disposed Window."

    member this.waitEvent f =
        this.assertWindowExists ()

        if eventQueues[windowId].Count = 0
        then
            if 0 = SDL.SDL_WaitEvent(&event) then
                failwith "Error: No event arrived."
            
            this.enqueueEvent event
            this.waitEvent f
        else
            eventQueues[windowId].Dequeue ()
            |> InternalEvent
            |> f
    
    member this.pollEvents f =
        this.assertWindowExists ()

        while 1 = SDL.SDL_PollEvent(&event) do
            this.enqueueEvent event

        while eventQueues[windowId].Count <> 0 do
            eventQueues[windowId].Dequeue ()
            |> InternalEvent
            |> f 
            
    member this.hideWindow () =
        SDL.SDL_HideWindow window

type Event =
    | Key of char
    | DownArrow
    | UpArrow
    | LeftArrow
    | RightArrow
    | Return
    | MouseButtonDown of int * int // x,y
    | MouseButtonUp of int * int // x,y
    | MouseMotion of int * int * int * int // x,y, relx, rely
    | TimerTick

type ClassifiedEvent<'e> =
    | React of 'e
    | Quit
    | Ignore

let toKeyboardEvent event =
    let (InternalEvent _event) = event
    match SDL.convertEvent _event with
    | SDL.Window wev when wev.event = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE -> Quit
    | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_ESCAPE -> Quit
    | SDL.KeyDown kevent -> React (KeyPress, toKeyboardKey kevent)
    | SDL.KeyUp kevent -> React (KeyRelease, toKeyboardKey kevent)
    | _ -> Ignore

let internal classifyEvent userClassify ev =
    let (InternalEvent _ev) = ev
    match SDL.convertEvent _ev with
        | SDL.Window wev when wev.event = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE -> Quit
        // | SDL.Window wevent->
        //     printfn "Window event %A" wevent.event
        //     // if wevent.event = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE then Quit
        //     // else Ignore(SDL.Window wevent)
        //     Ignore(SDL.Window wevent)
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_ESCAPE -> Quit
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_UP -> React UpArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_DOWN -> React DownArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_LEFT -> React LeftArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_RIGHT -> React RightArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_RETURN -> React Return
        | SDL.MouseButtonDown mouseButtonEvent ->
            (mouseButtonEvent.x,mouseButtonEvent.y) |> MouseButtonDown |> React
        | SDL.MouseButtonUp mouseButtonEvent ->
            (mouseButtonEvent.x,mouseButtonEvent.y) |> MouseButtonUp |> React
        | SDL.MouseMotion motion ->
            (motion.x,motion.y,motion.xrel, motion.yrel) |> MouseMotion |> React
        | SDL.TextInput tinput -> tinput |> Key |> React
        | SDL.User uev -> userClassify uev
        | _ -> Ignore

let private userClassify : SDL.SDL_UserEvent -> ClassifiedEvent<Event> = function
    | uev when uev.``type`` = TIMER_EVENT -> React TimerTick
    | _ -> Ignore

let private timer interval =
    let ticker _ = timerTickEvent () 
    let timer = new System.Timers.Timer(float interval)
    timer.AutoReset <- true
    timer.Elapsed.Add ticker
    timer.Start()
    Some timer

let runAppWithTimer (t:string) (w:int) (h:int) (interval:int option)
        (draw: 's -> drawing_fun)
        (react: 's -> Event -> 's option) (s: 's) : unit =
    let window = new Window(t, w, h)

    let mutable state = s
    let rec drawLoop redraw =
        if redraw then
            window.render (draw, state)
        match window.waitEvent (classifyEvent userClassify) with
            | Quit ->
                // printfn "We quit"
                window.hideWindow ()
                () // quit the interaction by exiting the loop
            | React ev ->
                let redraw =
                    match react state ev with
                        | Some s' -> state <- s'; true
                        | None   -> false
                drawLoop redraw
            | Ignore ->
                // printfn "We loop because of: %A" sdlEvent
                drawLoop false
    let timer = Option.bind (fun t -> timer t) interval
    
    drawLoop true
    timer |> Option.map (fun timer -> timer.Stop()) |> ignore
    //printfn "Out of loop"

let runApp (t:string) (w:int) (h:int) (draw: unit -> drawing_fun) : unit =
    let drawWState s = draw ()
    let react s e = None
    runAppWithTimer t w h None drawWState react 0
