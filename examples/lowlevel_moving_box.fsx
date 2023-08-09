#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha1"
#r "nuget:SixLabors.ImageSharp"
#r "nuget:SixLabors.ImageSharp.Drawing, 1.0.0-beta15"


open SixLabors.ImageSharp

open SixLabors.ImageSharp.PixelFormats

open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.Drawing

open Lowlevel

type state = int * int  // x-pos and direction

let WIDTH = 1024
let BOX_W = 200

let next (x, dir) =
    let dir = if x + dir < 0 || x + dir > WIDTH - BOX_W then -dir
              else dir
    (x + dir, dir)

let draw w h (x, _) =
  let img = new Image<Rgba32>(w, h)
  let color = fromRgb(0, 0, x * 255 / (WIDTH - BOX_W))
  let box = RectangleF(float32 x, 320.f, float32 BOX_W , 520.0f)
  img.Mutate(fun ctx -> ctx.Fill(color, box) |> ignore)
  img


let react (s:state) (ev:Lowlevel.event) : state option =
    match ev with
        | event.TimerTick ->
            s |> next |> Some
        | _ -> None

let main =
    runAppWithTimer "Timer Test" WIDTH 840 (Some 17) draw react (0, 5)
