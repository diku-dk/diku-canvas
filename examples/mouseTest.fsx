#r "nuget:DIKU.Canvas, 2.0.0-alpha9"
open Canvas
open Color

type state = Canvas.color // Click with the mouse changes window color

let w,h = 600,600 // window width and height

/// Prepare a Picture by the present state whenever needed
let draw (s:state): Picture =
    filledRectangle s w h
    |> make

/// React to whenever an event happens
/// All clicking and moving the mouse prints information on the command line.
/// Pressing the mouse down and holding it gives one color, releasing it another.
let react (s:state) (ev:Event) : state option =
    match ev with
        | Event.MouseButtonDown (x,y) ->
            printfn "Clicked at x: %d, y: %d" x y
            fromRgb x y 0 |> Some
        | Event.MouseButtonUp (x,y) ->
            printfn "Released at x: %d, y: %d" x y
            fromRgb 0 x y |> Some
        | Event.MouseMotion (x,y,xrel,yrel) ->
            printfn "moved! x: %d, y: %d, xrel: %d, yrel: %d" x y xrel yrel
            None // mouse motion does not change the color
        | _ -> None // Ignore all non-mouse events

// Start interaction session
let initialState = black // First state drawn by draw
let delayTime = None // microseconds (as an option type)
interact "MouseEvent Test" w h delayTime draw react initialState
