//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha6"
open Canvas

let w,h = 400,400 // The size of the canvas

/// A recursive function for calculating a square, outwards going spiral
/// starting in x,y and with a spacing of s.
let spiral s x y =
  let rec iterate s i x y =
    if i >= 350.0 then 
      emptyTree
    else 
      let p1 = (x,y)
      let p2 = (x+i,y)
      let p3 = (x+i,y+i)
      let p4 = (x-s,y+i)
      let p5 = (x-s,y-s)
      onto (piecewiseAffine red 1.0 [p1; p2])
        (onto (piecewiseAffine red 1.0 [p2; p3])
          (onto (piecewiseAffine red 1.0 [p3; p4])
            (onto (piecewiseAffine red 1.0 [p4; p5])
              (iterate s (i+2.0*s) (x-s) (y-s)))))
  iterate s s x y

/// Prepare a Picture by the present state whenever needed
let draw (): Picture = 
    make (spiral 20.0 200.0 200.0)

// Render the picture to the screen
render "Spiral" w h draw