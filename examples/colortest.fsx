#r "nuget:DIKU.Canvas, 1.0.0-alpha3"

open Canvas

type state =
    | Red
    | Green
    | Blue
    | Yellow

let getPalette = function
    | Red -> red
    | Green -> green
    | Blue -> blue
    | Yellow -> yellow

let draw w h (s:state) =
  let bm = Canvas.create w h
  let c = getPalette s
  setFillBox bm c (0,0) (w,h)
  bm

let react (s:state) (k:Canvas.key) : state option =
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

do runApp "ColorTest" 600 600 draw react Red
