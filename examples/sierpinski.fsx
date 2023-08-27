#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha9"
open Canvas
open Color

let w,l = 512,64 // size of window and base length of triangle

// This program draws a Sierpinski triangle with bounding box length,length.
let rec triangle (c:color) (minLength:float) (length:float): PrimitiveTree =
  if length <= minLength then // Smallest element to draw
    filledPolygon c [0.0,length; length,length; length/2.0,0.0; 0.0,length]
  else // every triangle consists of 3 smaller trangles
    let subTriangle = triangle c minLength (length/2.0)
    alignV subTriangle Center (alignH subTriangle Center subTriangle)

/// Prepare a Picture by the present state whenever needed
let draw = 
  let tree = 
    triangle blue (float l) (float w)
  let ((x1,y1),(x2,y2)) = getBoundingBox tree
  translate -x1 y1 tree |> make

// Render Sierpinski's triangle
render "Sierpinski" w w draw
