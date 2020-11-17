module ImgUtil

open System.Drawing

// colors
type color = C of System.Drawing.Color
let red : color = C Color.Red
let blue : color = C Color.Blue
let green : color = C Color.Green
let fromRgb (r:int,g:int,b:int) : color =
  C(Color.FromArgb(255,r,g,b))
let fromArgb (a:int,r:int,g:int,b:int) : color =
  C(Color.FromArgb(a,r,g,b))

let format = Imaging.PixelFormat.Format24bppRgb

// canvas operations
type canvas = Canvas of System.Drawing.Bitmap

let setPixel (C c) (x,y) (Canvas bmp) : unit =
  bmp.SetPixel (x,y,c)

let init h w (f:int*int->color) : canvas =
  let canvas = Canvas(new Bitmap (h, w, format))
  for i in [0..h-1] do
    for j in [0..w-1] do setPixel (f(i,j)) (i,j) canvas
  canvas

let mk h w : canvas =
  let white = fromRgb(255,255,255)
  in init h w (fun _ -> white)

let setLine (C c) (x1:int,y1:int) (x2:int,y2:int) (Canvas bmp) : unit =
  let graphics = Graphics.FromImage bmp
  let pen = new Pen(c)
  in graphics.DrawLine(pen, x1, y1, x2, y2);

let setBox c (x1,y1) (x2,y2) canvas =
  do setLine c (x1,y1) (x2,y1) canvas
  do setLine c (x2,y1) (x2,y2) canvas
  do setLine c (x2,y2) (x1,y2) canvas
  do setLine c (x1,y2) (x1,y1) canvas

// read a bitmap file
let fromFile (fname : string) : canvas =
  Canvas(new Bitmap(fname))

// save a bitmap as a png file
let toPngFile (fname : string) (Canvas bmp) : unit =
  bmp.Save(fname, Imaging.ImageFormat.Png) |> ignore
