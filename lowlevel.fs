module Lowlevel

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open System.Linq

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.Fonts
// colors
open SixLabors.ImageSharp
open System.Runtime.InteropServices

type Color =
    struct
        val internal color: SixLabors.ImageSharp.Color
        internal new(color: SixLabors.ImageSharp.Color) = { color = color }
    end

    static member AliceBlue = Color(SixLabors.ImageSharp.Color.AliceBlue)
    static member AntiqueWhite = Color(SixLabors.ImageSharp.Color.AntiqueWhite)
    static member Aqua = Color(SixLabors.ImageSharp.Color.Aqua)
    static member Aquamarine = Color(SixLabors.ImageSharp.Color.Aquamarine)
    static member Azure = Color(SixLabors.ImageSharp.Color.Azure)
    static member Beige = Color(SixLabors.ImageSharp.Color.Beige)
    static member Bisque = Color(SixLabors.ImageSharp.Color.Bisque)
    static member Black = Color(SixLabors.ImageSharp.Color.Black)
    static member BlanchedAlmond = Color(SixLabors.ImageSharp.Color.BlanchedAlmond)
    static member Blue = Color(SixLabors.ImageSharp.Color.Blue)
    static member BlueViolet = Color(SixLabors.ImageSharp.Color.BlueViolet)
    static member Brown = Color(SixLabors.ImageSharp.Color.Brown)
    static member BurlyWood = Color(SixLabors.ImageSharp.Color.BurlyWood)
    static member CadetBlue = Color(SixLabors.ImageSharp.Color.CadetBlue)
    static member Chartreuse = Color(SixLabors.ImageSharp.Color.Chartreuse)
    static member Chocolate = Color(SixLabors.ImageSharp.Color.Chocolate)
    static member Coral = Color(SixLabors.ImageSharp.Color.Coral)
    static member CornflowerBlue = Color(SixLabors.ImageSharp.Color.CornflowerBlue)
    static member Cornsilk = Color(SixLabors.ImageSharp.Color.Cornsilk)
    static member Crimson = Color(SixLabors.ImageSharp.Color.Crimson)
    static member Cyan = Color(SixLabors.ImageSharp.Color.Cyan)
    static member DarkBlue = Color(SixLabors.ImageSharp.Color.DarkBlue)
    static member DarkCyan = Color(SixLabors.ImageSharp.Color.DarkCyan)
    static member DarkGoldenrod = Color(SixLabors.ImageSharp.Color.DarkGoldenrod)
    static member DarkGray = Color(SixLabors.ImageSharp.Color.DarkGray)
    static member DarkGreen = Color(SixLabors.ImageSharp.Color.DarkGreen)
    static member DarkGrey = Color(SixLabors.ImageSharp.Color.DarkGrey)
    static member DarkKhaki = Color(SixLabors.ImageSharp.Color.DarkKhaki)
    static member DarkMagenta = Color(SixLabors.ImageSharp.Color.DarkMagenta)
    static member DarkOliveGreen = Color(SixLabors.ImageSharp.Color.DarkOliveGreen)
    static member DarkOrange = Color(SixLabors.ImageSharp.Color.DarkOrange)
    static member DarkOrchid = Color(SixLabors.ImageSharp.Color.DarkOrchid)
    static member DarkRed = Color(SixLabors.ImageSharp.Color.DarkRed)
    static member DarkSalmon = Color(SixLabors.ImageSharp.Color.DarkSalmon)
    static member DarkSeaGreen = Color(SixLabors.ImageSharp.Color.DarkSeaGreen)
    static member DarkSlateBlue = Color(SixLabors.ImageSharp.Color.DarkSlateBlue)
    static member DarkSlateGray = Color(SixLabors.ImageSharp.Color.DarkSlateGray)
    static member DarkSlateGrey = Color(SixLabors.ImageSharp.Color.DarkSlateGrey)
    static member DarkTurquoise = Color(SixLabors.ImageSharp.Color.DarkTurquoise)
    static member DarkViolet = Color(SixLabors.ImageSharp.Color.DarkViolet)
    static member DeepPink = Color(SixLabors.ImageSharp.Color.DeepPink)
    static member DeepSkyBlue = Color(SixLabors.ImageSharp.Color.DeepSkyBlue)
    static member DimGray = Color(SixLabors.ImageSharp.Color.DimGray)
    static member DimGrey = Color(SixLabors.ImageSharp.Color.DimGrey)
    static member DodgerBlue = Color(SixLabors.ImageSharp.Color.DodgerBlue)
    static member Firebrick = Color(SixLabors.ImageSharp.Color.Firebrick)
    static member FloralWhite = Color(SixLabors.ImageSharp.Color.FloralWhite)
    static member ForestGreen = Color(SixLabors.ImageSharp.Color.ForestGreen)
    static member Fuchsia = Color(SixLabors.ImageSharp.Color.Fuchsia)
    static member Gainsboro = Color(SixLabors.ImageSharp.Color.Gainsboro)
    static member GhostWhite = Color(SixLabors.ImageSharp.Color.GhostWhite)
    static member Gold = Color(SixLabors.ImageSharp.Color.Gold)
    static member Goldenrod = Color(SixLabors.ImageSharp.Color.Goldenrod)
    static member Gray = Color(SixLabors.ImageSharp.Color.Gray)
    static member Green = Color(SixLabors.ImageSharp.Color.Green)
    static member GreenYellow = Color(SixLabors.ImageSharp.Color.GreenYellow)
    static member Grey = Color(SixLabors.ImageSharp.Color.Grey)
    static member Honeydew = Color(SixLabors.ImageSharp.Color.Honeydew)
    static member HotPink = Color(SixLabors.ImageSharp.Color.HotPink)
    static member IndianRed = Color(SixLabors.ImageSharp.Color.IndianRed)
    static member Indigo = Color(SixLabors.ImageSharp.Color.Indigo)
    static member Ivory = Color(SixLabors.ImageSharp.Color.Ivory)
    static member Khaki = Color(SixLabors.ImageSharp.Color.Khaki)
    static member Lavender = Color(SixLabors.ImageSharp.Color.Lavender)
    static member LavenderBlush = Color(SixLabors.ImageSharp.Color.LavenderBlush)
    static member LawnGreen = Color(SixLabors.ImageSharp.Color.LawnGreen)
    static member LemonChiffon = Color(SixLabors.ImageSharp.Color.LemonChiffon)
    static member LightBlue = Color(SixLabors.ImageSharp.Color.LightBlue)
    static member LightCoral = Color(SixLabors.ImageSharp.Color.LightCoral)
    static member LightCyan = Color(SixLabors.ImageSharp.Color.LightCyan)
    static member LightGoldenrodYellow = Color(SixLabors.ImageSharp.Color.LightGoldenrodYellow)
    static member LightGray = Color(SixLabors.ImageSharp.Color.LightGray)
    static member LightGreen = Color(SixLabors.ImageSharp.Color.LightGreen)
    static member LightGrey = Color(SixLabors.ImageSharp.Color.LightGrey)
    static member LightPink = Color(SixLabors.ImageSharp.Color.LightPink)
    static member LightSalmon = Color(SixLabors.ImageSharp.Color.LightSalmon)
    static member LightSeaGreen = Color(SixLabors.ImageSharp.Color.LightSeaGreen)
    static member LightSkyBlue = Color(SixLabors.ImageSharp.Color.LightSkyBlue)
    static member LightSlateGray = Color(SixLabors.ImageSharp.Color.LightSlateGray)
    static member LightSlateGrey = Color(SixLabors.ImageSharp.Color.LightSlateGrey)
    static member LightSteelBlue = Color(SixLabors.ImageSharp.Color.LightSteelBlue)
    static member LightYellow = Color(SixLabors.ImageSharp.Color.LightYellow)
    static member Lime = Color(SixLabors.ImageSharp.Color.Lime)
    static member LimeGreen = Color(SixLabors.ImageSharp.Color.LimeGreen)
    static member Linen = Color(SixLabors.ImageSharp.Color.Linen)
    static member Magenta = Color(SixLabors.ImageSharp.Color.Magenta)
    static member Maroon = Color(SixLabors.ImageSharp.Color.Maroon)
    static member MediumAquamarine = Color(SixLabors.ImageSharp.Color.MediumAquamarine)
    static member MediumBlue = Color(SixLabors.ImageSharp.Color.MediumBlue)
    static member MediumOrchid = Color(SixLabors.ImageSharp.Color.MediumOrchid)
    static member MediumPurple = Color(SixLabors.ImageSharp.Color.MediumPurple)
    static member MediumSeaGreen = Color(SixLabors.ImageSharp.Color.MediumSeaGreen)
    static member MediumSlateBlue = Color(SixLabors.ImageSharp.Color.MediumSlateBlue)
    static member MediumSpringGreen = Color(SixLabors.ImageSharp.Color.MediumSpringGreen)
    static member MediumTurquoise = Color(SixLabors.ImageSharp.Color.MediumTurquoise)
    static member MediumVioletRed = Color(SixLabors.ImageSharp.Color.MediumVioletRed)
    static member MidnightBlue = Color(SixLabors.ImageSharp.Color.MidnightBlue)
    static member MintCream = Color(SixLabors.ImageSharp.Color.MintCream)
    static member MistyRose = Color(SixLabors.ImageSharp.Color.MistyRose)
    static member Moccasin = Color(SixLabors.ImageSharp.Color.Moccasin)
    static member NavajoWhite = Color(SixLabors.ImageSharp.Color.NavajoWhite)
    static member Navy = Color(SixLabors.ImageSharp.Color.Navy)
    static member OldLace = Color(SixLabors.ImageSharp.Color.OldLace)
    static member Olive = Color(SixLabors.ImageSharp.Color.Olive)
    static member OliveDrab = Color(SixLabors.ImageSharp.Color.OliveDrab)
    static member Orange = Color(SixLabors.ImageSharp.Color.Orange)
    static member OrangeRed = Color(SixLabors.ImageSharp.Color.OrangeRed)
    static member Orchid = Color(SixLabors.ImageSharp.Color.Orchid)
    static member PaleGoldenrod = Color(SixLabors.ImageSharp.Color.PaleGoldenrod)
    static member PaleGreen = Color(SixLabors.ImageSharp.Color.PaleGreen)
    static member PaleTurquoise = Color(SixLabors.ImageSharp.Color.PaleTurquoise)
    static member PaleVioletRed = Color(SixLabors.ImageSharp.Color.PaleVioletRed)
    static member PapayaWhip = Color(SixLabors.ImageSharp.Color.PapayaWhip)
    static member PeachPuff = Color(SixLabors.ImageSharp.Color.PeachPuff)
    static member Peru = Color(SixLabors.ImageSharp.Color.Peru)
    static member Pink = Color(SixLabors.ImageSharp.Color.Pink)
    static member Plum = Color(SixLabors.ImageSharp.Color.Plum)
    static member PowderBlue = Color(SixLabors.ImageSharp.Color.PowderBlue)
    static member Purple = Color(SixLabors.ImageSharp.Color.Purple)
    static member RebeccaPurple = Color(SixLabors.ImageSharp.Color.RebeccaPurple)
    static member Red = Color(SixLabors.ImageSharp.Color.Red)
    static member RosyBrown = Color(SixLabors.ImageSharp.Color.RosyBrown)
    static member RoyalBlue = Color(SixLabors.ImageSharp.Color.RoyalBlue)
    static member SaddleBrown = Color(SixLabors.ImageSharp.Color.SaddleBrown)
    static member Salmon = Color(SixLabors.ImageSharp.Color.Salmon)
    static member SandyBrown = Color(SixLabors.ImageSharp.Color.SandyBrown)
    static member SeaGreen = Color(SixLabors.ImageSharp.Color.SeaGreen)
    static member SeaShell = Color(SixLabors.ImageSharp.Color.SeaShell)
    static member Sienna = Color(SixLabors.ImageSharp.Color.Sienna)
    static member Silver = Color(SixLabors.ImageSharp.Color.Silver)
    static member SkyBlue = Color(SixLabors.ImageSharp.Color.SkyBlue)
    static member SlateBlue = Color(SixLabors.ImageSharp.Color.SlateBlue)
    static member SlateGray = Color(SixLabors.ImageSharp.Color.SlateGray)
    static member SlateGrey = Color(SixLabors.ImageSharp.Color.SlateGrey)
    static member Snow = Color(SixLabors.ImageSharp.Color.Snow)
    static member SpringGreen = Color(SixLabors.ImageSharp.Color.SpringGreen)
    static member SteelBlue = Color(SixLabors.ImageSharp.Color.SteelBlue)
    static member Tan = Color(SixLabors.ImageSharp.Color.Tan)
    static member Teal = Color(SixLabors.ImageSharp.Color.Teal)
    static member Thistle = Color(SixLabors.ImageSharp.Color.Thistle)
    static member Tomato = Color(SixLabors.ImageSharp.Color.Tomato)
    static member Transparent = Color(SixLabors.ImageSharp.Color.Transparent)
    static member Turquoise = Color(SixLabors.ImageSharp.Color.Turquoise)
    static member Violet = Color(SixLabors.ImageSharp.Color.Violet)
    static member Wheat = Color(SixLabors.ImageSharp.Color.Wheat)
    static member White = Color(SixLabors.ImageSharp.Color.White)
    static member WhiteSmoke = Color(SixLabors.ImageSharp.Color.WhiteSmoke)
    static member Yellow = Color(SixLabors.ImageSharp.Color.Yellow)
    static member YellowGreen = Color(SixLabors.ImageSharp.Color.YellowGreen)

