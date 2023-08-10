#r "nuget:SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.Fonts

/////////////////////////////////
// Jon's work on the interface //
/////////////////////////////////

/// Types
type Draw = unit -> Image<Rgba32> // A function that draws a shape

type Picture = 
  Leaf of Draw*int*int
  | Horizontal of Picture*Picture*int*int
  | Vertical of Picture*Picture*int*int
  | OnTop of Picture*Picture*int*int
  | AffineTransform of Picture*System.Numerics.Matrix3x2*Color*int*int
  | Scale of Picture*float*float*int*int
  | Rotate of Picture*float*float*float*int*int
  | Translate of Picture*float*float*int*int
  | Crop of Picture*int*int

/// Functions for combining images
let getSize (p:Picture): int*int =
  match p with
    Leaf(_,w,h)
    | Horizontal(_,_,w,h)
    | Vertical(_,_,w,h)
    | OnTop(_,_,w,h)
    | AffineTransform(_,_,_,w,h)
    | Scale(_,_,_,w,h)
    | Rotate(_,_,_,_,w,h)
    | Translate(_,_,_,w,h)
    | Crop(_,w,h) -> w,h

let Top = 0.0
let Left = 0.0
let Center = 0.5
let Bottom = 1.0
let Right = 1.0

let rec ontop (pic1:Picture) (a:float) (b:float) (pic2:Picture): Picture =
  if a < 0 || a > 1 || b < 0 || b > 1 then 
    raise (System.ArgumentOutOfRangeException ("a and b must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = (max w1 w2), (max h1 h2)
  OnTop(Crop(pic1, w, h), Crop(pic2, w, h), w, h)
and horizontal (pic1:Picture) (pos:float) (pic2:Picture): Picture =
  if pos < 0 || pos > 1 then 
    raise (System.ArgumentOutOfRangeException ("ppos must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = w1+w2, (max h1 h2)
  let s = float (h1-h2)
  let q1, q2 =
    if h1 > h2 then
      Translate(pic1, 0.0, 0.0, w, h), Translate(pic2, float w1, pos*s, w, h)
    else
      Translate(pic1, 0.0,-pos*s, w, h), Translate(pic2, float w1, 0.0, w, h)
  OnTop(q1, q2, w, h)
and vertical (pic1:Picture) (pos:float) (pic2:Picture): Picture =
  if pos < 0 || pos > 1 then 
    raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = max w1 w2, h1 + h2
  let s = float (w1-w2)
  let q1, q2 =
    if w1 > w2 then
      Translate(pic1, 0.0, 0.0, w, h), Translate(pic2, pos*s, float h1, w, h)
    else
      Translate(pic1, -pos*s, 0.0, w, h), Translate(pic2, 0.0, float h1, w, h)
  OnTop(q1, q2, w, h)

/// Drawing pictures
let rec sharpDraw (pic:Picture) : Image<Rgba32> =
  let mutateCrop (c: Color) (I: Image<Rgba32>) (w:int) (h:int): Image<Rgba32> = 
    I.Mutate(fun x -> x.Crop(min I.Width w, min I.Height h)
                       .Resize(ResizeOptions(Position = AnchorPositionMode.TopLeft,
                                             Size = Size(w, h), 
                                             Mode = ResizeMode.BoxPad))
                       .BackgroundColor(c)|>ignore)
    I
  let affinetransform (p:Picture) (M:System.Numerics.Matrix3x2) (c: Color) (w:int) (h:int): Image<Rgba32> =
    let I = sharpDraw p
    let transformation = AffineTransformBuilder().AppendMatrix(M)
    I.Mutate(fun x -> x.Transform(transformation)|>ignore)
    mutateCrop c I w h
  match pic with
    | Leaf(f, a, b) ->
      f()
    | Horizontal(p1, p2, a, b) ->
      let w1,_ = getSize p1
      let I = new Image<Rgba32>(a,b)
      let left = sharpDraw p1
      let right = sharpDraw p2
      I.Mutate(fun x -> 
        x.DrawImage(left, Point(0, 0), 1f) |> ignore
        x.DrawImage(right, Point(w1, 0), 1f) |> ignore)
      I
    | Vertical(p1, p2, a, b) ->
      let _,h1 = getSize p1
      let I = new Image<Rgba32>(a,b)
      let top = sharpDraw p1
      let bottom = sharpDraw p2
      I.Mutate(fun x -> 
        x.DrawImage(top, Point(0, 0), 1f) |> ignore
        x.DrawImage(bottom, Point(0, h1), 1f) |> ignore)
      I
    | OnTop(p1, p2, a, b) ->
      let I = new Image<Rgba32>(a,b)
      let lower = sharpDraw p1
      let upper = sharpDraw p2
      I.Mutate(fun x -> 
        x.DrawImage(lower, Point(0, 0), 1f) |> ignore
        x.DrawImage(upper, Point(0, 0), 1f) |> ignore)
      I
    | AffineTransform(p,M,c,w,h) ->
      affinetransform p M c w h
    | Scale(p,sx,sy,w,h) -> 
      let M = Matrix3x2Extensions.CreateScale(SizeF(float32 sx, float32 sy))
      affinetransform p M Color.Transparent w h
    | Rotate(p,cx,cy,rad,w,h) ->
      let M = Matrix3x2Extensions.CreateRotation(float32 rad, PointF(float32 cx, float32 cy))
      affinetransform p M Color.Transparent w h
    | Translate(p,dx,dy,w,h) ->
      let M = Matrix3x2Extensions.CreateTranslation(PointF(float32 dx, float32 dy))
      affinetransform p M Color.Transparent w h
    | Crop(p,w,h) ->
      let I = sharpDraw p
      if (w,h) = (getSize p) then I else mutateCrop Color.Transparent I w h

/// Graphics primitives
let box (c:Color) (w:int) (h:int): Picture =
  Leaf((fun () -> new Image<Rgba32>(w,h,c)),w,h)
let transparent = box Color.Transparent
let pointsOfList (lst: (float*float) list) = 
  lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
let curve (w: int) (h:int) (p: Pen) (lst: (float*float) list) : Picture = 
  Leaf((fun () -> 
    let I = new Image<Rgba32>(w,h,Color.Transparent)
    let points = lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
    I.Mutate(fun x -> x.DrawLines(p, points)|>ignore)
    I),w,h)
let filledPolygon (w: int) (h:int) (c: Color) (lst: (float*float) list) : Picture = 
  Leaf((fun () -> 
    let I = new Image<Rgba32>(w,h,Color.Transparent)
    let points = lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
    I.Mutate(fun x -> x.FillPolygon(DrawingOptions(), c, points)|>ignore)
    I),w,h)
let rectangle (w: int) (h:int) (p: Pen) ((x1,y1): (float*float)) ((x2,y2): (float*float)) : Picture = 
  Leaf((fun () -> 
    let I = new Image<Rgba32>(w,h,Color.Transparent)
    I.Mutate(fun x -> x.Draw(DrawingOptions(), p, RectangleF(float32 x1, float32 y1, float32 x2, float32 y2))|>ignore)
    I),w,h)
let filledRectangle (w: int) (h:int) (c: Color) ((x1,y1): (float*float)) ((x2,y2): (float*float)) : Picture = 
  Leaf((fun () -> 
    let I = new Image<Rgba32>(w,h,Color.Transparent)
    I.Mutate(fun x -> x.Fill(c, RectangleF(float32 x1, float32 y1, float32 x2, float32 y2))|>ignore)
    I),w,h)
let ellipsePoints ((cx,cy): (float*float)) ((rx,ry): (float*float)): (float*float) list =
  let n = int <| max 10.0 ((max rx ry)*10.0)
  List.map (fun i -> (cx+rx*cos i, cy+ry*sin i)) [for i in 0..(n-1) do yield 2.0*System.Math.PI*(float i)/(float (n-1))]
let ellipse (w: int) (h:int) (p: Pen) (c: (float*float)) (r: (float*float)) : Picture = 
  let lst = ellipsePoints c r
  curve w h p lst
let filledEllipse (w: int) (h:int) (col: Color) (c: float*float) (r: float*float) : Picture = 
  let lst = ellipsePoints c r
  filledPolygon w h col lst
let text (c: Color) (f: Font) (txt: string) : Picture = 
  let size = TextMeasurer.Measure(txt, TextOptions(f))
  let w,h = int size.Width, int size.Height
  Leaf((fun () -> 
    let I = new Image<Rgba32>(w,h,Color.Transparent)
    I.Mutate(fun x -> x.DrawText(txt, f, c, PointF(0f, 0f))|>ignore)
    I),w,h)

/// Tests
/// Testing creation of simple Picture
let p = box Color.LightGray 30 50
printfn "\nAn empty box:\n %A" p
(sharpDraw p).Save("p.jpg")
let q = box Color.Red 50 30
printfn "\nA full box: %A" q
(sharpDraw q).Save("q.jpg")

/// Testing Crop
let w,h = getSize p in
  let u = Crop(p, w-10, h-5) in 
    printfn "\nCrop p smaller: %A" u;
    (sharpDraw u).Save("cropSmall.jpg")
let w,h = getSize p in
  let u = Crop(p, w+10, h+5) in 
    printfn "\nCrop p larger: %A" u;
    (sharpDraw u).Save("cropLarge.jpg")

/// Testing Translate
let w,h = getSize p in
  let u = Translate(p, -10, -5, w,h) in 
    printfn "\nTranslate left-up keep size: %A" u;
    (sharpDraw u).Save("translatelus.jpg")
let w,h = getSize p in
  let u = Translate(p, 10, 5, w,h) in 
    printfn "\nTranslate right-down keep size: %A" u;
    (sharpDraw u).Save("translaterds.jpg")
let w,h = getSize p in
  let u = Translate(p, -10, -5, w+10,h+5) in 
    printfn "\nTranslate left-up bigger size: %A" u;
    (sharpDraw u).Save("translatelub.jpg")
let w,h = getSize p in
  let u = Translate(p, 10, 5, w+10,h+5) in 
    printfn "\nTranslate right-down bigger size: %A" u;
    (sharpDraw u).Save("translaterdb.jpg")

/// Testing creation of Picture tree
let r = horizontal p Top q
printfn "\nhorizontal p Top q: %A" r
(sharpDraw r).Save("r.jpg")
let s = vertical p Center q
printfn "\nvertical p Center q: %A" q
(sharpDraw s).Save("s.jpg")
let t = horizontal r Bottom s;
printfn "\nhorizontal r Bottom s: %A" t
(sharpDraw t).Save("nonTrivial.jpg")

/// Testing horizontal concatenation
let hPos = [Top; Center; Bottom];
let lstHorisontal0 = List.map (fun pos -> horizontal p pos p) hPos
printfn "\nHorizontally empty-empty: %A" (List.zip hPos lstHorisontal0)
List.iter (fun (v,p) -> (sharpDraw p).Save(sprintf "horizontal%g.jpg" v)) (List.zip hPos lstHorisontal0)
let lstHorisontal1 = List.map (fun pos -> horizontal p pos q) hPos
printfn "\nHorizontally empty-full: %A" (List.zip hPos lstHorisontal1)
List.iter (fun (v,p) -> (sharpDraw p).Save(sprintf "horizontal%g.jpg" v)) (List.zip hPos lstHorisontal1)
let lstHorisontal2 = List.map (fun pos -> horizontal q pos p) hPos
printfn "\nHorizontally full-empty: %A" (List.zip hPos lstHorisontal2)
List.iter (fun (v,p) -> (sharpDraw p).Save(sprintf "horizontal%g.jpg" v)) (List.zip hPos lstHorisontal2)

/// Testing vertical concatenation
let vPos = [Left; Center; Right];
let lstVertical0 = List.map (fun pos -> vertical p pos p) vPos
printfn "\nVertically empty-empty: %A" (List.zip vPos lstVertical0)
List.iter (fun (v,p) -> (sharpDraw p).Save(sprintf "vertical%g.jpg" v)) (List.zip hPos lstVertical0)
let lstVertical1 = List.map (fun pos -> vertical p pos q) vPos
printfn "\nVertically empty-full: %A" (List.zip vPos lstVertical1)
List.iter (fun (v,p) -> (sharpDraw p).Save(sprintf "vertical%g.jpg" v)) (List.zip hPos lstVertical1)
let lstVertical2 = List.map (fun pos -> vertical q pos p) vPos
printfn "\nVertically full-empty: %A" (List.zip vPos lstVertical2)
List.iter (fun (v,p) -> (sharpDraw p).Save(sprintf "vertical%g.jpg" v)) (List.zip hPos lstVertical2)

/// Testing stacking
let tPos = [0.0; 0.5; 1.0];
let lstOnTop0 = List.map (fun a -> List.map (fun b -> ontop p a b p) tPos) tPos
printfn "\nOnTop empty-empty: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop0))
List.iter (fun lst -> List.iter (fun p -> sharpDraw p|>ignore) lst) lstOnTop0
let lstOnTop1 = List.map (fun a -> List.map (fun b -> ontop p a b q) tPos) tPos
printfn "\nOnTop empty-full: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop1))
List.iter (fun lst -> List.iter (fun p -> sharpDraw p|>ignore) lst) lstOnTop1
let lstOnTop2 = List.map (fun a -> List.map (fun b -> ontop q a b p) tPos) tPos
printfn "\nOnTop full-empty: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop2))
List.iter (fun lst -> List.iter (fun p -> sharpDraw p|>ignore) lst) lstOnTop2

/// Testing rotation modification
let w,h = getSize t
let u = Rotate(t, 0, 0, 10.0*System.Math.PI/180.0, w, h)
printfn "\nRotate(t): %A" u
(sharpDraw u).Save("rotate.jpg")

/// Testing rotation and animated gifs
let gif = ontop (box Color.Black w h) 0.0 0.0 t |> sharpDraw
let frameDelay = 100
let gifMetaData = gif.Metadata.GetGifMetadata()
gifMetaData.RepeatCount <- 5us
let metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata()
metadata.FrameDelay <- frameDelay
for i in 1 .. 59 do
  let ti = Rotate(t, (float w)/2.0, (float h)/2.0, (float i)/60.0*2.0*System.Math.PI, w, h)
  let frame = ontop (box Color.Black w h) 0.0 0.0 ti |> sharpDraw
  let md = frame.Frames.RootFrame.Metadata.GetGifMetadata();
  md.FrameDelay <- frameDelay
  gif.Frames.AddFrame(frame.Frames.RootFrame) |> ignore
gif.SaveAsGif("animated.gif");

/// Testing content creations
let pen = new Pen(Color.White, 1f) in
  let p = curve 50 30 pen [(10.0,10.0); (20.0, 25.0); (10.0, 25.0); (10.0, 10.0)] in 
    (sharpDraw p).Save("curve.jpg")

let p = rectangle 50 30 pen (10.0,10.0) (30.0, 10.0) in 
  (sharpDraw p).Save("rectangle.jpg")

let p = rectangle 50 30 pen (10.0,10.0) (30.0, 10.0) in 
  (sharpDraw p).Save("rectangle.jpg")

let p = filledRectangle 50 30 Color.Red (10.0,10.0) (30.0, 10.0) in 
  (sharpDraw p).Save("filledRectangle.jpg")

let p = filledPolygon 50 30 Color.White [(10.0,10.0); (20.0, 25.0); (10.0, 25.0); (10.0, 10.0)] in 
  (sharpDraw p).Save("filledPolygon.jpg")

let p = ellipse 512 256 pen (256.0,128.0) (128.0,64.0) in 
  (sharpDraw p).Save("circle.jpg")

let p = filledEllipse 512 256 Color.White (256.0,128.0) (128.0,64.0) in 
  (sharpDraw p).Save("filledCircle.jpg")

let font = SystemFonts.CreateFont("Microsoft Sans Serif", 36f) in
  let p = text Color.White font "Hello World" in 
    (sharpDraw p).Save("text.jpg")
