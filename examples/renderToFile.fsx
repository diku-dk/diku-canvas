#r "nuget:DIKU.Canvas, 2.0.0-alpha8"
open Canvas

// The canvas size
let w,h = 256,256

// Make a non-trivial tree
let lst1 = [(10.0,10.0); (60.0, 80.0); (10.0, 80.0); (10.0, 10.0)]
let lst2 = [(0.0,0.0); (40.0, 30.0); (0.0, 30.0); (0.0, 0.0)]
let tri1 = piecewiseAffine green 1.0 lst1
let tri2 = filledPolygon purple lst2
let ell = ellipse blue 4.0 20.0 25.0
let tree = translate 50.0 50.0 (alignV (alignH tri1 Center tri2) Center ell)

// We force the background to be black
let bck = filledRectangle black w h
let fig = onto tree bck

// Render and save to file
renderToFile w h "renderToFile.png" (make fig)
