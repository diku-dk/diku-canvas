//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Lowlevel

#load "../canvasNT3.fsx"
open CanvasNT3 // overwriting Kens Canvas functions!

type state = int
let N = 360
let next d = (d % N) + 1

let w,h = 256,256
let lst = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let tri1 = piecewiseaffine green 1.0 lst 
let tri2 = scale 1.2 0.7 tri1
let ell = ellipse blue 4.0 20.0 25.0
let bck = filledrectangle black w h
let fig = alignv (alignh tri1 Center tri2) Left ell
let fw,fh = getSize fig

let draw i: drawing_fun = 
    let d = ontop (rotate (float fw/2.0) (float fh/2.0) ((float i)/(float (N-1))*2.0*System.Math.PI) fig) bck
    printfn "%s" (tostring d)
    explain d

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

drawToFile w h "canvasNT3Test.png" (draw 0)
[0..(N-1)] |> List.map draw |> drawToAnimatedGif w h 100 5 "canvasNT3Test.gif"
runAppWithTimer "CanvasNT2 Test" w h (Some 100) draw react 0
//runApp "CanvasNT2 Test" 256 256 draw
