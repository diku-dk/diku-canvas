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
    | AlignH of Picture*Picture*Size
    | AlignV of Picture*Picture*Size
    | OnTop of Picture*Picture*Size
    | Scale of Picture*float*float*Size
    | Rotate of Picture*float*float*float*Size
    | Translate of Picture*float*float*Size

let getSize (p:Picture): Size =
  match p with
    | PiecewiseAffine(_,_,_,sz)
    | Rectangle(_,_,sz)
    | Ellipse(_,_,sz)
    | AlignH(_,_,sz)
    | AlignV(_,_,sz)
    | OnTop(_,_,sz)
    | Scale(_,_,_,sz)
    | Rotate(_,_,_,_,sz)
    | Translate(_,_,_,sz) -> sz

let tostring (p:Picture): string =
    let rec loop (prefix:string) (p:Picture): string =
        let descentPrefix = (String.replicate prefix.Length " ")+"\u221F>"
        match p with
            | PiecewiseAffine(points,c,sw,sz) -> prefix+(sprintf "PiecewiseAffine: %A - %A" (c,sw) points)
            | Rectangle(c,sw,sz) -> prefix+(sprintf "Rectangle: %A - %A" (c,sw) sz)
            | Ellipse(c,sw,sz) -> prefix+(sprintf "Ellipse: %A - %A" (c,sw) ((fst sz)/2.0,(snd sz)/2.0))
            | AlignH(p1,p2,sz) -> sprintf "%sAlignH\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
            | AlignV(p1,p2,sz) -> sprintf "%sAlignV\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
            | OnTop(p1,p2,sz) -> sprintf "%sOnTop\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
            | Scale(q,sx,sy,sz) -> sprintf "%sAffineTransform %g %g\n%s" prefix sx sy (loop descentPrefix q)
            | Rotate(q,x,y,rad,sz) -> sprintf "%sRotate %g %g %g\n%s" prefix x y rad (loop descentPrefix q)
            | Translate(q,dx,dy,sz) -> sprintf "%sTranslate %g %g\n%s" prefix dx dy (loop descentPrefix q)
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

let rec ontop (pic1:Picture) (pic2:Picture): Picture =
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
    let s = abs (pos*float (h1-h2))
    if h1 > h2 then
        AlignH(pic1, Translate(pic2, 0.0, s, (w2, h2+s)), sz)
    elif h1 = h2 then
        AlignH(pic1, pic2, sz)
    else // something weird happens here, seems that clip removes the top of pic 2
        AlignH(Translate(pic1, 0.0, s, (w1, h1+s)), pic2, sz)
and alignv (pic1:Picture) (pos:float) (pic2:Picture): Picture =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
    let w1,h1 = getSize pic1
    let w2,h2 = getSize pic2
    let sz = max w1 w2, h1 + h2
    let s = abs (pos*float (w1-w2))
    if h1 > h2 then
        AlignV(pic1, Translate(pic2, s, 0.0, (w2+s, h2)), sz)
    elif h1 = h2 then
        AlignV(pic1, pic2, sz)
    else 
        AlignV(Translate(pic1, s, 0.0, (w1+s,h1)), pic2, sz)

/// Drawing content
let mutable i = 0
let colorLst = [Color.Blue; Color.Cyan; Color.Green; Color.Magenta; Color.Orange; Color.Purple; Color.Yellow; Color.Red]
let rec compile (expFlag: bool) (pic:Picture): PathTree =
    let wrap sz expFlag dc =
        if expFlag then
            let pen = solidPen colorLst[i] 1.0
            let b = Prim (pen, Lowlevel.Rectangle(fst sz, snd sz))
            i <- (i+1) % colorLst.Length
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
        let dc1 = compile expFlag p1
        let dc2 = compile expFlag p2
        let dc = dc1 <+> dc2
        wrap sz expFlag dc
    | AlignH(p1, p2, sz) ->
        let w1, _ = getSize p1
        let M = Matrix3x2.CreateTranslation(float32 w1,0f)
        let dc1 = compile expFlag p1
        let dc2 = transform M <| compile expFlag p2
        let dc = dc1 <+> dc2
        wrap sz expFlag dc
    | AlignV(p1, p2, sz) ->
        let _,h1 = getSize p1
        let M = Matrix3x2.CreateTranslation(0f,float32 h1)
        let dc1 = compile expFlag p1 
        let dc2 = transform M <| compile expFlag p2
        let dc = dc1 <+> dc2
        wrap sz expFlag dc
    | Translate(p, dx, dy, sz) ->
        let M = Matrix3x2.CreateTranslation(float32 dx,float32 dy)
        let dc = transform M <| compile expFlag p
        wrap sz expFlag dc
    | Scale(p, sx, sy, sz) -> 
        let S = Matrix3x2.CreateScale(float32 sx, float32 sy)
        let dc = transform S <| compile expFlag p
        wrap sz expFlag dc
    | Rotate(p, cx, cy, rad, sz) ->
        let R = Matrix3x2.CreateRotation(float32 rad)
        let T1 = Matrix3x2.CreateTranslation(float32 cx,float32 cy)
        let T2 = Matrix3x2.CreateTranslation(float32 -cx,float32 -cy)
        let M = T2*R*T1
        let w,h = float32 (fst sz), float32 (snd sz)
        let lst = 
            [0f,0f; 0f, h; w, h; w, 0f]
            |> List.map (fun (x,y) -> Vector2.Transform(Vector2(float32 x,float32 y),M))
            |> List.map (fun v -> (float v.X,float v.Y))
        let wMin, hMin = mapPairLst List.min lst 
        let wMax, hMax = mapPairLst List.max lst
        let ox,oy = (max 0.0 -wMin),(max 0.0 -hMin)
        let T3 = Matrix3x2.CreateTranslation(float32 ox,float32 oy)
        let dc = transform (M*T3) <| compile expFlag p
        wrap (wMax+ox,hMax+oy) expFlag dc

let make (p:Picture): drawing_fun = 
    i<-0
    compile false p |> drawPathTree
let explain (p:Picture): drawing_fun = 
    i<-0
    compile true p |> drawPathTree
