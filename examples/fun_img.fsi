//
// Library for Functional images
// Inspired by Conal Elliott's paper on the topic.
// Martin Elsman (c), MIT License
//

module FunImg

// Some type abbreviations
type frac = float                          // floats in interval [0;1]

// functional colors
type fcolor = frac * frac * frac * frac     // alpha,red,green,blue

val greyifyFunColor : fcolor -> fcolor
val boolToFunColor  : bool -> fcolor
val fracToFunColor  : frac -> fcolor

// convert between colors and system colors
val toColor   : fcolor -> ImgUtil.color
val fromColor : ImgUtil.color -> fcolor

type point = float * float
type 'a image = point -> 'a
type region = bool image
type cimage = fcolor image

val vstrip   : region
val checker  : region
val altRings : region

// polar points
type polar_point = float * float
val pi        : float
val fromPolar : polar_point -> point
val toPolar   : point -> polar_point

val polarChecker : int -> region
val wavDist      : frac image

// store a color image into a canvas; the float
// specifies the with and height of the image domain
val imgToCanvas : ImgUtil.canvas -> float -> cimage -> unit

// return a canvas of the specified width and height; the float
// specifies the with and height of the image domain
val imgCanvas : float -> cimage -> int -> int -> ImgUtil.canvas

val lerpC : frac -> fcolor -> fcolor -> fcolor

val bilerpC : frac -> frac -> fcolor -> fcolor -> fcolor -> fcolor -> fcolor

val overC : fcolor -> fcolor -> fcolor

val over : cimage -> cimage -> cimage

val cond : bool image -> 'a image -> 'a image -> 'a image

val lerpI : frac image -> cimage -> cimage -> cimage

val mystique : cimage
val rbRings : cimage
