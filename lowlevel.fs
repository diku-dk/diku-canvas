module Lowlevel

open System.Runtime.InteropServices
open System

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.Drawing

// colors
type color = SixLabors.ImageSharp.Color

let fromRgb (red:int,green:int,blue:int) : color =
    Color(Rgba32(r=byte(red), g=byte(green), b=byte(blue)))

let red : color = fromRgb(255, 0, 0)
let green : color = fromRgb(0, 255, 0)
let blue : color = fromRgb(0, 0, 255)
let yellow : color = fromRgb(255, 255, 0)
let lightgrey : color = fromRgb (220, 220, 220)
let white : color = fromRgb (255, 255, 255)
let black : color = fromRgb (0, 0, 0)

type image = SixLabors.ImageSharp.Image<Rgba32>

type drawing_context = SixLabors.ImageSharp.Processing.IImageProcessingContext

type drawing_fun = drawing_context -> drawing_context

type rect = {x : float32; y : float32; width : float32; height : float32}

let toRectangleF r =
    RectangleF(r.x, r.y, r.width, r.height)

let fillBox (color:color) (box:rect) (ctx:drawing_context) : drawing_context =
    ctx.Fill(color, toRectangleF box)

let drawBox (color:color) (lineWidth:int) (box:rect) (ctx:drawing_context) =
    ctx.Draw(color, float32 lineWidth, toRectangleF box)

// let rotate (degrees:float32) (ctx:drawing_context) =  // FIXME: find more useful Rotate method
//     ctx.Rotate(degrees)

type Font = SixLabors.Fonts.Font
type FontFamily = SixLabors.Fonts.FontFamily

let getFamily name = SixLabors.Fonts.SystemFonts.Get(name)

let makeFont (fam:FontFamily) (size:float32) = new SixLabors.Fonts.Font(fam, size)

let drawText (msg:string) (font:Font) (color:color) (x:float32) (y:float32) (ctx:drawing_context) =
    ctx.DrawText(msg, font, color, PointF(x, y))

type Brush = SixLabors.ImageSharp.Drawing.Processing.Brush
type Pen = SixLabors.ImageSharp.Drawing.Processing.Pen

let solidBrush (color:color) : Brush =
    Brushes.Solid(color)

let solidPen (color:color) (width:float) : Pen =
    Pens.Solid(color, float32 width)

// let drawLines (p: Pen) (points: (float*float) list) (ctx:drawing_context) : drawing_context =
//     let points_arr = points |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b))
//     ctx.DrawLines(p, points_arr)

type point = int * int
type pointF = float * float
type Matrix3x2 = System.Numerics.Matrix3x2
type TextOptions = SixLabors.Fonts.TextOptions

let toPointF (x:float, y:float) = PointF(x = float32 x, y = float32 y)

type PrimPath =
    | Arc of pointF * float * float * float * float * float
    | CubicBezier of pointF * pointF * pointF * pointF
    | Line of pointF * pointF
    | Lines of pointF list

and PathTree =
    | Empty
    | Prim of PrimPath
    | PathAdd of PathTree * PathTree
    | Transform of Matrix3x2 * PathTree
    | Text of string * TextOptions  // FIXME: Maybe we want a `Raw of IPathCollection` constructor instead of the Text constructors
    | TextAlong of string * TextOptions * PrimPath

let (<+>) p1 p2 =
    match p1, p2 with
        | Empty, _ -> p2
        | _, Empty -> p1
        | _ -> PathAdd (p1, p2)

let emptyPath = Empty

let pathFromList ps = List.fold (<+>) emptyPath ps

let transform mat = function
    | Transform (cur, p) ->
        Transform(mat * cur, p) // FIXME: maybe test for identify transformation?
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


type IPathCollectionArray = IPathCollection array

let flatten (p : PathTree) : IPathCollection list =  //FIXME: should maybe return a seq<IPathCollection>
    let rec traverse (acc : IPathCollection list) = function // tail-recursive traversal
        | [] -> acc
        | cur :: worklist ->
            match cur with
                | Empty ->
                    traverse acc worklist
                | Prim p ->
                    let path = PathCollection [| p |> toILineSegment |> toPath |] : IPathCollection
                    let acc =  path :: acc
                    traverse acc worklist
                | PathAdd (p1, p2) ->
                    traverse acc (p1 :: p2 :: worklist)
                | Transform (mat, p) ->
                    let transformed = traverse [] [p] |> List.map (fun p -> p.Transform(mat))
                    let acc = transformed @ acc
                    traverse acc worklist
                | Text (text, options) ->
                    let glyphs = TextBuilder.GenerateGlyphs(text, options)
                    let acc =  glyphs :: acc
                    traverse acc worklist
                | TextAlong(text, options, p) ->
                    let path = p |> toILineSegment |> toPath
                    let glyphs = TextBuilder.GenerateGlyphs(text, path, options)
                    let acc =  glyphs :: acc
                    traverse acc worklist
    traverse [] [p]


