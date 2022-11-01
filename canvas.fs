module Canvas
open System.Runtime.InteropServices
open System

// colors
type color = {r:byte;g:byte;b:byte;a:byte}

let fromArgb (r:int,g:int,b:int,a:int) : color =
  {r=byte(r); g=byte(g); b=byte(b); a=byte(a)}

let fromRgb (r:int,g:int,b:int) : color = fromArgb (r,g,b,255)

let red : color = fromRgb(255,0,0)
let green : color = fromRgb(0,255,0)
let blue : color = fromRgb(0,0,255)
let yellow : color = fromRgb(255,255,0)
let lightgrey : color = fromRgb (220,220,220)
let white : color = fromRgb (255,255,255)
let black : color = fromRgb (0,0,0)

let fromColor (c: color) : int * int * int * int =
  (int(c.r),int(c.g),int(c.b),int(c.a))

// canvas - 4 bytes for each pixel (r,g,b,a)
type canvas = {w    : int;
               h    : int;
               data : byte[]}

// create white opaque canvas
let create (w:int) (h:int) : canvas =
  {w = w;
   h = h;
   data = Array.create (h*w*4) 0xffuy}

let height (C:canvas) = C.h
let width (C:canvas) = C.w

// draw a single pixel if it is inside the bitmap
let setPixel (C:canvas) (c: color) (x:int,y:int) : unit =
  if x < 0 || y < 0 || x >= C.w || y >= C.h then ()
  else 
    let i = 4*(y*C.w+x)
    C.data.[i]   <- c.r;
    C.data.[i+1] <- c.g;
    C.data.[i+2] <- c.b;
    C.data.[i+3] <- c.a;

// draw a line by linear interpolation
let setLine (C:canvas) (c: color) (x1:int,y1:int) (x2:int,y2:int) : unit =
  let m = max (abs(y2-y1)) (abs(x2-x1))
  if m = 0 then ()
  else 
    for i in [0..m] do
    let x = ((m-i)*x1 + i*x2) / m
    let y = ((m-i)*y1 + i*y2) / m
    setPixel C c (x,y)

// draw a box
let setBox (C:canvas) (c:color) (x1:int,y1:int) (x2:int,y2:int) : unit =
  do setLine C c (x1,y1) (x2,y1)
  do setLine C c (x2,y1) (x2,y2)
  do setLine C c (x2,y2) (x1,y2)
  do setLine C c (x1,y2) (x1,y1)

let setFillBox (C:canvas) (c:color) (x1:int,y1:int) (x2:int,y2:int) : unit =
  for x in [x1..x2] do
    for y in [y1..y2] do
      do setPixel C c (x,y)

// Generate all the points on the circle in the first octant (45 degrees) centered at (0,0)
let circlePoints (r: int) : (int * int) list =
  // Function to find the first 45 degrees of the circle
  // using the midpoint circle algorithm
  let rec findOctantPoints (x: int) (y: int) (p: int) : (int * int) list =
    if x > y-1 // Sub by 1 to count the last point in the 
    then 
      let y' = y + 1
      if p <= 0
      then 
        let p' = p + 2*y' + 1
        (x, y) :: findOctantPoints x y' p'
      else
        let x' = x - 1
        let p' = p + 2*y' + 1 - 2*x'
        (x, y) :: findOctantPoints x' y' p'
    else []

  let eightPoints (x: int) (y: int) = 
      [(x, y); (-x, y); (x, -y); (-x, -y); 
        (y, x); (-y, x); (y, -x); (-y, -x)]

  // Initial variables for midpoint circle algorithm
  let x = r
  let p = 1-r
  let y = 0

  List.map (fun (x, y) -> eightPoints x y) (findOctantPoints x y p) |> List.concat

// Draw a circle centered at (x0,y0) with radius r
let setCircle (canvas:canvas) (color:color) (x0:int, y0:int) (r:int) : unit =
  let circle = circlePoints r
  let recenteredPoints: (int * int) list = List.map (fun (x, y) -> (x + x0, y + y0)) circle
  for (x,y) in recenteredPoints do
    setPixel canvas color (x, y)

let setFillCircle (canvas: canvas) (color: color) (x0, y0) (r: int) =
    // This is here since the equality check doesn't produce the same perimeter as the midpoint algorithm.
    setCircle canvas color (x0, y0) r

    // Find all points within the circle and draw them.

    let rSquared = r * r
    for x in -r .. r do
        for y in -r .. r do
            if x * x + y * y <= rSquared then
                setPixel canvas color (x0 + x, y0 + y)
  
// Draw an arc centered at (x0,y0) with radius r with a start angle and end angle
// Modulo used to normalise the angles                    
let inline (%!) a b = (a % b + b) % b

// Find angle interval when normalised to [0, 2PI]
let withinAngleSector (x: int) (y: int) (startAngle: float) (endAngle: float) =
    let angle = atan2 (float y) (float x)
    let angle = angle %! (2.0 * System.Math.PI)
    (angle >= startAngle && angle <= endAngle) || (angle >= startAngle - 2.0 * System.Math.PI && angle <= endAngle - 2.0 * System.Math.PI)