type InternalEvent = internal InternalEvent of SDL.SDL_Event

type point = int * int
type pointF = float * float
type Vector2 = System.Numerics.Vector2
type Matrix3x2 = System.Numerics.Matrix3x2
type Matrix4x4 = System.Numerics.Matrix4x4
type TextOptions = internal TextOptions of SixLabors.Fonts.TextOptions

let fromRgba (red:int) (green:int) (blue:int) (a:int) : Color =
    Color.FromRgba(byte red, byte green, byte blue, byte a)
    |> Color

let fromRgb (red:int) (green:int) (blue:int) : Color =
    Color.FromRgb(byte red, byte green, byte blue)
    |> Color

type image = SixLabors.ImageSharp.Image<Rgba32>
type DrawingContext = internal DrawingContext of SixLabors.ImageSharp.Processing.IImageProcessingContext
type drawing_fun = DrawingContext -> DrawingContext
type Font = internal Font of SixLabors.Fonts.Font
type FontFamily = internal FontFamily of SixLabors.Fonts.FontFamily
let systemFontNames: string list = [for i in  SixLabors.Fonts.SystemFonts.Families do yield i.Name]
let getFamily name =
    SixLabors.Fonts.SystemFonts.Get(name)
    |> FontFamily

let createFontFamilies (streams: IEnumerable<IO.Stream>) =
  let fontCollection = new FontCollection()
  streams.Select(fun (stream: IO.Stream) ->
     fontCollection.Add(stream) |> FontFamily
  )

