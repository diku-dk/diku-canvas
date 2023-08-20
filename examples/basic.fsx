#r "nuget:DIKU.Canvas, 2.0.0-alpha7"
open Canvas
open System

type state = int // a counter
/// Update state function
let sLim = 30
let next s = (s+1)%sLim

let w,h = 256,256 // size of the image

/// A tree of of a piecewise affine curve. Argument is ignores the argument
let makePiecewiseAffine _ = 
    let col = green
    let strokeWidth = 3.0
    let lst = [(10.0,10.0); (60.0, 80.0); (20.0, 50.0); (10.0, 10.0)]
    piecewiseAffine col strokeWidth lst

/// A filled polygon. Argument is ignores the argument
let makeFilledPolygon _ = 
    let col = green
    let strokeWidth = 3.0
    let lst = [(10.0,10.0); (60.0, 80.0); (20.0, 50.0); (10.0, 10.0)]
    filledPolygon col lst

/// A tree of a rectangle. Argument is ignores the argument
let makeRectangle _ = 
    let col = green
    let strokeWidth = 3.0
    rectangle col strokeWidth ((float w)/2.0) ((float h)/2.0)

/// A tree of a filled rectangle. Argument is ignores the argument
let makeFilledRectangle _ = 
    let col = green
    filledRectangle col ((float w)/2.0) ((float h)/2.0)

/// A tree of a cubic Bezier curve. Argument is ignores the argument
let rand = Random() // initialize a random number generator
let W, H = float w, float h
let bezierPoints = [(W/8.0,H/2.0); (W*3.0/8.0, H/2.0 + H/4.0 * rand.NextDouble()); (W*5.0/8.0, H/2.0 + H/4.0 * rand.NextDouble()); (W*7.0/8.0,H/2.0)]
let makeBezier _ =
    let col = green
    let strokeWidth = 3.0
    cubicBezier bezierPoints[0] bezierPoints[1] bezierPoints[2] bezierPoints[3] col strokeWidth

/// A tree of a filled cubic Bezier curve. Argument is ignores the argument
let makeFilledBezier _ = 
    let col = green
    filledCubicBezier bezierPoints[0] bezierPoints[1] bezierPoints[2] bezierPoints[3] col

/// A tree of a circular arc. Argument is ignores the argument
let makeArc _ = 
    let col = green
    let strokeWidth = 3.0
    arc (W/2.0,H/2.0) (W/4.0) (H/3.0) -45.0 180.0 col strokeWidth

/// A tree of a filled circular arc. Argument is ignores the argument
let makeFilledArc _ = 
    let col = green
    filledArc (W/2.0,H/2.0) (W/4.0) (H/3.0) -45.0 180.0 col

/// A tree of text of its input
let makeTxt i = 
    let fontName = systemFontNames[0] // or perhaps "Microsoft Sans Serif"
    let font = makeFont fontName 24.0
    let white = yellow
    text white 1.0 font (string i)

/// A tree which is a translated version of mkText
let makeTranslate i = 
    makeTxt i
    |> translate (float i) 0.0

/// A tree which is a rotated version of mkText
let makeRotate i = 
    let txt = makeTxt i
    let (x,y,w,h) = getRectangle txt
    txt |> rotate (x+w/2.0) (y+h/2.0) ((float i)*System.Math.PI/180.0)

/// A tree which is a scaled version of mkText
let makeScale i = 
    let factor = 1.0+3.0*(float i)/(float sLim)
    makeFilledPolygon 0
    |> scale (float factor) (float factor)

/// A tree where makePiecewiseAffine is stacked on top of makeTxt
let makeOntop i = onto (makePiecewiseAffine i) (makeTxt i)

/// A tree where makePiecewiseAffine is placed to the right of makeTxt. Parameter p gives makeTxt's vertical alignment wrt. makePiecewiseAffine
let makeAlignH p i = alignH (makePiecewiseAffine i) p (makeTxt i)

/// A tree where makePiecewiseAffine is placed above makeTxt.  Parameter p gives makeTxt's horizontal alignment wrt. makePiecewiseAffine
let makeAlignV p i = alignV (makePiecewiseAffine i) p (makeTxt i)

/// A tree where makeTxt is repeated 4 times in a checkerboard pattern.
let make4 i =
    let fontName = systemFontNames[0]
    let font = makeFont fontName (4.0*36.0)
    let p = 
        text white 1.0 font (string i)
    let q = alignV p Top p
    alignH q Top q

// Return the draw function by its name
let mkDraw (txt:string): (state -> PrimitiveTree) = 
    match txt with
        | "text" -> makeTxt
        | "piecewiseAffine" -> makePiecewiseAffine
        | "filledPolygon" -> makeFilledPolygon
        | "rectangle" -> makeRectangle
        | "filledRectangle" -> makeFilledRectangle
        | "arc" -> makeArc
        | "filledArc" -> makeFilledArc
        | "bezier" -> makeBezier
        | "filledBezier" -> makeFilledBezier
        | "translate" -> makeTranslate
        | "scale" -> makeScale // FIXME! throws SixLabors.ImageSharp.Drawing.Shapes.PolygonClipper.ClipperException
        | "rotate" -> makeRotate
        | "ontop" -> makeOntop
        | "4" -> make4
        | "alignHt" -> makeAlignH Top
        | "alignHc" -> makeAlignH Center
        | "alignHb" -> makeAlignH Bottom
        | "alignVl" -> makeAlignV Left
        | "alignVc" -> makeAlignV Center
        | "alignVr" -> makeAlignV Right
        | _ -> failwith "Unkown test"

/// React to whenever an event happens
let react (s:state) (ev:Event): state option =
    match ev with
        | Event.TimerTick -> Some (next s)
        | _ -> None

/// interact the picture generating draw function as specified by the user on the command line
let main args =
    if Array.length args < 2 then
        1 // return an error code to the commandline
    else
        printfn "From command line we got: %A" args
        let treeName = args[1] // make an alias for requested tree
        let pict = mkDraw treeName // retrieve the 
        let draw = fun i -> explain (pict i)
        let initialState = 0 // First state drawn by draw
        let delayTime = (Some 100) // microseconds (as an option type)
        interact treeName w h delayTime draw react initialState
        0 // return the success code to the commandline

// run a test as, e.g.,
//   dotnet fsi basic.fsx curve
// to run the curve test. All tests are listed in the mkDraw function
main fsi.CommandLineArgs
