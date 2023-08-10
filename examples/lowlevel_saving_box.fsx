#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha2"

open Lowlevel


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

let draw w (s:state) =
  let half = w / 2
  let (c1,c2,c3,c4) = getPalette s
  let boxAt x y = {x = float32 x; y = float32 y; width = float32 half; height = float32 half}
  let drawing =
      fun dc ->
          dc
          |> fillBox c1 (boxAt 0 0)
          |> fillBox c2 (boxAt 0 half)
          |> fillBox c3 (boxAt half half)
          |> fillBox c4 (boxAt half 0)
  drawToFile w w "boxes.png" drawing
  printfn "Saved!"
  drawing

let react (s:state) (k:Lowlevel.Event) : state option =
    match k with
        | KeyDown k ->
            match getControl k with
                | Some LeftArrow ->
                    printfn "Pressed left-arrow!"
                    cycleStateBackwards s |> Some

                | Some RightArrow ->
                    printfn "Pressed right-arrow!"
                    cycleState s |> Some
                | _ -> None
        | _ -> None

do runAppWithTimer "ColorBoxes" 600 600 None (draw 600) react RedStart
