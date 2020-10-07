type point = int * int          // a point (x, y) in the plane
type colour = int * int * int   // (red, green, blue), 0..255 each
type figure = point * point * point * colour

let triarea2 (x1,y1) (x2,y2) (x3,y3) : int =
  abs(x1*(y2-y3)+x2*(y3-y1)+x3*(y1-y2))

// finds colour of figure at point
let colourAt (x,y) (p1,p2,p3,col) =
  let sum = triarea2 p1 p2 (x,y) + triarea2 p1 p3 (x,y) + triarea2 p2 p3 (x,y)
  in if sum > triarea2 p1 p2 p3 then None else Some col

let checkColor (r,g,b) =
  0 <= r && r <= 255 &&
  0 <= g && g <= 255 &&
  0 <= b && b <= 255

let checkFigure (_,_,_,c) =
  checkColor c

let move fig (a,b) : figure =
  let ((x1,y1),(x2,y2),(x3,y3),c) = fig
  in ((x1+a,y1+b),(x2+a,y2+b),(x3+a,y3+b),c)

let minPoint (x1,y1) (x2,y2) =
  (min x1 x2,min y1 y2)

let maxPoint (x1,y1) (x2,y2) =
  (max x1 x2,max y1 y2)

let yellow = (255,255,0)
let blue = (0,0,255)
let red = (255,0,0)

let fig_test : figure =
  ((50,50),(100,100),(75, 115),blue)

let fig_test_move = move fig_test (-20,20)

let makePicture filnavn figur b h =
  let bmp = ImgUtil.mk b h
  for x in [0..b-1] do
    for y in [0..h-1] do
      let c =
        match colourAt (x,y) figur with
          | None -> (128,128,128)
          | Some c -> c
      do ImgUtil.setPixel (ImgUtil.fromRgb c) (x,y) bmp
  let target = filnavn + ".png"
  do ImgUtil.toPngFile target bmp
  do printfn "Wrote file: %s" target

do makePicture "fig_test" fig_test 200 200
do makePicture "fig_test_move" fig_test_move 200 200
