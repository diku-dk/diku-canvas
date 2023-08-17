//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"

open Lowlevel

type state = int

let defaultFont : Font =
    let fam = getFamily "Arial"
    let font = makeFont fam 36f
    font

let next d = (d % 100) + 1

let rectangle (width, height) =
    Lines [0.0, 0.0; width, 0.0; width, height; 0.0, height; 0.0, 0.0]

let draw d dc =
    let whitePen = solidPen white 2.0
    let bluePen = solidPen blue 2.0
    let greenBrush = solidBrush green
    let redBrush = solidBrush red
    let yellowBrush = solidBrush yellow
    let tri = Prim (whitePen, Lines [(0.0, 0.0); (100.0, 100.0); 0.0, 100.0; 0.0, 0.0])
    let text = Text (bluePen, "Hello"+(string d), TextOptions(defaultFont,
                                              Origin = System.Numerics.Vector2(0.0f, 0.0f) ))
    let box = Prim (redBrush, rectangle(60.0,60.0))
    let cir = Prim (greenBrush, Arc((30.0, 30.0), 10.0, 10.0, 0.0, 0.0, 360.0))
    let pac = Prim (yellowBrush, Arc((150.0, 150.0), 40.0, 40.0, 0.0, 0.0, 270.0))
    // Would have like to write something like
    //let pac = Arc((150.0, 150.0), 40.0, 40.0, 0.0, 20.0, 340.0))
    //      <+> Line((150.0, 150.0), ...) <+> Line((150.0, 150.0), ...)

    dc
    |> drawPathTree (box <+> tri <+> text <+> rotateDegreeAround 90.0 (0.0, 30.0) text <+> cir <+> pac)

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

let main =
    runAppWithTimer "Timer Test" 200 200 (Some 1000) draw react 0
