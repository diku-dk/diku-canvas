#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha1"

open Lowlevel

type state = int * int  // x-pos and direction

let WIDTH = 1024
let BOX_W = 200
let BOX_WF = float32 BOX_W

let next (x, dir) =
    let dir = if x + dir < 0 || x + dir > WIDTH - BOX_W then -dir
              else dir
    (x + dir, dir)

let draw dc (x, _) =
  let color = fromRgb(0, 0, x * 255 / (WIDTH - BOX_W))
  let box = {x = float32 x; y = 320.f; width = BOX_WF; height = BOX_WF}
  dc
  |> fillBox color box
  |> drawBox red 3 box


let react (s:state) (ev:Lowlevel.event) : state option =
    match ev with
        | event.TimerTick ->
            s |> next |> Some
        | _ -> None

let main =
    runAppWithTimer "Timer Test" WIDTH 840 (Some 17) draw react (0, 5)
