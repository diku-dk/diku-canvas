open ImgUtil // make the library available to use without prepending ImgUtil.
// create a canvas object with width=100 and height=200
let canvas = mk 100 200
// define red as a color
let (myRed:color) = fromRgb (255,0,0)
// Make three red dots. setPixel manipulates the given canvas. it returns unit.
// This is "weird" in f#. We are used to all functions having a result and all
// values being immutable.
// In this case, the canvas is being manipulated but not returned. (in academic speak, setPixel is an impure function)
do setPixel myRed (49,99) canvas
do setPixel myRed (50,98) canvas
do setPixel myRed (51,99) canvas

do setLine blue (20,20) (10,150) canvas

// Save it as a png file - it will have three red dots
// even though we did NOT have code like "canvas <- setPixel myRed (49,99) canvas"
// the contents of canvas are still updated.

do printfn "Writing file testfile.png"
do toPngFile "testfile.png" canvas

let canvas2 = fromFile "apple.jpg"

do setLine blue (20,20) (90,150) canvas2

do printfn "Writing file testfile2.png"
do toPngFile "testfile2.png" canvas2
