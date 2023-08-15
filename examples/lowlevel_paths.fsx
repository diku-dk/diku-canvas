//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"

module Lowlevel_path =
    open Lowlevel

    type state = int

    let defaultFont : Font =
        let fam = getFamily "Arial"
        let font = makeFont fam 36.0f
        font


    let next d = (d % 100) + 1

    let draw d dc =
        let color = white
        let tri = Lines [(0.0, 0.0); (100.0, 100.0); 0.0, 100.0; 0.0, 0.0] |> Prim
        let text = Text ("Hello"+(string d), TextOptions(defaultFont,
                                              Origin = System.Numerics.Vector2(0.0f, 0.0f) ))

        dc
        |> fillBox red {x = 0.0f ; y = 0.0f; width = 60.0f; height = 60.0f}
        |> drawPathTree (solidPen white 2.0) (tri <+> text <+> rotateDegreeAround 90.0 (0.0, 30.0) text)

    let react (s:state) (ev:Lowlevel.Event) : state option =
        match ev with
            | Event.TimerTick ->
                s |> next |> Some
            | _ -> None

    let main =
        runAppWithTimer "Timer Test" 200 200 (Some 1000) draw react 0
