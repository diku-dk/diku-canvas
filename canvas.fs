module Canvas
open System.Numerics

/// Types
type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush
type color = Color of Lowlevel.Color
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
    | Key of char
    | DownArrow
    | UpArrow
    | LeftArrow
    | RightArrow
    | Return
    | MouseButtonDown of int * int // x,y
    | MouseButtonUp of int * int // x,y
    | MouseMotion of int * int * int * int // x,y, relx, rely
    | TimerTick

let fromLowlevelEvent = function
    | Lowlevel.Key t -> Key t
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
let render t w h draw = Lowlevel.runApp t w h (correct draw) // FIXME: there is no reason for render to accept draw on functional form.
let fromRgba r g b a = Lowlevel.fromRgba r g b a |> Color
let fromRgb r g b = Lowlevel.fromRgb r g b |> Color

let aliceBlue : color = Color Lowlevel.Color.AliceBlue
let antiqueWhite : color = Color Lowlevel.Color.AntiqueWhite
let aqua : color = Color Lowlevel.Color.Aqua
let aquamarine : color = Color Lowlevel.Color.Aquamarine
let azure : color = Color Lowlevel.Color.Azure
let beige : color = Color Lowlevel.Color.Beige
let bisque : color = Color Lowlevel.Color.Bisque
let black : color = Color Lowlevel.Color.Black
let blanchedAlmond : color = Color Lowlevel.Color.BlanchedAlmond
let blue : color = Color Lowlevel.Color.Blue
let blueViolet : color = Color Lowlevel.Color.BlueViolet
let brown : color = Color Lowlevel.Color.Brown
let burlyWood : color = Color Lowlevel.Color.BurlyWood
let cadetBlue : color = Color Lowlevel.Color.CadetBlue
let chartreuse : color = Color Lowlevel.Color.Chartreuse
let chocolate : color = Color Lowlevel.Color.Chocolate
let coral : color = Color Lowlevel.Color.Coral
let cornflowerBlue : color = Color Lowlevel.Color.CornflowerBlue
let cornsilk : color = Color Lowlevel.Color.Cornsilk
let crimson : color = Color Lowlevel.Color.Crimson
let cyan : color = Color Lowlevel.Color.Cyan
let darkBlue : color = Color Lowlevel.Color.DarkBlue
let darkCyan : color = Color Lowlevel.Color.DarkCyan
let darkGoldenrod : color = Color Lowlevel.Color.DarkGoldenrod
let darkGray : color = Color Lowlevel.Color.DarkGray
let darkGreen : color = Color Lowlevel.Color.DarkGreen
let darkGrey : color = Color Lowlevel.Color.DarkGrey
let darkKhaki : color = Color Lowlevel.Color.DarkKhaki
let darkMagenta : color = Color Lowlevel.Color.DarkMagenta
let darkOliveGreen : color = Color Lowlevel.Color.DarkOliveGreen
let darkOrange : color = Color Lowlevel.Color.DarkOrange
let darkOrchid : color = Color Lowlevel.Color.DarkOrchid
let darkRed : color = Color Lowlevel.Color.DarkRed
let darkSalmon : color = Color Lowlevel.Color.DarkSalmon
let darkSeaGreen : color = Color Lowlevel.Color.DarkSeaGreen
let darkSlateBlue : color = Color Lowlevel.Color.DarkSlateBlue
let darkSlateGray : color = Color Lowlevel.Color.DarkSlateGray
let darkSlateGrey : color = Color Lowlevel.Color.DarkSlateGrey
let darkTurquoise : color = Color Lowlevel.Color.DarkTurquoise
let darkViolet : color = Color Lowlevel.Color.DarkViolet
let deepPink : color = Color Lowlevel.Color.DeepPink
let deepSkyBlue : color = Color Lowlevel.Color.DeepSkyBlue
let dimGray : color = Color Lowlevel.Color.DimGray
let dimGrey : color = Color Lowlevel.Color.DimGrey
let dodgerBlue : color = Color Lowlevel.Color.DodgerBlue
let firebrick : color = Color Lowlevel.Color.Firebrick
let floralWhite : color = Color Lowlevel.Color.FloralWhite
let forestGreen : color = Color Lowlevel.Color.ForestGreen
let fuchsia : color = Color Lowlevel.Color.Fuchsia
let gainsboro : color = Color Lowlevel.Color.Gainsboro
let ghostWhite : color = Color Lowlevel.Color.GhostWhite
let gold : color = Color Lowlevel.Color.Gold
let goldenrod : color = Color Lowlevel.Color.Goldenrod
let gray : color = Color Lowlevel.Color.Gray
let green : color = Color Lowlevel.Color.Green
let greenYellow : color = Color Lowlevel.Color.GreenYellow
let grey : color = Color Lowlevel.Color.Grey
let honeydew : color = Color Lowlevel.Color.Honeydew
let hotPink : color = Color Lowlevel.Color.HotPink
let indianRed : color = Color Lowlevel.Color.IndianRed
let indigo : color = Color Lowlevel.Color.Indigo
let ivory : color = Color Lowlevel.Color.Ivory
let khaki : color = Color Lowlevel.Color.Khaki
let lavender : color = Color Lowlevel.Color.Lavender
let lavenderBlush : color = Color Lowlevel.Color.LavenderBlush
let lawnGreen : color = Color Lowlevel.Color.LawnGreen
let lemonChiffon : color = Color Lowlevel.Color.LemonChiffon
let lightBlue : color = Color Lowlevel.Color.LightBlue
let lightCoral : color = Color Lowlevel.Color.LightCoral
let lightCyan : color = Color Lowlevel.Color.LightCyan
let lightGoldenrodYellow : color = Color Lowlevel.Color.LightGoldenrodYellow
let lightGray : color = Color Lowlevel.Color.LightGray
let lightGreen : color = Color Lowlevel.Color.LightGreen
let lightGrey : color = Color Lowlevel.Color.LightGrey
let lightPink : color = Color Lowlevel.Color.LightPink
let lightSalmon : color = Color Lowlevel.Color.LightSalmon
let lightSeaGreen : color = Color Lowlevel.Color.LightSeaGreen
let lightSkyBlue : color = Color Lowlevel.Color.LightSkyBlue
let lightSlateGray : color = Color Lowlevel.Color.LightSlateGray
let lightSlateGrey : color = Color Lowlevel.Color.LightSlateGrey
let lightSteelBlue : color = Color Lowlevel.Color.LightSteelBlue
let lightYellow : color = Color Lowlevel.Color.LightYellow
let lime : color = Color Lowlevel.Color.Lime
let limeGreen : color = Color Lowlevel.Color.LimeGreen
let linen : color = Color Lowlevel.Color.Linen
let magenta : color = Color Lowlevel.Color.Magenta
let maroon : color = Color Lowlevel.Color.Maroon
let mediumAquamarine : color = Color Lowlevel.Color.MediumAquamarine
let mediumBlue : color = Color Lowlevel.Color.MediumBlue
let mediumOrchid : color = Color Lowlevel.Color.MediumOrchid
let mediumPurple : color = Color Lowlevel.Color.MediumPurple
let mediumSeaGreen : color = Color Lowlevel.Color.MediumSeaGreen
let mediumSlateBlue : color = Color Lowlevel.Color.MediumSlateBlue
let mediumSpringGreen : color = Color Lowlevel.Color.MediumSpringGreen
let mediumTurquoise : color = Color Lowlevel.Color.MediumTurquoise
let mediumVioletRed : color = Color Lowlevel.Color.MediumVioletRed
let midnightBlue : color = Color Lowlevel.Color.MidnightBlue
let mintCream : color = Color Lowlevel.Color.MintCream
let mistyRose : color = Color Lowlevel.Color.MistyRose
let moccasin : color = Color Lowlevel.Color.Moccasin
let navajoWhite : color = Color Lowlevel.Color.NavajoWhite
let navy : color = Color Lowlevel.Color.Navy
let oldLace : color = Color Lowlevel.Color.OldLace
let olive : color = Color Lowlevel.Color.Olive
let oliveDrab : color = Color Lowlevel.Color.OliveDrab
let orange : color = Color Lowlevel.Color.Orange
let orangeRed : color = Color Lowlevel.Color.OrangeRed
let orchid : color = Color Lowlevel.Color.Orchid
let paleGoldenrod : color = Color Lowlevel.Color.PaleGoldenrod
let paleGreen : color = Color Lowlevel.Color.PaleGreen
let paleTurquoise : color = Color Lowlevel.Color.PaleTurquoise
let paleVioletRed : color = Color Lowlevel.Color.PaleVioletRed
let papayaWhip : color = Color Lowlevel.Color.PapayaWhip
let peachPuff : color = Color Lowlevel.Color.PeachPuff
let peru : color = Color Lowlevel.Color.Peru
let pink : color = Color Lowlevel.Color.Pink
let plum : color = Color Lowlevel.Color.Plum
let powderBlue : color = Color Lowlevel.Color.PowderBlue
let purple : color = Color Lowlevel.Color.Purple
let rebeccaPurple : color = Color Lowlevel.Color.RebeccaPurple
let red : color = Color Lowlevel.Color.Red
let rosyBrown : color = Color Lowlevel.Color.RosyBrown
let royalBlue : color = Color Lowlevel.Color.RoyalBlue
let saddleBrown : color = Color Lowlevel.Color.SaddleBrown
let salmon : color = Color Lowlevel.Color.Salmon
let sandyBrown : color = Color Lowlevel.Color.SandyBrown
let seaGreen : color = Color Lowlevel.Color.SeaGreen
let seaShell : color = Color Lowlevel.Color.SeaShell
let sienna : color = Color Lowlevel.Color.Sienna
let silver : color = Color Lowlevel.Color.Silver
let skyBlue : color = Color Lowlevel.Color.SkyBlue
let slateBlue : color = Color Lowlevel.Color.SlateBlue
let slateGray : color = Color Lowlevel.Color.SlateGray
let slateGrey : color = Color Lowlevel.Color.SlateGrey
let snow : color = Color Lowlevel.Color.Snow
let springGreen : color = Color Lowlevel.Color.SpringGreen
let steelBlue : color = Color Lowlevel.Color.SteelBlue
let tan : color = Color Lowlevel.Color.Tan
let teal : color = Color Lowlevel.Color.Teal
let thistle : color = Color Lowlevel.Color.Thistle
let tomato : color = Color Lowlevel.Color.Tomato
let transparent : color = Color Lowlevel.Color.Transparent
let turquoise : color = Color Lowlevel.Color.Turquoise
let violet : color = Color Lowlevel.Color.Violet
let wheat : color = Color Lowlevel.Color.Wheat
let white : color = Color Lowlevel.Color.White
let whiteSmoke : color = Color Lowlevel.Color.WhiteSmoke
let yellow : color = Color Lowlevel.Color.Yellow
let yellowGreen : color = Color Lowlevel.Color.YellowGreen

