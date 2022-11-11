module Canvas
open System.Runtime.InteropServices
open System

// colors
type color = {red:byte;green:byte;blue:byte;alpha:byte}

let fromArgb (red:int,green:int,blue:int,alpha:int) : color =
  {red=byte(red); green=byte(green); blue=byte(blue); alpha=byte(alpha)}

let fromRgb (red:int,green:int,blue:int) : color = fromArgb (red,green,blue,255)

let red : color = fromRgb(255,0,0)
let green : color = fromRgb(0,255,0)
let blue : color = fromRgb(0,0,255)
let yellow : color = fromRgb(255,255,0)
let lightgrey : color = fromRgb (220,220,220)
let white : color = fromRgb (255,255,255)
let black : color = fromRgb (0,0,0)

let fromColor (color: color) : int * int * int * int =
  (int(color.red),int(color.green),int(color.blue),int(color.alpha))

// canvas - 4 bytes for each pixel (red,green,blue,alpha)
type canvas = {width    : int;
               height    : int;
               data : byte[]}

type point = int * int

// create white opaque canvas
let create (width:int) (height:int) : canvas =
  {width = width;
   height = height;
   data = Array.create (height*width*4) 0xffuy}

let height (canvas:canvas) = canvas.height
let width (canvas:canvas) = canvas.width

// draw alpha single pixel if it is inside the bitmap
let setPixel (canvas:canvas) (color: color) (x:int,y:int) : unit =
  if x < 0 || y < 0 || x >= canvas.width || y >= canvas.height then ()
  else 
    let i = 4*(y*canvas.width+x)
    canvas.data.[i]   <- color.red;
    canvas.data.[i+1] <- color.green;
    canvas.data.[i+2] <- color.blue;
    canvas.data.[i+3] <- color.alpha;

// draw alpha line by linear interpolation
let setLine (canvas:canvas) (color: color) (x1:int,y1:int) (x2:int,y2:int) : unit =
  let m = max (abs(y2-y1)) (abs(x2-x1))
  if m = 0 then ()
  else 
    for i in [0..m] do
    let x = ((m-i)*x1 + i*x2) / m
    let y = ((m-i)*y1 + i*y2) / m
    setPixel canvas color (x,y)

// draw alpha box
let setBox (canvas:canvas) (color:color) (p1:point) (p2:point) : unit =
  do setLine canvas color p1 p2
  do setLine canvas color p1 p2
  do setLine canvas color p1 p2
  do setLine canvas color p1 p2

let setFillBox (canvas:canvas) (color:color) ((x1, y1):point) ((x2,y2):point) : unit =
  for x in [x1..x2] do
    for y in [y1..y2] do
      do setPixel canvas color (x,y)

// get alpha pixel color from alpha bitmap
let getPixel (canvas:canvas) ((x, y):point) : color =   // rgba
  let i = 4*(y*canvas.width+x)
  {red = canvas.data.[i];
   green = canvas.data.[i+1];
   blue = canvas.data.[i+2];
   alpha = canvas.data.[i+3]}

// initialize alpha new bitmap
let init (width:int) (height:int) (f:point->color) : canvas =    // rgba
  let data = Array.create (height*width*4) 0xffuy
  for y in [0..height-1] do
    for x in [0..width-1] do
      let color = f (x,y)
      let i = 4*(y*width+x)
      data[i] <- color.red
      data[i+1] <- color.green
      data[i+2] <- color.blue
      data[i+3] <- color.alpha
  {height = height; width = width; data = data}

// scale alpha bitmap
let scale (canvas:canvas) (w2:int) (h2:int) : canvas =
  let scale_x x = (x * canvas.width) / w2
  let scale_y y = (y * canvas.height) / h2
  init w2 h2 (fun (x,y) -> getPixel canvas (scale_x x, scale_y y))



// Files
// read an image file
let fromFile (filename : string) : canvas =
    use stream = System.IO.File.OpenRead filename
    let image = StbImageSharp.ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha)
    {height = image.Height; width = image.Width; data = image.Data }

