// Replace this path with wherever img_util.dll is located
// Default path after `dotnet build` is the path below
#r "../bin/Debug/net6.0/img_util.dll"

open ImgUtil

let rec pow2 x = if x <= 1 then x else 2*pow2(x-1)

let setBoxF c (x1,y1) (x2,y2) =
  setBox c (int(x1),int(y1)) (int(x2),int(y2))

let rec tri bm T (w,h) (x,y) =
  if w <= T then setBoxF red (x,y) (x+w,y+h) bm
  else let (w_half,h_half) = (w/2.0, h/2.0)
       do tri bm T (w_half,h_half) (x+w_half/2.0,y)
       do tri bm T (w_half,h_half) (x,y+h_half)
       do tri bm T (w_half,h_half) (x+w_half,y+h_half)

type state = int

let margin = 30.0

let draw w h (s:state) =
  let bm = ImgUtil.mk w h
  let T = float(512 * pow2 s / w)
  let (w,h) = (float(w)-2.0*margin,
               float(h)-2.0*margin)
  let () = tri bm T (w,h) (margin,margin)
  in bm

let react (s:state) (k:ImgUtil.key) : state option =
    match getKey k with
        | Space ->
            printfn "Pressed space!"
            None
        | LeftArrow ->
            printfn "Pressed left-arrow!"
            None
        | RightArrow ->
            printfn "Pressed right-arrow!"
            None
        | DownArrow -> Some (min 10 (s+1))
        | UpArrow -> Some (max 2 (s-1))
        | _ -> None

let init = 5

do runApp "Sierp" 600 600 draw react init
