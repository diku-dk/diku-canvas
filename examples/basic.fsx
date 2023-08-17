//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Canvas

type state = int // a counter
/// Update state function
let next s = 10+(s-10+1)%30

/// A tree of of a piecewise affine curve. Argument is ignores the argument
let makeCurve _ = 
    let col = color.Green
    let strokeWidth = 1.0
    let lst = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
    piecewiseaffine col strokeWidth lst


/// A tree of text of its input
let makeTxt i = 
    let fontName = systemFontNames[0]
    let font = makeFont fontName 36.0
    let white = color.Green
    text white 1.0 font (string i)

/// A tree which is a translated version of mkText
let makeTranslate i = 
    makeTxt i
    |> translate (float i) 0.0

/// A tree where makeCurve is stacked on top of makeTxt
let makeOntop i = onto (makeCurve i) (makeTxt i)

/// A tree where makeCurve is placed to the right of makeTxt. Parameter p gives makeTxt's vertical alignment wrt. makeCurve
let makeAlignH p i = alignh (makeTxt i) p (makeCurve i)

/// A tree where makeCurve is placed above makeTxt.  Parameter p gives makeTxt's horizontal alignment wrt. makeCurve
let makeAlignV p i = alignv (makeCurve i) p (makeTxt i)

/// A tree where makeTxt is repeated 4 times in a checkerboard pattern.
let make4 i =
    let fontName = systemFontNames[0]
    let font = makeFont fontName 36.0
    let p = 
        text color.White 1.0 font (string i)
    let q = alignv p Top p
    alignh q Top q

// Return the draw function by its name
let mkDraw (txt:string) = 
    match txt with
        | "text" -> makeTxt
        | "curve" -> makeCurve
        | "translate" -> makeTranslate
        | "ontop" -> makeOntop
        | "4" -> make4
        | "alignht" -> makeAlignH Top
        | "alignhc" -> makeAlignH Center
        | "alignhb" -> makeAlignH Bottom
        | "alignvl" -> makeAlignV Left
        | "alignvc" -> makeAlignV Center
        | "alignvr" -> makeAlignV Right
        | _ -> failwith "Unkown test"

/// React to whenever an event happens
let react (s:state) (ev:Lowlevel.Event): state option =
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
        let draw = explain (pict 0)
        let w,h = 256,256
        let initialState = 0.0 // First state drawn by draw
        let delayTime = (Some 100) // microseconds (as an option type)
        interact treeName w h delayTime draw react initialState
        0 // return the success code to the commandline

// run a test as, e.g.,
//   dotnet fsi basic curve
// to run the curve test. All tests are listed in the mkDraw function
main fsi.CommandLineArgs
