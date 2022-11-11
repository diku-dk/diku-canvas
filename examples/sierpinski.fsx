#r "nuget:DIKU.Canvas"

open Canvas

// This program draws a Sierpinski triangle. An interactive version
// can be found in "keyboard_example.fsx".
let rec triangle (canvas:canvas) length (x, y) =
  if length < 12 then
    setBox canvas blue (x,y) (x+length,y+length) 
  else 
    let halfLen = length / 2
    triangle canvas halfLen (x+halfLen/2, y)
    triangle canvas halfLen (x, y+halfLen)
    triangle canvas halfLen (x+halfLen, y+halfLen)

let draw w h = 
  let canvas = Canvas.create w h
  triangle canvas 512 (44, 44)
  canvas

runSimpleApp "Sierpinski" 600 600 draw
