#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha3"
open Lowlevel

#load "../canvasNT.fsx"
module CNT = CanvasNT

type state = int // a counter

let next x =
    x+1


let pen = makePen green 5.0

let drawCurve _ dc =
    printfn "Curve";
    drawLines pen [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)] dc


let mkDraw n = 
    match n with
        | "text" ->
            let txt = "Hello World"
            let font = CNT.createFont "Microsoft Sans Serif" 36.0
            let w,h = CNT.measureText font txt
            let draw _ dc = CNT.textDC white font "Hello World"
            w,h,(draw 0)
        | "curve" ->
            100, 100, drawCurve
        | "combine" ->
            let txt = "Hello World"
            let font = CNT.createFont "Microsoft Sans Serif" 36.0
            let w,h = CNT.measureText font txt
            let drawTxt _ = printfn "Txt"; drawText "Hello World" font white 0f 0f
            (max 100 w),(max 100 h), fun state dc ->
                                         dc |> drawCurve state |> drawTxt state

        | _ -> failwith "Unkown test"

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

let main args =
    if Array.length args < 2 then
        1
    else
        let w, h, draw = mkDraw args[1]
        runAppWithTimer "Text Test" (int w) (int h) None draw react 0
        0

[<EntryPoint>]
main fsi.CommandLineArgs