type Rectangle = float*float*float*float // x1,y1,x2,y2: x2>x1 && y2>y1
type Size = float*float // w,h
type Point = float*float
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

let getSize ((x1,y1,x2,y2):Rectangle) : Size =
        (x2-x1,y2-y1) // always positive!
let getRectangle (p:PrimitiveTree): Rectangle = // FIXME: This should be renamed to boundingBox
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
    let (x1,y1,x2,y2) = getRectangle p
    Translate(p,dx,dy,(x1+dx,y1+dy,x2+dx,y2+dy))
let scale (sx:float) (sy:float) (p: PrimitiveTree): PrimitiveTree =
    let (x1,y1,x2,y2) = getRectangle p
    if abs (sx*(x2-x1)) < 1.0 || abs (sy*(y2-y1)) < 1.0 then 
        Empty((0.0,0.0,0.0,0.0))
    else
        Scale(p,sx,sy,(sx*x1,sy*y1,sx*x2,sy*y2))
let rotate (x:float) (y:float) (rad:float) (p: PrimitiveTree): PrimitiveTree =
    let rect = getRectangle p
    Rotate(p,x,y,rad,rect) // FIXME: what should the bounding box be here? Enlarge or clipped?

let mapPairLst (f: 'a list -> 'b) (lst: ('a*'a) list): 'b*'b =
    List.unzip lst |> fun (a,b) -> f a, f b
