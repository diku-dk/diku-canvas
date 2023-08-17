//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Canvas

// The canvas size
let w,h = 256,256

// Make a non-trivial tree
let lst1 = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let lst2 = [(0.0,0.0); (40.0, 30.0); (0.0, 30.0); (0.0, 0.0)]
let tri1 = piecewiseaffine color.Green 1.0 lst1
let tri2 = filledpolygon color.Purple lst2
let ell = ellipse color.Blue 4.0 20.0 25.0
let tree = translate 50.0 50.0 (alignv (alignh tri1 Center tri2) Center ell)

// We force the background to be black
let bck = filledrectangle color.Black w h
let fig = onto tree bck

// Render and save to file
renderToFile w h "renderToFile.png" (make fig)
