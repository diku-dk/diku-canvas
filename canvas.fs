module Canvas
open System.Numerics

/// Types
type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush
type color = Color.color
type Font = Font of Lowlevel.Font
type FontFamily = FontFamily of Lowlevel.FontFamily
let systemFontNames = Lowlevel.systemFontNames
let getFamily (name:string) = Lowlevel.getFamily name |> FontFamily
let makeFont (fam:string) (size:float) = Lowlevel.makeFont (Lowlevel.getFamily fam) size |> Font
let measureText (Font f) (txt:string) = Lowlevel.measureText f txt

type pointF = Lowlevel.pointF
type Picture = Picture of Lowlevel.drawing_fun
type point = pointF

let correct draw s = let (Picture pic) = draw s in pic

type Event =
    /// A key with at letter is pressed
    | Key of key: char
    /// A key is released
    | KeyUp of key: string
    /// Down arrow is pressed
    | DownArrow
    /// Up arrow is pressed
    | UpArrow
    /// Left arrow is pressed
    | LeftArrow
    /// Right arrow is pressed
    | RightArrow
    /// Return key is pressed
    | Return
    /// The left mouse button is clicked at coordinate `(x,y)`
    | MouseButtonDown of x: int * y: int // x,y
    /// The left mouse button is released at coordinate `(x,y)`
    | MouseButtonUp of x: int * y: int // x,y
    /// The mouse is moved
    | MouseMotion of x: int * y: int * relx: int * rely: int // x,y, relx, rely
    /// A tick event from the timer
    | TimerTick

let fromLowlevelEvent = function
    | Lowlevel.Key t -> Key t
    | Lowlevel.KeyUp t -> KeyUp t
    | Lowlevel.DownArrow -> DownArrow
    | Lowlevel.UpArrow -> UpArrow
    | Lowlevel.LeftArrow -> LeftArrow
    | Lowlevel.RightArrow -> RightArrow
    | Lowlevel.Return -> Return
    | Lowlevel.MouseButtonDown (x,y) -> MouseButtonDown (x,y)
    | Lowlevel.MouseButtonUp (x,y) -> MouseButtonUp (x,y)
    | Lowlevel.MouseMotion (x, y, relx, rely) -> MouseMotion (x, y, relx, rely)
    | Lowlevel.TimerTick -> TimerTick

let renderToFile width height filePath (Picture draw) = Lowlevel.drawToFile width height filePath draw
let animateToFile width height frameDelay repeatCount filePath drawLst =
    Lowlevel.drawToAnimatedGif width height frameDelay repeatCount filePath
                               (List.map (function Picture draw -> draw) drawLst)
let interact t w h interval draw react s =
    Lowlevel.runAppWithTimer t w h interval (correct draw) (fun s ev -> react s (fromLowlevelEvent ev)) s
let render t w h (Picture draw) = Lowlevel.runApp t w h (fun _ -> draw)
let fromRgba r g b a = Color.fromRgba r g b a
let fromRgb r g b = Color.fromRgb r g b

type Point = float*float
type Rectangle = Point*Point // (x1,y1),(x2,y2): x2>x1 && y2>y1
type Size = float*float // w,h both non-negative
type PrimitiveTree = 
    | Empty of Rectangle
    | PiecewiseAffine of (pointF list)*color*float*Rectangle
    | FilledPolygon of (pointF list)*color*Rectangle
    | Rectangle of color*float*Rectangle
    | FilledRectangle of color*Rectangle
    | Arc of pointF * float * float * float * float * color * float * Rectangle
    | FilledArc of pointF * float * float * float * float * color * Rectangle
    | CubicBezier of pointF * pointF * pointF * pointF * color * float * Rectangle
    | FilledCubicBezier of pointF * pointF * pointF * pointF * color * Rectangle
    | Ellipse of color*float*Rectangle
    | FilledEllipse of color*Rectangle
    | Text of string * color * Font * Rectangle
    | AlignH of PrimitiveTree*PrimitiveTree*float*Rectangle
    | AlignV of PrimitiveTree*PrimitiveTree*float*Rectangle
    | Onto of PrimitiveTree*PrimitiveTree*Rectangle
    | Scale of PrimitiveTree*float*float*Rectangle
    | Rotate of PrimitiveTree*float*float*float*Rectangle
    | Translate of PrimitiveTree*float*float*Rectangle

