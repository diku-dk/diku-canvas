
open System.Windows.Forms
open FunImg

let (<<>>) : float -> float -> bool =
  fun (x:float) y -> abs(x-y) < 0.05
let (<||>) : region -> region -> region =
  fun r1 r2 -> fun p -> r1 p || r2 p
let invY (f : 'a image) : 'a image =
  fun (x,y) -> f (x,-y)

let f : region =
  fun (x,y) -> y <<>> (x*x*x - 2.0*x*x + 1.5)

let coord : region =
  fun (x,y) -> x <<>> 0.0 || y <<>> 0.0

let circ : region =                          // $1 = x^2 + y^2$
  fun (x,y) -> 1.0 <<>> (x*x + y*y)

let ccirc =
  boolToFunColor << (coord <||> circ)

let img i =
  match i with
    | 0 -> fracToFunColor << wavDist
    | 1 -> rbRings
    | 2 -> mystique
    | 3 -> invY(boolToFunColor << f)
    | 4 -> boolToFunColor << coord
    | 5 -> boolToFunColor << circ
    | 6 -> ccirc
    | _ -> boolToFunColor << checker

let max x y = if x > y then x else y
let min x y = if x < y then x else y

do ImgUtil.runApp "Img" 600 480
           (fun w h (s,i) ->
              imgBitmap (float s) (img i) w h)
           (fun (s,i) e ->
              if e = Gdk.Key.u then Some (s+1,i)
              else if e = Gdk.Key.d then Some (max 0 (s-1),i)
              else if e = Gdk.Key.r then Some (s,min 7 (i+1))
              else if e = Gdk.Key.l then Some (s,max 0 (i-1))
              else None) (10,0)
