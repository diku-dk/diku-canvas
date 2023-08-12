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

// Working with files
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

/// Graphics primitives
let pointsOfList (lst: (float*float) list) = 
  lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
let ellipsePoints ((cx,cy): (float*float)) ((rx,ry): (float*float)): (float*float) list =
  let n = int <| max 10.0 ((max rx ry)*10.0)
  List.map (fun i -> (cx+rx*cos i, cy+ry*sin i)) [for i in 0..(n-1) do yield 2.0*System.Math.PI*(float i)/(float (n-1))]

let affinetransform (M:System.Numerics.Matrix3x2) (c: Color) (ctx:drawing_context): drawing_context = 
  let transformation = AffineTransformBuilder().AppendMatrix(M)
  ctx.Transform(transformation)
let translate (dx:float) (dy:float) (ctx:drawing_context): drawing_context =
  let M = Matrix3x2Extensions.CreateTranslation(PointF(float32 dx, float32 dy))
  affinetransform M Color.Transparent ctx
let scale (sx:float) (sy:float) (ctx:drawing_context): drawing_context =
  let M = Matrix3x2Extensions.CreateScale(SizeF(float32 sx, float32 sy))
  affinetransform M Color.Transparent ctx
let rotate (cx:float) (cy:float) (rad: float) (ctx:drawing_context): drawing_context =
  let M = Matrix3x2Extensions.CreateRotation(float32 rad, PointF(float32 cx, float32 cy))
  affinetransform M Color.Transparent ctx

let measure (ctx:drawing_context): int*int =
  let sz = ctx.GetCurrentSize()
  (sz.Width,sz.Height)

let systemFonts = SystemFonts.Families
let createFont (fname: string) (sz: float): Font = SystemFonts.CreateFont(fname, float32 sz)
let measureText (f: Font) (txt: string): Size = 
  let size = TextMeasurer.Measure(txt, TextOptions(f))
  (float size.Width, float size.Height)
let text (c: Color) (f: Font) (txt: string) (x:float, y:float) (ctx:drawing_context): drawing_context = 
  ctx.DrawText(txt, f, c, PointF(float32 x, float32 y))

let curve (p: Pen) (lst: (float*float) list) (ctx:drawing_context): drawing_context = 
  let points = lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
  ctx.DrawLines(p, points)
let filledPolygon (c: Color) (lst: (float*float) list) (ctx:drawing_context): drawing_context = 
  let points = lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
  ctx.FillPolygon(DrawingOptions(), c, points)
let rectangle (p: Pen) ((x1,y1): (float*float)) ((x2,y2): (float*float)) (ctx:drawing_context): drawing_context = 
  ctx.Draw(DrawingOptions(), p, RectangleF(float32 x1, float32 y1, float32 x2, float32 y2))
let filledRectangle (c: Color) ((x1,y1): (float*float)) ((x2,y2): (float*float)) (ctx:drawing_context): drawing_context = 
  ctx.Fill(c, RectangleF(float32 x1, float32 y1, float32 x2, float32 y2))
let ellipse (p: Pen) (c: (float*float)) (r: (float*float)) (ctx:drawing_context): drawing_context = 
  let lst = ellipsePoints c r
  curve p lst ctx
let filledEllipse (w: int) (h:int) (col: Color) (c: float*float) (r: float*float) (ctx:drawing_context): drawing_context = 
  let lst = ellipsePoints c r
  filledPolygon col lst ctx
let crop (c: Color) (w:int) (h:int) (ctx:drawing_context): drawing_context = 
  let sz = ctx.GetCurrentSize()
  ctx.Crop(min sz.Width w, min sz.Height h)
     .Resize(ResizeOptions(Position = AnchorPositionMode.TopLeft,
                           Size = Size(w, h), 
                           Mode = ResizeMode.BoxPad))
     .BackgroundColor(c)

/// Functions for combining images
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

/// Drawing content
let rec compile (pic:Picture) (explain: bool): drawing_fun =
  let wrap dc w h explain =
    if explain then
      let p = makePen blue 1.0
      let box = rectangle p (0.0,0.0) (float (w-1), float (h-1)) 
      box >> dc
    else
      dc
  match pic with
    | Leaf(dc, w, h) ->
      wrap dc w h explain
    | Horizontal(p1, p2, w, h) ->
      let w1, _ = getSize p1
      let left = compile p1 explain
      let right = compile p2 explain
      let dc = right >> translate w1 0.0 >> left
      wrap dc w h explain
    | Vertical(p1, p2, w, h) ->
      let _,h1 = getSize p1
      let top = compile p1 explain
      let bottom = compile p2 explain
      let dc = bottom >> translate 0.0 h1 >> top
      wrap dc w h explain
    | OnTop(p1, p2, w, h) ->
      let lower = compile p1 explain
      let upper = compile p2 explain
      let dc = upper >> lower
      wrap dc w h explain
    | Translate(p, dx, dy, w, h) ->
      let dc = compile p explain >> translate dx dy
      wrap dc w h explain
    | AffineTransform(p, M, c, w, h) ->
      let dc = compile p explain >> affinetransform M c
      wrap dc w h explain
    | Scale(p, sx, sy, w, h) -> 
      let dc = compile p explain >> scale sx sy
      wrap dc w h explain
    | Rotate(p, cx, cy, rad, w, h) ->
      let dc = compile p explain >> rotate cx cy rad
      wrap dc w h explain
    | Crop(p, w, h) ->
      let dc = compile p explain>> crop Color.Transparent w h
      wrap dc w h explain

let combine (pic:Picture): drawing_fun = compile pic false
let explain (pic:Picture): drawing_fun = compile pic true
