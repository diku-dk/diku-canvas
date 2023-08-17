//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Canvas

type state = color // state is a color

let w,h = 600,600 // window size

/// Prepare a Picture by the present state whenever needed
let draw (s:state): Picture =
    filledrectangle s w h
    |> make

/// React to whenever an event happens
/// All but left-, right-, up-, and down-arrows are ignored. Each of these
/// key-presses produces a window with a unique color.
/// Present state is ignored.
let react _ (ev:Event) : state option =
    match ev with
        | Event.KeyDown(k) ->
            match getControl k with
                | Some ControlKey.LeftArrow ->
                    printfn "Going red!"
                    Some color.Red
                | Some ControlKey.RightArrow ->
                    printfn "Going blue!"
                    Some color.Blue
                | Some ControlKey.DownArrow ->
                    printfn "Going green!"
                    Some color.Green
                | Some ControlKey.UpArrow->
                    printfn "Going yellow!"
                    Some color.Yellow
                | _ -> None // unknown key, state is not updated
        | _ -> None // ignore non key events

// Start interaction session
let initialState = color.Red // First state drawn by draw
let delayTime = None // no delay time
interact "ColorTest" w h delayTime draw react initialState