let strokeArc (canvas: canvas) (color: color) (x0: int, y0: int) (r: int) (startAngle: float) (endAngle: float) : unit =
  // If ∣θ₁-θ₀∣ ≥ 2π then draw a full circle since it is a more efficient implementation.
  if abs(endAngle - startAngle) >= 2.0 * System.Math.PI
  then setCircle canvas color (x0, y0) r
  // Else draw using the arc drawing algorithm.
  else
    let startAngle = startAngle %! (2.0 * System.Math.PI)
    let endAngle = endAngle %! (2.0 * System.Math.PI)
    let endAngle = if startAngle >= endAngle then endAngle + 2.0 * System.Math.PI else endAngle

    let filteredPoints = circlePoints r |> List.filter (fun (x, y) -> withinAngleSector x y startAngle endAngle)

    filteredPoints |> List.iter (fun (x, y) -> setPixel canvas color (x0 + x, y0 + y))

let fillArc (canvas: canvas) (color: color) (x0: int, y0: int) (r: int) (startAngle: float) (endAngle: float) : unit =
  // If ∣θ₁-θ₀∣ ≥ 2π then draw a full circle since it is a more efficient implementation.
  if abs(endAngle - startAngle) >= 2.0 * System.Math.PI
  then setFillCircle canvas color (x0, y0) r
  // Else draw using the arc drawing algorithm.
  else
    let startAngle = startAngle %! (2.0 * System.Math.PI)
    let endAngle = endAngle %! (2.0 * System.Math.PI)
    let endAngle = if startAngle >= endAngle then endAngle + 2.0 * System.Math.PI else endAngle

    let rSquared = r * r
    for x in -r .. r do
        for y in -r .. r do
            if x * x + y * y <= rSquared then
                if withinAngleSector x y startAngle endAngle then
                    setPixel canvas color (x0 + x, y0 + y)



// get a pixel color from a bitmap
let getPixel (C:canvas) (x:int,y:int) : color =   // rgba
  let i = 4*(y*C.w+x)
  {r = C.data.[i];
   g = C.data.[i+1];
   b = C.data.[i+2];
   a = C.data.[i+3]}

// initialize a new bitmap
let init (w:int) (h:int) (f:int*int->color) : canvas =    // rgba
  let data = Array.create (h*w*4) 0xffuy
  for y in [0..h-1] do
    for x in [0..w-1] do
      let c = f (x,y)
      let i = 4*(y*w+x)
      data[i] <- c.r
      data[i+1] <- c.g
      data[i+2] <- c.b
      data[i+3] <- c.a
  {h = h; w = w; data = data}

// scale a bitmap
let scale (C:canvas) (w2:int) (h2:int) : canvas =
  let scale_x x = (x * C.w) / w2
  let scale_y y = (y * C.h) / h2
  init w2 h2 (fun (x,y) -> getPixel C (scale_x x, scale_y y))



// Files
// read an image file
let fromFile (filename : string) : canvas =
    use stream = System.IO.File.OpenRead filename
    let image = StbImageSharp.ImageResult.FromStream(stream, StbImageSharp.ColorComponents.RedGreenBlueAlpha)
    {h = image.Height; w = image.Width; data = image.Data }

// save a bitmap as a png file
let toPngFile (canvas : canvas) (filename : string) : unit =
    use stream = System.IO.File.OpenWrite filename
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

// start an app that can listen to key-events
let runApp (t:string) (w:int) (h:int)
           (draw: int -> int -> 's -> canvas)
           (onKeyDown: 's -> key -> 's option) (s:'s) : unit =

    let state = ref s

    SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore

    let viewWidth, viewHeight = w, h
    let mutable window, renderer = IntPtr.Zero, IntPtr.Zero
    let windowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |||
                      SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS

    SDL.SDL_CreateWindowAndRenderer(viewWidth, viewHeight, windowFlags, &window, &renderer) |> ignore
    SDL.SDL_SetWindowTitle(window, t) |> ignore
    let texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_RGBA32, SDL.SDL_TEXTUREACCESS_STREAMING, viewWidth, viewHeight)

    let frameBuffer = Array.create (viewWidth * viewHeight *4 ) (byte(0))
    let bufferPtr = IntPtr ((Marshal.UnsafeAddrOfPinnedArrayElement (frameBuffer, 0)).ToPointer ())
    let mutable keyEvent = SDL.SDL_KeyboardEvent 0u

    let rec drawLoop redraw =
        if redraw then
            let canvas = draw w h (!state)
            // FIXME: what if canvas does not have dimensions w*h? See issue #13

            Array.blit canvas.data 0 frameBuffer 0 canvas.data.Length

            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, viewWidth * 4) |> ignore
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

let runSimpleApp t w h (draw: int->int->canvas) : unit =
  runApp t w h (fun w h () -> draw w h)
               (fun _ _ -> None) ()

let show (canvas : canvas) (t : string) : unit =
  runApp t (width canvas) (height canvas) (fun _ _ () -> canvas) (fun _ _ -> None) ()



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

let turtleDraw (w:int,h:int) (title:string) (pic:turtleCmd list) : unit =
  let C = create w h
  let center = (w/2,h/2)
  let dir_up = 90
  let initState = (center,dir_up,black,false)
  let lines = interp initState pic []
  for (p1,p2,c) in lines do
    do setLine C c p1 p2
  do show C title
