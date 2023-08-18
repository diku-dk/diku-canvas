module internal Lowlevel

open System.Runtime.InteropServices
open System

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.Drawing

// colors
type Color = SixLabors.ImageSharp.Color

let fromRgba (red:int) (green:int) (blue:int) (a:int) : Color =
    Color.FromRgba(byte red, byte green, byte blue, byte a)
let fromRgb (red:int) (green:int) (blue:int) : Color =
    Color.FromRgb(byte red, byte green, byte blue)

let red : Color = fromRgb 255 0 0
let green : Color = fromRgb 0 255 0
let blue : Color = fromRgb 0 0 255
let yellow : Color = fromRgb 255 255 0
let lightgrey : Color = fromRgb 220 220 220
let white : Color = fromRgb 255 255 255
let black : Color = fromRgb 0 0 0

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
    | GetTransform of (Matrix3x2 -> PathDefinition)
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
                | GetTransform f ->
                    let transformed = f curT
                    loop builder curT (transformed :: SetTransform curT :: worklist)
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

let emptyPath = Empty

let pathFromList ps = List.fold (<+>) emptyPath ps

let transform (mat:Matrix3x2) = function
    | Transform (cur, p) ->
        if mat.IsIdentity then Transform(cur, p)
        else let res = mat * cur
             if res.IsIdentity then p
             else Transform(res, p)
    | p ->
        Transform(mat, p)

let rotateDegreeAround (degree:float) point p =
    let rad = GeometryUtilities.DegreeToRadian (float32 degree)
    let mat = Matrix3x2Extensions.CreateRotation(rad, toPointF point)
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

type ClassifiedEvent =
    | React of Event
    | Quit
    | Ignore of SDL.Event

let classifyEvent userClassify ev =
    match SDL.convertEvent ev with
        | SDL.Quit -> Quit
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
        | ev -> Ignore ev

let runAppWithTimer (t:string) (w:int) (h:int) (interval:int option)
           (draw: 's -> drawing_fun)
           (react: 's -> Event -> 's option) (s:'s) : unit =

    let state = ref s

    SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore

    let viewWidth, viewHeight = w, h
    let mutable window, renderer = IntPtr.Zero, IntPtr.Zero
    let windowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |||
                      SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS

    window <- SDL.SDL_CreateWindow(t, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                                   viewWidth, viewHeight, windowFlags)
    renderer <- SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |||
                                                   SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC)

    let texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_RGBA32, SDL.SDL_TEXTUREACCESS_STREAMING, viewWidth, viewHeight)

    let frameBuffer = Array.create (viewWidth * viewHeight *4 ) (byte 0)
    let bufferPtr = IntPtr ((Marshal.UnsafeAddrOfPinnedArrayElement (frameBuffer, 0)).ToPointer ())

    // Set up a timer
    let TIMER_EVENT = SDL.SDL_RegisterEvents 1 // TODO: check that we succeed
    match interval with
        Some interv when interv > 0 ->
            let ticker _ =
                let mutable ev = SDL.SDL_Event()
                ev.``type`` <- TIMER_EVENT
                SDL.SDL_PushEvent(&ev) |> ignore

            let timer = new System.Timers.Timer(float interv)
            timer.AutoReset <- true
            timer.Elapsed.Add ticker
            timer.Start()
        | _ -> // No timer
            ()

    let userClassify : SDL.SDL_UserEvent -> ClassifiedEvent = function
        | uev when uev.``type`` = TIMER_EVENT -> React TimerTick
        | uev -> Ignore(SDL.User uev)

    let mutable event = SDL.SDL_Event()
    let img = new Image<Rgba32>(w, h, Color.Black) // FIXME: can we use framebuffer directly?

    let rec drawLoop redraw =
        if redraw then
            img.Mutate(fun ctx ->
                       (draw (!state) ctx)
                          // .Crop(min w img.Width, min h img.Height)
                          // .Resize(ResizeOptions(Position = AnchorPositionMode.TopLeft,
                          //                       Size = Size(w, h),
                          //                       Mode = ResizeMode.BoxPad))
                       |> ignore)
            img.CopyPixelDataTo(frameBuffer)
            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, viewWidth * 4) |> ignore
            SDL.SDL_RenderClear(renderer) |> ignore
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero) |> ignore
            SDL.SDL_RenderPresent(renderer) |> ignore
            img.Mutate(fun ctx -> ctx.Clear(Color.Black) |> ignore)
            ()

        let ret = SDL.SDL_WaitEvent(&event)
        if ret = 0 then () // an error happened so we exit
        else
            match classifyEvent userClassify event with
                | Quit ->
                    // printfn "We quit"
                    () // quit the interaction by exiting the loop
                |  React ev ->
                    let redraw =
                        match react (!state) ev with
                            | Some s -> (state := s; true)
                            | None   -> false
                    drawLoop redraw
                | Ignore sdlEvent ->
                    // printfn "We loop because of: %A" sdlEvent
                    drawLoop false

    SDL.SDL_StartTextInput()
    drawLoop true

    SDL.SDL_DestroyTexture texture
    SDL.SDL_DestroyRenderer renderer
    SDL.SDL_DestroyWindow window
    SDL.SDL_Quit()
    ()

let runApp (t:string) (w:int) (h:int) (draw: unit -> drawing_fun) : unit =
    let drawWState s = draw ()
    let react s e = None
    runAppWithTimer t w h None drawWState react 0
