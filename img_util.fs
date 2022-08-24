module ImgUtil

// colors
type color = {r:byte;g:byte;b:byte;a:byte}

let fromArgb (r:int,g:int,b:int,a:int) : color =
  {r=byte(r); g=byte(g); b=byte(b); a=byte(a)}

let fromRgb (r:int,g:int,b:int) : color = fromArgb (r,g,b,255)

let red : color = fromRgb(255,0,0)
let green : color = fromRgb(0,255,0)
let blue : color = fromRgb(0,0,255)
let yellow : color = fromRgb(255,255,0)


let fromColor (c: color) : int * int * int * int =
  (int(c.r),int(c.g),int(c.b),int(c.a))

// canvas - 4 bytes for each pixel (r,g,b,a)
type canvas = {w:int;h:int;data:byte[]}

// create white opaque canvas
let mk w h : canvas =
  {w=w; h=h;
   data=Array.create (h*w*4) 0xffuy}

let height (C:canvas) = C.h
let width (C:canvas) = C.w

// draw a single pixel if it is inside the bitmap
let setPixel (c: color) (x,y) (C:canvas) : unit =
  if x < 0 || y < 0 || x >= C.w || y >= C.h then ()
  else let i = 4*(y*C.w+x)
  // Even though SDL_PIXELFORMAT is RGBA8888
  // We need to structure it in reverse, so in reality ABGR
       in (C.data.[i]   <- c.a;
           C.data.[i+1] <- c.b;
           C.data.[i+2] <- c.g;
           C.data.[i+3] <- c.r;
           )

// draw a line by linear interpolation
let setLine (c: color) (x1:int,y1:int) (x2:int,y2:int) C : unit =
  let m = max (abs(y2-y1)) (abs(x2-x1))
  in if m = 0 then ()
     else for i in [0..m] do
            let x = ((m-i)*x1 + i*x2) / m
            let y = ((m-i)*y1 + i*y2) / m
            in setPixel c (x,y) C

// draw a box
let setBox c (x1,y1) (x2,y2) C =
  do setLine c (x1,y1) (x2,y1) C
  do setLine c (x2,y1) (x2,y2) C
  do setLine c (x2,y2) (x1,y2) C
  do setLine c (x1,y2) (x1,y1) C

let setFillBox c (x1,y1) (x2,y2) C =
    for x in [x1..x2] do
        for y in [y1..y2] do
            do setPixel c (x,y) C


// get a pixel color from a bitmap
let getPixel (C:canvas) (x:int,y:int) : color =   // rgba
  let i = 4*(y*C.w+x)
  in {r=C.data.[i]; g=C.data.[i+1];
      b=C.data.[i+2]; a=C.data.[i+3]}

// initialize a new bitmap
let init (w:int) (h:int) (f:int*int->color) : canvas =    // rgba
  let data = Array.create (h*w*4) 0xffuy
  for y in [0..h-1] do
    for x in [0..w-1] do
      let c = f (x,y)
      let i = 4*(y*w+x)
      in (data.[i] <- c.a;
          data.[i+1] <- c.b;
          data.[i+2] <- c.g;
          data.[i+3] <- c.r)
  {h=h;w=w;data=data}

// scale a bitmap
let scale (C:canvas) w2 h2 : canvas =
  let scale_x x = (x * C.w) / w2
  let scale_y y = (y * C.h) / h2
  in init w2 h2 (fun (x,y) -> getPixel C (scale_x x, scale_y y))

// read a bitmap file
let fromFile (fname : string) : canvas = failwith "not implemented, yet"
  // use pb0 = new Gdk.Pixbuf(fname)
  // use pb = pb0.AddAlpha(false,0x0uy,0x0uy,0x0uy)
  // let h = pb.Height
  // let w = pb.Width
  // let len = h*w*4
  // let data = Array.create len 0xffuy
  // in (System.Runtime.InteropServices.Marshal.Copy(pb.Pixels,data,0,len);
  //     {h=h;w=w;data=data})

// create a pixbuf from a bitmap
let toPixbuf (C:canvas) =
  failwith "Not implemented"
//  new Gdk.Pixbuf(C.data,true,8,C.w,C.h,4*C.w)

// save a bitmap as a png file
let toPngFile (fname : string) (C:canvas) : unit =
  failwith "Not implemented"

// start and run an application with an action
let runApplication (action:unit -> unit) =
  failwith "not implemented"
  // (Application.Init();
  //  Application.Invoke(fun _ _ -> action());
  //  Application.Run())

type key = Keysym of int
open System.Runtime.InteropServices
open System


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
           (f:int -> int -> 's -> canvas)
           (onKeyDown: 's -> key -> 's option) (s:'s) : unit =

    let state = ref s

    SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore

    let viewWidth, viewHeight = w, h
    let mutable window, renderer = IntPtr.Zero, IntPtr.Zero
    let windowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |||
                      SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS

    SDL.SDL_CreateWindowAndRenderer(viewWidth, viewHeight, windowFlags, &window, &renderer) |> ignore
    let texture = SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_RGBA8888, SDL.SDL_TEXTUREACCESS_STREAMING, viewWidth, viewHeight)

    let frameBuffer = Array.create (viewWidth * viewHeight *4 ) (byte(0))
    let bufferPtr = IntPtr ((Marshal.UnsafeAddrOfPinnedArrayElement (frameBuffer, 0)).ToPointer ())
    let mutable keyEvent = SDL.SDL_KeyboardEvent 0u

    let rec drawLoop() =
        let C = f w h (!state)
        Array.blit C.data 0 frameBuffer 0 C.data.Length

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
                 match onKeyDown (!state) (Keysym (int k)) with
                     | Some s -> state := s
                     | None   -> ()
                 drawLoop()
        else
            drawLoop()
    drawLoop ()

    SDL.SDL_DestroyTexture(texture)
    SDL.SDL_DestroyRenderer(renderer)
    SDL.SDL_DestroyWindow(window)
    SDL.SDL_Quit()
    ()

let runSimpleApp t w h (f:int->int->canvas) : unit =
  runApp t w h (fun w h () -> f w h)
               (fun _ _ -> None) ()

let show t (C:canvas) : unit =
  runApp t (width C) (height C) (fun w h () -> scale C w h) (fun _ _ -> None) ()