let getSize (((x1,y1),(x2,y2)):Rectangle) : Size =
        (abs (x2-x1),abs (y2-y1))
let getBoundingBox (p:PrimitiveTree): Rectangle =
    match p with
        | Empty(rect)
        | PiecewiseAffine(_,_,_,rect)
        | FilledPolygon(_,_,rect)
        | Rectangle(_,_,rect)
        | FilledRectangle(_,rect)
        | Arc(_,_,_,_,_,_,_,rect)
        | FilledArc(_,_,_,_,_,_,rect)
        | CubicBezier(_,_,_,_,_,_,rect)
        | FilledCubicBezier(_,_,_,_,_,rect)
        | Ellipse(_,_,rect)
        | FilledEllipse(_,rect)
        | Text(_,_,_,rect)
        | AlignH(_,_,_,rect)
        | AlignV(_,_,_,rect)
        | Onto(_,_,rect)
        | Scale(_,_,_,rect)
        | Rotate(_,_,_,_,rect)
        | Translate(_,_,_,rect) -> rect

let toString (p:PrimitiveTree): string =
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
            | Arc(center,rx,ry,start,sweep,c,sw,rect) ->
                sprintf "%sArc (color,stroke)=%A center=%A radii=%A start=%A sweep=%A" prefix (c,sw) center (rx,ry) start sweep
            | FilledArc(center,rx,ry,start,sweep,c,rect) ->
                sprintf "%sFilledArc color=%A center=%A radii=%A start=%A sweep=%A" prefix c center (rx,ry) start sweep
            | CubicBezier(point1,point2,point3,point4,c,sw,rect) ->
                sprintf "%sCubicBezier (color,stroke)=%A coordinates=%A" prefix (c,sw) [point1,point2,point3,point4]
            | FilledCubicBezier(point1,point2,point3,point4,c,rect) ->
                sprintf "%sFilledCubicBezier color=%A coordinates=%A" prefix c [point1,point2,point3,point4]
            | Ellipse(c,sw,rect) -> 
                let w,h = getSize rect
                sprintf "%sEllipse (color,stroke)=%A (radiusX,radiusY)=%A" prefix (c,sw) (w/2.0,h/2.0)
            | FilledEllipse(c,rect) -> 
                let w,h = getSize rect
                sprintf "%sFilledEllipse color=%A (radiusX,radiusY)=%A" prefix c (w/2.0,h/2.0)
            | Text(txt,c,font,rect) ->
                sprintf "%sText color=%A font=%A %A" prefix c font txt
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
let translate (dx:float) (dy:float) (p: PrimitiveTree): PrimitiveTree =
    let ((x1,y1),(x2,y2)) = getBoundingBox p
    Translate(p,dx,dy,((x1+dx,y1+dy),(x2+dx,y2+dy)))
let scale (sx:float) (sy:float) (p: PrimitiveTree): PrimitiveTree =
    let ((x1,y1),(x2,y2)) = getBoundingBox p
    if abs (sx*(x2-x1)) < 1.0 || abs (sy*(y2-y1)) < 1.0 then 
        Empty(((0.0,0.0),(0.0,0.0)))
    else
        Scale(p,sx,sy,((sx*x1,sy*y1),(sx*x2,sy*y2)))
let rotate (x:float) (y:float) (rad:float) (p: PrimitiveTree): PrimitiveTree =
    let rect = getBoundingBox p
    Rotate(p,x,y,rad,rect) // FIXME: what should the bounding box be here? Enlarge or clipped?

let mapPairLst (f: 'a list -> 'b) (lst: ('a*'a) list): 'b*'b =
    List.unzip lst |> fun (a,b) -> f a, f b
