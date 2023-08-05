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

/// <summary>Gets a canvas color from RGB values.</summary>
/// <param name="red">Red color value.</param>
/// <param name="green">Green color value.</param>
/// <param name="blue">Blue color value.</param>
/// <returns>A canvas color.</returns>
val fromRgb   : red: int * green: int * blue: int -> color

/// <summary>Gets a canvas color from ARGB values.</summary>
/// <param name="red">Red color value.</param>
/// <param name="green">Green color value.</param>
/// <param name="blue">Blue color value.</param>
/// <param name="alpha">Alpha (opaqueness) value.</param>
/// <returns>A canvas color.</returns>
val fromArgb  : red: int * green: int * blue: int * alpha: int -> color

/// <summary>Gets the ARGB value from a canvas color.</summary>
/// <param name="color">A canvas color.</param>
/// <param name="red">Red color value.</param>
/// <param name="green">Green color value.</param>
/// <param name="blue">Blue color value.</param>
/// <param name="alpha">Alpha value.</param>
/// <returns>A tuple containing the ARGB values.</returns>
/// <remarks>The tuple is returned as (red, green, blue, alpha).</remarks>
val fromColor : color -> int * int * int * int

// canvas
type canvas

/// <summary>Creates a canvas with a given width and height.</summary>
/// <param name="width">Width of the canvas.</param>
/// <param name="height">Height of the canvas.</param>
/// <returns>A canvas with a given width and height.</returns>
val create      : width: int -> height: int -> canvas

/// <summary>Creates a canvas with a given width and height where each pixel's color is defined by a given pixel-wise color-mapping function.</summary>
/// <param name="width">Width of the canvas.</param>
/// <param name="height">Height of the canvas.</param>
/// <param name="colorMapping">A function which takes in a pixel's coordinates and will output a color; this color will be drawn onto the coordinates.</param>
///<returns>A canvas with a given width and height.</returns>
val init        : width: int -> height: int -> colorMapping: (int*int -> color) -> canvas

/// <summary>Draws a pixel with a given color on a canvas.</summary>
/// <param name="canvas">The canvas to draw onto.</param>
/// <param name="color">The color to draw.</param>
/// <param name="point">The x, y coordinates of the pixel.</param>
val setPixel    : canvas -> color: color -> point: int * int -> unit

/// <summary>Gets the color of a given pixel.</summary>
/// <param name="canvas">The canvas to get the pixel from.</param>
/// <param name="point">The x, y coordinates of the pixel.</param>
///<returns>Returns the color of a given pixel. </returns>
val getPixel    : canvas -> point: int * int -> color

/// <summary>Draws a line of a given color from A to B</summary>
/// <param name="canvas">The canvas to draw onto.</param>
/// <param name="color">The color of the line.</param>
/// <param name="pointA">The first x, y coordinate.</param>
/// <param name="pointB">The second x, y coordinate.</param>
val setLine     : canvas -> color -> pointA: int * int -> pointB: int * int -> unit

/// <summary>Draws a box of a given color.</summary>
/// <param name="canvas">The canvas to draw onto.</param>
/// <param name="color">The color of the box.</param>
/// <param name="pointA">The first x, y coordinate.</param>
/// <param name="pointB">The second x, y coordinate.</param>
/// <remarks>PointA is the top left corner. PointB is the bottom right corner.</remarks>
val setBox      : canvas -> color -> pointA: int * int -> pointB: int * int -> unit

/// <summary>Draws a box filled with a given color.</summary>
/// <param name="canvas">The canvas to draw onto.</param>
/// <param name="color">The color of the box.</param>
/// <param name="pointA">The first x, y coordinate.</param>
/// <param name="pointB">The second x, y coordinate.</param>
/// <remarks>PointA is the top left corner. PointB is the bottom right corner.</remarks>
val setFillBox  : canvas -> color -> pointA: int * int -> pointB: int * int -> unit

/// <summary>Get the width of a given canvas.</summary>
/// <param name="canvas">The canvas.</param>
/// <returns>Returns the width of the canvas.</returns>
val width       : canvas -> int

