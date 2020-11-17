module ImgUtil

/// colors
type color
val red      : color
val blue     : color
val green    : color
val fromRgb  : int * int * int -> color
val fromArgb : int * int * int * int -> color

/// canvas
type canvas
val mk        : int -> int -> canvas
val init      : int -> int -> (int*int -> color) -> canvas
val setPixel  : color -> int * int -> canvas -> unit
val setLine   : color -> int * int -> int * int -> canvas -> unit
val setBox    : color -> int * int -> int * int -> canvas -> unit

/// read canvas from a file
val fromFile : string -> canvas

/// save a bitmap as a png file
val toPngFile : string -> canvas -> unit
