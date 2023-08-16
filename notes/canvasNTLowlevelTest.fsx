#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha3"
open Lowlevel

#load "../canvasNT.fsx"
open CanvasNT // overwriting Kens Canvas functions!

type state = int // a counter

let pen = makePen green 1.0
let lst = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let wMin, hMin = List.unzip lst |> fun (a,b) -> List.min a, List.min b
let wc, hc = List.unzip lst |> fun (a,b) -> 1.0+wMin+List.max a, 1.0+hMin+List.max b
let makeCurveDC =
    wc, hc, (fun (s:state) (dc:drawing_context) -> 
        dc |> _curve pen lst)
//        dc |> _curve pen lst
//           |> scale 0.9 0.5
//           |> translate 1.0 2.0)
let makeCurve = 
    let w,h,ctx = makeCurveDC
    //w,h,(fun (i:state) (dc:drawing_context) -> 
    //    explain (curve pen lst) dc)
    w,h,fun (i:state) -> explain (curve pen lst)

let txt = "99"
let font = createFont "Microsoft Sans Serif" 36.0
let w,h = measureText font txt
let makeTxtDC =
    w,h,(fun (i:state) (dc:drawing_context) -> 
        dc |>_text white font (string i) (0.0,0.0))
let makeTxt = 
    w,h,fun (i:state) -> explain (text white font (string i) (0.0,0.0))

let makeOntopDC =
    let w1,h1,ctx1 = makeCurveDC
    let w2,h2,ctx2 = makeTxtDC
    (max w1 w2), (max h1 h2), (fun (i:state) (dc:drawing_context) -> 
        dc |> ctx1 i |> ctx2 i)
let makeOntop = 
    let w1,h1,ctx1 = makeCurveDC
    let w2,h2,ctx2 = makeTxtDC
    let w,h = (max w1 w2), (max h1 h2)
    w, h, (fun (i:state) (dc:drawing_context) ->
        explain (OnTop(Leaf(ctx1 i,int w1,int h1),Leaf(ctx2 i,int w2,int h2),int w,int h)) dc)

let makeHorizontal =
    let w1,h1,ctx1 = makeCurveDC
    let w2,h2,ctx2 = makeTxtDC
    let w,h = w1+w2, (max h1 h2)
    w, h, (fun (i:state) (dc:drawing_context) ->
        explain (Horizontal(Leaf(ctx1 i,int w1,int h1),Leaf(ctx2 i,int w2,int h2),int w,int h)) dc)
let makeVertical =
    let w1,h1,ctx1 = makeCurveDC
    let w2,h2,ctx2 = makeTxtDC
    let w,h = (max w1 w2), h1+h2
    w, h, (fun (i:state) (dc:drawing_context) ->
        explain (Vertical(Leaf(ctx1 i,int w1,int h1),Leaf(ctx2 i,int w2,int h2),int w,int h)) dc)

let mkDraw n = 
    match n with
        | "textdc" ->     makeTxtDC
        | "text" ->       makeTxt
        | "curvedc" ->    makeCurveDC
        | "curve" ->      makeCurve
        | "ontopdc" ->    makeOntopDC
        | "ontop" ->      makeOntop
        | "horizontal" -> makeHorizontal
        | "vertical" ->   makeVertical
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
