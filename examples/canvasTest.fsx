//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Canvas

type state = int
let N = 360
let next d = (d % N) + 1

let w,h = 256,256
let bck = filledrectangle color.Black w h
let lst1 = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let lst2 = [(0.0,0.0); (40.0, 30.0); (0.0, 30.0); (0.0, 0.0)]
let tri1 = piecewiseaffine color.Green 1.0 lst1
let tri2 = filledpolygon color.Purple lst2
let ell = ellipse color.Blue 4.0 20.0 25.0
let fig = translate 50.0 50.0 (alignv (alignh tri1 Center tri2) Center ell)
let fw,fh = getSize <| getRectangle fig

let draw (i:state): Picture = 
    let d = ontop bck (rotate (float fw/2.0) (float fh/2.0) ((float i)/(float (N-1))*2.0*System.Math.PI) fig)
    printfn "%s" (tostring d)
    make d

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

//drawToFile w h "canvasNT3Test.png" (draw 0)
//[0..(N-1)] |> List.map draw |> drawToAnimatedGif w h 100 5 "canvasNT3Test.gif"
runAppWithTimer "CanvasNT2 Test" w h (Some 100) draw react 0
//runApp "CanvasNT2 Test" 256 256 (fun () -> draw 0)
