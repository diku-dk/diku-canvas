//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"

module Lowlevel_path =
    open Lowlevel

    type state = int

    let defaultFont : Font =
        let fam = getFamily "Arial"
        let font = makeFont fam 36f
        font


    let next d = (d % 100) + 1

    let draw d dc =
        let whitePen = solidPen green 2.0
        let bluePen = solidPen blue 2.0
        let redPen = solidBrush red
        let tri = Prim (whitePen, Lines [(0.0, 0.0); (100.0, 100.0); 0.0, 100.0; 0.0, 0.0])
        let text = Text (bluePen, "Hello"+(string d), TextOptions(defaultFont,
                                              Origin = System.Numerics.Vector2(0.0f, 0.0f) ))
        let box = Prim (redPen,Rectangle(60.0,60.0))

        dc
//        |> fillBox red {x = 0.0f ; y = 0.0f; width = 60.0f; height = 60.0f}
        |> drawPathTree (tri <+> text <+> rotateDegreeAround 90.0 (0.0, 30.0) text <+> box)

    let react (s:state) (ev:Lowlevel.Event) : state option =
        match ev with
            | Event.TimerTick ->
                s |> next |> Some
            | _ -> None

    let main =
        runAppWithTimer "Timer Test" 200 200 (Some 1000) draw react 0
