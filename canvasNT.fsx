#r "nuget:SixLabors.ImageSharp"
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
#r "nuget:DIKU.Canvas"
open Canvas

/////////////////////////////////
// Jon's work on the interface //
/////////////////////////////////

type HPosition = Top | Center | Bottom 
type VPosition = Left | Center | Right 
type Draw = int -> int -> int -> int -> unit // A function that draws a shape
type Picture = 
  Empty of int*int
  | Leaf of Draw*int*int
  | Horizontal of Picture*Picture*int*int
  | Vertical of Picture*Picture*int*int
  | OnTop of Picture*Picture*int*int
let getSize (p:Picture): int*int =
  match p with
    Empty(w, h)
    | Leaf(_,w,h)
    | Horizontal(_,_,w,h)
    | Vertical(_,_,w,h)
    | OnTop(_,_,w,h) -> w,h
let rec horizontal (pic1:Picture) (pos:HPosition) (pic2:Picture): Picture =
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = w1+w2, (max h1 h2)
  let dh = abs (h1-h2)
  let full1 = Empty(w1,dh)
  let half1 = Empty(w1,dh/2)
  let full2 = Empty(w2,dh)
  let half2 = Empty(w2,dh/2)
  match pos, h1, h2 with
    | Top, h1, h2 when h1 > h2 -> Horizontal(pic1, vertical pic2 Left full2, w, h)
    | Top, h1, h2 when h1 = h2 -> Horizontal(pic1, pic2, w, h)
    | Top, _, _ -> Horizontal(vertical pic1 Left full1, pic2, w, h)
    | HPosition.Center, h1, h2 when h1 > h2 -> horizontal pic1 Top (vertical half2 Left pic2)
    | HPosition.Center, h1, h2 when h1 = h2 -> Horizontal(pic1, pic2, w, h)
    | HPosition.Center, _, _ -> horizontal (vertical half1 Left pic1) Top pic2
    | Bottom, h1, h2 when h1 > h2 -> Horizontal(pic1, vertical full2 Left pic2, w, h)
    | Bottom, h1, h2 when h1 = h2 -> Horizontal(pic1, pic2, w, h)
    | Bottom, _, _ -> Horizontal(vertical full1 Left pic1, pic2, w, h)
and vertical (pic1:Picture) (pos:VPosition) (pic2:Picture): Picture =
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = (max w1 w2), h1 + h2
  let dw = abs (w1-w2)
  let full1 = Empty(dw,h1)
  let half1 = Empty(dw/2,h1)
  let full2 = Empty(dw,h2)
  let half2 = Empty(dw/2,h2)
  match pos, w1, w2 with
    | Left, w1, w2 when w1 > w2 -> Vertical(pic1, horizontal pic2 Top full2, w, h)
    | Left, w1, w2 when w1 = w2 -> Vertical(pic1, pic2, w, h)
    | Left, _, _ -> Vertical(horizontal pic1 Top full1, pic2, w, h)
    | VPosition.Center, w1, w2 when w1 > w2 -> vertical pic1 Left (horizontal half2 Top pic2)
    | VPosition.Center, w1, w2 when w1 = w2 -> Vertical(pic1, pic2, w, h)
    | VPosition.Center, _, _ -> vertical (horizontal half1 Top pic1) Left pic2 
    | Right, w1, w2 when w1 > w2 -> Vertical(pic1, horizontal full2 Top pic2, w, h)
    | Right, w1, w2 when w1 = w2 -> Vertical(pic1, pic2, w, h)
    | Right, _, _ -> Vertical(horizontal full1 Top pic1, pic2, w, h)