let drawCollection (pen: Pen) (col:IPathCollection) (ctx:drawing_context) : drawing_context =
    ctx.Draw(pen, col)

let fillCollection (brush:Brush) (options:DrawingOptions) (paths:IPathCollection) (ctx:drawing_context) : drawing_context =
    ctx.Fill(options, brush, paths)

let drawPathTree (pen: Pen) (paths:PathTree) (ctx:drawing_context) : drawing_context =
    flatten paths |> List.fold (fun ctx col -> drawCollection pen col ctx) ctx

let fillPathTree (brush:Brush) (options:DrawingOptions) (paths:PathTree) (ctx:drawing_context) : drawing_context =
    flatten paths |> List.fold (fun ctx col -> fillCollection brush options col ctx) ctx


let drawToFile width heigth (filePath:string) (draw:drawing_fun) =
    let img = new Image<Rgba32>(width, heigth)
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
            gif.SaveAsGif(filePath);
        | _ -> ()

type ControlKey =
    | DownArrow
    | UpArrow
    | LeftArrow
    | RightArrow
    | Space

type Event =
    | KeyDown of int
    | TimerTick
    | MouseButtonDown of int * int // x,y
    | MouseButtonUp of int * int // x,y
    | MouseMotion of int * int * int * int // x,y, relx, rely


let getControl (k:int) : ControlKey option =
    let kval = uint32 k
    match kval with
        | _ when kval = SDL.SDLK_SPACE -> Some Space
        | _ when kval = SDL.SDLK_UP -> Some UpArrow
        | _ when kval = SDL.SDLK_DOWN -> Some DownArrow
        | _ when kval = SDL.SDLK_LEFT -> Some LeftArrow
        | _ when kval = SDL.SDLK_RIGHT -> Some RightArrow
        | _ -> None


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

    let frameBuffer = Array.create (viewWidth * viewHeight *4 ) (byte(0))
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

    let mutable event = SDL.SDL_Event()
    let img = new Image<Rgba32>(w, h) // FIXME: can we use framebuffer directly?

    let rec drawLoop redraw =
        if redraw then
            img.Mutate(fun ctx ->
                       (draw (!state) ctx)
                          .Crop(min w img.Width, min h img.Height)
                          .Resize(ResizeOptions(Position = AnchorPositionMode.TopLeft,
                                                Size = Size(w, h),
                                                Mode = ResizeMode.BoxPad))
                       |> ignore)
            img.CopyPixelDataTo(frameBuffer)
            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, viewWidth * 4) |> ignore
            SDL.SDL_RenderClear(renderer) |> ignore
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero) |> ignore
            SDL.SDL_RenderPresent(renderer) |> ignore
            img.Mutate(fun ctx -> ctx.Clear(Color.Transparent) |> ignore)
            ()

        let ret = SDL.SDL_WaitEvent(&event)
        if ret = 0 then () // an error happened so we exit
        else
            match SDL.convertEvent event with
                | SDL.Quit ->
                    // printfn "We quit"
                    () // quit the game by exiting the loop
                | SDL.KeyDown keyEvent when keyEvent.keysym.sym = SDL.SDLK_ESCAPE -> ()

                | SDL.KeyDown keyEvent ->
                    let k = keyEvent.keysym.sym |> int |> KeyDown
                    let redraw =
                        match react (!state) k with
                            | Some s -> (state := s; true)
                            | None   -> false
                    drawLoop redraw
                | SDL.User uev when uev.``type`` = TIMER_EVENT ->
                    let redraw =
                        match react (!state) TimerTick with
                            | Some s -> (state := s; true)
                            | None   -> false
                    drawLoop redraw
                | SDL.MouseButtonDown mouseButtonEvent ->
                    let mouseEvent = (mouseButtonEvent.x,mouseButtonEvent.y) |> MouseButtonDown
                    let redraw =
                        match react (!state) mouseEvent with
                            | Some s -> (state := s; true)
                            | None -> false
                    drawLoop redraw
                | SDL.MouseButtonUp mouseButtonEvent ->
                    let mouseEvent = (mouseButtonEvent.x,mouseButtonEvent.y) |> MouseButtonUp
                    let redraw =
                        match react (!state) mouseEvent with
                            | Some s -> (state := s; true)
                            | None -> false
                    drawLoop redraw
                | SDL.MouseMotion mouseMotion ->
                    let mouseEvent = (mouseMotion.x,mouseMotion.y,mouseMotion.xrel, mouseMotion.yrel) |> MouseMotion
                    let redraw =
                        match react (!state) mouseEvent with
                            | Some s -> (state := s; true)
                            | None -> false
                    drawLoop redraw
                | ev ->
                    // printfn "We loop because of: %A" ev
                    drawLoop false

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
