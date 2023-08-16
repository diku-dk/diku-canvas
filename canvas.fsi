module Canvas

///<summary>A color define as 4 values red, green, blue, and alpha.</summary>
type color = Lowlevel.color
///<summary>A font defined by a font name and its size in dots-per-inch (dpi).</summary>
type Font = Lowlevel.Font
///<summary>A collection of font variations specified by the family name.</summary>
type FontFamily = Lowlevel.FontFamily

///<summary>Represents the size of a picture, defined by x1,y1,x2,y2 where x2>x1 and y2>y1.</summary>
type Rectangle = float*float*float*float

///<summary>Represents the size of a picture, defined by its width and height.</summary>
type Size = float*float

///<summary>Represents one of the 5 control keys: DownArrow, UpArrow, LeftArrow, RightArrow, and Space.</summary>
type ControlKey = Lowlevel.ControlKey
///<summary>Represents one of the events: KeyDown, TimerTick, MouseButtonDown, MouseButtonUp, and MouseMotion.</summary>
type Event = Lowlevel.Event

///<summary>A tree of graphics primitives, define as a tree of graphics primitives.</summary>
type PrimitiveTree

///<summary>A picture, define as a tree of graphics primitives.</summary>
type Picture = Lowlevel.drawing_fun

///<summary>An alignh position-value for aligning boxes along their top edge.</summary>
val Top: float
///<summary>An alignv position-value for aligning boxes along their left edge.</summary>
val Left: float
///<summary>An alignh and alignv position-value for aligning boxes along their center.</summary>
val Center: float
///<summary>An alignh position-value for aligning boxes along their bottom edge.</summary>
val Bottom: float
///<summary>An alignv position-value for aligning boxes along their right edge.</summary>
val Right: float

///<summary>Retrieves the size of a given graphic primitive tree.</summary>
///<param name="p">The graphic primitive tree for which to get the size.</param>
///<returns>The bounding box of the tree as x1,y1,x2,y2 where x2>x1 and y2>y1.</returns>
val getRectangle : p: PrimitiveTree -> Rectangle

///<summary>Converts a rectangle into a size.</summary>
///<param name="rect">The bounding box as a rectangle x1,y1,x2,y2 where x2>x1 and y2>y1.</param>
///<returns>The width and height of the bounding box.</returns>
val getSize : rect:Rectangle -> Size

///<summary>Translates a given graphic primitive tree by the specified distances along the x and y axes.</summary>
///<param name="dx">The distance to translate the graphic primitive tree along the x-axis.</param>
///<param name="dy">The distance to translate the graphic primitive tree along the y-axis.</param>
///<param name="p">The graphic primitive tree to be translated.</param>
///<returns>A new graphic primitive tree object that is translated by the specified distances.</returns>
val translate : dx:float -> dy:float -> p: PrimitiveTree -> PrimitiveTree 

///<summary>Scales a given graphic primitive tree by the specified factors along the x and y axes.</summary>
///<param name="sx">The scaling factor along the x-axis.</param>
///<param name="sy">The scaling factor along the y-axis.</param>
///<param name="p">The graphic primitive tree to be scaled.</param>
///<returns>A new graphic primitive tree object that is scaled by the specified factors.</returns>
val scale: sx:float -> sy:float -> p: PrimitiveTree -> PrimitiveTree 

///<summary>Rotates a given graphic primitive tree by the specified angle in radians around a point.</summary>
///<param name="x">The x-coordinate of the point around which to rotate.</param>
///<param name="y">The y-coordinate of the point around which to rotate.</param>
///<param name="rad">The angle in radians to rotate the graphic primitive tree.</param>
///<param name="p">The graphic primitive tree to be rotated.</param>
///<returns>A new graphic primitive tree object that is rotated by the specified angle.</returns>
val rotate: x:float -> y:float -> rad:float -> p: PrimitiveTree -> PrimitiveTree 

///<summary>Creates a piecewise affine transformation on a graphic primitive tree.</summary>
///<param name="c">The color to be used.</param>
///<param name="sw">The stroke width.</param>
///<param name="lst">The list of control points for the transformation.</param>
///<returns>A new graphic primitive tree object representing the transformed graphic primitive tree.</returns>
val piecewiseaffine: c:color -> sw: float -> lst:(float*float) list -> PrimitiveTree 

///<summary>Creates a filled polygon with the specified vertices.</summary>
///<param name="c">The color of the polygon.</param>
///<param name="lst">The list of vertices of the polygon.</param>
///<returns>A new graphic primitive tree object representing the filled polygon.</returns>
val filledpolygon: c:color -> lst:(float*float) list -> PrimitiveTree 

///<summary>Creates a rectangle with the specified width, height, and stroke width.</summary>
///<param name="c">The color of the rectangle.</param>
///<param name="sw">The stroke width of the rectangle.</param>
///<param name="w">The width of the rectangle.</param>
///<param name="h">The height of the rectangle.</param>
///<returns>A new graphic primitive tree object representing the rectangle.</returns>
val rectangle: c:color -> sw: float -> w:float -> h:float -> PrimitiveTree 

///<summary>Creates a filled rectangle with the specified width and height.</summary>
///<param name="c">The color of the rectangle.</param>
///<param name="w">The width of the rectangle.</param>
///<param name="h">The height of the rectangle.</param>
///<returns>A new graphic primitive tree object representing the filled rectangle.</returns>
val filledrectangle: c:color -> w:float -> h:float -> PrimitiveTree 

