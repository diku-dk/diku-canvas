#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha3"
open Lowlevel

#load "../canvasNT.fsx"
module CNT = CanvasNT

type state = int // a counter

let pen = makePen green 5.0
let drawCurve _ dc =
    dc |>
    CNT.curveDC pen [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
    |> CNT.scaleDC 2.0 0.5
    |> CNT.translateDC 10.0 3.0
let txt = "999"
let font = CNT.createFont "Microsoft Sans Serif" 36.0
let w,h = CNT.measureText font txt
let drawTxT i dc = 
    CNT.textDC white font (string i) (0.0,0.0) dc

let mkDraw n = 
    match n with
        | "text" ->
            w, h, drawTxT
        | "curve" ->
            100, 100, drawCurve
        | "combine" ->
            (max 100 w), (max 100 h), fun state dc ->
                                         dc |> drawCurve state |> drawTxT state
        | _ -> failwith "Unkown test"

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick -> Some (s+1)
        | _ -> None

let main args =
    if Array.length args < 2 then
        1
    else
        let w, h, draw = mkDraw args[1]
        runAppWithTimer "Text Test" (int w) (int h) (Some 1000) draw react 0
        0

[<EntryPoint>]
main fsi.CommandLineArgs
