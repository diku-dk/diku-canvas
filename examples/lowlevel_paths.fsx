#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"

module Lowlevel_path =
    open Lowlevel

    type state = int

    let next d = (d % 100) + 1

    let draw d dc =
        let color = white
        let p = Lines [(0.0, 0.0); (100.0, 100.0); 0.0, 100.0; 0.0, 0.0] |> Prim
        dc
        |> drawPathTree (solidPen white 2.0) p

    let react (s:state) (ev:Lowlevel.Event) : state option =
        match ev with
            | Event.TimerTick ->
                s |> next |> Some
            | _ -> None

    let main =
        runAppWithTimer "Timer Test" 200 200 None draw react 0
