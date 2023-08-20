#r "nuget:DIKU.Canvas, 2.0.0-alpha7"
open Canvas

type state = float // a rotation degree

let w,h = 256,256 // the size of the canvas

// A non-trivial tree
let lst1 = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let lst2 = [(0.0,0.0); (40.0, 30.0); (0.0, 30.0); (0.0, 0.0)]
let tri1 = piecewiseAffine green 1.0 lst1
let tri2 = filledPolygon purple lst2
let ell = ellipse blue 4.0 20.0 25.0
let tree = translate 50.0 50.0 (alignV (alignH tri1 Center tri2) Center ell)
let bck = filledRectangle black w h

/// Prepare a Picture by the present state whenever needed
let draw (i:state): Picture = 
    let fw,fh = getSize <| getRectangle tree
    let cx,cy = fw/2.0, fh/2.0
    let rad = i*System.Math.PI/180.0
    let fig = onto (rotate cx cy rad tree) bck // Force black background
    make fig
// make a list of Pictures for degrees 0 to 259
let lst = List.map (fun i -> draw (float i)) [0..259]

// Write the list of rendered frames to an animated gif file
let repeatCount = 5 // First state drawn by draw
let frameDelay = 10 // milliseconds
animateToFile w h frameDelay repeatCount "animateGif.gif" lst
