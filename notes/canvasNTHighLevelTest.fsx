#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha3"
open Lowlevel

#load "../canvasNT.fsx"
open CanvasNT // overwriting Kens Canvas functions!

type state = int // a counter

let pen = makePen green 1.0
let lst = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let makeCurve _ = curve pen lst

let font = createFont "Microsoft Sans Serif" 36.0
let makeTxt i = text white font (string i) (0.0,0.0)

let makeTranslate i = 
    text white font (string i) (0.0,0.0) 
    |> translate (float i) 0.0

let makeOntop i = ontop (makeCurve i) (makeTxt i)

let makeAlignH p i = alignh (makeTxt i) p (makeCurve i)

let makeAlignV p i = alignv (makeCurve i) p (makeTxt i)

let make4 i =
    let p = 
        text white font (string i) (0.0,0.0) 
    let q = alignv p Top p
    alignh q Top q

let mkDraw n = 
    match n with
        | "text" -> makeTxt
        | "curve" -> makeCurve
        | "translate" -> makeTranslate
        | "ontop" -> makeOntop
        | "4" -> make4
        | "alignht" -> makeAlignH 0.0
        | "alignhc" -> makeAlignH 0.5
        | "alignhb" -> makeAlignH 1.0
        | "alignvl" -> makeAlignV 0.0
        | "alignvc" -> makeAlignV 0.5
        | "alignvr" -> makeAlignV 1.0
        | _ -> failwith "Unkown test"

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick -> Some (10+(s-10+1)%30)
        | _ -> None

let main args =
    if Array.length args < 2 then
        1
    else
        let pict = mkDraw args[1]
        let draw = fun i -> printfn "%s" (tostring (pict i)); explain (pict i)
        runAppWithTimer "Text Test" 256 256 (Some 1000) draw react 10
        0

[<EntryPoint>]
main fsi.CommandLineArgs
