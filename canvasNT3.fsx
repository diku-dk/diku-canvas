module CanvasNT3
#r "nuget: SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing"

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
type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush
type Color = SixLabors.ImageSharp.Color
type Font = SixLabors.Fonts.Font
type FontFamily = SixLabors.Fonts.FontFamily
type pointF = Lowlevel.pointF

type Size = float*float
type Picture = 
    | PiecewiseAffine of (pointF list)*Color*float*Size
    | Rectangle of Color*float*Size
    | Ellipse of Color*float*Size
    | AlignH of Picture*Picture*float*Size
    | AlignV of Picture*Picture*float*Size
    | OnTop of Picture*Picture*Size
    | Scale of Picture*float*float*Size
    | Rotate of Picture*float*float*float*Size
    | Translate of Picture*float*float*Size

let getSize (p:Picture): Size =
  match p with
    | PiecewiseAffine(_,_,_,sz)
    | Rectangle(_,_,sz)
    | Ellipse(_,_,sz)
    | AlignH(_,_,_,sz)
    | AlignV(_,_,_,sz)
    | OnTop(_,_,sz)
    | Scale(_,_,_,sz)
    | Rotate(_,_,_,_,sz)
    | Translate(_,_,_,sz) -> sz

let tostring (p:Picture): string =
    let rec loop (prefix:string) (p:Picture): string =
        let descentPrefix = (String.replicate prefix.Length " ")+"\u221F>"
        match p with
            | PiecewiseAffine(points,c,sw,sz) -> sprintf "%sPiecewiseAffine (color,stroke)=%A coordinates=%A" prefix (c,sw) points
            | Rectangle(c,sw,sz) -> sprintf "%sRectangle (color,stroke)=%A (width,height)=%A" prefix (c,sw) sz
            | Ellipse(c,sw,sz) -> sprintf "%sEllipse (color,stroke)=%A (radiusX,radiusY)=%A" prefix (c,sw) ((fst sz)/2.0,(snd sz)/2.0)
            | AlignH(p1,p2,pos,sz) -> sprintf "%sAlignH position=%g\n%s\n%s" prefix pos (loop descentPrefix p1) (loop descentPrefix p2)
            | AlignV(p1,p2,pos,sz) -> sprintf "%sAlignV position=%g\n%s\n%s" prefix pos (loop descentPrefix p1) (loop descentPrefix p2)
            | OnTop(p1,p2,sz) -> sprintf "%sOnTop\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
            | Scale(q,sx,sy,sz) -> sprintf "%sScale (scaleX,scaleY)=%A\n%s" prefix (sx, sy) (loop descentPrefix q)
            | Rotate(q,x,y,rad,sz) -> sprintf "%sRotate (centerX,centerY)=%A radius=%g\n%s" prefix (x, y) rad (loop descentPrefix q)
            | Translate(q,dx,dy,sz) -> sprintf "%sTranslate (dx,dy)=%A\n%s" prefix (dx, dy) (loop descentPrefix q)
    loop "" p

/// Graphics primitives
//let affinetransform (M:System.Numerics.Matrix3x2) (p: Picture): Picture = 
//  let sz = getSize p
//  AffineTransform(p,M,w,h)
let translate (dx:float) (dy:float) (p: Picture): Picture =
    let sz = getSize p
    Translate(p,dx,dy,(dx+fst sz,dy+snd sz))
let scale (sx:float) (sy:float) (p: Picture): Picture =
    let sz = getSize p
    Scale(p,sx,sy,(sx*fst sz,sy*snd sz))
let rotate (x:float) (y:float) (rad:float) (p: Picture): Picture =
    let sz = getSize p
    Rotate(p,x,y,rad,sz)

let mapPairLst (f: 'a list -> 'b) (lst: ('a*'a) list): 'b*'b =
    List.unzip lst |> fun (a,b) -> f a, f b
let piecewiseaffine (c:Color) (sw: float) (lst: (float*float) list): Picture = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let sz = 1.0+wMin+wMax, 1.0+hMin+hMax
    PiecewiseAffine(lst, c, sw, sz) 
let rectangle (c: Color) (sw: float) (w: float) (h: float): Picture = 
    Rectangle(c,sw,(w,h)) 
let ellipse (c: Color) (sw: float) (rx: float) (ry:float): Picture = 
    Ellipse(c,sw, (2.0*rx, 2.0*ry)) 

/// Functions for combining images
let Top = 0.0
let Left = 0.0
let Center = 0.5
let Bottom = 1.0
let Right = 1.0

let rec ontop (pic1:Picture) ((posX,posY): float*float) (pic2:Picture): Picture =
    let w1,h1 = getSize pic1
    let w2,h2 = getSize pic2
    let sz = (max w1 w2), (max h1 h2)
    OnTop(pic1, pic2, sz)
