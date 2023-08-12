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
type Color = SixLabors.ImageSharp.Color
type image = SixLabors.ImageSharp.Image<Rgba32>
type movie = image list
type Pen = SixLabors.ImageSharp.Drawing.Processing.Pen
type Font = SixLabors.Fonts.Font
type Picture = 
  Leaf of drawing_fun*int*int
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
    | Horizontal(_,_,w,h) -> w,h
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

let round (x: float): int = int (x+0.5)
let rec ontop (pic1:Picture) (pic2:Picture): Picture =
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = (max w1 w2), (max h1 h2)
  OnTop(pic1, pic2, w, h)
and horizontal (pic1:Picture) (pos:float) (pic2:Picture): Picture =
  if pos < 0 || pos > 1 then 
    raise (System.ArgumentOutOfRangeException ("ppos must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = w1+w2, (max h1 h2)
  let s = abs (pos*float (h1-h2))
  if h1 > h2 then
    Horizontal(pic1, Translate(pic2, 0.0, s, w2, h2+round s), w, h)
  else // something weird happens here, seems that clip removes the top of pic 2
    Horizontal(Translate(pic1, 0.0, s, w1, h1+round s), Translate(pic2, 0.0, -s, w1, h1), w, h)
and vertical (pic1:Picture) (pos:float) (pic2:Picture): Picture =
  if pos < 0 || pos > 1 then 
    raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = max w1 w2, h1 + h2
  let s = abs (pos*float (w1-w2))
  if w1 > w2 then
    Vertical(pic1, Translate(pic2, s, 0.0, w2+round s, h2), w, h)
  else
    Vertical(Translate(pic1, s, 0.0, w1+round s, h1), Translate(pic2, -s, 0.0, w2, h2), h, w)

/// Graphics primitives
let pointsOfList (lst: (float*float) list) = 
  lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
let ellipsePoints ((cx,cy): (float*float)) ((rx,ry): (float*float)): (float*float) list =
  let n = int <| max 10.0 ((max rx ry)*10.0)
  List.map (fun i -> (cx+rx*cos i, cy+ry*sin i)) [for i in 0..(n-1) do yield 2.0*System.Math.PI*(float i)/(float (n-1))]

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

let measureDC (ctx:drawing_context): int*int =
  let sz = ctx.GetCurrentSize()
  (sz.Width,sz.Height)
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

/// Drawing pictures
let rec drawDC (pic:Picture) : drawing_fun =
  match pic with
    | Leaf(f, _, _) ->
      f
    | Horizontal(p1, p2, w, h) ->
      let w1, _ = getSize p1
      let left = drawDC p1
      let right = drawDC p2
      fun ctx -> 
        ctx |> right |> translateDC w1 0.0 |> left
    | Vertical(p1, p2, _, _) ->
      let _,h1 = getSize p1
      let top = drawDC p1
      let bottom = drawDC p2
      fun ctx -> 
        ctx |> bottom |> translateDC 0.0 h1 |> top
    | OnTop(p1, p2, _, _) ->
      let lower = drawDC p1
      let upper = drawDC p2
      fun ctx -> 
        ctx |> upper |> lower
    | Translate(p,dx,dy,_, _) ->
      fun ctx -> 
        ctx |> drawDC p |> translateDC dx dy
    | AffineTransform(p,M,c,_, _) ->
      fun ctx -> 
        ctx |> drawDC p |> affinetransformDC M c
    | Scale(p,sx,sy,_, _) -> 
      fun ctx -> 
        ctx |> drawDC p |> scaleDC sx sy
    | Rotate(p,cx,cy,rad,_, _) ->
      fun ctx -> 
        ctx |> drawDC p |> rotateDC cx cy rad
    | Crop(p, w, h) ->
      fun ctx -> 
        ctx |> drawDC p |> cropDC Color.Transparent w h
