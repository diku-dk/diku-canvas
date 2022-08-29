//
// Simple turtle graphics
//
// Compile with "fsharpc -r img_util.dll turtle.fs"
// Run with "mono32 turtle.exe" (on mac); otherwise use "mono"
//

type color = ImgUtil.color

type cmd =
  | SetColor of color
  | Turn of int       // degrees right (0-360)
  | Move of int       // 1 unit = 1 pixel
  | PenUp
  | PenDown

// Interpreter turning commands into lines

type point = int * int
type line = point * point * color

let initColor = ImgUtil.fromRgb (255,255,255)

let pi = System.Math.PI

let rec interp (p:point,d:int,c:color,up:bool) (cmds : cmd list) : line list =
  match cmds with
    | [] -> []
    | SetColor c :: cmds -> interp (p,d,c,up) cmds
    | Turn i :: cmds -> interp (p,d-i,c,up) cmds
    | PenUp :: cmds -> interp (p,d,c,true) cmds
    | PenDown :: cmds -> interp (p,d,c,false) cmds
    | Move i :: cmds ->
      let r = 2.0 * pi * float d / 360.0
      let dx = int(float i * cos r)
      let dy = -int(float i * sin r)
      let (x,y) = p
      let p2 = (x+dx,y+dy)
      let lines = interp (p2,d,c,up) cmds
      if up then lines else (p,p2,c)::lines

let rec repeat n cmds =
  if n <= 0 then []
  else cmds @ repeat (n-1) cmds

let circ sz n =
  let x = 360 / n
  in repeat n [Move sz;Turn x]

let star sz =
  repeat 5 [Move sz; Turn 144]

let rec tree sz =
   if sz < 5 then [Move sz; PenUp; Move (-sz); PenDown]
   else [Move (sz/3);
         Turn -30] @ tree (sz*2/3) @ [Turn 30;
         Move (sz/6);
         Turn 25] @ tree (sz/2) @ [Turn -25;
         Move (sz/3);
         Turn 25] @ tree (sz/2) @ [Turn -25;
         Move (sz/6);
         PenUp;
         Move (-sz/3);
         Move (-sz/6);
         Move (-sz/3);
         Move (-sz/6);
         PenDown]

let pic = circ 50 12 @
          (SetColor ImgUtil.green ::
           circ 20 24) @
          [SetColor ImgUtil.blue;
           PenUp;
           Turn -45;
           Move 200;
           PenDown] @
           circ 30 6 @
           [PenUp;
            Turn 120;
            Move 200;
            PenDown;
            SetColor ImgUtil.red] @
           star 70 @
           [PenUp; Turn 130; Move 400; Turn 150; PenDown] @ tree 200
           @ [PenUp;Turn 90; Move 100; Turn -85; PenDown] @ tree 150

let lightgrey = ImgUtil.fromRgb (220,220,220)
let white = ImgUtil.fromRgb (255,255,255)
let black = ImgUtil.fromRgb (0,0,0)

let draw (w,h) pic =
  let bmp = ImgUtil.mk w h
  for x in [0..w-1] do
    for y in [0..h-1] do
      ImgUtil.setPixel white (x,y) bmp
  let initState = ((w/2,h/2),90,black,false)
  let lines = interp initState pic
  for (p1,p2,c) in lines do
    ImgUtil.setLine c p1 p2 bmp
  ImgUtil.show "Logo" bmp

do draw (600,600) pic

let triangle x =
  [Turn 30; Move x; Turn 120; Move x;
   Turn 120; Move x; Turn 90]

let pic2 =
  [SetColor ImgUtil.red] @
  triangle 100 @
  [PenUp;
   Move -200;   Turn 90; Move 20; Turn -90;
   PenDown;
   SetColor ImgUtil.green] @
  star 150

//do draw (600,600) pic2

let pic3 = Move -200 :: tree 300

// do draw (600,600) pic3