/// <summary>Get the height of a given canvas.</summary>
/// <param name="canvas">The canvas.</param>
/// <returns>Returns the height of the canvas.</returns>
val height      : canvas -> int

/// <summary>Stretch a given canvas to a new width and height.</summary>
/// <param name="canvas">The canvas to stretch.</param>
/// <param name="width">The new width.</param>
/// <param name="height">The new height.</param>
/// <returns>Returns a new stretched canvas.</returns>
val scale       : canvas -> width: int -> height: int -> canvas

/// <summary>A type representing a turtle command.</summary>
/// <typeparam name="SetColor">The color to draw with.</typeparam>
/// <typeparam name="Turn">Turn the turtle by some degrees clockwise.</typeparam>
/// <typeparam name="Turn">Move the turtle by some pixels.</typeparam>
/// <typeparam name="PenUp">Stops drawing.</typeparam>
/// <typeparam name="PenDown">Begins drawing.</typeparam>
type turtleCmd = 
  | SetColor of color
  | Turn of int       // degrees right (0-360)
  | Move of int       // 1 unit = 1 pixel
  | PenUp
  | PenDown
  
/// <summary>Initialises a new canvas and draws a picture according to a given list of turtle commands.</summary>
/// <param name="dimensions">The width and height of the canvas.</param>
/// <param name="title">The title of the canvas.</param>
/// <param name="turtleCommands">A list of turtle commands which the turtle will draw onto the canvas.</param>
/// <remarks>The initial state of the turtle points 90 deg (upwards) and pen down.</remarks>
val turtleDraw : dimensions: int*int -> title: string -> turtleCommands: turtleCmd list -> unit

/// <summary>Load a canvas from an image file (e.g., a png-file).</summary>
/// <param name="filePath">The absolute path to the image file.</param>
/// <returns>Returns a canvas with the image drawn onto it.</returns>
val fromFile  : filePath: string -> canvas

/// <summary>Save a canvas to a png file.</summary>
/// <param name="canvas">The canvas to save.</param>
/// <param name="filePath">The absolute path to the image file.</param>
/// <returns>Returns a canvas with the image drawn onto it.</returns>
val toPngFile : canvas -> filePath: string -> unit

/// <summary>Shows a canvas.</summary>
/// <param name="canvas">The canvas to show.</param>
/// <param name="title">The title of the canvas.</param>
val show      : canvas -> title: string  -> unit

/// <summary>Runs a simple non-interactive application.</summary>
/// <param name="title">The title of the canvas.</param>
/// <param name="width">The width of the canvas.</param>
/// <param name="height">The height of the canvas.</param>
/// <param name="draw">A function that draws a canvas.</param>
val runSimpleApp : title: string -> width: int -> height: int
                -> draw: (int -> int -> canvas)
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

// Convert a `key : Keysym of int` into an `ImgUtilKey`
// Not pretty, but "good enough"
/// <summary>Converts a key to ImgUtilKey.</summary>
/// <param name="key">The key to convert</param>
val getKey : key -> ImgUtilKey

/// <summary>Runs a simple interactive application.</summary>
/// <param name="title">The title of the canvas.</param>
/// <param name="width">The width of the canvas.</param>
/// <param name="height">The height of the canvas.</param>
/// <param name="draw">A function that draws a canvas.</param>
/// <param name="react">A function that reacts to keyboard inputs.</param>
/// <param name="state">A generic state.</param>
val runApp    : title: string -> width: int -> height: int
             -> draw: (int -> int -> 's -> canvas)
             -> react: ('s -> key -> 's option)
             -> state: 's -> unit

type event =
    | KeyDown of key
    | TimerTick
    | MouseButtonDown of int * int // x,y
    | MouseButtonUp of int * int // x,y
    | MouseMotion of int * int * int * int // x,y, relx, rely

/// Start an app that can listen to key-events and timer-events
val runAppWithTimer: string -> int -> int -> int option
             -> (int -> int -> 's -> canvas)
             -> ('s -> event -> 's option)
             -> 's -> unit
