#r "nuget:DIKU.Canvas, 2.0.0-alpha9"
open Canvas
open Color

type state = color // state is a color

let w,h = 600,600 // window size

/// Prepare a Picture by the present state whenever needed
let draw (s:state): Picture =
    filledRectangle s w h
    |> make

/// React to whenever an event happens
/// All but left-, right-, up-, and down-arrows are ignored. Each of these
/// key-presses produces a window with a unique color.
/// Present state is ignored.
let react _ (ev:Event) : state option =
    match ev with
        | LeftArrow ->
            printfn "Going red!"
            Some red
        | RightArrow ->
            printfn "Going blue!"
            Some blue
        | DownArrow ->
            printfn "Going green!"
            Some green
        | UpArrow->
            printfn "Going yellow!"
            Some yellow
        | _ -> None // ignore event, state is not updated

// Start interaction session
let initialState = red // First state drawn by draw
let delayTime = None // no delay time
interact "ColorTest" w h delayTime draw react initialState