let makeFont (FontFamily fam:FontFamily) (size:float) =
    fam.CreateFont(float32 size, SixLabors.Fonts.FontStyle.Regular)
    |> Font

let makeFontCSharp (fam, size) = makeFont fam size

let fontFamilies =
    ReadOnlyCollection(List(List.map getFamily systemFontNames))

let measureText (Font f:Font) (txt:string) =
    let options = new SixLabors.Fonts.TextOptions(f)
    options.WrappingLength <- infinityf
    options.HorizontalAlignment <- HorizontalAlignment.Left
    options.VerticalAlignment <- VerticalAlignment.Top 
    let rect = SixLabors.Fonts.TextMeasurer.MeasureSize(txt, new SixLabors.Fonts.TextOptions(f))
    (float rect.Width,float rect.Height)

let measureTextCSharp (font, txt) =
    let (x, y) = measureText font txt
    new Vector2(float32 x, float32 y)

type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush

let solidBrush (color: Color) : Tool =
    Brush (Brushes.Solid(color.color))
let solidPen (color: Color) (width: float) : Tool =
    Pen (Pens.Solid(color.color, float32 width))

let textOptions (Font font) =
    SixLabors.Fonts.TextOptions font
    |> TextOptions

let toPointF (x:float, y:float) = PointF(x = float32 x, y = float32 y)
let toVector2 (x:float, y:float) = System.Numerics.Vector2(float32 x, float32 y)

type PathDefinition =
    | EmptyDef
    | ArcTo of float * float * float * bool * bool * pointF
    | CubicBezierTo of pointF * pointF * pointF
    | LineTo of pointF
    | MoveTo of pointF
    | QuadraticBezierTo of pointF * pointF
    | SetTransform of Matrix3x2
    | LocalTransform of Matrix3x2 * PathDefinition
    | StartFigure
    | CloseFigure
    | CloseAllFigures
    | Combine of PathDefinition * PathDefinition

let (<++>) p1 p2 =
    match p1, p2 with
        | EmptyDef, _ -> p2
        | _, EmptyDef -> p1
        | defs -> Combine defs

