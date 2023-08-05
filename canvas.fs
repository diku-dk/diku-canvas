module Canvas
open System.Runtime.InteropServices
open System

// colors
type color = {r:byte;g:byte;b:byte;a:byte}

let fromArgb (red:int,green:int,blue:int,alpha:int) : color =
  {r=byte(red); g=byte(green); b=byte(blue); a=byte(alpha)}

let fromRgb (red:int,green:int,blue:int) : color = fromArgb (red,green,blue,255)

let red : color = fromRgb(255,0,0)
let green : color = fromRgb(0,255,0)
let blue : color = fromRgb(0,0,255)
let yellow : color = fromRgb(255,255,0)
let lightgrey : color = fromRgb (220,220,220)
let white : color = fromRgb (255,255,255)
let black : color = fromRgb (0,0,0)

let fromColor (color: color) : int * int * int * int =
  (int(color.r),int(color.g),int(color.b),int(color.a))

// canvas - 4 bytes for each pixel (r,g,b,a)
type canvas = {w    : int;
               h    : int;
               data : byte[]}

// create white opaque canvas
let create (width:int) (height:int) : canvas =
  {w = width;
   h = height;
   data = Array.create (height*width*4) 0xffuy}

let height (C:canvas) = C.h
let width (C:canvas) = C.w

// draw a single pixel if it is inside the bitmap
let setPixel (C:canvas) (color: color) ((x:int,y:int) as point) : unit =
  if x < 0 || y < 0 || x >= C.w || y >= C.h then ()
  else 
    let i = 4*(y*C.w+x)
    C.data.[i]   <- color.r;
    C.data.[i+1] <- color.g;
    C.data.[i+2] <- color.b;
    C.data.[i+3] <- color.a;

// draw a line by linear interpolation
let setLine (C:canvas) (color: color) ((x1:int,y1:int) as pointA) ((x2:int,y2:int) as pointB) : unit =
  let m = max (abs(y2-y1)) (abs(x2-x1))
  if m = 0 then ()
  else 
    for i in 0..m do
        let x = ((m-i)*x1 + i*x2) / m
        let y = ((m-i)*y1 + i*y2) / m
        setPixel C color (x,y)

// draw a box
let setBox (C:canvas) (c:color) ((x1:int,y1:int) as pointA) ((x2:int,y2:int) as pointB) : unit =
  do setLine C c (x1,y1) (x2,y1)
  do setLine C c (x2,y1) (x2,y2)
  do setLine C c (x2,y2) (x1,y2)
  do setLine C c (x1,y2) (x1,y1)

let setFillBox (C:canvas) (c:color) ((x1:int,y1:int) as pointA) ((x2:int,y2:int) as pointB): unit =
  for x in x1..x2 do
    for y in y1..y2 do
      setPixel C c (x,y)

// get a pixel color from a bitmap
let getPixel (C:canvas) ((x:int,y:int) as point) : color =   // rgba
  let i = 4*(y*C.w+x)
  {r = C.data.[i];
   g = C.data.[i+1];
   b = C.data.[i+2];
   a = C.data.[i+3]}

// initialize a new bitmap
let init (width:int) (height:int) (colorMapping:int*int->color) : canvas =    // rgba
  let data = Array.create (height*width*4) 0xffuy
  for y in 0..height-1 do
    for x in 0..width-1 do
      let c = colorMapping (x,y)
      let i = 4*(y*width+x)
      data[i] <- c.r
      data[i+1] <- c.g
      data[i+2] <- c.b
      data[i+3] <- c.a
  {h = height; w = width; data = data}

// scale a bitmap
let scale (C:canvas) (width:int) (height:int) : canvas =
  let scale_x x = (x * C.w) / width
  let scale_y y = (y * C.h) / height
  init width height (fun (x,y) -> getPixel C (scale_x x, scale_y y))



// Files
// read an image file
let fromFile (filePath : string) : canvas =
    use stream = System.IO.File.OpenRead filePath
    let image = StbImageSharp.ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha)
    {h = image.Height; w = image.Width; data = image.Data }

// save a bitmap as a png file
let toPngFile (canvas : canvas) (filePath : string) : unit =
    use stream = System.IO.File.OpenWrite filePath
    let imageWriter = new StbImageWriteSharp.ImageWriter()
    imageWriter.WritePng(canvas.data, canvas.w, canvas.h,
                         StbImageWriteSharp.ColorComponents.RedGreenBlueAlpha, stream)

