#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha3"

open Lowlevel

type state = int

let WIDTH = 300

let defaultFont : Font =
    let fam = getFamily "Arial"
    let font = makeFont fam 20.0f
    font

let pen = makePen green 5.0
let drawCurve dx dy =
    let dx, dy = float dx, float dy
    drawLines pen [(10.0+dx,10.0+dy); (60.0+dx, 80.0+dy); (10.0+dx, 80.0+dy); (10.0+dx, 10.0+dy)]



let next d = (d % 100) + 1

let draw d dc =
    let mid = float32 (WIDTH / 2)
    let color = white
    let text = sprintf "F# is %d%% fun" d
    dc
    |> fillBox red {x = mid + 30.0f ; y = mid - 15.0f; width = 60.0f; height = 60.0f}
    |> drawCurve (mid + 30.0f) (mid - 15.0f)
    |> drawText text defaultFont color mid mid
    |> fillBox blue {x = mid ; y = mid; width = 2.0f; height = 2.0f}

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

let main =
    runAppWithTimer "Timer Test" WIDTH WIDTH (Some 125) draw react 0