let construct pd : IPath =
    let initial = Matrix3x2.Identity
    let builder = PathBuilder initial
    let rec loop (builder:PathBuilder) curT = function // tail-recursive traversal
        | [] -> builder
        | cur :: worklist ->
            match cur with
                | EmptyDef ->
                    loop builder curT worklist
                | ArcTo(radiusX, radiusY, rotation, largeArc, sweep, point) ->
                    let builder = builder.ArcTo(float32 radiusX, float32 radiusY,
                                                float32 rotation, largeArc, sweep,
                                                toPointF point)
                    loop builder curT worklist
                | CubicBezierTo(secondControlPoint, thirdControlPoint, point) ->
                    let builder = builder.CubicBezierTo(toVector2 secondControlPoint,
                                                        toVector2 thirdControlPoint,
                                                        toVector2 point)
                    loop builder curT worklist
                | LineTo p ->
                    let builder = builder.LineTo(toPointF p)
                    loop builder curT worklist
                | MoveTo p ->
                    let builder = builder.MoveTo(toPointF p)
                    loop builder curT worklist
                | QuadraticBezierTo(secondControlPoint, point) ->
                    let builder = builder.QuadraticBezierTo(toVector2 secondControlPoint,
                                                            toVector2 point)
                    loop builder curT worklist
                | SetTransform mat ->
                    let builder = builder.SetTransform mat
                    loop builder mat worklist
                | LocalTransform (mat, pd) ->
                    let builder = builder.SetTransform mat
                    loop builder mat (pd :: SetTransform curT :: worklist)
                | StartFigure ->
                    let builder = builder.StartFigure()
                    loop builder curT worklist
                | CloseFigure ->
                    let builder = builder.CloseFigure()
                    loop builder curT worklist
                | CloseAllFigures ->
                    let builder = builder.CloseAllFigures()
                    loop builder curT worklist
                | Combine (p1, p2) ->
                    loop builder curT (p1 :: p2 :: worklist)
    (loop builder initial pd).Build()

type PrimPath =
    | Arc of pointF * float * float * float * float * float
    | CubicBezier of pointF * pointF * pointF * pointF
    | Line of pointF * pointF
    | Lines of pointF list

and PathTree =
    | Empty
    | Prim of Tool * PrimPath
    | PathAdd of PathTree * PathTree
    | Transform of Matrix3x2 * PathTree
    | Text of Tool * string * TextOptions  // FIXME: Maybe we want a `Raw of IPathCollection` constructor instead of the Text constructors
    | TextAlong of Tool * string * TextOptions * PrimPath

let (<+>) p1 p2 =
    match p1, p2 with
        | Empty, _ -> p2
        | _, Empty -> p1
        | _ -> PathAdd (p1, p2)

let pathAdd = (<+>)

let emptyPath = Empty

let pathFromList ps = List.fold pathAdd emptyPath ps

let transform (mat:Matrix3x2) = function
    | Transform (cur, p) ->
        if mat.IsIdentity then Transform(cur, p)
        else let res = mat * cur
             if res.IsIdentity then p
             else Transform(res, p)
    | p ->
        Transform(mat, p)

let rotateDegreeAround (degrees:float) point p =
    let mat = Matrix3x2Extensions.CreateRotationDegrees(float32 degrees, toPointF point)
    transform mat p

let rotateRadiansAround (radians:float) point p =
    let mat = Matrix3x2Extensions.CreateRotation(float32 radians, toPointF point)
    transform mat p

let toILineSegment : PrimPath -> ILineSegment = function
    | Arc(center, rX, rY, rotation, start, sweep) ->
        ArcLineSegment(toPointF center, SizeF(float32 rX, float32 rY), float32 rotation, float32 start, float32 sweep)
    | CubicBezier(start, c1, c2, endPoint) ->
        CubicBezierLineSegment(toPointF start, toPointF c1, toPointF c2, toPointF endPoint, [||])
    | Line(start, endP) ->
        LinearLineSegment(toPointF start, toPointF endP)
    | Lines points ->
        LinearLineSegment(points |> Seq.map toPointF |> Seq.toArray)

let toPath (ilineseg:ILineSegment) : IPath =
    Path [| ilineseg |]

let flatten (p : PathTree) : (Tool*IPathCollection) list =  //FIXME: should maybe return a seq<IPathCollection>
    let rec traverse (acc : (Tool*IPathCollection) list) = function // tail-recursive traversal
        | [] -> List.rev acc
        | cur :: worklist ->
            match cur with
                | Empty ->
                    traverse acc worklist
                | Prim (pen, p) ->
                    let path = PathCollection [| p |> toILineSegment |> toPath |] : IPathCollection
                    let acc =  (pen,path) :: acc
                    traverse acc worklist
                | PathAdd (p1, p2) ->
                    traverse acc (p1 :: p2 :: worklist)
                | Transform (mat, p) ->
                    let transformed = traverse [] [p] |> List.map (fun (pen,p) -> (pen,p.Transform(mat)))
                    let acc = transformed @ acc
                    traverse acc worklist
                | Text (pen, text, TextOptions options) ->
                    let glyphs = TextBuilder.GenerateGlyphs(text, options)
                    let acc =  (pen,glyphs) :: acc
                    traverse acc worklist
                | TextAlong(pen, text, TextOptions options, p) ->
                    let path = p |> toILineSegment |> toPath
                    let glyphs = TextBuilder.GenerateGlyphs(text, path, options)
                    let acc =  (pen, glyphs) :: acc
                    traverse acc worklist
    traverse [] [p]

let drawCollection ((tool,col):Tool * IPathCollection) (ctx: DrawingContext): DrawingContext =
    let (DrawingContext _ctx) = ctx
    
    ignore <|
    match tool with
        | Pen pen ->
            _ctx.Draw(pen, col)
        | Brush brush ->
            _ctx.Fill(DrawingOptions(), brush, col)
    
    DrawingContext _ctx

let drawPathTree (paths:PathTree) (ctx: DrawingContext) : DrawingContext =
    flatten paths |> List.fold (fun ctx penNCol -> drawCollection penNCol ctx) ctx

let drawToFile width height (filePath:string) (draw: drawing_fun) =
    let img = new Image<Rgba32>(width, height)
    img.Mutate(DrawingContext >> draw >> ignore)
    img.Save(filePath)