and alignh (pic1:Picture) (pos:float) (pic2:Picture): Picture =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("ppos must be in [0,1]"))
    let w1,h1 = getSize pic1
    let w2,h2 = getSize pic2
    let sz = w1+w2, (max h1 h2)
    AlignH(pic1,pic2,pos,sz)
and alignv (pic1:Picture) (pos:float) (pic2:Picture): Picture =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
    let w1,h1 = getSize pic1
    let w2,h2 = getSize pic2
    let sz = max w1 w2, h1 + h2
    AlignV(pic1, pic2, pos, sz)

/// Drawing content
let colorLst = [Color.Blue; Color.Cyan; Color.Green; Color.Magenta; Color.Orange; Color.Purple; Color.Yellow; Color.Red]
let rec compile (idx:int) (expFlag: bool) (pic:Picture): PathTree =
    let next = (idx+1) % colorLst.Length
    let wrap sz expFlag dc =
        if expFlag then
            let pen = solidPen colorLst[idx] 1.0
            let b = Prim (pen, Lowlevel.Rectangle(fst sz, snd sz))
            dc <+> b
        else
            dc
    match pic with
    | PiecewiseAffine(lst, c, sw, sz) ->
        let dc = Prim (solidPen c sw, Lowlevel.Lines lst)
        wrap sz expFlag dc
    | Rectangle(c, sw, sz) ->
        let pen = solidPen c sw
        let dc = Prim (pen, Lowlevel.Rectangle(fst sz, snd sz))
        wrap sz expFlag dc
    | Ellipse(c, sw, sz) ->
        let pen = solidPen c sw
        let dc = Prim (pen, Lowlevel.Ellipse(fst sz, snd sz))
        wrap sz expFlag dc
    | OnTop(p1, p2, sz) ->
        let dc1 = compile next expFlag p1
        let dc2 = compile next expFlag p2
        let dc = dc1 <+> dc2
        wrap sz expFlag dc
    | AlignH(p1, p2, pos, sz) ->
        let w1,h1 = getSize p1
        let w2,h2 = getSize p2
        let sz = w1+w2, (max h1 h2)
        let s = float32 (abs (pos*float (h1-h2)))
        let dc1 = compile next expFlag p1
        let dc2 = compile next expFlag p2
        let dc =
            if h1 > h2 then
                let M = Matrix3x2.CreateTranslation(float32 w1,s)
                dc1 <+> (transform M <| dc2)
            elif h1 = h2 then
                dc1 <+> dc2
            else
                let M1 = Matrix3x2.CreateTranslation(0f,s)
                let M2 = Matrix3x2.CreateTranslation(float32 w1,0f)
                (transform M1 <| dc1) <+> (transform M2 <| dc2)
        wrap sz expFlag dc
    | AlignV(p1, p2, pos, sz) ->
        let w1,h1 = getSize p1
        let w2,h2 = getSize p2
        let sz = max w1 w2, h1 + h2
        let s = float32 (abs (pos*float (h1-h2)))
        let dc1 = compile next expFlag p1 
        let dc2 = compile next expFlag p2
        let dc =
            if w1 > w2 then
                let M = Matrix3x2.CreateTranslation(s,float32 h1)
                dc1 <+> (transform M <| dc2)
            elif h1 = h2 then
                dc1 <+> dc2
            else 
                let M1 = Matrix3x2.CreateTranslation(s,0f)
                let M2 = Matrix3x2.CreateTranslation(0f,float32 h1)
                (transform M1 <| dc1) <+> (transform M2 <| dc2)
        wrap sz expFlag dc
    | Translate(p, dx, dy, sz) ->
        let M = Matrix3x2.CreateTranslation(float32 dx,float32 dy)
        let dc = transform M <| compile next expFlag p
        wrap sz expFlag dc
    | Scale(p, sx, sy, sz) -> 
        let S = Matrix3x2.CreateScale(float32 sx, float32 sy)
        let dc = transform S <| compile next expFlag p
        wrap sz expFlag dc
    | Rotate(p, cx, cy, rad, sz) ->
        let R = Matrix3x2.CreateRotation(float32 rad)
        let T1 = Matrix3x2.CreateTranslation(float32 cx,float32 cy)
        let T2 = Matrix3x2.CreateTranslation(float32 -cx,float32 -cy)
        let M = T2*R*T1
        let dc = transform M <| compile next expFlag p
        wrap sz expFlag dc

let make (p:Picture): drawing_fun = 
    compile 0 false p |> drawPathTree
let explain (p:Picture): drawing_fun = 
    compile 0 true p |> drawPathTree
