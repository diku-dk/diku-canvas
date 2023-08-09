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



type event =
    | KeyDown of int
    | TimerTick
    | MouseButtonDown of int * int // x,y
    | MouseButtonUp of int * int // x,y
    | MouseMotion of int * int * int * int // x,y, relx, rely


let runAppWithTimer (t:string) (w:int) (h:int) (interval:int option)
           (draw: int -> int -> 's -> image)
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
            let img = draw w h (!state)  // FIXME: who own the image? can we mutate it?
            use tmp_img =
                img.Clone(fun ctx -> ctx.Crop(min w img.Width, min h img.Height)
                                        .Resize(ResizeOptions(Position = AnchorPositionMode.TopLeft,
                                                              Size = Size(w, h),
                                                              Mode = ResizeMode.BoxPad))
                                     |> ignore)
            tmp_img.CopyPixelDataTo(frameBuffer)
            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, viewWidth * 4) |> ignore
            SDL.SDL_RenderClear(renderer) |> ignore
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero) |> ignore
            SDL.SDL_RenderPresent(renderer) |> ignore
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
