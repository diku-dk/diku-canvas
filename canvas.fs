module Canvas
open System.Numerics

/// Types
type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush
type color = Lowlevel.color
type Font = Lowlevel.Font
type FontFamily = Lowlevel.FontFamily
let systemFontNames = Lowlevel.systemFontNames
let getFamily (name:string) = Lowlevel.getFamily name
let makeFont (fam:string) (size:float) = Lowlevel.makeFont (getFamily fam) size
let measureText (f:Font) (txt:string) = Lowlevel.measureText f txt

type pointF = Lowlevel.pointF
type Picture = Lowlevel.drawing_fun
let renderToFile width height filePath draw = Lowlevel.drawToFile width height filePath draw 
let animateToFile width height frameDelay repeatCount filePath drawLst = Lowlevel.drawToAnimatedGif width height frameDelay repeatCount filePath drawLst
let interact t w h interval draw react s = Lowlevel.runAppWithTimer t w h interval draw react s
let render t w h draw = Lowlevel.runApp t w h draw
let fromRgba r g b a = Lowlevel.fromRgba r g b a
let fromRgb r g b = Lowlevel.fromRgb r g b

type ControlKey = Lowlevel.ControlKey
type Event = Lowlevel.Event

type Rectangle = float*float*float*float // x1,y1,x2,y2: x2>x1 && y2>y1
type Size = float*float // w,h
type PrimitiveTree = 
    | Empty of Rectangle
    | PiecewiseAffine of (pointF list)*color*float*Rectangle
    | FilledPolygon of (pointF list)*color*Rectangle
    | Rectangle of color*float*Rectangle
    | FilledRectangle of color*Rectangle
    | Ellipse of color*float*Rectangle
    | FilledEllipse of color*Rectangle
    | Text of string * color * float * Font * Rectangle
    | AlignH of PrimitiveTree*PrimitiveTree*float*Rectangle
    | AlignV of PrimitiveTree*PrimitiveTree*float*Rectangle
    | Onto of PrimitiveTree*PrimitiveTree*Rectangle
    | Scale of PrimitiveTree*float*float*Rectangle
    | Rotate of PrimitiveTree*float*float*float*Rectangle
    | Translate of PrimitiveTree*float*float*Rectangle

let getSize ((x1,y1,x2,y2):Rectangle) : Size =
        (x2-x1,y2-y1) // always positive!
let getRectangle (p:PrimitiveTree): Rectangle =
    match p with
        | Empty(rect)
        | PiecewiseAffine(_,_,_,rect)
        | FilledPolygon(_,_,rect)
        | Rectangle(_,_,rect)
        | FilledRectangle(_,rect)
        | Ellipse(_,_,rect)
        | FilledEllipse(_,rect)
        | Text(_,_,_,_,rect)
        | AlignH(_,_,_,rect)
        | AlignV(_,_,_,rect)
        | Onto(_,_,rect)
        | Scale(_,_,_,rect)
        | Rotate(_,_,_,_,rect)
        | Translate(_,_,_,rect) -> rect

let tostring (p:PrimitiveTree): string =
    let rec loop (prefix:string) (p:PrimitiveTree): string =
        let descentPrefix = (String.replicate prefix.Length " ")+"\u221F>"
        match p with
            | Empty(rect) -> 
                sprintf "%sEmpty" prefix
            | PiecewiseAffine(points,c,sw,rect) -> 
                sprintf "%sPiecewiseAffine (color,stroke)=%A coordinates=%O" prefix (c,sw) points
            | FilledPolygon(points,c,rect) -> 
                sprintf "%sFilledPolygon color=%A coordinates=%A" prefix c points
            | Rectangle(c,sw,rect) -> 
                sprintf "%sRectangle (color,stroke)=%A coordinates=%A" prefix (c,sw) rect
            | FilledRectangle(c,rect) -> 
                sprintf "%sFilledRectangle color%A cordinates=%A" prefix c rect
            | Ellipse(c,sw,rect) -> 
                let w,h = getSize rect
                sprintf "%sEllipse (color,stroke)=%A (radiusX,radiusY)=%A" prefix (c,sw) (w/2.0,h/2.0)
            | FilledEllipse(c,rect) -> 
                let w,h = getSize rect
                sprintf "%sFilledEllipse color=%A (radiusX,radiusY)=%A" prefix c (w/2.0,h/2.0)
            | Text(txt,c,sw,font,rect) ->
                sprintf "%sText (color,stroke)=%A font=%A %A" prefix (c,sw) font txt
            | AlignH(p1,p2,pos,rect) -> 
                sprintf "%sAlignH position=%g\n%s\n%s" prefix pos (loop descentPrefix p1) (loop descentPrefix p2)
            | AlignV(p1,p2,pos,rect) -> 
                sprintf "%sAlignV position=%g\n%s\n%s" prefix pos (loop descentPrefix p1) (loop descentPrefix p2)
            | Onto(p1,p2,rect) -> 
                sprintf "%sOnto\n%s\n%s" prefix (loop descentPrefix p1) (loop descentPrefix p2)
            | Scale(q,sx,sy,rect) -> 
                sprintf "%sScale (scaleX,scaleY)=%A\n%s" prefix (sx, sy) (loop descentPrefix q)
            | Rotate(q,x,y,rad,rect) -> 
                sprintf "%sRotate (centerX,centerY)=%A radius=%g\n%s" prefix (x, y) rad (loop descentPrefix q)
            | Translate(q,dx,dy,rect) -> 
                sprintf "%sTranslate (x,y)=%A\n%s" prefix (dx, dy) (loop descentPrefix q)
    loop "" p