type key = Keysym of int

type ImgUtilKey =
    | Unknown
    | DownArrow
    | UpArrow
    | LeftArrow
    | RightArrow
    | Space

let getKey (k:key) : ImgUtilKey =
    let kval = match k with | Keysym k -> uint32 k
    match kval with
        | _ when kval = SDL.SDLK_SPACE -> Space
        | _ when kval = SDL.SDLK_UP -> UpArrow
        | _ when kval = SDL.SDLK_DOWN -> DownArrow
        | _ when kval = SDL.SDLK_LEFT -> LeftArrow
        | _ when kval = SDL.SDLK_RIGHT -> RightArrow
        | _ -> Unknown



type event =
    | KeyDown of key
    | TimerTick
    | MouseButtonDown of int * int // x,y
    | MouseButtonUp of int * int // x,y
    | MouseMotion of int * int * int * int // x,y, relx, rely


let runAppWithTimer (t:string) (w:int) (h:int) (interval:int option)
           (draw: int -> int -> 's -> canvas)
           (react: 's -> event -> 's option) (s:'s) : unit =

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

    let rec drawLoop redraw =
        if redraw then
            let canvas = draw w h (!state)
            // FIXME: what if canvas does not have dimensions w*h? See issue #13

            Array.blit canvas.data 0 frameBuffer 0 canvas.data.Length

            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, viewWidth * 4) |> ignore
            SDL.SDL_RenderClear(renderer) |> ignore
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero) |> ignore
            SDL.SDL_RenderPresent(renderer) |> ignore

        let ret = SDL.SDL_WaitEvent(&event)
        if ret = 0 then () // an error happened so we exit
        else
            match SDL.convertEvent event with
                | SDL.Quit -> () // quit the game by exiting the loop
                | SDL.KeyDown keyEvent when keyEvent.keysym.sym = SDL.SDLK_ESCAPE -> ()

                | SDL.KeyDown keyEvent ->
                    let k = keyEvent.keysym.sym |> int |> Keysym |> KeyDown
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
                | _ ->
                    drawLoop false

    drawLoop true

    SDL.SDL_DestroyTexture texture
    SDL.SDL_DestroyRenderer renderer
    SDL.SDL_DestroyWindow window
    SDL.SDL_Quit()
    ()



// start an app that can listen to key-events
let runApp (title:string) (width:int) (height:int)
           (draw: int -> int -> 's -> canvas)
           (react: 's -> key -> 's option) (state:'s) : unit =
    runAppWithTimer title width height None draw
       (fun s -> function (KeyDown k) -> react s k
                        | _ -> None)
       state

let runSimpleApp title width height (draw: int->int->canvas) : unit =
  runApp title width height (fun w h () -> draw w h)
               (fun _ _ -> None) ()

let show (canvas : canvas) (title : string) : unit =
  runApp title (width canvas) (height canvas) (fun _ _ () -> canvas) (fun _ _ -> None) ()



//////////
// Turtle commands

type turtleCmd =
  | SetColor of color
  | Turn of int       // degrees right (0-360)
  | Move of int       // 1 unit = 1 pixel
  | PenUp
  | PenDown

// Interpreter turning commands into lines

type point = int * int
type line = point * point * color

let pi = System.Math.PI

let rec interp (p:point,d:int,c:color,up:bool) (cmds : turtleCmd list) (acc:line list) : line list =
  match cmds with
    | [] -> acc
    | SetColor c :: cmds -> interp (p,d,c,up) cmds acc
    | Turn i :: cmds -> interp (p,d-i,c,up) cmds acc
    | PenUp :: cmds -> interp (p,d,c,true) cmds acc
    | PenDown :: cmds -> interp (p,d,c,false) cmds acc
    | Move i :: cmds ->
      let r = 2.0 * pi * float d / 360.0
      let dx = int(float i * cos r)
      let dy = -int(float i * sin r)
      let (x,y) = p
      let p2 = (x+dx,y+dy)
      let acc2 = if up then acc else (p,p2,c)::acc
      interp (p2,d,c,up) cmds acc2

let turtleDraw ((w:int,h:int) as dimentions) (title:string) (turtleCommands:turtleCmd list) : unit =
  let C = create w h
  let center = (w/2,h/2)
  let dir_up = 90
  let initState = (center,dir_up,black,false)
  let lines = interp initState turtleCommands []
  for (p1,p2,c) in lines do
    do setLine C c p1 p2
  do show C title
