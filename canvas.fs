module Canvas
open System.Numerics

/// Types
type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush
type color = Lowlevel.color
type Font = Lowlevel.Font
type FontFamily = Lowlevel.FontFamily
type pointF = Lowlevel.pointF
type Picture = Lowlevel.drawing_fun
let (<+>) = Lowlevel.(<+>)
let drawToFile width height filePath draw = Lowlevel.drawToFile width height filePath draw 
let drawToAnimatedGif width height frameDelay repeatCount filePath drawLst = Lowlevel.drawToAnimatedGif width height frameDelay repeatCount filePath drawLst
let runAppWithTimer t w h interval draw react s = Lowlevel.runAppWithTimer t w h interval draw react s
let runApp t w h draw = Lowlevel.runApp t w h draw

type ControlKey = Lowlevel.ControlKey
type Event = Lowlevel.Event

type Size = float*float
type PrimitiveTree = 
    | PiecewiseAffine of (pointF list)*color*float*Size
    | FilledPolygon of (pointF list)*color*Size
    | Rectangle of color*float*Size
    | FilledRectangle of color*Size
    | Ellipse of color*float*Size
    | FilledEllipse of color*Size
    | AlignH of PrimitiveTree*PrimitiveTree*float*Size
    | AlignV of PrimitiveTree*PrimitiveTree*float*Size
    | OnTop of PrimitiveTree*PrimitiveTree*Size
    | Scale of PrimitiveTree*float*float*Size
    | Rotate of PrimitiveTree*float*float*float*Size
    | Translate of PrimitiveTree*float*float*Size

let getSize (p:PrimitiveTree): Size =
  match p with
    | PiecewiseAffine(_,_,_,sz)
    | FilledPolygon(_,_,sz)
    | Rectangle(_,_,sz)
    | FilledRectangle(_,sz)
    | Ellipse(_,_,sz)
    | FilledEllipse(_,sz)
    | AlignH(_,_,_,sz)
    | AlignV(_,_,_,sz)
    | OnTop(_,_,sz)
    | Scale(_,_,_,sz)
    | Rotate(_,_,_,_,sz)
    | Translate(_,_,_,sz) -> sz

let tostring (p:PrimitiveTree): string =
    let rec loop (prefix:string) (p:PrimitiveTree): string =
        let descentPrefix = (String.replicate prefix.Length " ")+"\u221F>"
        match p with
            | PiecewiseAffine(points,c,sw,sz) -> sprintf "%sPiecewiseAffine (color,stroke)=%A coordinates=%A" prefix (c,sw) points
            | FilledPolygon(points,c,sz) -> sprintf "%sFilledPolygon color=%A coordinates=%A" prefix c points
            | Rectangle(c,sw,sz) -> sprintf "%sRectangle (color,stroke)=%A (width,height)=%A" prefix (c,sw) sz
            | FilledRectangle(c,sz) -> sprintf "%sFilledRectangle color%A (width,height)=%A" prefix c sz
            | Ellipse(c,sw,sz) -> sprintf "%sEllipse (color,stroke)=%A (radiusX,radiusY)=%A" prefix (c,sw) ((fst sz)/2.0,(snd sz)/2.0)
            | FilledEllipse(c,sz) -> sprintf "%sFilledEllipse color=%A (radiusX,radiusY)=%A" prefix c ((fst sz)/2.0,(snd sz)/2.0)
            | AlignH(p1,p2,pos,sz) -> sprintf "%sAlignH position=%g\n%s\n%s" prefix pos (loop descentPrefix p1) (loop descentPrefix p2)
            | AlignV(p1,p2,pos,sz) -> sprintf "%sAlignV position=%g\n%s\n%s" prefix pos (loop descentPrefix p1) (loop descentPrefix p2)
            | OnTop(p1,p2,sz) -> sprintf "%sOnTop\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
            | Scale(q,sx,sy,sz) -> sprintf "%sScale (scaleX,scaleY)=%A\n%s" prefix (sx, sy) (loop descentPrefix q)
            | Rotate(q,x,y,rad,sz) -> sprintf "%sRotate (centerX,centerY)=%A radius=%g\n%s" prefix (x, y) rad (loop descentPrefix q)
            | Translate(q,dx,dy,sz) -> sprintf "%sTranslate (dx,dy)=%A\n%s" prefix (dx, dy) (loop descentPrefix q)
    loop "" p

/// Graphics primitives
//let affineLowlevel.transform (M:System.Numerics.Matrix3x2) (p: PrimitiveTree): PrimitiveTree = 
//  let sz = getSize p
//  AffineTransform(p,M,w,h)
let translate (dx:float) (dy:float) (p: PrimitiveTree): PrimitiveTree =
    let sz = getSize p
    Translate(p,dx,dy,(dx+fst sz,dy+snd sz))
let scale (sx:float) (sy:float) (p: PrimitiveTree): PrimitiveTree =
    let sz = getSize p
    Scale(p,sx,sy,(sx*fst sz,sy*snd sz))
let rotate (x:float) (y:float) (rad:float) (p: PrimitiveTree): PrimitiveTree =
    let sz = getSize p
    Rotate(p,x,y,rad,sz)

let mapPairLst (f: 'a list -> 'b) (lst: ('a*'a) list): 'b*'b =
    List.unzip lst |> fun (a,b) -> f a, f b