let drawToAnimatedGif width height (frameDelay:int) (repeatCount:int) (filePath:string) (drawLst:drawing_fun list) =
    match drawLst with
        draw::rst ->
            let gif = new Image<Rgba32>(width, height)
            gif.Mutate(DrawingContext >> draw >> ignore)
            let gifMetaData = gif.Metadata.GetGifMetadata()
            gifMetaData.RepeatCount <- uint16 repeatCount
            let metadata = gif.Frames.RootFrame.Metadata.GetGifMetadata()
            metadata.FrameDelay <- frameDelay
            List.iter (fun (draw:drawing_fun) -> 
                let frame = new Image<Rgba32>(width, height)
                frame.Mutate(DrawingContext >> draw >> ignore)
                let metadata = frame.Frames.RootFrame.Metadata.GetGifMetadata()
                metadata.FrameDelay <- frameDelay
                gif.Frames.AddFrame(frame.Frames.RootFrame) |> ignore
            ) rst
            gif.SaveAsGif(filePath)
        | _ -> ()

let internal unpackFont (Font f) = f

type PathCollection = internal PathCollection of IPathCollection

let createText (text, font) = 
    TextBuilder.GenerateGlyphs(text, SixLabors.Fonts.TextOptions (unpackFont font))
    |> PathCollection

type Image = internal Image of SixLabors.ImageSharp.Image

let createImage (buffer: ReadOnlySpan<byte>) =
    Image.Load(buffer)
    |> Image

let renderImage (x,
                 y,
                 Image image,
                 DrawingContext ctx) =
    ctx.DrawImage(image, new Point(x, y), 1f)
    |> ignore

let renderBrushPath (color: Color, PathCollection path: PathCollection, DrawingContext ctx) =
    let brush = Brushes.Solid(color.color)
    ctx.Fill(DrawingOptions(), brush, path)
    |> ignore

let renderPenPath (width, color: Color, PathCollection path: PathCollection, DrawingContext ctx) =
    let pen = Pens.Solid(color.color, width)
    ctx.Draw(pen, path)
    |> ignore

let transformPath (PathCollection path, matrix) =
    path.Transform(matrix)
    |> PathCollection

let setSizeImage (Image image, x: int, y: int) =
    image.Clone (fun ctx ->
        ctx.Resize(x, y) |> ignore
    ) |> Image

let transformImage (Image image, matrix) =
    // let resampler = KnownResamplers.Lanczos3
    // let transformer = new Processors.Transforms.AffineTransformProcessor(matrix, resampler, image.Size)
    let builder = new AffineTransformBuilder()
    builder.AppendMatrix(matrix) |> ignore
    image.Clone (fun ctx ->
        ctx.Transform(builder) |> ignore
    ) |> Image

let cropImage (Image image, x: int, y: int, w: int, h: int) =
    image.Clone (fun ctx ->
        ctx.Crop(new Rectangle(new Point(x, y), new Size(w, h))) |> ignore
    ) |> Image

let measureImage (Image image) =
    (image.Width, image.Height)

type KeyAction =
    | KeyPress = 0
    | KeyRelease = 1

type KeyboardKey =
    | Unknown = 0 // Matches the case of an invalid, non-existent, or unsupported keyboard key.
    | Space = 1 // The spacebar key
    | Apostrophe = 2 // The apostrophe key '''
    | Comma = 3 // The comma key ','
    | Minus = 4 // The minus key '-'
    | Plus = 5 // The plus key '+'
    | Period = 6 // The period/dot key '.'
    | Slash = 7 // The slash key '/'
    | Num0 = 8 // The 0 key.
    | Num1 = 9 // The 1 key.
    | Num2 = 10 // The 2 key.
    | Num3 = 11 // The 3 key.
    | Num4 = 12 // The 4 key.
    | Num5 = 13 // The 5 key.
    | Num6 = 14 // The 6 key.
    | Num7 = 15 // The 7 key.
    | Num8 = 16 // The 8 key.
    | Num9 = 17 // The 9 key.
    | Semicolon = 18 // The semicolon key.
    | Equal = 19 // The equal key.
    | A = 20 // The A key.
    | B = 21 // The B key.
    | C = 22 // The C key.
    | D = 23 // The D key.
    | E = 24 // The E key.
    | F = 25 // The F key.
    | G = 26 // The G key.
    | H = 27 // The H key.
    | I = 28 // The I key.
    | J = 29 // The J key.
    | K = 30 // The K key.
    | L = 31 // The L key.
    | M = 32 // The M key.
    | N = 33 // The N key.
    | O = 34 // The O key.
    | P = 35 // The P key.
    | Q = 36 // The Q key.
    | R = 37 // The R key.
    | S = 38 // The S key.
    | T = 39 // The T key.
    | U = 40 // The U key.
    | V = 41 // The V key.
    | W = 42 // The W key.
    | X = 43 // The X key.
    | Y = 44 // The Y key.
    | Z = 45 // The Z key.
    | LeftBracket = 46 // The left bracket(opening bracket) key '['
    | Backslash = 47 // The backslash '\'
    | RightBracket = 48 // The right bracket(closing bracket) key ']'
    | GraveAccent = 49 // The grave accent key '`'
    | AcuteAccent = 50 // The acute accent key ("inverted" grave accent) '´'
    | Escape = 51 // The escape key.
    | Enter = 52 // The enter key.
    | Tab = 53 // The tab key.
    | Backspace = 54 // The backspace key.
    | Insert = 55 // The insert key.
    | Delete = 56 // The delete key.
    | Right = 57 // The right arrow key.
    | Left = 58 // The left arrow key.
    | Down = 59 // The down arrow key.
    | Up = 60 // The up arrow key.
    | PageUp = 61 // The page up key.
    | PageDown = 62 // The page down key.
    | Home = 63 // The home key.
    | End = 64 // The end key.
    | CapsLock = 65 // The caps lock key.
    | ScrollLock = 66 // The scroll lock key.
    | NumLock = 67 // The num lock key.
    | PrintScreen = 68 // The print screen key.
    | Pause = 69 // The pause key.
    | F1 = 70 // The F1 key.
    | F2 = 71 // The F2 key.
    | F3 = 72 // The F3 key.
    | F4 = 73 // The F4 key.
    | F5 = 74 // The F5 key.
    | F6 = 75 // The F6 key.
    | F7 = 76 // The F7 key.
    | F8 = 77 // The F8 key.
    | F9 = 78 // The F9 key.
    | F10 = 79 // The F10 key.
    | F11 = 80 // The F11 key.
    | F12 = 81 // The F12 key.
    | KeyPad0 = 82 // The 0 key on the key pad.
    | KeyPad1 = 83 // The 1 key on the key pad.
    | KeyPad2 = 84 // The 2 key on the key pad.
    | KeyPad3 = 85 // The 3 key on the key pad.
    | KeyPad4 = 86 // The 4 key on the key pad.
    | KeyPad5 = 87 // The 5 key on the key pad.
    | KeyPad6 = 88 // The 6 key on the key pad.
    | KeyPad7 = 89 // The 7 key on the key pad.
    | KeyPad8 = 90 // The 8 key on the key pad.
    | KeyPad9 = 91 // The 9 key on the key pad.
    | KeyPadDecimal = 92 // The decimal key on the key pad.
    | KeyPadDivide = 93 // The divide key on the key pad.
    | KeyPadMultiply = 94 // The multiply key on the key pad.
    | KeyPadSubtract = 95 // The subtract key on the key pad.
    | KeyPadAdd = 96 // The add key on the key pad.
    | KeyPadEnter = 97 // The enter key on the key pad.
    | KeyPadEqual = 98 // The equal key on the key pad.
    | LeftShift = 99 // The left shift key.
    | LeftControl = 100 // The left control key.
    | LeftAlt = 101 // The left alt key.
    | LeftSuper = 102 // The left super key.
    | RightShift = 103 // The right shift key.
    | RightControl = 104 // The right control key.
    | RightAlt = 105 // The right alt key.
    | RightSuper = 106 // The right super key.
    | Menu = 107 // The menu key.
    | Diaresis = 108 // The Diaresis key '¨'
    | LessThan = 109 // The less than sign '<'
    | GreaterThan = 110 // The greater than sign '>'
    | FractionOneHalf = 111 // The "vulgar fraction one half" key '½'
    | DanishAA = 112 // The Danish AA key 'Å'
    | DanishAE = 113 // The Danish AE key 'Æ'
    | DanishOE = 114 // The Danish OE key 'Ø'


