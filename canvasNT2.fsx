module CanvasNT2
#r "nuget: SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.Fonts
open System.Numerics;

/////////////////////////////////
// Jon's work on the interface //
/////////////////////////////////

/// Types
type path_context = (float*float) list
type path_fun = path_context -> path_context
type Color = SixLabors.ImageSharp.Color
type Pen = SixLabors.ImageSharp.Drawing.Processing.Pen
type Font = SixLabors.Fonts.Font
type Picture = 
  | PiecewiseAffine of path_context*Color*float*float*float
  | Rectangle of Color*float*float*float
  | Ellipse of Color*float*float*float
  | AlignH of Picture*Picture*float*float
  | AlignV of Picture*Picture*float*float
  | OnTop of Picture*Picture*float*float
  | AffineTransform of Picture*System.Numerics.Matrix3x2*float*float
  | Scale of Picture*float*float*float*float
  | Rotate of Picture*float*float*float*float*float
  | Translate of Picture*float*float*float*float
type Size = float*float
let getSize (p:Picture): float*float =
  match p with
    | PiecewiseAffine(_,_,_,w,h)
    | Rectangle(_,_,w,h)
    | Ellipse(_,_,w,h)
    | AlignH(_,_,w,h)
    | AlignV(_,_,w,h)
    | OnTop(_,_,w,h)
    | AffineTransform(_,_,w,h)
    | Scale(_,_,_,w,h)
    | Rotate(_,_,_,_,w,h)
    | Translate(_,_,_,w,h) -> w,h
//    | Crop(_,w,h)
let tostring (p:Picture): string =
  let rec loop (prefix:string) (p:Picture): string =
    let descentPrefix = (String.replicate prefix.Length " ")+"\u221F>"
    match p with
      | PiecewiseAffine(points,c,sw,w,h) -> prefix+(sprintf "PiecewiseAffine: %A - %A" (c,sw) points)
      | Rectangle(c,sw,w,h) -> prefix+(sprintf "Rectangle: %A - %A" (c,sw) (w,h))
      | Ellipse(c,sw,w,h) -> prefix+(sprintf "Ellipse: %A - %A" (c,sw) (w/2.0,h/2.0))
      | AlignH(p1,p2,w,h) -> sprintf "%sAlignH\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
      | AlignV(p1,p2,w,h) -> sprintf "%sAlignV\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
      | OnTop(p1,p2,w,h) -> sprintf "%sOnTop\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
      | AffineTransform(q,M,w,h) -> sprintf "%sAffineTransform %A\n%s" prefix M (loop descentPrefix q)
      | Scale(q,sx,sy,w,h) -> sprintf "%sAffineTransform %g %g\n%s" prefix sx sy (loop descentPrefix q)
      | Rotate(q,x,y,rad,w,h) -> sprintf "%sRotate %g %g %g\n%s" prefix x y rad (loop descentPrefix q)
      | Translate(q,dx,dy,w,h) -> sprintf "%sTranslate %g %g\n%s" prefix dx dy (loop descentPrefix q)
      //| Crop(_,w,h)
  loop "" p

/// Graphics primitives
let affinetransform (M:System.Numerics.Matrix3x2) (p: Picture): Picture = 
  let w, h = getSize p
  AffineTransform(p,M,w,h)
let translate (dx:float) (dy:float) (p: Picture): Picture =
  let w, h = getSize p
  Translate(p,dx,dy,w+dx,h+dy)
let scale (sx:float) (sy:float) (p: Picture): Picture =
  let w, h = getSize p
  Scale(p,sx,sy,w*sx,h*sy)
let rotate (x:float) (y:float) (rad:float) (p: Picture): Picture =
  let w, h = getSize p
  Rotate(p,x,y,rad,w,h)

let piecewiseaffine (c:Color) (sw: float) (lst: (float*float) list): Picture = 
  let mapPairLst (f: 'a list -> 'b) (lst: ('a*'a) list): 'b*'b =
    List.unzip lst |> fun (a,b) -> f a, f b
  let wMin, hMin = mapPairLst List.min lst 
  let wMax, hMax = mapPairLst List.max lst
  let w, h = 1.0+wMin+wMax, 1.0+hMin+hMax
  PiecewiseAffine(lst, c, sw, w, h) 
let rectangle (c: Color) (sw: float) (w: float) (h: float): Picture = 
  Rectangle(c,sw,w,h) 
let ellipse (c: Color) (sw: float) (rx: float) (ry:float): Picture = 
  Ellipse(c,sw, 2.0*rx, 2.0*ry) 


/// Functions for combining images
let Top = 0.0
let Left = 0.0
let Center = 0.5
let Bottom = 1.0
let Right = 1.0

let rec ontop (pic1:Picture) (pic2:Picture): Picture =
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = (max w1 w2), (max h1 h2)
  OnTop(pic1, pic2, w, h)