let piecewiseAffine (c:color) (sw: float) (lst: Point list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let rect = ((wMin,hMin),(wMax,hMax))
    PiecewiseAffine(lst, c, sw, rect) 
let filledPolygon (c:color) (lst: Point list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let rect = ((wMin,hMin),(wMax,hMax))
    FilledPolygon(lst, c, rect)
let rectangle (c: color) (sw: float) (w: float) (h: float): PrimitiveTree = 
    Rectangle(c,sw,((0.0,0.0),(w,h))) 
let filledRectangle (c: color) (w: float) (h: float): PrimitiveTree = 
    FilledRectangle(c,((0.0,0.0),(w,h))) 
let arc (c:color) (sw:float) (center:Point) (rx:float) (ry:float) (start:float) (sweep:float) =
    Arc(center,rx,ry,start,sweep,c,sw,(((fst center)-rx,(snd center)-ry),((fst center)+rx,(snd center)+ry)))
let filledArc (c:color) (center:Point) (rx:float) (ry:float) (start:float) (sweep:float) =
    FilledArc(center,rx,ry,start,sweep,c,(((fst center)-rx,(snd center)-ry),((fst center)+rx,(snd center)+ry)))
let cubicBezier (c:color) (sw:float) (point1:Point) (point2:Point) (point3:Point) (point4:Point) =
    let xLst = List.map fst [point1;point2;point3;point4]
    let yLst = List.map snd [point1;point2;point3;point4]
    CubicBezier(point1,point2,point3,point4,c,sw,((List.min xLst, List.min yLst), (List.max xLst, List.max yLst)))
let filledCubicBezier (c:color) (point1:Point) (point2:Point) (point3:Point) (point4:Point) =
    let xLst = List.map fst [point1;point2;point3;point4]
    let yLst = List.map snd [point1;point2;point3;point4]
    FilledCubicBezier(point1,point2,point3,point4,c,((List.min xLst, List.min yLst), (List.max xLst, List.max yLst)))
let ellipse (c: color) (sw: float) (rx: float) (ry:float): PrimitiveTree = 
    Ellipse(c,sw,((-rx,-ry),(rx,ry))) 
let filledEllipse (c: color) (rx: float) (ry:float): PrimitiveTree = 
    FilledEllipse(c, ((-rx,-ry),(rx,ry))) 
let text (c: color) (f: Font) (txt:string): PrimitiveTree =
    let w,h = measureText f txt
    Text(txt, c, f, ((0.0,0.0),(w,h))) 
let emptyTree = Empty(((0.0,0.0),(0.0,0.0)))

/// Functions for combining images
type Position = Position of float
let Top = Position 0.0
let Left = Position 0.0
let Center = Position 0.5
let Bottom = Position 1.0
let Right = Position 1.0

let rec onto (pic1:PrimitiveTree) (pic2:PrimitiveTree): PrimitiveTree =
    let (x11,y11),(x21,y21) = getBoundingBox pic1
    let (x12,y12),(x22,y22) = getBoundingBox pic2
    let rect = ((min x11 x12, min y11 y12), (max x21 x22, max y21 y22))
    Onto(pic1, pic2, rect)
and alignH (pic1:PrimitiveTree) (Position pos) (pic2:PrimitiveTree): PrimitiveTree = //FIXME: The positions are possibly flipped since y is down
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("ppos must be in [0,1]"))
    let rect1 = getBoundingBox pic1
    let rect2 = getBoundingBox pic2
    let _,h1 = getSize rect1
    let _,h2 = getSize rect2
    let s = pos*(h1-h2)
    let (x11,y11),(x21,y21) = rect1
    let (x12,y12),(x22,y22) = rect2
    let x12a = x12+x21-x12
    let x22a = x22+x21-x12
    let y12a = y12+y11-y12+s
    let y22a = y22+y11-y12+s
    let rect = ((x11, min y11 y12a), (x22a, max y21 y22a))
    AlignH(pic1,pic2,pos,rect)
and alignV (pic1:PrimitiveTree) (Position pos) (pic2:PrimitiveTree): PrimitiveTree =
    if pos < 0 || pos > 1 then 
        raise (System.ArgumentOutOfRangeException ("pos must be in [0,1]"))
    let rect1 = getBoundingBox pic1
    let rect2 = getBoundingBox pic2
    let w1,_ = getSize rect1
    let w2,_ = getSize rect2
    let s = pos*(w1-w2)
    let (x11,y11),(x21,y21) = rect1
    let (x12,y12),(x22,y22) = rect2
    let x12a = x12+x11-x12+s
    let x22a = x22+x11-x12+s
    let y12a = y12+y21-y12
    let y22a = y22+y21-y12
    let rect = ((min x11 x12a, y11), (max x21 x22a, y22a))
    AlignV(pic1, pic2, pos, rect)

/// Drawing content
let colorLst = [Lowlevel.Color.Blue; Lowlevel.Color.Cyan; Lowlevel.Color.Green; Lowlevel.Color.Magenta; Lowlevel.Color.Orange; Lowlevel.Color.Purple; Lowlevel.Color.Yellow; Lowlevel.Color.Red]
let _ellipse (t:Lowlevel.Tool) (((x1,y1),(x2,y2)):Rectangle): Lowlevel.PathTree =
    let cx, cy = ((x1+x2)/2.0), ((y1+y2)/2.0)
    let rx, ry = ((x2-x1)/2.0), ((y2-y1)/2.0)
    let n = int <| max 10.0 ((max rx ry)*10.0)
    let lst = 
        [0..(n-1)]
            |> List.map (fun i -> 2.0*System.Math.PI*(float i)/(float (n-1)))
            |> List.map (fun i -> (cx+rx*cos i, cy+ry*sin i))
    Lowlevel.Prim (t, Lowlevel.Lines lst)
let _rectangle (t:Lowlevel.Tool) (((x1,y1),(x2,y2)):Rectangle): Lowlevel.PathTree =
        let lst = [(x1,y1);(x1,y2);(x2,y2);(x2,y1);(x1,y1)]
        Lowlevel.Prim (t, Lowlevel.Lines lst)

// A CPS monad
type ContinuationBuilder() =
    member this.Return x = fun k -> k x
    member this.ReturnFrom x = x
    member this.Bind (m, f) = (fun k -> m (fun a -> f a k))
    member this.Delay f = fun k -> f () k

let cps = ContinuationBuilder()


let rec compileCps idx expFlag pic = cps {
    let next = (idx+1) % colorLst.Length
    let wrap M rect dc =
        if expFlag then
            let pen = Lowlevel.solidPen colorLst[idx] 1.0
            let b = _rectangle pen rect
            Lowlevel.(<+>) dc (Lowlevel.transform M <| b)
        else
            dc
    match pic with
    | Empty _ ->
        return Lowlevel.Empty
    | PiecewiseAffine(lst, Color.Color c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim (pen, Lowlevel.Lines lst)
        return wrap Matrix3x2.Identity rect dc
    | FilledPolygon(lst, Color.Color c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim (brush, Lowlevel.Lines lst)
        return wrap Matrix3x2.Identity rect dc
    | Rectangle(Color.Color c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = _rectangle pen rect
        return wrap Matrix3x2.Identity rect dc
    | FilledRectangle(Color.Color c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = _rectangle brush rect
        return wrap Matrix3x2.Identity rect dc
    | Arc(center,rx,ry,start,sweep,Color.Color c,sw,rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim(pen, Lowlevel.Arc(center,rx,ry,0.0,start,sweep))
        return wrap Matrix3x2.Identity rect dc
    | FilledArc(center,rx,ry,start,sweep,Color.Color c,rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim(brush,Lowlevel.Arc(center,rx,ry,0.0,start,sweep))
        return wrap Matrix3x2.Identity rect dc
    | CubicBezier(point1,point2,point3,point4,Color.Color c,sw,rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim(pen, Lowlevel.CubicBezier(point1,point2,point3,point4))
        return wrap Matrix3x2.Identity rect dc
    | FilledCubicBezier(point1,point2,point3,point4,Color.Color c,rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim(brush, Lowlevel.CubicBezier(point1,point2,point3,point4))
        return wrap Matrix3x2.Identity rect dc
    | Ellipse(Color.Color c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = _ellipse pen rect
        return wrap Matrix3x2.Identity rect dc
    | FilledEllipse(Color.Color c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = _ellipse brush rect
        return wrap Matrix3x2.Identity rect dc
    | Text(txt, Color.Color c, Font font, rect) ->
        let brush = Lowlevel.solidBrush c
        let opt = Lowlevel.TextOptions font
        let dc = Lowlevel.Text (brush, txt, opt)
        return wrap Matrix3x2.Identity rect dc
    | Onto(p1, p2, rect) ->
        let! dc1 = compileCps next expFlag p1
        let! dc2 = compileCps next expFlag p2
        let dc = Lowlevel.(<+>) dc2 dc1 // dc2 is drawn first
        return wrap Matrix3x2.Identity rect dc
    | AlignH(p1, p2, pos, rect) ->
        let rect1 = getBoundingBox p1
        let rect2 = getBoundingBox p2
        let _,h1 = getSize rect1
        let _,h2 = getSize rect2
        let s = pos*(h1-h2)
        let (x11,y11),(x21,y21) = rect1
        let (x12,y12),(x22,y22) = rect2
        let! dc1 = compileCps next expFlag p1
        let! dc2 = compileCps next expFlag p2
        let dc =
            let M = Matrix3x2.CreateTranslation(float32 (x21-x12),float32 (y11-y12+s))
            Lowlevel.(<+>) dc1 (Lowlevel.transform M <| dc2)
        return wrap Matrix3x2.Identity rect dc
    | AlignV(p1, p2, pos, rect) ->
        let rect1 = getBoundingBox p1
        let rect2 = getBoundingBox p2
        let w1,_ = getSize rect1
        let w2,_ = getSize rect2
        let s = pos*(w1-w2)
        let (x11,y11),(x21,y21) = rect1
        let (x12,y12),(x22,y22) = rect2
        let! dc1 = compileCps next expFlag p1
        let! dc2 = compileCps next expFlag p2
        let dc =
            let M = Matrix3x2.CreateTranslation(float32 (x11-x12+s),float32 (y21-y12))
            Lowlevel.(<+>) dc1 (Lowlevel.transform M <| dc2)
        return wrap Matrix3x2.Identity rect dc
    | Translate(p, dx, dy, rect) ->
        let M = Matrix3x2.CreateTranslation(float32 dx,float32 dy)
        let! dc = compileCps next expFlag p
        return Lowlevel.transform M dc
        //wrap M rect false dc
    | Scale(p, sx, sy, rect) -> 
        let M = Matrix3x2.CreateScale(float32 sx, float32 sy)
        let! dc = compileCps next expFlag p
        return Lowlevel.transform M dc
        //wrap M rect false dc
    | Rotate(p, cx, cy, rad, rect) ->
        let R = Matrix3x2.CreateRotation(float32 rad)
        let T1 = Matrix3x2.CreateTranslation(float32 cx,float32 cy)
        let T2 = Matrix3x2.CreateTranslation(float32 -cx,float32 -cy)
        let M = T2*R*T1
        let! dc = compileCps next expFlag p
        return Lowlevel.transform M dc
        //wrap M rect false dc
    }

let compile (idx:int) (expFlag: bool) (pic:PrimitiveTree): Lowlevel.PathTree =
    compileCps idx expFlag pic id


let make (p:PrimitiveTree): Picture = 
    compile 0 false p |> Lowlevel.drawPathTree |> Picture
let explain (p:PrimitiveTree): Picture = 
    compile 0 true p |> Lowlevel.drawPathTree |> Picture