let private toKeyboardKey key =
    match SDL.stringFromKeyboard key with
    | "Space" -> KeyboardKey.Space
    | "'" -> KeyboardKey.Apostrophe
    | "," -> KeyboardKey.Comma
    | "-" -> KeyboardKey.Minus
    | "+" -> KeyboardKey.Plus
    | "." -> KeyboardKey.Period
    | "/" -> KeyboardKey.Slash
    | "0" -> KeyboardKey.Num0
    | "1" -> KeyboardKey.Num1
    | "2" -> KeyboardKey.Num2
    | "3" -> KeyboardKey.Num3
    | "4" -> KeyboardKey.Num4
    | "5" -> KeyboardKey.Num5
    | "6" -> KeyboardKey.Num6
    | "7" -> KeyboardKey.Num7
    | "8" -> KeyboardKey.Num8
    | "9" -> KeyboardKey.Num9
    | ";" -> KeyboardKey.Semicolon
    | "=" -> KeyboardKey.Equal
    | "A" -> KeyboardKey.A
    | "B" -> KeyboardKey.B
    | "C" -> KeyboardKey.C
    | "D" -> KeyboardKey.D
    | "E" -> KeyboardKey.E
    | "F" -> KeyboardKey.F
    | "G" -> KeyboardKey.G
    | "H" -> KeyboardKey.H
    | "I" -> KeyboardKey.I
    | "J" -> KeyboardKey.J
    | "K" -> KeyboardKey.K
    | "L" -> KeyboardKey.L
    | "M" -> KeyboardKey.M
    | "N" -> KeyboardKey.N
    | "O" -> KeyboardKey.O
    | "P" -> KeyboardKey.P
    | "Q" -> KeyboardKey.Q
    | "R" -> KeyboardKey.R
    | "S" -> KeyboardKey.S
    | "T" -> KeyboardKey.T
    | "U" -> KeyboardKey.U
    | "V" -> KeyboardKey.V
    | "W" -> KeyboardKey.W
    | "X" -> KeyboardKey.X
    | "Y" -> KeyboardKey.Y
    | "Z" -> KeyboardKey.Z
    | "[" -> KeyboardKey.LeftBracket
    | "\\" -> KeyboardKey.Backslash
    | "]" -> KeyboardKey.RightBracket
    | "`" -> KeyboardKey.GraveAccent
    | "´" -> KeyboardKey.AcuteAccent
    | "Escape" -> KeyboardKey.Escape
    | "Return" -> KeyboardKey.Enter // This should probably be Return.
    | "Tab" -> KeyboardKey.Tab
    | "Backspace" -> KeyboardKey.Backspace
    | "Insert" -> KeyboardKey.Insert
    | "Delete" -> KeyboardKey.Delete
    | "Right" -> KeyboardKey.Right
    | "Left" -> KeyboardKey.Left
    | "Down" -> KeyboardKey.Down
    | "Up" -> KeyboardKey.Up
    | "PageUp" -> KeyboardKey.PageUp
    | "PageDown" -> KeyboardKey.PageDown
    | "Home" -> KeyboardKey.Home
    | "End" -> KeyboardKey.End
    | "CapsLock" -> KeyboardKey.CapsLock
    | "ScrollLock" -> KeyboardKey.ScrollLock
    | "Numlock" -> KeyboardKey.NumLock // Has to be lowercase.
    | "PrintScreen" -> KeyboardKey.PrintScreen
    | "Pause" -> KeyboardKey.Pause
    | "F1" -> KeyboardKey.F1
    | "F2" -> KeyboardKey.F2
    | "F3" -> KeyboardKey.F3
    | "F4" -> KeyboardKey.F4
    | "F5" -> KeyboardKey.F5
    | "F6" -> KeyboardKey.F6
    | "F7" -> KeyboardKey.F7
    | "F8" -> KeyboardKey.F8
    | "F9" -> KeyboardKey.F9
    | "F10" -> KeyboardKey.F10
    | "F11" -> KeyboardKey.F11
    | "F12" -> KeyboardKey.F12
    | "Keypad 0" -> KeyboardKey.KeyPad0
    | "Keypad 1" -> KeyboardKey.KeyPad1
    | "Keypad 2" -> KeyboardKey.KeyPad2
    | "Keypad 3" -> KeyboardKey.KeyPad3
    | "Keypad 4" -> KeyboardKey.KeyPad4
    | "Keypad 5" -> KeyboardKey.KeyPad5
    | "Keypad 6" -> KeyboardKey.KeyPad6
    | "Keypad 7" -> KeyboardKey.KeyPad7
    | "Keypad 8" -> KeyboardKey.KeyPad8
    | "Keypad 9" -> KeyboardKey.KeyPad9
    | "Keypad ." -> KeyboardKey.KeyPadDecimal
    | "Keypad /" -> KeyboardKey.KeyPadDivide
    | "Keypad *" -> KeyboardKey.KeyPadMultiply
    | "Keypad -" -> KeyboardKey.KeyPadSubtract
    | "Keypad +" -> KeyboardKey.KeyPadAdd
    | "Keypad Enter" -> KeyboardKey.KeyPadEnter
    | "Keypad =" -> KeyboardKey.KeyPadEqual
    | "Left Shift" -> KeyboardKey.LeftShift
    | "Left Ctrl" -> KeyboardKey.LeftControl
    | "Left Alt" -> KeyboardKey.LeftAlt
    | "Left GUI" -> KeyboardKey.LeftSuper
    | "Right Shift" -> KeyboardKey.RightShift
    | "Right Ctrl" -> KeyboardKey.RightControl
    | "Right Alt" -> KeyboardKey.RightAlt
    | "Right GUI" -> KeyboardKey.RightSuper
    | "Menu" -> KeyboardKey.Menu // Not sure if this does anything.
    | "¨" -> KeyboardKey.Diaresis
    | "<" -> KeyboardKey.LessThan
    | ">" -> KeyboardKey.GreaterThan
    | "½" -> KeyboardKey.FractionOneHalf
    | "å" -> KeyboardKey.DanishAA
    | "æ" -> KeyboardKey.DanishAE
    | "ø" -> KeyboardKey.DanishOE
    | _ -> KeyboardKey.Unknown

