//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Debug"
#r "nuget:DIKU.Canvas, 1.0.2"

open Canvas

type state =
    | Red
    | Green
    | Blue
    | Yellow
    | Black

let getPalette = function
    | Red -> red
    | Green -> green
    | Blue -> blue
    | Yellow -> yellow
    | Black -> black

let draw w h (s:state) =
  let bm = Canvas.create w h
  let c = getPalette s
  setFillBox bm c (0,0) (w,h)
  bm

let react (s:state) (ev:Canvas.event) : state option =
    match ev with
        | TimerTick ->
            printfn "Time flies by"
            Some Black
        | KeyDown k ->
            match getKey k with
                | LeftArrow ->
                    printfn "Going red!"
                    Some Red
                | RightArrow ->
                    printfn "Going blue!"
                    Some Blue
                | DownArrow ->
                    printfn "Going green!"
                    Some Green
                | UpArrow ->
                    printfn "Going yellow!"
                    Some Yellow
                | _ -> None

do runAppWithTimer "Timer Test" 600 600 (Some 2000) draw react Red
