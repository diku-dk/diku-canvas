#r "nuget:DIKU.Canvas, 2.0.0-alpha8"
open Canvas

type state = int * int  // x-pos and direction

// The size of the window and the edge-size of the box
let w,h,boxW = 1024,512,200

/// Update state function
let next (x, dir) =
    let dir = 
        if x + dir < 0 || x + dir > w - boxW then 
            -dir
        else
            dir
    (x + dir, dir)

/// Prepare a Picture by the present state whenever needed
let draw ((x, _):state): Picture =
    let boundingColor = red
    let color = fromRgb 0 0 (x * 255 / (w - boxW))
    onto (rectangle boundingColor 2.0 (float boxW) (float boxW))
        (filledRectangle color (float boxW) (float boxW))
    |> translate (float x) (float (h-boxW)/2.0)
    |> make

/// React to whenever an event happens
let react (s:state) (ev:Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

// Start interaction session
let initialState = (0, 5) // First state drawn by draw
let delayTime = (Some 16) // microseconds (as an option type)
interact "Moving box" w h delayTime draw react initialState