///<summary>Creates an ellipse with the specified radii and stroke width.</summary>
///<param name="c">The color of the ellipse.</param>
///<param name="sw">The stroke width of the ellipse.</param>
///<param name="rx">The horizontal radius of the ellipse.</param>
///<param name="ry">The vertical radius of the ellipse.</param>
///<returns>A new graphic primitive tree object representing the ellipse.</returns>
val ellipse: c:color -> sw:float -> rx:float -> ry:float -> PrimitiveTree 

///<summary>Creates a filled ellipse with the specified radii.</summary>
///<param name="c">The color of the ellipse.</param>
///<param name="rx">The horizontal radius of the ellipse.</param>
///<param name="ry">The vertical radius of the ellipse.</param>
///<returns>A new graphic primitive tree object representing the filled ellipse.</returns>
val filledellipse: c:color -> rx:float -> ry:float -> PrimitiveTree 

///<summary>Places one graphic primitive tree on top of another.</summary>
///<param name="pic1">The first graphic primitive tree, which will be placed on top of the second.</param>
///<param name="pic2">The second graphic primitive tree, over which the first graphic primitive tree will be placed.</param>
///<returns>A new graphic primitive tree object representing the combined image with the first graphic primitive tree on top of the second.</returns>
val ontop : pic1: PrimitiveTree -> pic2: PrimitiveTree -> PrimitiveTree 

///<summary>Aligns two graphic primitive trees horizontally at a specific position.</summary>
///<param name="pic1">The first graphic primitive tree to be aligned.</param>
///<param name="pos">The position at which to align the graphic primitive trees along the horizontal axis.</param>
///<param name="pic2">The second graphic primitive tree to be aligned.</param>
///<returns>A new graphic primitive tree object representing the two graphic primitive trees aligned horizontally at the specified position.</returns>
val alignh : pic1: PrimitiveTree -> pos:float -> pic2: PrimitiveTree -> PrimitiveTree 

///<summary>Aligns two graphic primitive trees vertically at a specific position.</summary>
///<param name="pic1">The first graphic primitive tree to be aligned.</param>
///<param name="pos">The position at which to align the graphic primitive trees along the vertical axis.</param>
///<param name="pic2">The second graphic primitive tree to be aligned.</param>
///<returns>A new graphic primitive tree object representing the two graphic primitive trees aligned vertically at the specified position.</returns>
val alignv : pic1: PrimitiveTree -> pos:float -> pic2: PrimitiveTree -> PrimitiveTree 

///<summary>Creates a Picture object from a given graphic primitive tree.</summary>
///<param name="p">The graphic primitive tree object used to create the graphic primitive tree.</param>
///<returns>A new Picture object representing the given graphic primitive tree.</returns>
val make : p:PrimitiveTree -> Picture

///<summary>Provides a visual explanation of a graphic primitive tree object.</summary>
///<param name="p">The graphic primitive tree object to be explained.</param>
///<returns>A new Picture object representing a visual explanation of the given graphic primitive tree.</returns>
val explain : p:PrimitiveTree -> Picture

///<summary>Converts a graphic primitive tree into its string representation.</summary>
///<param name="p">The graphic primitive tree to be converted into a string.</param>
///<returns>A string representing the given graphic primitive tree.</returns>
val tostring : p:PrimitiveTree -> string

///<summary>Draws a picture generated by make or explain to a file with the specified width and height.</summary>
///<param name="width">The width of the output image in pixels.</param>
///<param name="height">The height of the output image in pixels.</param>
///<param name="filePath">The file path where the image will be saved.</param>
///<param name="draw">The picture object to be drawn.</param>
///<returns>A unit value indicating the completion of the operation.</returns>
val drawToFile : width:int -> height:int -> filePath:string -> draw:Picture -> unit

///<summary>Draws a list of pictures generated by make or explain to an animated GIF file with the specified width, height, frame delay, and repeat count.</summary>
///<param name="width">The width of each frame in pixels.</param>
///<param name="heigth">The height of each frame in pixels. (Note: there's a typo in the parameter name; it should likely be "height").</param>
///<param name="frameDelay">The delay between frames in the animation, in milliseconds.</param>
///<param name="repeatCount">The number of times the animation should repeat. Use 0 for infinite looping.</param>
///<param name="filePath">The file path where the animated GIF will be saved.</param>
///<param name="drawLst">The list of picture objects to be drawn as frames in the animation.</param>
///<returns>A unit value indicating the completion of the operation.</returns>
val drawToAnimatedGif : width:int -> height:int -> frameDelay:int -> repeatCount:int -> filePath:string -> drawLst:Picture list -> unit

///<summary>Runs an application with a timer, defining the drawing and reaction functions.</summary>
///<param name="t">The title of the application window.</param>
///<param name="w">The width of the application window in pixels.</param>
///<param name="h">The height of the application window in pixels.</param>
///<param name="interval">An optional interval for the timer, in milliseconds. If provided, it specifies the time between timer ticks.</param>
///<param name="draw">A function that takes the current state and returns a Picture object representing the current visual state of the application.</param>
///<param name="react">A function that takes the current state and an Event object and returns an optional new state, allowing the application to react to events.</param>
///<param name="s">The initial state of the application.</param>
///<returns>A unit value indicating the completion of the operation.</returns>
val runAppWithTimer : t:string -> w:int -> h:int -> interval:int option -> draw: ('s -> Picture) -> react: ('s -> Event -> 's option) -> s:'s-> unit

///<summary>Runs an application, defining the drawing function.</summary>
///<param name="t">The title of the application window.</param>
///<param name="w">The width of the application window in pixels.</param>
///<param name="h">The height of the application window in pixels.</param>
///<param name="draw">A function that returns a Picture object representing the current visual state of the application.</param>
///<returns>A unit value indicating the completion of the operation.</returns>
val runApp : t:string -> w:int -> h:int -> draw: (unit -> Picture) ->  unit
