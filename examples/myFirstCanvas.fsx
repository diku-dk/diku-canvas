#r "nuget:DIKU.Canvas, 2.0.0-alpha8"
open Canvas

let w,h = 256,256
let tree = filledRectangle green ((float w)/2.0) ((float h)/2.0)
let draw = fun _ -> make tree
render "My first canvas" w h draw
