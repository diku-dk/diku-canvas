#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha2"
open Lowlevel

#load "../canvasNT.fsx"
open CanvasNT

type state = int // a counter

let next x =
    x+1

let mkDraw n = 
    match n with
        | "text" ->
            let txt = "Hello World"
            let font = createFont "Microsoft Sans Serif" 36.0
            let w,h = measureText font txt
            let draw _ dc = textDC Color.White font "Hello World"
            w,h,(draw 0)
        | "curve" ->
            let pen = new Pen(Color.White, 1f)
            let draw _ dc = curveDC pen [(10.0,10.0); (20.0, 25.0); (10.0, 25.0); (10.0, 10.0)]
            50,30,(draw 0)
        | "combine" ->
            let txt = "Hello World"
            let font = createFont "Microsoft Sans Serif" 36.0
            let w,h = measureText font txt
            let drawTxt _ dc = printfn "Txt"; textDC Color.White font "Hello World"
            let pen = new Pen(Color.White, 1f)
            let drawCurve _ dc = printfn "Curve"; curveDC pen [(10.0,10.0); (20.0, 25.0); (10.0, 25.0); (10.0, 10.0)]
            (max 50 w),(max 30 h), drawCurve 0 >> drawTxt 0 
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
        runAppWithTimer "Text Test" (int w) (int h) (Some 0) draw react 0
        0
[<EntryPoint>]
main fsi.CommandLineArgs