let piecewiseAffine (c:color) (sw: float) (lst: Point list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let rect = (wMin,hMin,wMax,hMax)
    PiecewiseAffine(lst, c, sw, rect) 
let filledPolygon (c:color) (lst: Point list): PrimitiveTree = 
    let wMin, hMin = mapPairLst List.min lst 
    let wMax, hMax = mapPairLst List.max lst
    let rect = (wMin,hMin,wMax,hMax)
    FilledPolygon(lst, c, rect)
let rectangle (c: color) (sw: float) (w: float) (h: float): PrimitiveTree = 
    Rectangle(c,sw,(0.0,0.0,w,h)) 
let filledRectangle (c: color) (w: float) (h: float): PrimitiveTree = 
    FilledRectangle(c,(0.0,0.0,w,h)) 
let arc (center:Point) (rx:float) (ry:float) (start:float) (sweep:float) (c:color) (sw:float) = // FIXME: tighter boundary box
    Arc(center,rx,ry,start,sweep,c,sw,((fst center)-rx,(snd center)-ry,(fst center)+rx,(snd center)+ry))
let filledArc (center:Point) (rx:float) (ry:float) (start:float) (sweep:float) (c:color) =
    FilledArc(center,rx,ry,start,sweep,c,((fst center)-rx,(snd center)-ry,(fst center)+rx,(snd center)+ry))
let cubicBezier (point1:Point) (point2:Point) (point3:Point) (point4:Point) (c:color) (sw:float) =
    let xLst = List.map fst [point1;point2;point3;point4]
    let yLst = List.map snd [point1;point2;point3;point4]
    CubicBezier(point1,point2,point3,point4,c,sw,(List.min xLst, List.min yLst, List.max xLst, List.max yLst))
let filledCubicBezier (point1:Point) (point2:Point) (point3:Point) (point4:Point) (c:color) =
    let xLst = List.map fst [point1;point2;point3;point4]
    let yLst = List.map snd [point1;point2;point3;point4]
    FilledCubicBezier(point1,point2,point3,point4,c,(List.min xLst, List.min yLst, List.max xLst, List.max yLst))
let ellipse (c: color) (sw: float) (rx: float) (ry:float): PrimitiveTree = 
    Ellipse(c,sw,(-rx,-ry,rx,ry)) 
let filledEllipse (c: color) (rx: float) (ry:float): PrimitiveTree = 
    FilledEllipse(c, (-rx,-ry,rx,ry)) 
let text (c: color) (sw:float) (f: Font) (txt:string): PrimitiveTree = //FIXME: remove unused sw
    let w,h = measureText f txt
    Text(txt, c, f, (0.0,0.0,w,h)) 
let emptyTree = Empty((0.0,0.0,0.0,0.0))

/// Functions for combining images
type Position = Position of float
let Top = Position 0.0
let Left = Position 0.0
let Center = Position 0.5
let Bottom = Position 1.0
let Right = Position 1.0

let rec onto (pic1:PrimitiveTree) (pic2:PrimitiveTree): PrimitiveTree =
    let x11,y11,x21,y21 = getRectangle pic1
    let x12,y12,x22,y22 = getRectangle pic2
    let rect = (min x11 x12, min y11 y12, max x21 x22, max y21 y22)
    Onto(pic1, pic2, rect)
and alignH (pic1:PrimitiveTree) (Position pos) (pic2:PrimitiveTree): PrimitiveTree = //FIXME: The positions are possibly flipped since y is down
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
and alignV (pic1:PrimitiveTree) (Position pos) (pic2:PrimitiveTree): PrimitiveTree =
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
let colorLst = [Lowlevel.Color.Blue; Lowlevel.Color.Cyan; Lowlevel.Color.Green; Lowlevel.Color.Magenta; Lowlevel.Color.Orange; Lowlevel.Color.Purple; Lowlevel.Color.Yellow; Lowlevel.Color.Red]
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
    | PiecewiseAffine(lst, Color c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim (pen, Lowlevel.Lines lst)
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledPolygon(lst, Color c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim (brush, Lowlevel.Lines lst)
        wrap Matrix3x2.Identity rect expFlag dc
    | Rectangle(Color c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = _rectangle pen rect
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledRectangle(Color c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = _rectangle brush rect
        wrap Matrix3x2.Identity rect expFlag dc
    | Arc(center,rx,ry,start,sweep,Color c,sw,rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim(pen, Lowlevel.Arc(center,rx,ry,0.0,start,sweep))
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledArc(center,rx,ry,start,sweep,Color c,rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim(brush,Lowlevel.Arc(center,rx,ry,0.0,start,sweep))
        wrap Matrix3x2.Identity rect expFlag dc
    | CubicBezier(point1,point2,point3,point4,Color c,sw,rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = Lowlevel.Prim(pen, Lowlevel.CubicBezier(point1,point2,point3,point4))
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledCubicBezier(point1,point2,point3,point4,Color c,rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = Lowlevel.Prim(brush, Lowlevel.CubicBezier(point1,point2,point3,point4))
        wrap Matrix3x2.Identity rect expFlag dc
    | Ellipse(Color c, sw, rect) ->
        let pen = Lowlevel.solidPen c sw
        let dc = _ellipse pen rect
        wrap Matrix3x2.Identity rect expFlag dc
    | FilledEllipse(Color c, rect) ->
        let brush = Lowlevel.solidBrush c 
        let dc = _ellipse brush rect
        wrap Matrix3x2.Identity rect expFlag dc
    | Text(txt, Color c, Font font, rect) ->
        let brush = Lowlevel.solidBrush c
        let opt = Lowlevel.TextOptions font
        let dc = Lowlevel.Text (brush, txt, opt)
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
    compile 0 false p |> Lowlevel.drawPathTree |> Picture
let explain (p:PrimitiveTree): Picture = 
    compile 0 true p |> Lowlevel.drawPathTree |> Picture
