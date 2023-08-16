#r "nuget:DIKU.Canvas"

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
  let canvas = create w h
  let c = getPalette s
  setFillBox canvas c (0,0) (w,h)
  canvas

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
