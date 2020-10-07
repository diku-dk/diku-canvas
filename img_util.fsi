module ImgUtil

// colors
type color
val red      : color
val blue     : color
val green    : color
val fromRgb  : int * int * int -> color
val fromArgb : int * int * int * int -> color
val fromColor : color -> int * int * int * int

// bitmaps
type bitmap
val mk       : int -> int -> bitmap
val init     : int -> int -> (int*int -> color) -> bitmap
val setPixel : color -> int * int -> bitmap -> unit
val getPixel : bitmap -> int * int -> color
val setLine  : color -> int * int -> int * int -> bitmap -> unit
val setBox   : color -> int * int -> int * int -> bitmap -> unit
val width    : bitmap -> int
val height   : bitmap -> int
val scale    : bitmap -> int -> int -> bitmap

// read a bitmap file
val fromFile : string -> bitmap

// save a bitmap as a png file
val toPngFile : string -> bitmap -> unit

// show bitmap in a gui
val show : string -> bitmap -> unit

// start a simple app
val runSimpleApp : string -> int -> int
                -> (bitmap -> unit) -> unit

type Key = Gdk.Key

// start an app that can listen to key-events
val runApp : string -> int -> int
          -> (int -> int -> 's -> bitmap)
          -> ('s -> Key -> 's option)
          -> 's -> unit
