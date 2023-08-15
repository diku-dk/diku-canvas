//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Lowlevel

#load "../canvasNT3.fsx"
open CanvasNT3 // overwriting Kens Canvas functions!

type state = int
let lst = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let a = piecewiseaffine green 1.0 lst 
let b = alignh a Center (scale 1.2 0.7 a)
let c = alignv b Left (ellipse blue 2.0 20.0 25.0)
let w,h = getSize c
let d = rotate (w/2.0) (h/2.0) (20.0*System.Math.PI/180.0) c
printfn "%s" (tostring d)

let draw _: drawing_fun = 
    explain d

let react _ (ev:Lowlevel.Event) : state option =
    None

runAppWithTimer "CanvasNT2 Test" 256 256 None draw react 0
