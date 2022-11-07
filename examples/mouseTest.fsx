#i "nuget:/home/mads/Documents/diku/instruktor/pop2022/imgUtilTesting/diku-canvas/bin/Debug"
#r "nuget:DIKU.Canvas, 1.0.2-alpha1"

open Canvas

type state = Canvas.color

let getColClick = function
    | (x,y) -> fromRgb (x , y, 0)

let getColRelease = function
    | (x,y) -> fromRgb (0, x, y)
    
let draw w h (s:state) =
  let bm = Canvas.create w h
  setFillBox bm s (0,0) (w,h)
  bm

let react (s:state) (ev:Canvas.event) : state option =
    match ev with
        | MouseButtonDown (x,y) ->
            printfn "Clicked at x: %d, y: %d" x y
            getColClick (x,y) |> Some
        | MouseButtonUp (x,y) ->
            printfn "Released at x: %d, y: %d" x y
            getColRelease (x,y) |> Some
        | MouseMotion (x,y,xrel,yrel) ->
            printfn "moved! x: %d, y: %d, xrel: %d, yrel: %d" x y xrel yrel
            None
        // Ignore all non-mouse events
        | _ -> None
do runAppWithTimer "MouseEvent Test" 600 600 None draw react (black)
