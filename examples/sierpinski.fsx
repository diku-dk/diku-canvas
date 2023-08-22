#r "nuget:DIKU.Canvas, 2.0.0-alpha8"
open Canvas

let w,h,l = 600,600,512 // size of window and base length of triangle

// This program draws a Sierpinski triangle with bounding box length,length.
let rec triangle (length:int) (x:int, y:int): PrimitiveTree =
  if length < 12 then // Smallest element to draw
    filledRectangle blue length length
    |> translate x y
  else // every triangle consists of 3 smaller trangles
    let halfLen = length / 2
    onto (triangle halfLen (x+halfLen/2, y))
      (onto (triangle halfLen (x, y+halfLen))
        (triangle halfLen (x+halfLen, y+halfLen)))

/// Prepare a Picture by the present state whenever needed
let draw (): Picture = 
    let x,y = (w-l)/2, (h-l)/2
    let tri = triangle l (x,y)
    make tri

// Render Sierpinski's triangle
render "Sierpinski" w h draw
