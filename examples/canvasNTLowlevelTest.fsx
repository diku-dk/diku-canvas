#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha3"
open Lowlevel

#load "../canvasNT.fsx"
module CNT = CanvasNT

type state = int // a counter

let pen = makePen green 1.0
let lst = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let wMin, hMin = List.unzip lst |> fun (a,b) -> List.min a, List.min b
let wc, hc = List.unzip lst |> fun (a,b) -> 1.0+wMin+List.max a, 1.0+hMin+List.max b
let drawCurve _ dc =
    dc |>
    CNT.curveDC pen lst
    |> CNT.scaleDC 2.0 0.5
    |> CNT.translateDC 10.0 3.0

let txt = "99"
let font = CNT.createFont "Microsoft Sans Serif" 36.0
let w,h = CNT.measureText font txt
let drawTxT i dc = 
    let str = sprintf "%2d" i // Seems to remove initial spaces
    CNT.textDC white font str (0.0,0.0) dc

let outlinePen = makePen blue 1.0
let drawCurvePicture i dc =
    let cBox = CNT.Leaf(CNT.rectangleDC outlinePen (0.0, 0.0) (wc,hc), int wc, int hc)
    let c = CNT.Leaf(CNT.curveDC pen lst,int wc,int hc)
    let tBox = CNT.Leaf(CNT.rectangleDC outlinePen (0.0, 0.0) (w,h), int w, int h)
    let t = CNT.Leaf(CNT.textDC white font (string i) (0.0,0.0),int w,int h)
    let h = CNT.horizontal (CNT.ontop tBox t) CNT.Bottom (CNT.ontop cBox c)
    dc |> CNT.drawDC h

let mkDraw n = 
    match n with
        | "text" ->
            w, h, drawTxT
        | "curve" ->
            100, 100, drawCurve
        | "picture" ->
            256, 256, drawCurvePicture
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
