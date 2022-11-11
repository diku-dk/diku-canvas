module Canvas

// colors
type color
val red       : color
val blue      : color
val green     : color
val yellow    : color
val lightgrey : color
val white     : color
val black     : color
val fromRgb   : int * int * int -> color
val fromArgb  : int * int * int * int -> color
val fromColor : color -> int * int * int * int

// canvas
type canvas
type point = int * int
val create      : int -> int -> canvas
val init        : int -> int -> (point -> color) -> canvas
val setPixel    : canvas -> color -> point -> unit
val getPixel    : canvas -> point -> color
val setLine     : canvas -> color -> point -> point -> unit
val setBox      : canvas -> color -> point -> point -> unit
val setFillBox  : canvas -> color -> point -> point -> unit
val width       : canvas -> int
val height      : canvas -> int
val scale       : canvas -> int -> int -> canvas

/// Turtle graphics
type turtleCmd = 
  | SetColor of color
  | Turn of int       // degrees right (0-360)
  | Move of int       // 1 unit = 1 pixel
  | PenUp
  | PenDown
val turtleDraw : point -> string -> turtleCmd list -> unit


/// Load a canvas from an image file (e.g., a png-file)
val fromFile  : string -> canvas

/// Save a canvas as a png file
val toPngFile : canvas -> string -> unit


/// Show canvas in a GUI window
val show      : canvas -> string  -> unit

/// Start a simple app
val runSimpleApp : string -> int -> int
                -> (int -> int -> canvas)
                -> unit

/// Simplification of SDL keys used for keypress/keydown/keyup events
type key = Keysym of int

/// Wrapper-type for supported keys in Canvas, for easier/prettier pattern-matching
type ImgUtilKey =
    | Unknown
    | DownArrow
    | UpArrow
    | LeftArrow
    | RightArrow
    | Space

/// Convert a `key : Keysym of int` into an `ImgUtilKey`
/// Not pretty, but "good enough"
val getKey : key -> ImgUtilKey

/// Start an app that can listen to key-events
val runApp    : string -> int -> int
             -> (int -> int -> 's -> canvas)
             -> ('s -> key -> 's option)
             -> 's -> unit
