// Replace this path with wherever img_util.dll is located
// Default path after `dotnet build` is the path below
#r "../bin/Debug/net6.0/img_util.dll"

open ImgUtil

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
  let bm = ImgUtil.mk w h
  let c = getPalette s
  setFillBox c (0,0) (w,h) bm
  bm

let react (s:state) (k:ImgUtil.key) : state option =
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
