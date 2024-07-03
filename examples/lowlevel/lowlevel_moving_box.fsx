#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
//#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.1.0-alpha1"
open Canvas
open Lowlevel

type state = int * int  // x-pos and direction

let WIDTH = 1024
let BOX_W = 200
let BOX_WF = float BOX_W

let next (x, dir) =
    let dir = if x + dir < 0 || x + dir > WIDTH - BOX_W then -dir
              else dir
    (x + dir, dir)

let rectangle x y width height =
    Lines [x, y; x + width, y; x+width, y+height; x, y+height; x, y]


let draw (x, _) dc =
  let color = fromRgb 0 0 (x * 255 / (WIDTH - BOX_W))
  let brush = solidBrush color
  let box = Prim (brush, rectangle (float x) 320.0 BOX_WF BOX_WF)
  dc
    |> drawPathTree box


let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

let main =
    runAppWithTimer "Timer Test" WIDTH 840 (Some 17) draw react (0, 5)
