//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha6"
open Canvas

type state = float // a rotation degree

let w,h = 256,256 // the size of the canvas
let N = 360.0 // Number of degrees to sample

/// Update state function
let next d = (d % N) + 1.0

// A non-trivial tree
let lst1 = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let lst2 = [(0.0,0.0); (40.0, 30.0); (0.0, 30.0); (0.0, 0.0)]
let tri1 = piecewiseAffine green 1.0 lst1
let tri2 = filledPolygon purple lst2
let ell = ellipse blue 4.0 20.0 25.0
let fig = translate 50.0 50.0 (alignV (alignH tri1 Center tri2) Center ell)

/// Prepare a Picture by the present state whenever needed
let draw (i:state): Picture = 
    let fw,fh = getSize <| getRectangle fig
    let cx,cy = fw/2.0, fh/2.0
    let rad = i/(N-1.0)*2.0*System.Math.PI
    let figi = rotate cx cy rad fig
    printfn "%s" (toString figi) // Print to screen,e.g., for debugging
    make figi

/// React to whenever an event happens
let react (s:state) (ev:Event) : state option =
    match ev with
        | Event.TimerTick -> Some (next s)
        | _ -> None // all other events are ignored

// Start interaction session
let initialState = 0.0 // First state drawn by draw
let delayTime = (Some 100) // microseconds (as an option type)
interact "animate" w h delayTime draw react initialState