let round (a:float): int = int (a+0.5)
let ontop (pic1:Picture) (a:float) (b:float) (pic2:Picture): Picture =
  if a < 0 || a > 1 || b < 0 || b > 1 then 
    raise (System.ArgumentOutOfRangeException ("a and b must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let s = float (w1-w2)
  let w, h = (max w1 w2), (max h1 h2)
  let pic3, pic4 = 
    match s, int (a*s) with
      | s, 0 when s > 0.0 -> pic1, (horizontal pic2 Top (Empty(w1-w2,h2)))
      | s, dw when s > 0.0 && dw = w1-w2 -> pic1, horizontal (Empty(dw, h2)) Top pic2
      | s, dw when s > 0.0 -> pic1, (horizontal (horizontal (Empty(dw, h2)) Top pic2) Top (Empty(w1-dw-w2,h2)))
      | s, 0 when s < 0.0 -> (horizontal pic1 Top (Empty(w2-w1,h1))), pic2
      | s, dw when s < 0.0 && dw = w1-w2 -> horizontal (Empty(-dw, h1)) Top pic1, pic2
      | s, dw when s < 0.0 -> (horizontal (horizontal (Empty(-dw, h1)) Top pic1) Top (Empty(w2+dw-w1,h1))), pic2
      | _ -> pic1, pic2
  let _,h3 = getSize pic3
  let _,h4 = getSize pic4
  let t = float (h3-h4)
  let pic5, pic6 = 
    match t, int (b*t) with
      | t, 0 when t > 0.0 -> pic3, (vertical pic4 Left (Empty(w, h3-h4)))
      | t, dh when t > 0.0 && dh = h3-h4 -> pic3, vertical (Empty(w, dh)) Left pic4
      | t, dh when t > 0.0 -> pic3, (vertical (vertical (Empty(w, dh)) Left pic4) Left (Empty(w, h3-dh-h4)))
      | t, 0 when t < 0.0 -> (vertical pic3 Left (Empty(w, h4-h3))), pic4
      | t, dh when t < 0.0 && dh = h3-h4 -> vertical (Empty(w,-dh)) Left pic3, pic4
      | t, dh when t < 0.0 -> (vertical (vertical (Empty(w,-dh)) Left pic3) Left (Empty(w, h4+dh-h3))), pic4
      | _ -> pic3, pic4
  OnTop(pic5, pic6, w, h)

let draw (pic:Picture) (title:string) : unit =
  let rec drawPart (c:canvas) ((x,y): int*int) (pic:Picture) : unit =
    match pic with
      | Empty(a,b) -> setFillBox c lightgrey (x+1,y+1) (x+a-2,y+b-2); ()
      | Leaf(f, a, b) -> setFillBox c red (x+1,y+1) (x+a-2,y+b-2) //f c x y; (a, b)
      | Horizontal(p1, p2, a, b) -> setFillBox c black (x+1,y+1) (x+a-2,y+b-2); drawPart c (x,y) p1; let w,_ = getSize p1 in drawPart c (x+w,y) p2
      | Vertical(p1, p2, a, b) -> setFillBox c black (x+1,y+1) (x+a-2,y+b-2); drawPart c (x,y) p1; let _,h = getSize p1 in drawPart c (x,y+h) p2
      | OnTop(p1, p2, a, b) -> setFillBox c black (x+1,y+1) (x+a-2,y+b-2); drawPart c (x,y) p1; drawPart c (x,y) p2
  let w,h = getSize pic
  let c = create w h
  drawPart c (0,0) pic |> ignore
  show c title

let rec sharpDraw (pic:Picture) : Image<Rgba32> =
  match pic with
    | Empty(a,b) -> 
        Image<Rgba32>(a,b,Color.LightGray)
    | Leaf(f, a, b) ->
        Image<Rgba32>(a,b,Color.Red) //f c x y; (a, b)
    | Horizontal(p1, p2, a, b) ->
        let w1,_ = getSize p1
        let I = Image<Rgba32>(a,b)
        let left = sharpDraw p1
        let right = sharpDraw p2
        I.Mutate(fun o -> 
          o.DrawImage(left, Point(0, 0), 1f);
          o.DrawImage(right, Point(w1, 0), 1f) |> ignore)
        I
    | Vertical(p1, p2, a, b) ->
        let _,h1 = getSize p1
        let I = Image<Rgba32>(a,b)
        let top = sharpDraw p1
        let bottom = sharpDraw p2
        I.Mutate(fun o -> 
          o.DrawImage(top, Point(0, 0), 1f);
          o.DrawImage(bottom, Point(0, h1), 1f) |> ignore)
        I
    | OnTop(p1, p2, a, b) ->
        let I = Image<Rgba32>(a,b)
        let lower = sharpDraw p1
        let upper = sharpDraw p2
        I.Mutate(fun o -> 
          o.DrawImage(top, Point(0, 0), 1f);
          o.DrawImage(bottom, Point(0, 0), 1f) |> ignore)
        I

let p = Empty(30,50)
printfn "\nAn empty box:\n %A" p
//draw p "p"
(sharpDraw p).Save("p.jpg")
let q = Leaf((fun x y w h -> ()),50,30)
printfn "\nA full box: %A" q
//draw q "q"
(sharpDraw q).Save("q.jpg")

let r = horizontal p Top q
let s = vertical p VPosition.Center q
let t = horizontal r Bottom s;
(sharpDraw t).Save("nonTrivial.jpg")

(*
let hPos = [Top; HPosition.Center; Bottom];
let lstHorisontal0 = List.map (fun pos -> horizontal p pos p) hPos
printfn "\nHorizontally empty-empty: %A" (List.zip hPos lstHorisontal0)
List.iter (fun (v,p) -> draw p (string v)) (List.zip hPos lstHorisontal0)
let lstHorisontal1 = List.map (fun pos -> horizontal p pos q) hPos
printfn "\nHorizontally empty-full: %A" (List.zip hPos lstHorisontal1)
List.iter (fun (v,p) -> draw p (string v)) (List.zip hPos lstHorisontal1)
let lstHorisontal2 = List.map (fun pos -> horizontal q pos p) hPos
printfn "\nHorizontally full-empty: %A" (List.zip hPos lstHorisontal2)
List.iter (fun (v,p) -> draw p (string v)) (List.zip hPos lstHorisontal2)

let vPos = [Left; VPosition.Center; Right];
let lstVertical0 = List.map (fun pos -> vertical p pos p) vPos
printfn "\nVertically empty-empty: %A" (List.zip vPos lstVertical0)
List.iter (fun (v,p) -> draw p (string v)) (List.zip hPos lstVertical0)
let lstVertical1 = List.map (fun pos -> vertical p pos q) vPos
printfn "\nVertically empty-full: %A" (List.zip vPos lstVertical1)
List.iter (fun (v,p) -> draw p (string v)) (List.zip hPos lstVertical1)
let lstVertical2 = List.map (fun pos -> vertical q pos p) vPos
printfn "\nVertically full-empty: %A" (List.zip vPos lstVertical2)
List.iter (fun (v,p) -> draw p (string v)) (List.zip hPos lstVertical2)

let tPos = [0.0; 0.5; 1.0];
let lstOnTop0 = List.map (fun a -> List.map (fun b -> ontop p a b p) tPos) tPos
printfn "\nOnTop empty-empty: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop0))
List.iter (fun lst -> List.iter (fun p -> draw p "p") lst) lstOnTop0
let lstOnTop1 = List.map (fun a -> List.map (fun b -> ontop p a b q) tPos) tPos
printfn "\nOnTop empty-full: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop1))
List.iter (fun lst -> List.iter (fun p -> draw p "p") lst) lstOnTop1
let lstOnTop2 = List.map (fun a -> List.map (fun b -> ontop q a b p) tPos) tPos
printfn "\nOnTop full-empty: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop2))
List.iter (fun lst -> List.iter (fun p -> draw p "p") lst) lstOnTop2

let r = horizontal p Top q
draw r "non-trivial tree"
let s = vertical p VPosition.Center q
draw s "non-trivial tree"
let t = horizontal r Bottom s;
List.iter (fun s -> printfn "%A" (getSize s)) [r;s]
printfn "horizontal %A Bottom %A = %A" r s t
draw t "non-trivial tree"
*)
/////////////////////////////////