//
// Library for Functional images
// Inspired by Conal Elliott's paper on the topic from the
// book "The Fun of Programming".
// Martin Elsman (c), MIT License
//

module FunImg

  // Some type abbreviations
  type frac = float                          // floats in interval [0;1]
  type fcolor = frac * frac * frac * frac     // alpha,red,green,blue

  let toColor ((a,r,g,b):fcolor) : ImgUtil.color =
    ImgUtil.fromArgb (int(255.0*a),int(255.0*r),int(255.0*g),int(255.0*b))

  let fromColor (c:ImgUtil.color) : fcolor =
    let (a,r,g,b) = ImgUtil.fromColor c
    let conv i = (float)i / 255.0
    in (conv a, conv r, conv g, conv b)

  let greyifyFunColor ((a,r,g,b):fcolor) : fcolor =
    let v = (r + g + b) / 3.0
    in (a,v,v,v)

  type point = float * float
  type 'a image = point -> 'a
  type region = bool image

  let vstrip : region =
    fun (x,y) -> abs x <= 0.5

  let even x = x % 2 = 0

  let floori x = int(floor x)

  let checker : region =
    fun (x,y) -> even(floori x + floori y)

  let distO (x,y) = sqrt(x*x+y*y)

  let altRings : region =
    even << floori << distO

  type polar_point = float * float

  let pi = System.Math.PI
  let fromPolar ((r,t):polar_point) : point = (r*cos t, r*sin t)
  let toPolar ((x,y):point) : polar_point = (distO (x,y), atan2 y x)

  let polarChecker n : region =
    let sc (r,t) = (r, t * float n / pi)
    in checker << sc << toPolar

  let wavDist : frac image =
    fun p -> (1.0 + cos (pi * distO p)) / 2.0

  let imgToBitmap (bmp:ImgUtil.bitmap) (width:float) (img:fcolor image): unit =
    let (w,h) = (ImgUtil.width bmp, ImgUtil.height bmp)
    for x in [0..w-1] do
      for y in [0..h-1] do
        let p_x = width * float (x - w/2) / float w
        let p_y = width * float (y - h/2) / float h
        let fc = img (p_x,p_y)
        let c = toColor fc
        in ImgUtil.setPixel c (x,y) bmp

  let imgBitmap (width:float) (img:fcolor image) w h : ImgUtil.bitmap =
    let bmp = ImgUtil.mk w h
    do imgToBitmap bmp width img
    bmp

  let boolToFunColor b =
    if b then (1.0,0.0,0.0,0.0)
    else (1.0,1.0,1.0,1.0)

  let fracToFunColor (f:frac) = (1.0,f,f,f)

  let lerpC w ((r1,g1,b1,a1):fcolor) ((r2,g2,b2,a2):fcolor) : fcolor =
    let h x1 x2 = w * x1 + (1.0-w)*x2
    in (h r1 r2, h g1 g2, h b1 b2, h a1 a2)

  let bilerpC w h c1 c2 c3 c4 =
    let c_low = lerpC w c1 c2
    let c_high = lerpC w c3 c4
    in lerpC h c_low c_high

  let overC ((r1,g1,b1,a1):fcolor) ((r2,g2,b2,a2):fcolor) : fcolor =
    let h x1 x2 = x1 + (1.0-a1)*x2
    in (h r1 r2,h g1 g2, h b1 b2, h a1 a2)

  let lift1 h f1 p = h (f1 p)
  let lift2 h f1 f2 p = h (f1 p) (f2 p)
  let lift3 h f1 f2 f3 p = h (f1 p) (f2 p) (f3 p)

  type imageC = fcolor image

  let over : imageC -> imageC -> imageC =
    fun x y -> lift2 overC x y

  let cond : bool image -> 'a image -> 'a image -> 'a image =
    fun x y c -> lift3 (fun a b c -> if a then b else c) x y c

  let lerpI : frac image -> imageC -> imageC -> imageC =
    fun x y z -> lift3 lerpC x y z

  let constant a (p:point) = a
  let blueI = constant (1.0,0.0,0.0,1.0)
  let redI = constant (1.0,1.0,0.0,0.0)
  let greenI = constant (1.0,0.0,1.0,0.0)
  let yellowI = constant (1.0,0.0,1.0,1.0)

  let rbRings = lerpI wavDist redI blueI
  let mystique : imageC =
    lerpI (constant 0.2) (boolToFunColor<<checker) rbRings
