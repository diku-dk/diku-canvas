#r "nuget:DIKU.Canvas"

open Canvas

type state =
    | RedStart
    | GreenStart
    | BlueStart
    | YellowStart

let cycleState = function
    | RedStart -> GreenStart
    | GreenStart -> BlueStart
    | BlueStart -> YellowStart
    | YellowStart -> RedStart

let cycleStateBackwards = function
    | RedStart -> YellowStart
    | GreenStart -> RedStart
    | BlueStart -> GreenStart
    | YellowStart -> BlueStart

let getPalette (c:state) : (color * color * color * color) =
    match c with
        | RedStart -> (red,green,blue,yellow)
        | GreenStart -> (green,blue,yellow,red)
        | BlueStart -> (blue,yellow,red,green)
        | YellowStart -> (yellow,red,green,blue)

let draw w h (s:state) =
  let canvas = Canvas.create w h
  let half = w / 2
  let (c1,c2,c3,c4) = getPalette s
  setFillBox canvas c1 (0, 0) (half-1,half-1)
  setFillBox canvas c2 (0,half-1) (half-1,h)
  setFillBox canvas c3 (half,half) (w,h)
  setFillBox canvas c4 (half,0) (w,half-1)
  canvas

let react (s:state) (k:Canvas.key) : state option =
    match getKey k with
        | LeftArrow ->
            printfn "Pressed left-arrow!"
            cycleStateBackwards s |> Some

        | RightArrow ->
            printfn "Pressed right-arrow!"
            cycleState s |> Some
        | _ -> None

do runApp "ColorBoxes" 600 600 draw react RedStart