/// Graphics primitives
//let affineLowlevel.transform (M:System.Numerics.Matrix3x2) (p: PrimitiveTree): PrimitiveTree = 
//  let sz = getSize <| getRectangle p
//  AffineTransform(p,M,w,h)
let translate (dx:float) (dy:float) (p: PrimitiveTree): PrimitiveTree =
    let (x1,y1,x2,y2) = getRectangle p
    Translate(p,dx,dy,(x1+dx,y1+dy,x2+dx,y2+dy))
let scale (sx:float) (sy:float) (p: PrimitiveTree): PrimitiveTree =
    let (x1,y1,x2,y2) = getRectangle p
    Scale(p,sx,sy,(sx*x1,sy*y1,sx*x2,sy*y2))
let rotate (x:float) (y:float) (rad:float) (p: PrimitiveTree): PrimitiveTree =
    let rect = getRectangle p
    Rotate(p,x,y,rad,rect) // FIXME: what should the bounding box be here? Enlarge or clipped?

let mapPairLst (f: 'a list -> 'b) (lst: ('a*'a) list): 'b*'b =
    List.unzip lst |> fun (a,b) -> f a, f b
let piecewiseaffine (c:color) (sw: float) (lst: (float*float) list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let rect = (wMin,hMin,wMax,hMax)
    PiecewiseAffine(lst, c, sw, rect) 
let filledpolygon (c:color) (lst: (float*float) list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let rect = (wMin,hMin,wMax,hMax)
    FilledPolygon(lst, c, rect)
let rectangle (c: color) (sw: float) (w: float) (h: float): PrimitiveTree = 
    Rectangle(c,sw,(0.0,0.0,w,h)) 
let filledrectangle (c: color) (w: float) (h: float): PrimitiveTree = 
    FilledRectangle(c,(0.0,0.0,w,h)) 
let ellipse (c: color) (sw: float) (rx: float) (ry:float): PrimitiveTree = 
    Ellipse(c,sw,(-rx,-ry,rx,ry)) 
let filledellipse (c: color) (rx: float) (ry:float): PrimitiveTree = 
    FilledEllipse(c, (-rx,-ry,rx,ry)) 
let text (c: color) (sw:float) (f: Font) (txt:string): PrimitiveTree = 
    let w,h = measureText f txt
    Text(txt, c, sw, f, (0.0,0.0,w,h)) 
let emptyTree = Empty((0.0,0.0,0.0,0.0))

/// Functions for combining images
let Top = 0.0
let Left = 0.0
let Center = 0.5
let Bottom = 1.0
let Right = 1.0

let rec onto (pic1:PrimitiveTree) (pic2:PrimitiveTree): PrimitiveTree =
    let x11,y11,x21,y21 = getRectangle pic1
    let x12,y12,x22,y22 = getRectangle pic2
    let rect = (min x11 x12, min y11 y12, max x21 x22, max y21 y22)
    Onto(pic1, pic2, rect)
and alignh (pic1:PrimitiveTree) (pos:float) (pic2:PrimitiveTree): PrimitiveTree =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("ppos must be in [0,1]"))
    let x11,y11,x21,y21 = getRectangle pic1
    let x12,y12,x22,y22 = getRectangle pic2
    let w1,h1 = getSize <| getRectangle pic1
    let w2,h2 = getSize <| getRectangle pic2
    let s = pos*(h1-h2)
    let x12a = x12+x21-x12
    let x22a = x22+x21-x12
    let y12a = y12+y11-y12+s
    let y22a = y22+y11-y12+s
    let rect = (x11, min y11 y12a, x22a, max y21 y22a)
    AlignH(pic1,pic2,pos,rect)
and alignv (pic1:PrimitiveTree) (pos:float) (pic2:PrimitiveTree): PrimitiveTree =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
    let x11,y11,x21,y21 = getRectangle pic1
    let x12,y12,x22,y22 = getRectangle pic2
    let w1,h1 = getSize <| getRectangle pic1
    let w2,h2 = getSize <| getRectangle pic2
    let s = pos*(w1-w2)
    let x12a = x12+x11-x12+s
    let x22a = x22+x11-x12+s
    let y12a = y12+y21-y12
    let y22a = y22+y21-y12
    let rect = (min x11 x12a, y11, max x21 x22a, y22a)
    AlignV(pic1, pic2, pos, rect)

/// Drawing content
let colorLst = [color.Blue; color.Cyan; color.Green; color.Magenta; color.Orange; color.Purple; color.Yellow; color.Red]
let _ellipse (t:Lowlevel.Tool) ((x1,y1,x2,y2):Rectangle): Lowlevel.PathTree =
    let cx, cy = ((x1+x2)/2.0), ((y1+y2)/2.0)
    let rx, ry = ((x2-x1)/2.0), ((y2-y1)/2.0)
    let n = int <| max 10.0 ((max rx ry)*10.0)
    let lst = 
        [0..(n-1)]
            |> List.map (fun i -> 2.0*System.Math.PI*(float i)/(float (n-1)))
            |> List.map (fun i -> (cx+rx*cos i, cy+ry*sin i))
    Lowlevel.Prim (t, Lowlevel.Lines lst)
let _rectangle (t:Lowlevel.Tool) ((x1,y1,x2,y2):Rectangle): Lowlevel.PathTree =
        let lst = [(x1,y1);(x1,y2);(x2,y2);(x2,y1);(x1,y1)]
        Lowlevel.Prim (t, Lowlevel.Lines lst)
let rec compile (idx:int) (expFlag: bool) (pic:PrimitiveTree): Lowlevel.PathTree =
    let next = (idx+1) % colorLst.Length
    let wrap M rect expFlag dc =
        if expFlag then
            let pen = Lowlevel.solidPen colorLst[idx] 1.0
            let b = _rectangle pen rect
            Lowlevel.(<+>) dc (Lowlevel.transform M <| b)
        else
            dc
    match pic with
    | Empty(rect) -> 
        Lowlevel.Empty
    | PiecewiseAffine(lst, c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim (pen, Lowlevel.Lines lst)
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledPolygon(lst, c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim (brush, Lowlevel.Lines lst)
        wrap Matrix3x2.Identity rect expFlag dc
    | Rectangle(c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = _rectangle pen rect
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledRectangle(c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = _rectangle brush rect
        wrap Matrix3x2.Identity rect expFlag dc
    | Ellipse(c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = _ellipse pen rect
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledEllipse(c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = _ellipse brush rect
        wrap Matrix3x2.Identity rect expFlag dc
    | Text(txt, c, sw, font, rect) ->
        let pen = Lowlevel.solidPen c sw
        let opt = Lowlevel.TextOptions(font)
        let dc = Lowlevel.Text (pen, txt, opt)
        wrap Matrix3x2.Identity rect expFlag dc
    | Onto(p1, p2, rect) ->
        let dc1 = compile next expFlag p1
        let dc2 = compile next expFlag p2
        let dc = Lowlevel.(<+>) dc2 dc1 // dc2 is drawn first
        wrap Matrix3x2.Identity rect expFlag dc
    | AlignH(p1, p2, pos, rect) ->
        let x11,y11,x21,y21 = getRectangle p1
        let x12,y12,x22,y22 = getRectangle p2
        let w1,h1 = getSize (x11,y11,x21,y21)
        let w2,h2 = getSize (x12,y12,x22,y22)
        let s = pos*(h1-h2)
        let dc1 = compile next expFlag p1
        let dc2 = compile next expFlag p2
        let dc =
            let M = Matrix3x2.CreateTranslation(float32 (x21-x12),float32 (y11-y12+s))
            Lowlevel.(<+>) dc1 (Lowlevel.transform M <| dc2)
        wrap Matrix3x2.Identity rect expFlag dc
    | AlignV(p1, p2, pos, rect) ->
        let x11,y11,x21,y21 = getRectangle p1
        let x12,y12,x22,y22 = getRectangle p2
        let w1,h1 = getSize (x11,y11,x21,y21)
        let w2,h2 = getSize (x12,y12,x22,y22)
        let s = pos*(w1-w2)
        let dc1 = compile next expFlag p1 
        let dc2 = compile next expFlag p2
        let dc =
            let M = Matrix3x2.CreateTranslation(float32 (x11-x12+s),float32 (y21-y12))
            Lowlevel.(<+>) dc1 (Lowlevel.transform M <| dc2)
        wrap Matrix3x2.Identity rect expFlag dc
    | Translate(p, dx, dy, rect) ->
        let M = Matrix3x2.CreateTranslation(float32 dx,float32 dy)
        Lowlevel.transform M <| compile next expFlag p
        //wrap M rect false dc
    | Scale(p, sx, sy, rect) -> 
        let M = Matrix3x2.CreateScale(float32 sx, float32 sy)
        Lowlevel.transform M <| compile next expFlag p
        //wrap M rect false dc
    | Rotate(p, cx, cy, rad, rect) ->
        let R = Matrix3x2.CreateRotation(float32 rad)
        let T1 = Matrix3x2.CreateTranslation(float32 cx,float32 cy)
        let T2 = Matrix3x2.CreateTranslation(float32 -cx,float32 -cy)
        let M = T2*R*T1
        Lowlevel.transform M <| compile next expFlag p
        //wrap M rect false dc

let make (p:PrimitiveTree): Picture = 
    compile 0 false p |> Lowlevel.drawPathTree
let explain (p:PrimitiveTree): Picture = 
    compile 0 true p |> Lowlevel.drawPathTree