// save alpha bitmap as alpha png file
let toPngFile (canvas : canvas) (filename : string) : unit =
    use stream = System.IO.File.OpenWrite filename
    let imageWriter = new StbImageWriteSharp.ImageWriter()
    imageWriter.WritePng(canvas.data, canvas.width, canvas.height,
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

// start an app that can listen to key-events
let runApp (title:string) (width:int) (height:int)
           (draw: int -> int -> 's -> canvas)
           (onKeyDown: 's -> key -> 's option) (s:'s) : unit =

    let state = ref s

    SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore

    let view, height = width, height
    let mutable window, renderer = IntPtr.Zero, IntPtr.Zero
    let windowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |||
                      SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS

    SDL.SDL_CreateWindowAndRenderer(view, height, windowFlags, &window, &renderer) |> ignore
    SDL.SDL_SetWindowTitle(window, title) |> ignore
    let texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_RGBA32, SDL.SDL_TEXTUREACCESS_STREAMING, view, height)

    let frameBuffer = Array.create (view * height * 4) (byte(0))
    let bufferPtr = IntPtr ((Marshal.UnsafeAddrOfPinnedArrayElement (frameBuffer, 0)).ToPointer ())
    let mutable keyEvent = SDL.SDL_KeyboardEvent 0u

    let rec drawLoop redraw =
        if redraw then
            let canvas = draw width height (!state)
            // FIXME: what if canvas does not have dimensions width*height? See issue #13

            Array.blit canvas.data 0 frameBuffer 0 canvas.data.Length

            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, view * 4) |> ignore
            SDL.SDL_RenderClear(renderer) |> ignore
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero) |> ignore
            SDL.SDL_RenderPresent(renderer) |> ignore

        let ret = SDL.SDL_WaitEvent(&keyEvent)
        if ret = 0 then () // an error happened so we exit
        else if keyEvent.``type`` = SDL.SDL_QUIT then ()
        else if (keyEvent.``type`` = SDL.SDL_KEYDOWN) then
             if keyEvent.keysym.sym = SDL.SDLK_ESCAPE then
                 () // quit the game by exiting the loop
             else
                 let k = keyEvent.keysym.sym
                 let redraw =
                     match onKeyDown (!state) (Keysym (int k)) with
                         | Some s -> (state := s; true)
                         | None   -> false
                 drawLoop redraw
        else
            drawLoop false

    drawLoop true

    SDL.SDL_DestroyTexture texture
    SDL.SDL_DestroyRenderer renderer
    SDL.SDL_DestroyWindow window
    SDL.SDL_Quit()
    ()

let runSimpleApp title width height (draw: int->int->canvas) : unit =
  runApp title width height (fun width height () -> draw width height)
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
type line = point * point * color

let rec turtleInterpreter (point:point, angle:int, color:color, isDrawing:bool) (cmds : turtleCmd list) (acc:line list) : line list =
  match cmds with
    | [] -> acc
    | SetColor color :: cmds -> turtleInterpreter (point, angle, color, isDrawing) cmds acc
    | Turn dAngle :: cmds -> turtleInterpreter (point, angle-dAngle, color, isDrawing) cmds acc
    | PenUp :: cmds -> turtleInterpreter (point, angle, color, false) cmds acc
    | PenDown :: cmds -> turtleInterpreter (point, angle, color, true) cmds acc
    | Move length :: cmds ->
      let red = 2.0 * System.Math.PI * float angle / 360.0
      let dx = int(float length * cos red)
      let dy = -int(float length * sin red)
      let (x,y) = point
      let point2 = (x+dx,y+dy)
      let acc2 = if isDrawing then (point, point2, color) :: acc else acc
      turtleInterpreter (point2, angle, color, up) cmds acc2

let turtleDraw (width:int, height:int) (title:string) (pic:turtleCmd list) : unit =
  let canvas = create width height
  let center = (width/2, height/2)
  let dir_up = 90 // Turtle is initialised to pointing upwards
  let initialState = (center, dir_up, black, true)
  let lines = turtleInterpreter initialState pic []
  for (point1, point2, color) in lines do
    do setLine canvas color point1 point2
  do show canvas title
