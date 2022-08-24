// Replace this path with wherever img_util.dll is located
// Default path after `dotnet build` is the path below
#r "../bin/Debug/net6.0/img_util.dll"

open ImgUtil

type state =
    | RedStart
    | GreenStart
    | BlueStart
    | YellowStart

let cycleState = function
    | RedStart -> GreenStart
    | GreenStart -> BlueStart
    | BlueStart -> YellowStart
    | YellowStart -> RedStart

let cycleStateBackwards = function
    | RedStart -> YellowStart
    | GreenStart -> RedStart
    | BlueStart -> GreenStart
    | YellowStart -> BlueStart

let getPalette (c:state) : (color * color * color * color) =
    match c with
        | RedStart -> (red,green,blue,yellow)
        | GreenStart -> (green,blue,yellow,red)
        | BlueStart -> (blue,yellow,red,green)
        | YellowStart -> (yellow,red,green,blue)

let draw w h (s:state) =
  let bm = ImgUtil.mk w h
  let half = w / 2
  let (c1,c2,c3,c4) = getPalette s
  setFillBox c1 (0, 0) (half-1,half-1) bm
  setFillBox c2 (0,half-1) (half-1,h) bm
  setFillBox c3 (half,half) (w,h) bm
  setFillBox c4 (half,0) (w,half-1) bm  
  bm

let react (s:state) (k:ImgUtil.key) : state option =
    match getKey k with
        | LeftArrow ->
            printfn "Pressed left-arrow!"
            cycleStateBackwards s |> Some

        | RightArrow ->
            printfn "Pressed right-arrow!"
            cycleState s |> Some
        | _ -> None

do runApp "ColorBoxes" 600 600 draw react RedStart
