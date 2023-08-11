module CanvasNT
#r "nuget: SixLabors.ImageSharp"
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
type Color = SixLabors.ImageSharp.Color
type image = SixLabors.ImageSharp.Image<Rgba32>
type movie = image list
type Pen = SixLabors.ImageSharp.Drawing.Processing.Pen
type Font = SixLabors.Fonts.Font
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
type Size = float*float

/// Functions for combining images
let save (I:image) (fname:string): unit =
  I.Save(fname)

let saveAnimatedGif (frameDelay:int) (repeatCount:int) (movie:image list) (fname: string): unit =
  match movie with
    gif::rst ->
      let gifMetaData = gif.Metadata.GetGifMetadata()
      gifMetaData.RepeatCount <- uint16 5
      let metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata()
      metadata.FrameDelay <- frameDelay
      List.iter (fun (frame:image) -> 
          let metadata = frame.Frames.RootFrame.Metadata.GetGifMetadata()
          metadata.FrameDelay <- frameDelay
          gif.Frames.AddFrame(frame.Frames.RootFrame) |> ignore
        ) rst
      gif.SaveAsGif(fname);
    | _ -> ()

let systemFonts = SystemFonts.Families
let createFont (fname: string) (sz: float): Font = SystemFonts.CreateFont(fname, float32 sz)
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
let pointsOfList (lst: (float*float) list) = 
  lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
let ellipsePoints ((cx,cy): (float*float)) ((rx,ry): (float*float)): (float*float) list =
  let n = int <| max 10.0 ((max rx ry)*10.0)
  List.map (fun i -> (cx+rx*cos i, cy+ry*sin i)) [for i in 0..(n-1) do yield 2.0*System.Math.PI*(float i)/(float (n-1))]

let box (c:Color) (w:int) (h:int): Picture =
  Leaf((fun () -> new Image<Rgba32>(w,h,c)),w,h)
let transparent = box Color.Transparent
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

let affinetransformDC (M:System.Numerics.Matrix3x2) (c: Color) (ctx:drawing_context): drawing_context = 
  let transformation = AffineTransformBuilder().AppendMatrix(M)
  ctx.Transform(transformation)
let translateDC (dx:float) (dy:float) (ctx:drawing_context): drawing_context =
  let M = Matrix3x2Extensions.CreateTranslation(PointF(float32 dx, float32 dy))
  affinetransformDC M Color.Transparent ctx
let scaleDC (sx:float) (sy:float) (ctx:drawing_context): drawing_context =
  let M = Matrix3x2Extensions.CreateScale(SizeF(float32 sx, float32 sy))
  affinetransformDC M Color.Transparent ctx
let rotateDC (cx:float) (cy:float) (rad: float) (ctx:drawing_context): drawing_context =
  let M = Matrix3x2Extensions.CreateRotation(float32 rad, PointF(float32 cx, float32 cy))
  affinetransformDC M Color.Transparent ctx
let measureText (f: Font) (txt: string): Size = 
  let size = TextMeasurer.Measure(txt, TextOptions(f))
  (float size.Width, float size.Height)
let textDC (c: Color) (f: Font) (txt: string) (x:float, y:float) (ctx:drawing_context): drawing_context = 
  ctx.DrawText(txt, f, c, PointF(float32 x, float32 y))
let curveDC (p: Pen) (lst: (float*float) list) (ctx:drawing_context): drawing_context = 
  let points = lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
  ctx.DrawLines(p, points)
let filledPolygonDC (c: Color) (lst: (float*float) list) (ctx:drawing_context): drawing_context = 
  let points = lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
  ctx.FillPolygon(DrawingOptions(), c, points)
let rectangleDC (p: Pen) ((x1,y1): (float*float)) ((x2,y2): (float*float)) (ctx:drawing_context): drawing_context = 
  ctx.Draw(DrawingOptions(), p, RectangleF(float32 x1, float32 y1, float32 x2, float32 y2))
let filledRectangleDC (c: Color) ((x1,y1): (float*float)) ((x2,y2): (float*float)) (ctx:drawing_context): drawing_context = 
  ctx.Fill(c, RectangleF(float32 x1, float32 y1, float32 x2, float32 y2))
let ellipseDC (p: Pen) (c: (float*float)) (r: (float*float)) (ctx:drawing_context): drawing_context = 
  let lst = ellipsePoints c r
  curveDC p lst ctx
let filledEllipseDC (w: int) (h:int) (col: Color) (c: float*float) (r: float*float) (ctx:drawing_context): drawing_context = 
  let lst = ellipsePoints c r
  filledPolygonDC col lst ctx
let cropDC (c: Color) (w:int) (h:int) (ctx:drawing_context): drawing_context = 
  let sz = ctx.GetCurrentSize()
  ctx.Crop(min sz.Width w, min sz.Height h)
     .Resize(ResizeOptions(Position = AnchorPositionMode.TopLeft,
                           Size = Size(w, h), 
                           Mode = ResizeMode.BoxPad))
     .BackgroundColor(c)