let private TIMER_EVENT =
    match SDL.SDL_RegisterEvents 1 with
    | UInt32.MaxValue -> failwith "Error: Could not allocate a user-defined Timer Event."
    | x -> x

let timerTickEvent () =
    let mutable ev = SDL.SDL_Event()
    ev.``type`` <- TIMER_EVENT
    SDL.SDL_PushEvent(&ev) |> ignore

type Window(t:string, w:int, h:int) =
    let mutable disposed = false
    static let mutable eventQueues = Map.empty
    let width, height = w, h
    let title = t
    let mutable window, renderer, texture, bufferPtr = IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero
    let mutable frameBuffer = Array.create 0 (byte 0)
    let mutable event = SDL.SDL_Event()
    let mutable img = None
    let mutable windowId = 0u
    let mutable background = Color.Black
    let windowFlags =
        SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN |||
        SDL.SDL_WindowFlags.SDL_WINDOW_INPUT_FOCUS
    do
        let is_initial = 0 = Map.count eventQueues
        if is_initial then
            SDL.SDL_SetMainReady()
            SDL.SDL_Init(SDL.SDL_INIT_VIDEO) |> ignore
            //SDL.SDL_SetHint(SDL.SDL_HINT_QUIT_ON_LAST_WINDOW_CLOSE, "0") |> ignore

        window <- SDL.SDL_CreateWindow(t, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED,
                                width, height, windowFlags)

        if window = 0 then
            let err = Marshal.PtrToStringUTF8(SDL.SDL_GetError())
            failwith err

        renderer <- SDL.SDL_CreateRenderer(window, -1, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED |||
                                                SDL.SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC)

        texture <- SDL.SDL_CreateTexture(renderer, SDL.SDL_PIXELFORMAT_RGBA32, SDL.SDL_TEXTUREACCESS_STREAMING, width, height)

        frameBuffer <- Array.create (width * height * 4) (byte 0)
        bufferPtr <- IntPtr ((Marshal.UnsafeAddrOfPinnedArrayElement (frameBuffer, 0)).ToPointer ())

        img <- new Image<Rgba32>(w, h, background.color) |> Some

        if is_initial then
            SDL.SDL_StartTextInput()
        
        windowId <- SDL.SDL_GetWindowID(window)
        eventQueues <- eventQueues.Add (windowId, Queue())

    member this.Cleanup () = 
        if not disposed then
            disposed <- true
            this.HideWindow ()
            SDL.SDL_DestroyTexture texture
            //printfn "Texture destroyed"
            SDL.SDL_DestroyRenderer renderer
            //printfn "Render destroyed"
            SDL.SDL_DestroyWindow window
            //printfn "Window destroyed"
            eventQueues <- eventQueues.Remove windowId

            if 0 = Map.count eventQueues then
                SDL.SDL_QuitSubSystem(SDL.SDL_INIT_VIDEO) |> ignore

            window <- IntPtr.Zero
            renderer <- IntPtr.Zero
            texture <- IntPtr.Zero
            bufferPtr <- IntPtr.Zero
            frameBuffer <- Array.create 0 (byte 0)
            event <- SDL.SDL_Event()
            img <- None
            windowId <- 0u
    
    interface IDisposable with
        member this.Dispose () =
            this.Cleanup()
    
    override this.Finalize () =
        this.Cleanup()

    member this.Clear () =
         Option.map(fun (img: Image<Rgba32>) ->
            img.Mutate(fun ctx -> ctx.Clear(background.color) |> ignore)
         ) img |> ignore

    member this.Render (draw: Action<DrawingContext>) =
        Option.map(fun (img: Image<Rgba32>) ->
            img.Mutate (fun ctx ->
                draw.Invoke(DrawingContext ctx)
                ctx.Crop(min width img.Width, min height img.Height)
                   .Resize(
                        options = ResizeOptions(Position = AnchorPositionMode.TopLeft,
                        Size = Size(width, height),
                        Mode = ResizeMode.BoxPad)
                    )
                |> ignore
            )
            img.CopyPixelDataTo(frameBuffer)
            SDL.SDL_UpdateTexture(texture, IntPtr.Zero, bufferPtr, width * 4) |> ignore
            SDL.SDL_RenderClear(renderer) |> ignore
            SDL.SDL_RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero) |> ignore
            SDL.SDL_RenderPresent(renderer) |> ignore
        ) img |> ignore
        this.Clear ()
        ()

    member private this.EnqueueEvent (event: SDL.SDL_Event)  =
        if event.window.windowID = 0u
        then
            Map.iter (
                fun _ (q: Queue<SDL.SDL_Event>) -> q.Enqueue event
            ) eventQueues
        else
            match Map.tryFind event.window.windowID eventQueues with
            | Some queue -> queue.Enqueue event
            | None -> ()
    
    member private this.AssertWindowExists () =
        if Map.containsKey windowId eventQueues |> not then
            failwith "Error: You can not get events from a disposed Window."

    member this.WaitEvent (f : Func<InternalEvent, 'a>) =
        this.AssertWindowExists ()

        if eventQueues[windowId].Count = 0
        then
            if 0 = SDL.SDL_WaitEvent(&event) then
                failwith "Error: No event arrived."
            
            this.EnqueueEvent event
            this.WaitEvent f
        else
            let ev =
                eventQueues[windowId].Dequeue ()
                |> InternalEvent
            
            f.Invoke(ev)
    
    member this.PollEvents (f : Action<InternalEvent>) =
        this.AssertWindowExists ()

        while 1 = SDL.SDL_PollEvent(&event) do
            this.EnqueueEvent event

        while eventQueues[windowId].Count <> 0 do
            let ev =
                eventQueues[windowId].Dequeue ()
                |> InternalEvent
            
            f.Invoke(ev)
            
    member this.HideWindow () =
        SDL.SDL_HideWindow window

    member this.SetClearColor (r, g, b) =
        background <- fromRgb r g b
    
    member this.Title
        with get () = title
    
    member this.Width
        with get () = width
    
    member this.Height
        with get () = height

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

type ClassifiedEvent<'e> =
    | React of 'e
    | Quit
    | Ignore

let toKeyboardEvent event =
    let (InternalEvent _event) = event
    match SDL.convertEvent _event with
    | SDL.Window wev when wev.event = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE -> Quit
    | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_ESCAPE -> Quit
    | SDL.KeyDown kevent -> React (KeyAction.KeyPress, toKeyboardKey kevent)
    | SDL.KeyUp kevent -> React (KeyAction.KeyRelease, toKeyboardKey kevent)
    | _ -> Ignore

let private classifyEvent userClassify ev =
    let (InternalEvent _ev) = ev
    match SDL.convertEvent _ev with
        | SDL.Window wev when wev.event = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE -> Quit
        // | SDL.Window wevent->
        //     printfn "Window event %A" wevent.event
        //     // if wevent.event = SDL.SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE then Quit
        //     // else Ignore(SDL.Window wevent)
        //     Ignore(SDL.Window wevent)
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_ESCAPE -> Quit
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_UP -> React UpArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_DOWN -> React DownArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_LEFT -> React LeftArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_RIGHT -> React RightArrow
        | SDL.KeyDown kevent when kevent.keysym.sym = SDL.SDLK_RETURN -> React Return
        | SDL.MouseButtonDown mouseButtonEvent ->
            (mouseButtonEvent.x,mouseButtonEvent.y) |> MouseButtonDown |> React
        | SDL.MouseButtonUp mouseButtonEvent ->
            (mouseButtonEvent.x,mouseButtonEvent.y) |> MouseButtonUp |> React
        | SDL.MouseMotion motion ->
            (motion.x,motion.y,motion.xrel, motion.yrel) |> MouseMotion |> React
        | SDL.TextInput tinput -> tinput |> Key |> React
        | SDL.User uev -> userClassify uev
        | _ -> Ignore

let private userClassify : SDL.SDL_UserEvent -> ClassifiedEvent<Event> = function
    | uev when uev.``type`` = TIMER_EVENT -> React TimerTick
    | _ -> Ignore

let private timer interval =
    let ticker _ = timerTickEvent () 
    let timer = new System.Timers.Timer(float interval)
    timer.AutoReset <- true
    timer.Elapsed.Add ticker
    timer.Start()
    Some timer

let runAppWithTimer (t:string) (w:int) (h:int) (interval:int option)
        (draw: 's -> drawing_fun)
        (react: 's -> Event -> 's option) (s: 's) : unit =
    let window = new Window(t, w, h)

    let mutable state = s
    let rec drawLoop redraw =
        if redraw then
            window.Render (draw state >> ignore)
        match window.WaitEvent (classifyEvent userClassify) with
            | Quit ->
                // printfn "We quit"
                window.HideWindow ()
                () // quit the interaction by exiting the loop
            | React ev ->
                let redraw =
                    match react state ev with
                        | Some s' -> state <- s'; true
                        | None   -> false
                drawLoop redraw
            | Ignore ->
                // printfn "We loop because of: %A" sdlEvent
                drawLoop false
    let timer = Option.bind (fun t -> timer t) interval
    
    drawLoop true
    timer |> Option.map (fun timer -> timer.Stop()) |> ignore
    //printfn "Out of loop"

let runApp (t:string) (w:int) (h:int) (draw: unit -> drawing_fun) : unit =
    let drawWState s = draw ()
    let react s e = None
    runAppWithTimer t w h None drawWState react 0