and alignh (pic1:Picture) (pos:float) (pic2:Picture): Picture =
  if pos < 0 || pos > 1 then 
    raise (System.ArgumentOutOfRangeException ("ppos must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = w1+w2, (max h1 h2)
  let s = abs (pos*float (h1-h2))
  if h1 > h2 then
    AlignH(pic1, Translate(pic2, 0.0, s, w2, h2+s), w, h)
  elif h1 = h2 then
    AlignH(pic1, pic2, w, h)
  else // something weird happens here, seems that clip removes the top of pic 2
    AlignH(Translate(pic1, 0.0, s, w1, h1+s), pic2, w, h)
and alignv (pic1:Picture) (pos:float) (pic2:Picture): Picture =
  if pos < 0 || pos > 1 then 
    raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
  let w1,h1 = getSize pic1
  let w2,h2 = getSize pic2
  let w, h = max w1 w2, h1 + h2
  let s = abs (pos*float (w1-w2))
  if h1 > h2 then
    AlignV(pic1, Translate(pic2, s, 0.0, w2+s, h2), w, h)
  elif h1 = h2 then
    AlignV(pic1, pic2, w, h)
  else 
    AlignV(Translate(pic1, s, 0.0, w1+s,h1), pic2, w, h)

/// Drawing content
let mutable i = 0
let colorLst = [Color.Blue; Color.Cyan; Color.Green; Color.Magenta; Color.Orange; Color.Purple; Color.Yellow; Color.Red]
let rec compile (M:Matrix3x2) (pic:Picture) (expFlag: bool): drawing_fun =
  let ofPathContext (c:Color) (sw:float) (lst:path_context):drawing_fun = 
      let points = lst |> Array.ofList |> Array.map (fun (a,b) -> PointF(float32 a, float32 b)) 
      let path = PathBuilder(M).AddLines(points).Build()
      let p = makePen c sw
      fun (ctx:drawing_context) -> ctx.Draw(p, path)
  let box c sw w h = 
    [0.0,0.0; 0.0,h; w,h; w,0.0; 0.0,0.0]
    |> ofPathContext c sw
  let wrap M w h expFlag dc =
    if expFlag then
      let b = box colorLst[i] 1.0 w h
      i <- (i+1) % colorLst.Length
      dc >> b
    else
      dc
  match pic with
    | PiecewiseAffine(lst, c, sw, w, h) ->
      lst
      |> ofPathContext c sw
      |> wrap M w h expFlag
    | Rectangle(c, sw, w, h) ->
      box c sw w h
      |> wrap M w h expFlag
    | Ellipse(c, sw, w, h) ->
      let cx, cy = (w/2.0), (h/2.0)
      let rx, ry = cx, cy
      let n = int <| max 10.0 ((max rx ry)*10.0)
      [0..(n-1)]
      |> List.map (fun i -> 2.0*System.Math.PI*(float i)/(float (n-1)))
      |> List.map (fun i -> (cx+rx*cos i, cy+ry*sin i))
      |> ofPathContext c sw
      |> wrap M w h expFlag
    | OnTop(p1, p2, w, h) ->
      let dc1 = compile M p1 expFlag
      let dc2 = compile M p2 expFlag
      let dc = dc1 >> dc2
      wrap M w h expFlag dc
    | AlignH(p1, p2, w, h) ->
      let w1, h1 = getSize p1
      let T = Matrix3x2.CreateTranslation(float32 w1,0f)
      let dc1 = compile M p1 expFlag
      let dc2 = compile (T*M) p2 expFlag // How should they be combined?
      let dc = dc1 >> dc2
      wrap M w h expFlag dc
    | AlignV(p1, p2, w, h) ->
      let w1,h1 = getSize p1
      let T = Matrix3x2.CreateTranslation(0f,float32 h1)
      let dc1 = compile M p1 expFlag
      let dc2 = compile (T*M) p2 expFlag
      let dc = dc1 >> dc2
      wrap M w h expFlag dc
    | Translate(p, dx, dy, w, h) ->
      let T = Matrix3x2.CreateTranslation(float32 dx,float32 dy)
      let dc = compile (T*M) p expFlag
      wrap M w h expFlag dc
    | AffineTransform(p, A, w, h) ->
      let dc = compile (A*M) p expFlag
      wrap M w h expFlag dc
    | Scale(p, sx, sy, w, h) -> 
      let S = Matrix3x2.CreateScale(float32 sx, float32 sy)
      let dc = compile (S*M) p expFlag
      wrap M w h expFlag dc
    | Rotate(p, cx, cy, rad, w, h) ->
      let R = Matrix3x2.CreateRotation(float32 rad)
      let T1 = Matrix3x2.CreateTranslation(float32 cx,float32 cy)
      let T2 = Matrix3x2.CreateTranslation(float32 -cx,float32 -cy)
      let dc = compile (T2*R*T1*M) p expFlag
      wrap M w h expFlag dc
//    | Crop(p, w, h) ->
//      let dc = _crop Color.Transparent w h
//      wrap M w h expFlag dc

let make (pic:Picture): drawing_fun = 
  i<-0; compile Matrix3x2.Identity pic false
let explain (pic:Picture): drawing_fun = 
  i<-0; compile Matrix3x2.Identity pic true