let piecewiseaffine (c:color) (sw: float) (lst: (float*float) list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let sz = 1.0+wMin+wMax, 1.0+hMin+hMax
    PiecewiseAffine(lst, c, sw, sz) 
let filledpolygon (c:color) (lst: (float*float) list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let sz = 1.0+wMin+wMax, 1.0+hMin+hMax
    FilledPolygon(lst, c, sz) 
let rectangle (c: color) (sw: float) (w: float) (h: float): PrimitiveTree = 
    Rectangle(c,sw,(w,h)) 
let filledrectangle (c: color) (w: float) (h: float): PrimitiveTree = 
    FilledRectangle(c,(w,h)) 
let ellipse (c: color) (sw: float) (rx: float) (ry:float): PrimitiveTree = 
    Ellipse(c,sw, (2.0*rx, 2.0*ry)) 
let filledellipse (c: color) (rx: float) (ry:float): PrimitiveTree = 
    FilledEllipse(c, (2.0*rx, 2.0*ry)) 

/// Functions for combining images
let Top = 0.0
let Left = 0.0
let Center = 0.5
let Bottom = 1.0
let Right = 1.0

let rec ontop (pic1:PrimitiveTree) (pic2:PrimitiveTree): PrimitiveTree =
    let w1,h1 = getSize pic1
    let w2,h2 = getSize pic2
    let sz = (max w1 w2), (max h1 h2)
    OnTop(pic1, pic2, sz)
and alignh (pic1:PrimitiveTree) (pos:float) (pic2:PrimitiveTree): PrimitiveTree =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("ppos must be in [0,1]"))
    let w1,h1 = getSize pic1
    let w2,h2 = getSize pic2
    let sz = w1+w2, (max h1 h2)
    AlignH(pic1,pic2,pos,sz)
and alignv (pic1:PrimitiveTree) (pos:float) (pic2:PrimitiveTree): PrimitiveTree =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
    let w1,h1 = getSize pic1
    let w2,h2 = getSize pic2
    let sz = max w1 w2, h1 + h2
    AlignV(pic1, pic2, pos, sz)

/// Drawing content
let colorLst = [color.Blue; color.Cyan; color.Green; color.Magenta; color.Orange; color.Purple; color.Yellow; color.Red]
let rec compile (idx:int) (expFlag: bool) (pic:PrimitiveTree): Lowlevel.PathTree =
    let next = (idx+1) % colorLst.Length
    let wrap sz expFlag dc =
        if expFlag then
            let pen = Lowlevel.solidPen colorLst[idx] 1.0
            let b = Lowlevel.Prim (pen, Lowlevel.Rectangle(fst sz, snd sz))
            dc <+> b
        else
            dc
    match pic with
    | PiecewiseAffine(lst, c, sw, sz) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim (pen, Lowlevel.Lines lst)
        wrap sz expFlag dc
    | FilledPolygon(lst, c, sz) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim (brush, Lowlevel.Lines lst)
        wrap sz expFlag dc
    | Rectangle(c, sw, sz) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim (pen, Lowlevel.Rectangle(fst sz, snd sz))
        wrap sz expFlag dc
    | FilledRectangle(c, sz) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim (brush, Lowlevel.Rectangle(fst sz, snd sz))
        wrap sz expFlag dc
    | Ellipse(c, sw, sz) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim (pen, Lowlevel.Ellipse(fst sz, snd sz))
        wrap sz expFlag dc
    | FilledEllipse(c, sz) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim (brush, Lowlevel.Ellipse(fst sz, snd sz))
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
                dc1 <+> (Lowlevel.transform M <| dc2)
            elif h1 = h2 then
                dc1 <+> dc2
            else
                let M1 = Matrix3x2.CreateTranslation(0f,s)
                let M2 = Matrix3x2.CreateTranslation(float32 w1,0f)
                (Lowlevel.transform M1 <| dc1) <+> (Lowlevel.transform M2 <| dc2)
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
                dc1 <+> (Lowlevel.transform M <| dc2)
            elif h1 = h2 then
                dc1 <+> dc2
            else 
                let M1 = Matrix3x2.CreateTranslation(s,0f)
                let M2 = Matrix3x2.CreateTranslation(0f,float32 h1)
                (Lowlevel.transform M1 <| dc1) <+> (Lowlevel.transform M2 <| dc2)
        wrap sz expFlag dc
    | Translate(p, dx, dy, sz) ->
        let M = Matrix3x2.CreateTranslation(float32 dx,float32 dy)
        let dc = Lowlevel.transform M <| compile next expFlag p
        wrap sz expFlag dc
    | Scale(p, sx, sy, sz) -> 
        let S = Matrix3x2.CreateScale(float32 sx, float32 sy)
        let dc = Lowlevel.transform S <| compile next expFlag p
        wrap sz expFlag dc
    | Rotate(p, cx, cy, rad, sz) ->
        let R = Matrix3x2.CreateRotation(float32 rad)
        let T1 = Matrix3x2.CreateTranslation(float32 cx,float32 cy)
        let T2 = Matrix3x2.CreateTranslation(float32 -cx,float32 -cy)
        let M = T2*R*T1
        let dc = Lowlevel.transform M <| compile next expFlag p
        wrap sz expFlag dc

let make (p:PrimitiveTree): Picture = 
    compile 0 false p |> Lowlevel.drawPathTree
let explain (p:PrimitiveTree): Picture = 
    compile 0 true p |> Lowlevel.drawPathTree
