#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 1.0.2-alpha1"

open Canvas

type state = int * int  // x-pos and direction

let WIDTH = 1024
let BOX_W = 200

let next (x, dir) =
    let dir = if x + dir < 0 || x + dir > WIDTH - BOX_W then -dir
              else dir
    (x + dir, dir)

let draw w h (x, _) =
  let canvas = Canvas.create w h
  let color = fromRgb(0, 0, x * 255 / (WIDTH - BOX_W))
  setFillBox canvas color (x,320) (x+BOX_W, 520)
  canvas

let react (s:state) (ev:Canvas.event) : state option =
    match ev with
        | TimerTick ->
            s |> next |> Some
        | _ -> None

let main =
    runAppWithTimer "Timer Test" WIDTH 840 (Some 17) draw react (0, 5)
