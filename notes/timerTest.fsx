#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#r "nuget:DIKU.Canvas, 2.0.0-alpha6"

open Lowlevel

type state =
    | Red
    | Green
    | Blue
    | Yellow
    | Black
    | White

let getPalette = function
    | Red -> red
    | Green -> green
    | Blue -> blue
    | Yellow -> yellow
    | Black -> black
    | White -> white

let rectangle width height =
    Lines [0.0, 0.0; width, 0.0; width, height; 0.0, height; 0.0, 0.0]


let draw w h (s:state) ctx =
  let c = getPalette s
  let box = Prim (solidBrush c, rectangle w h)
  drawPathTree box ctx

let react (s:state) (ev:Event) : state option =
    match ev with
        | TimerTick ->
            printfn "Time flies by"
            Some (match s with Black -> White | _ -> Black)
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
        | _ ->
            printfn "Ignored ev: %A" ev
            None

let w, h = 600, 600

do runAppWithTimer "Timer Test" w h (Some 2000) (draw w h) react Red
