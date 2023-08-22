module Canvas

/// <summary>A color define as 4 values red, green, blue, and alpha.</summary>
type color

val aliceBlue : color
val antiqueWhite : color
val aqua : color
val aquamarine : color
val azure : color
val beige : color
val bisque : color
val black : color
val blanchedAlmond : color
val blue : color
val blueViolet : color
val brown : color
val burlyWood : color
val cadetBlue : color
val chartreuse : color
val chocolate : color
val coral : color
val cornflowerBlue : color
val cornsilk : color
val crimson : color
val cyan : color
val darkBlue : color
val darkCyan : color
val darkGoldenrod : color
val darkGray : color
val darkGreen : color
val darkGrey : color
val darkKhaki : color
val darkMagenta : color
val darkOliveGreen : color
val darkOrange : color
val darkOrchid : color
val darkRed : color
val darkSalmon : color
val darkSeaGreen : color
val darkSlateBlue : color
val darkSlateGray : color
val darkSlateGrey : color
val darkTurquoise : color
val darkViolet : color
val deepPink : color
val deepSkyBlue : color
val dimGray : color
val dimGrey : color
val dodgerBlue : color
val firebrick : color
val floralWhite : color
val forestGreen : color
val fuchsia : color
val gainsboro : color
val ghostWhite : color
val gold : color
val goldenrod : color
val gray : color
val green : color
val greenYellow : color
val grey : color
val honeydew : color
val hotPink : color
val indianRed : color
val indigo : color
val ivory : color
val khaki : color
val lavender : color
val lavenderBlush : color
val lawnGreen : color
val lemonChiffon : color
val lightBlue : color
val lightCoral : color
val lightCyan : color
val lightGoldenrodYellow : color
val lightGray : color
val lightGreen : color
val lightGrey : color
val lightPink : color
val lightSalmon : color
val lightSeaGreen : color
val lightSkyBlue : color
val lightSlateGray : color
val lightSlateGrey : color
val lightSteelBlue : color
val lightYellow : color
val lime : color
val limeGreen : color
val linen : color
val magenta : color
val maroon : color
val mediumAquamarine : color
val mediumBlue : color
val mediumOrchid : color
val mediumPurple : color
val mediumSeaGreen : color
val mediumSlateBlue : color
val mediumSpringGreen : color
val mediumTurquoise : color
val mediumVioletRed : color
val midnightBlue : color
val mintCream : color
val mistyRose : color
val moccasin : color
val navajoWhite : color
val navy : color
val oldLace : color
val olive : color
val oliveDrab : color
val orange : color
val orangeRed : color
val orchid : color
val paleGoldenrod : color
val paleGreen : color
val paleTurquoise : color
val paleVioletRed : color
val papayaWhip : color
val peachPuff : color
val peru : color
val pink : color
val plum : color
val powderBlue : color
val purple : color
val rebeccaPurple : color
val red : color
val rosyBrown : color
val royalBlue : color
val saddleBrown : color
val salmon : color
val sandyBrown : color
val seaGreen : color
val seaShell : color
val sienna : color
val silver : color
val skyBlue : color
val slateBlue : color
val slateGray : color
val slateGrey : color
val snow : color
val springGreen : color
val steelBlue : color
val tan : color
val teal : color
val thistle : color
val tomato : color
val transparent : color
val turquoise : color
val violet : color
val wheat : color
val white : color
val whiteSmoke : color
val yellow : color
val yellowGreen : color

/// <summary>A font defined by a font name and its size in dots-per-inch (dpi).</summary>
type Font
/// <summary>A collection of font variations specified by the family name.</summary>
type FontFamily

/// <summary>Represents the size of a picture, defined by x1,y1,x2,y2 where x2>x1 and y2>y1.</summary>
type Rectangle = float*float*float*float

/// <summary>Represents the size of a picture, defined by its width and height.</summary>
type Size = float*float

/// <summary>Represents a coordinate in a picture as (x,y).</summary>
type Point = float*float

/// <summary>Represents one of the events: Key, DownArrow, UpArrow, LeftArrow, RightArow, Return, TimerTick, MouseButtonDown, MouseButtonUp, and MouseMotion.</summary>
type Event =
    | Key of char
    | DownArrow
    | UpArrow
    | LeftArrow
    | RightArrow
    | Return
    | MouseButtonDown of int * int // x,y
    | MouseButtonUp of int * int // x,y
    | MouseMotion of int * int * int * int // x,y, relx, rely
    | TimerTick


/// <summary>A tree of graphics primitives, define as a tree of graphics primitives.</summary>
type PrimitiveTree

/// <summary>A picture.</summary>
type Picture

type Position

/// <summary>An alignh position-value for aligning boxes along their top edge.</summary>
val Top: Position
/// <summary>An alignv position-value for aligning boxes along their left edge.</summary>
val Left: Position
/// <summary>An alignh and alignv position-value for aligning boxes along their center.</summary>
val Center: Position
/// <summary>An alignh position-value for aligning boxes along their bottom edge.</summary>
val Bottom: Position
/// <summary>An alignv position-value for aligning boxes along their right edge.</summary>
val Right: Position

/// <summary>The list of names of available system fonts.</summary>
val systemFontNames: string list

/// <summary>An empty graphics primitive tree.</summary>
val emptyTree: PrimitiveTree

/// <summary>
/// Retrieves a FontFamily of a given name.
/// </summary>
/// <param name="name">The name of the font family to retrieve.</param>
/// <returns>The FontFamily corresponding to the specified name.</returns>
val getFamily: name: string -> FontFamily

/// <summary>
/// Creates a Font object using the given family name and size.
/// </summary>
/// <param name="fam">The name of the font family to use.</param>
/// <param name="size">The size of the font.</param>
/// <returns>The newly created Font object.</returns>
val makeFont: fam: string -> size: float -> Font

/// <summary>
/// Measures the size of the given text when rendered with the specified font.
/// </summary>
/// <param name="f">The font used to render the text.</param>
/// <param name="txt">The text to measure.</param>
/// <returns>A tuple representing the width and height of the measured text.</returns>
val measureText: f: Font -> txt: string -> (float * float)

/// <summary>
/// Creates a PrimitiveTree representing the given text with specified properties.
/// </summary>
/// <param name="c">The color of the text.</param>
/// <param name="sw">The stroke width of the text.</param>
/// <param name="f">The font used to render the text.</param>
/// <param name="txt">The text to render.</param>
/// <returns>A PrimitiveTree object representing the rendered text.</returns>
/// <remarks>
/// This function is used to create a text primitive with specific styling. The stroke width parameter is accepted but currently not used within the function. For example:
/// <code>
///    let fontName = "Microsoft Sans Serif"
///    let font = makeFont fontName 24.0
///    let white = yellow
///    let tree = text white 1.0 font "Hello World"
/// </code>
/// creates a graphic primitive of the string "Hello World".
/// </remarks>
val text: c: color -> sw: float -> f: Font -> txt: string -> PrimitiveTree

/// <summary>Retrieves the bounding box of a given graphic primitive tree.</summary>
/// <param name="p">The graphic primitive tree for which to get the size.</param>
/// <returns>The bounding box of the tree as x1,y1,x2,y2 where x2>x1 and y2>y1.</returns>
val getRectangle : p: PrimitiveTree -> Rectangle

/// <summary>Converts a rectangle into a size.</summary>
/// <param name="rect">The bounding box as a rectangle x1,y1,x2,y2 where x2>x1 and y2>y1.</param>
/// <returns>The width and height of the bounding box.</returns>
val getSize : rect:Rectangle -> Size

/// <summary>Translates a given graphic primitive tree by the specified distances along the x and y axes.</summary>
/// <param name="dx">The distance to translate the graphic primitive tree along the x-axis.</param>
/// <param name="dy">The distance to translate the graphic primitive tree along the y-axis.</param>
/// <param name="p">The graphic primitive tree to be translated.</param>
/// <returns>A new graphic primitive tree object that is translated by the specified distances.</returns>
/// <remarks>
/// The translate function takes a PrimitiveTree and two floating-point values representing the distances to translate
/// the tree along the x and y axes, respectively. It constructs a new Translate tree that encapsulates the original tree with a translation primitive.
/// For example:
/// <code>
///    ellipse darkCyan 2.0 85.0 64.0 |> translate 128.0 128.0
/// </code>
/// translates the ellipse with radii 85 and 64 and center in 0,0 a such that the resulting ellipse has its center in 128,128.
/// The ellipse's bounding box is translated accordingly.
/// </remarks>
val translate : dx:float -> dy:float -> p: PrimitiveTree -> PrimitiveTree 

/// <summary>Scales a given graphic primitive tree by the specified factors along the x and y axes.</summary>
/// <param name="sx">The scaling factor along the x-axis.</param>
/// <param name="sy">The scaling factor along the y-axis.</param>
/// <param name="p">The graphic primitive tree to be scaled.</param>
/// <returns>A new graphic primitive tree object that is scaled by the specified factors.</returns>
/// <remarks>
/// The scale function takes a PrimitiveTree and two floating-point values representing the scaling factor along
/// the x and y axes, respectively. It constructs a new Scale tree that encapsulates the original tree.
/// For example:
/// <code>
///    ellipse darkCyan 2.0 85.0 64.0 |> scale 1.0 2.0
/// </code>
/// scales the ellipse with radii 85 and 64 with its center in 0,0 such that resulting ellipse has radii 85 and 128
/// and center in 0,0. The ellipse's bounding box is scaled accordingly.
/// </remarks>
val scale: sx:float -> sy:float -> p: PrimitiveTree -> PrimitiveTree 

/// <summary>Rotates a given graphic primitive tree by the specified angle in radians around a point.</summary>
/// <param name="x">The x-coordinate of the point around which to rotate.</param>
/// <param name="y">The y-coordinate of the point around which to rotate.</param>
/// <param name="rad">The angle in radians to rotate the graphic primitive tree.</param>
/// <param name="p">The graphic primitive tree to be rotated.</param>
/// <returns>A new graphic primitive tree object that is rotated by the specified angle.</returns>
/// <remarks>
/// The rotate function takes a PrimitiveTree and three floating-point values representing the x and y coordinates
/// of the center of rotation and the amount of rotation in radians. It constructs a new Rotate tree that
/// encapsulates the original tree.
/// For example:
/// <code>
///    ellipse darkCyan 2.0 85.0 64.0 |> rotate 0.0 0.0 (45.0*System.Math.PI/180.0)
/// </code>
/// rotates the ellipse 45 degrees around the point 0,0. The ellipse's bounding box is currently unchanged
/// which may cause undesired effects when used in together with alignH and alignV and this may change in
/// future releases.
/// </remarks>
val rotate: x:float -> y:float -> rad:float -> p: PrimitiveTree -> PrimitiveTree 

/// <summary>Creates a piecewise affine transformation on a graphic primitive tree.</summary>
/// <param name="c">The color to be used.</param>
/// <param name="sw">The stroke width.</param>
/// <param name="lst">The list of control points for the transformation.</param>
/// <returns>A new graphic primitive tree object representing the transformed graphic primitive tree.</returns>
/// <remarks>
/// The piecewiseAffine function takes a color, a stroke width, and list of pairs of x and y coordinates and 
/// makes a PrimitiveTree representing a piecewise affine curve also known as a piecewise linear curve. Note
/// any smooth curve can be approximately arbitrarily well as piecewise affine curve when the coordinate pairs
/// are sampled sufficiently closely along the smooth curve. Example of code generating a piecewise affine curve
/// tree is:
/// <code>
///    let col = darkOliveGreen
///    let strokeWidth = 2.0
///    piecewiseAffine col strokeWidth [(0.0,0.0);(10.0,80.0);(20.0,40.0);(0.0,0.0)]
/// </code>
/// which generates a PrimitiveTree representing a set of lines, colored filledPolygon and with a width of 2.0. In this case is also a closed polygon
/// and a triangle. The bounding box is the smallest axis aligned rectangle enclosing the shape which in this case is denoted by the 
/// corners (0.0,0.0) and (20.0,80.0). 
/// </remarks>
val piecewiseAffine: c:color -> sw: float -> lst:(float*float) list -> PrimitiveTree 

/// <summary>Creates a filled polygon with the specified vertices.</summary>
/// <param name="c">The color of the polygon.</param>
/// <param name="lst">The list of vertices of the polygon.</param>
/// <returns>A new graphic primitive tree object representing the filled polygon.</returns>
/// <remarks>
/// The filledPolygon function takes a color, and list of pairs of x and y coordinates and 
/// makes a PrimitiveTree representing a polygon. Example of code generating a filled polygon tree is:
/// <code>
///    let col = darkOliveGreen
///    filledPolygon col [(0.0,0.0);(10.0,80.0);(20.0,40.0)]
/// </code>
/// which generates a PrimitiveTree representing a closed polygon which is filled with the color dargOliveGreen.
/// Note that a filledPolygon with its outline marked in another color can be achieved by using this function
/// together with the onto and the piecewiseAffine functions. The bounding box is the smallest axis aligned
/// rectangle enclosing the shape which in this case is denoted by the corners (0.0,0.0) and (20.0,80.0).
/// </remarks>
val filledPolygon: c:color -> lst:(float*float) list -> PrimitiveTree 

/// <summary>Creates a rectangle with the specified width, height, and stroke width.</summary>
/// <param name="c">The color of the rectangle.</param>
/// <param name="sw">The stroke width of the rectangle.</param>
/// <param name="w">The width of the rectangle.</param>
/// <param name="h">The height of the rectangle.</param>
/// <returns>A new graphic primitive tree object representing the rectangle.</returns>
/// <remarks>
/// The rectangle function takes a color, a stroke width, a width, and a height and makes a PrimitiveTree
/// representing a rectangle, whose lower left corner is (0.0,0.0) and upper right corner is (w,h). Example 
/// of code generating a rectangle tree is:
/// <code>
///    let col = goldenrod
///    let strokeWidth = 1.0
///    rectangle col strokeWidth 20.0 80.0
/// </code>
/// which generates a PrimitiveTree representing a rectangle drawn with the color goldenrod and the line
/// width 1.0. The bounding box is the same as the rectangle which in this case is (0.0,0.0) and (20.0,80.0).
/// </remarks>
val rectangle: c:color -> sw: float -> w:float -> h:float -> PrimitiveTree 

/// <summary>Creates a filled rectangle with the specified width and height.</summary>
/// <param name="c">The color of the rectangle.</param>
/// <param name="w">The width of the rectangle.</param>
/// <param name="h">The height of the rectangle.</param>
/// <returns>A new graphic primitive tree object representing the filled rectangle.</returns>
/// <remarks>
/// The filledRectangle function takes a color, a width, and a height and makes a PrimitiveTree
/// representing a rectangle, whose lower left corner is (0.0,0.0) and upper right corner is (w,h). Example 
/// of code generating a rectangle tree is:
/// <code>
///    let col = goldenrod
///    filledRectangle col 20.0 80.0
/// </code>
/// which generates a PrimitiveTree representing a rectangle filled with the color goldenrod. The bounding
/// box is the same as the rectangle which in this case is (0.0,0.0) and (20.0,80.0). Note that a
/// filledRectangle with its outline marked in another color can be achieved by using this function
/// together with the onto and the rectangle functions.
/// </remarks>
val filledRectangle: c:color -> w:float -> h:float -> PrimitiveTree 

/// <summary>Draws an elliptical arc.</summary>
/// <param name="center">The center point of the ellipse.</param>
/// <param name="rx">The horizontal radius of the ellipse.</param>
/// <param name="ry">The vertical radius of the ellipse.</param>
/// <param name="start">The starting angle of the arc in degrees.</param>
/// <param name="sweep">The sweep angle of the arc in degrees.</param>
/// <param name="c">The color of the arc.</param>
/// <param name="sw">The stroke width of the arc.</param>
/// <returns>A primitive tree representing the arc.</returns>
/// <remarks>
/// The arc function represents part of an ellipse. It takes a color, a line width, a center (rx, ry), 
/// a start angle in radians, and the span of degrees in radians to draw from start.  The drawing direction 
/// takes it origin along the x-axis and increases in angle clockwise. Example of code generating an arc tree is:
/// <code>
///    let col = goldenrod
///    let strokeWidth = 3.0
///    arc (128.0,128.0) 64.0 32.0 (-45.0*System.Math.PI/180.0) System.Math.PI col strokeWidth
/// </code>
/// which generates a PrimitiveTree representing an arc curve from -45 degrees to 135 degrees with color goldenred
/// and width 3.0. The bounding box is the same as the full ellipse. This may change in the future.
/// </remarks>
val arc: center:Point -> rx:float -> ry:float -> start:float -> sweep:float -> c:color -> sw:float  -> PrimitiveTree //FIXME: reorder arguments to be similar to other functions. remove rx and ry to mimic ellipse

/// <summary>Draws a filled elliptical arc.</summary>
/// <param name="center">The center point of the ellipse.</param>
/// <param name="rx">The horizontal radius of the ellipse.</param>
/// <param name="ry">The vertical radius of the ellipse.</param>
/// <param name="start">The starting angle of the arc in degrees.</param>
/// <param name="sweep">The sweep angle of the arc in degrees.</param>
/// <param name="c">The fill color of the arc.</param>
/// <returns>A primitive tree representing the filled arc.</returns>
/// <remarks>
/// The arc function represents part of a filled ellipse. It takes a color, a center (rx, ry), a start angle in
/// radians, and the span of degrees in radians to draw from start. The drawing direction takes it origin along
/// the x-axis and increases in angle clockwise. Example of code generating an arc tree is:
/// <code>
///    let col = goldenrod
///    let strokeWidth = 3.0
///    filledArc (128.0,128.0) 64.0 32.0 (-45.0*System.Math.PI/180.0) System.Math.PI col
/// </code>
/// which generates a PrimitiveTree representing an filled arc from -45 degrees to 135 degrees. The bounding
/// box is the same as the full ellipse. This may change in the future.
/// </remarks>
val filledArc: center:Point -> rx:float -> ry:float -> start:float -> sweep:float -> c:color -> PrimitiveTree //FIXME: reorder arguments to be similar to other functions. remove rx and ry to mimic ellipse

/// <summary>Draws a cubic Bezier curve.</summary>
/// <param name="point1">The first control point of the curve.</param>
/// <param name="point2">The second control point of the curve.</param>
/// <param name="point3">The third control point of the curve.</param>
/// <param name="point4">The fourth control point of the curve.</param>
/// <param name="c">The color of the curve.</param>
/// <param name="sw">The stroke width of the curve.</param>
/// <returns>A primitive tree representing the cubic Bezier curve.</returns>
/// <remarks>
/// The cubic bezier function represents a cubic bezier curve which takes 4 points, a color, and stroke width.
/// The curve starts and ends in point index 0 and 3, and the line from index 0 to 1 gives the derivative at 0,
/// while the line from index 2 to 3 gives the derivative at 3. Example of code generating an arc tree is:
/// <code>
///    let col = ivory
///    let strokeWidth = 3.0
///    cubicBezier (10.0,10.0) (12.0,10.0) (56.0,60.0) (70.0,70.0) col strokeWidth
/// </code>
/// which generates a PrimitiveTree representing a bezier curve from (10.0,10.0) to (70.0, 70.0). The bounding
/// box is the rectangle spanning the minimum and maximum values of all the x- and y-coordinates in the 4 points.
/// </remarks>
val cubicBezier: point1:Point -> point2:Point -> point3:Point -> point4:Point -> c:color -> sw:float -> PrimitiveTree //FIXME: reorder arguments to be similar to other functions

/// <summary> Draws a filled cubic Bezier curve.</summary>
/// <param name="point1">The first control point of the curve.</param>
/// <param name="point2">The second control point of the curve.</param>
/// <param name="point3">The third control point of the curve.</param>
/// <param name="point4">The fourth control point of the curve.</param>
/// <param name="c">The fill color of the curve.</param>
/// <returns>A primitive tree representing the filled cubic Bezier curve.</returns>
/// <remarks>
/// The filled cubic bezier function represents a cubic bezier curve which takes 4 points, and a color.
/// The curve starts and ends in point index 0 and 3, and the line from index 0 to 1 gives the derivative at 0,
/// while the line from index 2 to 3 gives the derivative at 3. The fill area is between the curve and the
/// straight line between point 0 and 3. Example of code generating an arc tree is:
/// <code>
///    let col = ivory
///    filledCubicBezier (10.0,10.0) (12.0,10.0) (56.0,60.0) (70.0,70.0) col
/// </code>
/// which generates a PrimitiveTree representing a filled bezier curve from (10.0,10.0) to (70.0, 70.0). 
/// The bounding box is the rectangle spanning the minimum and maximum values of all the x- and
/// y-coordinates in the 4 points.
/// </remarks>
val filledCubicBezier: point1:Point -> point2:Point -> point3:Point -> point4:Point -> c:color -> PrimitiveTree //FIXME: reorder arguments to be similar to other functions

/// <summary>Creates an ellipse with the specified radii and stroke width.</summary>
/// <param name="c">The color of the ellipse.</param>
/// <param name="sw">The stroke width of the ellipse.</param>
/// <param name="rx">The horizontal radius of the ellipse.</param>
/// <param name="ry">The vertical radius of the ellipse.</param>
/// <returns>A new graphic primitive tree object representing the ellipse.</returns>
/// <remarks>
/// The ellipse function represents an ellipse with center in 0,0, radius rx and ry along the x- and y-axis,
/// and a color and strokeWidth. Example of code generating an ellipse tree is:
/// <code>
///    let col = ivory
///    let strokeWidth = 3.0
///    ellipse col strokeWidth 10.0 20.0
/// </code>
/// which generates a PrimitiveTree representing an ellipse of radii 10.0 and 20.0 with a line drawn in ivory
/// and which is 3 wide. The bounding box is the smallest rectangle enclosing the ellipse, which in this case
/// is (-10,-20) to (10.20).
/// </remarks>
val ellipse: c:color -> sw:float -> rx:float -> ry:float -> PrimitiveTree 

/// <summary>Creates a filled ellipse with the specified radii.</summary>
/// <param name="c">The color of the ellipse.</param>
/// <param name="rx">The horizontal radius of the ellipse.</param>
/// <param name="ry">The vertical radius of the ellipse.</param>
/// <returns>A new graphic primitive tree object representing the filled ellipse.</returns>
/// <remarks>
/// The filled ellipse function represents an ellipse with center in 0,0, radius rx and ry along the x- and
/// y-axis, and a color and strokeWidth. Example of code generating a filled ellipse tree is:
/// <code>
///    let col = ivory
///    filledEllipse ivory 10.0 20.0
/// </code>
/// which generates a PrimitiveTree representing an filled ellipse of radii 10.0 and 20.0 in ivory. The
/// bounding box is the smallest rectangle enclosing the ellipse, which in this case is (-10,-20) to (10.20).
/// </remarks>
val filledEllipse: c:color -> rx:float -> ry:float -> PrimitiveTree

/// <summary>Places one graphic primitive tree on top of another.</summary>
/// <param name="pic1">The first graphic primitive tree, which will be placed on top of the second.</param>
/// <param name="pic2">The second graphic primitive tree, over which the first graphic primitive tree will be placed.</param>
/// <returns>A new graphic primitive tree object representing the combined image with the first graphic primitive tree on top of the second.</returns>
/// <returns>A new graphic primitive tree object representing the filled ellipse.</returns>
/// <remarks>
/// The onto function joins two trees where pic1 will be drawn on top of pic2. The following code-example:
/// <code>
///    let boundary = ellipse red 3.0 10.0 20.0
///    let ell = filledEllipse blue 10.0 20.0
///    onto boundary ell
/// </code>
/// represents a tree of a filled ellipse in blue with its boundary on top in red and with a line thickness of
/// 3. The bounding box is the smallest rectangle enclosing the ellipse, which in this case is (-10,-20) to
/// (10.20).
/// </remarks>
val onto : pic1: PrimitiveTree -> pic2: PrimitiveTree -> PrimitiveTree 

/// <summary>Aligns two graphic primitive trees horizontally at a specific position.</summary>
/// <param name="pic1">The first graphic primitive tree to be aligned.</param>
/// <param name="pos">One of Top, Center, or Bottom, defining how pic1 and pic2 are to be aligned.</param>
/// <param name="pic2">The second graphic primitive tree to be aligned.</param>
/// <returns>A new graphic primitive tree object representing the two graphic primitive trees aligned horizontally at the specified position.</returns>
/// <remarks>
/// The alignH function joins two trees where pic2 is translated such that its boundary box's left edge is aligned
/// with pic1's boundary box's right edge. When pos=Bottom then the boxes are align along their edge with
/// lowest x-value, i.e., closest to the top edge of the image. When pos=Center then pic2's midpoint in the
/// y-direction is aligned with pic1's midpoint in the y-direction. When pos=Top then pic2's boundary box's
/// highest x-value is aligned with pic1's boundary box's highest x-value. The following code-example:
/// <code>
///    let box1 = rectangle goldenrod 1.0 20.0 80.0
///    let box2 = rectangle yellow 1.0 30.0 30.0
///    alignH box1 Top box2
/// </code>
/// represents a new tree of a box from (0,0) to (20,80) in goldenrod and another (20,50) to (50,80)in yellow.
/// The bounding box is the enclosing box in this case from (0,0) to (50,80).
/// </remarks>
val alignH : pic1: PrimitiveTree -> pos:Position -> pic2: PrimitiveTree -> PrimitiveTree

/// <summary>Aligns two graphic primitive trees vertically at a specific position.</summary>
/// <param name="pic1">The first graphic primitive tree to be aligned.</param>
/// <param name="pos">One of Left, Center, or Right, defining how pic1 and pic2 are to be aligned.</param>
/// <param name="pic2">The second graphic primitive tree to be aligned.</param>
/// <returns>A new graphic primitive tree object representing the two graphic primitive trees aligned vertically at the specified position.</returns>
/// <remarks>
/// The alignV function joins two trees where pic2 is translated such that its boundary box's bottom edge is
/// aligned with pic1's boundary box's top edge. When pos=Left then the boxes are align along their edge with
/// lowest y-value, i.e., closest to the left edge of the image. When pos=Center then pic2's midpoint in the
/// x-direction is aligned with pic1's midpoint in the x-direction. When pos=Right then pic2's boundary box's
/// highest y-value is aligned with pic1's boundary box's highest y-value. The following code-example:
/// <code>
///    let box1 = rectangle goldenrod 1.0 20.0 80.0
///    let box2 = rectangle yellow 1.0 30.0 30.0
///    alignV box1 Right box2
/// </code>
/// represents a new tree of a box from (0,0) to (20,80) in goldenrod and another (-10,80) to (20,100)in yellow.
/// The bounding box is the enclosing box in this case from (-10,0) to (20,100).
/// </remarks>
val alignV : pic1: PrimitiveTree -> pos:Position -> pic2: PrimitiveTree -> PrimitiveTree

/// <summary>Converts a graphic primitive tree into its string representation.</summary>
/// <param name="p">The graphic primitive tree to be converted into a string.</param>
/// <returns>A string representing the given graphic primitive tree.</returns>
/// <remarks>
/// The toString function returns a string representation of a primitive tree including all
/// graphics primitives, transformations, and combinations, one per line. Enclosed trees are shown below with
/// indentation according to their level of indentation. For example:
/// <code>
///    let box1 = rectangle goldenrod 1.0 20.0 80.0
///    let box2 = rectangle yellow 1.0 30.0 30.0
///    let tree = alignH (alignV box1 Right box2) Center box1  
///    printfn "%s" (toString tree)
/// </code>
/// results in
/// <code>
///    AlignH position=0.5
///    ∟>AlignV position=1
///      ∟>Rectangle (color,stroke)=(Color DAA520FF, 1.0) coordinates=(0.0, 0.0, 20.0, 80.0)
///      ∟>Rectangle (color,stroke)=(Color FFFF00FF, 1.0) coordinates=(0.0, 0.0, 30.0, 30.0)
///    ∟>Rectangle (color,stroke)=(Color DAA520FF, 1.0) coordinates=(0.0, 0.0, 20.0, 80.0)
/// </code>
/// which demonstrates that alignV encloses box1 and box2 including data about each element.
/// </remarks>
val toString : p:PrimitiveTree -> string

/// <summary>Generate a color.</summary>
/// <param name="r">The amount of red.</param>
/// <param name="g">The amount of green.</param>
/// <param name="b">The amount of blue.</param>
/// <param name="a">The degree to which the color is transparent.</param>
/// <returns>A color.</returns>
/// <remarks>
/// The fromRgba produces the color representation of the given red, green, blue, and alpha values. All values
/// are en the range 0 to 255 and in the example:
/// <code>
///    fromRgba 255 0 0 128
/// </code>
/// the resulting color is semi-transparent red.
/// </remarks>
val fromRgba : r:int -> g:int -> b:int -> a:int -> color

/// <summary>Generate a non-transparent color.</summary>
/// <param name="r">The amount of red.</param>
/// <param name="g">The amount of green.</param>
/// <param name="b">The amount of blue.</param>
/// <returns>A color.</returns>
/// <remarks>
/// The fromRgba produces the color representation of the given red, green, blue. All values
/// are en the range 0 to 255 and in the example:
/// <code>
///    fromRgba 255 0 0
/// </code>
/// the resulting color is (non-transparent) red.
/// </remarks>
val fromRgb : r:int -> g:int -> b:int -> color

/// <summary>Creates a Picture object from a given graphic primitive tree.</summary>
/// <param name="p">The graphic primitive tree object used to create the graphic primitive tree.</param>
/// <returns>A new Picture object representing the given graphic primitive tree.</returns>
/// <remarks>
/// The functions make and explain both converts a primitive tree to a picture, which can be rendered. For example,
/// <code>
///    let tree = ellipse darkCyan 2.0 85.0 64.0 |> translate 128.0 128.0
///    let picture = make tree
///    renderToFile 256 256 "sample.tif" picture
/// </code>
/// makes a primitive tree consisting of a single ellipse, converts the tree to Picture and renders it to the
/// file sample.tif as a tif-file.
/// </remarks>
val make : p:PrimitiveTree -> Picture

/// <summary>Provides a visual explanation of a graphic primitive tree object.</summary>
/// <param name="p">The graphic primitive tree object to be explained.</param>
/// <returns>A new Picture object representing a visual explanation of the given graphic primitive tree.</returns>
/// <remarks>
/// The functions make and explain both converts a primitive tree to a picture, which can be rendered. For example,
/// <code>
///    let tree = ellipse darkCyan 2.0 85.0 64.0 |> translate 128.0 128.0
///    let picture = explain tree
///    renderToFile 256 256 "sample.tif" picture
/// </code>
/// makes a primitive tree consisting of a single ellipse, converts the tree to Picture and renders it to the
/// file sample.tif as a tif-file. In contrast to make, explain adds bounding boxes of the graphics primitives
/// in random colors. This is useful for debugging composition expressions.
/// </remarks>
val explain : p:PrimitiveTree -> Picture

/// <summary>Draws a picture generated by make or explain to a file with the specified width and height.</summary>
/// <param name="width">The width of the output image in pixels.</param>
/// <param name="height">The height of the output image in pixels.</param>
/// <param name="filePath">The file path where the image will be saved.</param>
/// <param name="draw">The picture object to be drawn.</param>
/// <returns>A unit value indicating the completion of the operation.</returns>
/// <remarks>
/// The functions renders a Picture to a file. For example,
/// <code>
///    let tree = ellipse darkCyan 2.0 85.0 64.0 |> translate 128.0 128.0
///    let picture = make tree
///    renderToFile 256 256 "sample.tif" picture
/// </code>
/// makes a primitive tree consisting of a single ellipse, converts the tree to Picture and renders it to the
/// file sample.tif as a tif-file. The following formats are supported: Bmp, Gif, Jpeg, Pbm, Png, Tiff, Tga, WebP,
/// and the desired format is specified by the file name suffix. Typically the suffix is given as lower case, and
/// and Tiff images uses either of the tiff or tif suffixes.
/// </remarks>
val renderToFile : width:int -> height:int -> filePath:string -> draw:Picture -> unit

/// <summary>Draws a list of pictures generated by make or explain to an animated GIF file with the specified width, height, frame delay, and repeat count.</summary>
/// <param name="width">The width of each frame in pixels.</param>
/// <param name="heigth">The height of each frame in pixels. (Note: there's a typo in the parameter name; it should likely be "height").</param>
/// <param name="frameDelay">The delay between frames in the animation in milliseconds.</param>
/// <param name="repeatCount">The number of times the animation should repeat. Use 0 for infinite looping.</param>
/// <param name="filePath">The file path where the animated GIF will be saved.</param>
/// <param name="drawLst">The list of picture objects to be drawn as frames in the animation.</param>
/// <returns>A unit value indicating the completion of the operation.</returns>
/// <remarks>
/// The functions renders a list of Pictures to a file as an animated gif. For example,
/// <code>
///    let w,h = 256,256
///    let bck = filledRectangle black w h
///    let tree (i:float) : PrimitiveTree =
///       onto (ellipse darkCyan 2.0 85.0 64.0 |> translate i i) bck
///    let sequence = List.map (fun j -> make (tree j)) [0.0 .. 128.0]
///    let frameDelay = 10
///    let repeatCount = 5
///    animateToFile 256 256 frameDelay repeatCount "sample.gif" sequence
/// </code>
/// makes a list of pictures of a primitive tree translated for a number of steps and saves the sequence
/// as an animated gif. In the example, the onto command is used to ensure that each frame is shown on a
/// black background.
/// </remarks>
val animateToFile : width:int -> height:int -> frameDelay:int -> repeatCount:int -> filePath:string -> drawLst:Picture list -> unit


/// <summary>Runs an application, defining the drawing function.</summary>
/// <param name="t">The title of the application window.</param>
/// <param name="w">The width of the application window in pixels.</param>
/// <param name="h">The height of the application window in pixels.</param>
/// <param name="draw">A function that returns a Picture object representing the current visual state of the application.</param>
/// <returns>A unit value indicating the completion of the operation.</returns>
/// <remarks>
/// The function render shows a Picture in a window on the screen. For example,
/// <code>
///    let tree = ellipse darkCyan 2.0 85.0 64.0 |> translate 128.0 128.0
///    let picture () = make tree
///    render "Sample title" 256 256 pictureFct
/// </code>
/// creates a translated ellipse graphic primitive tree converts it to a Picture using make and shows on the
/// screen with render. Note that render takes a function as argument.
/// </remarks>
val render : t:string -> w:int -> h:int -> draw: (unit -> Picture) ->  unit

/// <summary>Runs an application with a timer, defining the drawing and reaction functions.</summary>
/// <param name="t">The title of the application window.</param>
/// <param name="w">The width of the application window in pixels.</param>
/// <param name="h">The height of the application window in pixels.</param>
/// <param name="interval">An optional interval for the timer in microseconds.</param>
/// <param name="draw">A function that takes the current state and returns a Picture object representing the current visual state of the application.</param>
/// <param name="react">A function that takes the current state and an Event object and returns an optional new state, allowing the application to react to events.</param>
/// <param name="s">The initial state of the application.</param>
/// <returns>A unit value indicating the completion of the operation.</returns>
/// <remarks>
/// The interact function can render still images in a window, show animations in a window, and
/// to interact with the user via the keyboard and the mouse. For example,
/// <code>
///    let tree = ellipse darkCyan 2.0 85.0 64.0 |> translate 128.0 128.0
///    let draw _ = make tree
///    let react _ _ = None
///    interact "Render an image" 256 256 None draw react 0
/// </code>
/// The main workhorses of <c>interact</c> are the draw and react functions, which communicate via a user-defined state
/// value. In the above example, the state value is implicitly defined as an integer, since the last argument
/// of interact is of integer type, but both draw and react ignore the state. To make an animation with
/// interact, the react function must react on TimerTicks. Consider the following example,
/// <code>
///    type state = float
///    let tree (i:state) : PrimitiveTree = 
///        ellipse darkCyan 2.0 85.0 64.0 |> translate i i
///    let draw (j: state) : Picture = make (tree j)
///    let react (j: state) (ev:Event) : state option = 
///        match ev with
///            | Event.TimerTick -> Some ((j+1.0)%128.0)
///            | _ -> None
///    let interval = Some 100
///    let initialState = 0.0
///    interact "Render an image" 256 256 interval draw react initialState
/// </code>
/// Here, we define a state type as float, which is the value controlling what to draw. The draw function
/// is then a function, which takes a state and produces a Picture in this case the make of an tree containing
/// an ellipse. The react function is set to listen for TimerTick events. When one such event occurs, it returns
/// the next value of the state wrapped in an option type (Some). Other events may happen, but they are all
/// ignored by returning None. Note that there is no mutable value, which contains the present value of the
/// state. Further, note that the draw function is called inside interact, whenever interact deems it necessary,
/// such as when react has been called to produce a new value of the state.
/// 
/// The function react is called when the following events occur:
///   Key of char - when the user presses a regular key
///   DownArrow - when the user presses the down arrow
///   UpArrow - when the user presses the up arrow
///   LeftArrow - when the user presses the left arrow
///   RightArrow - when the user presses the right arrow
///   Return - when the user presses the return key
///   MouseButtonDown(x,y)- when the user presses the left mouse button
///   MouseButtonUp(x,y) - when the user releases the left mouse button
///   MouseMotion(x,y,relx,rely) - when the user moves the mouse
///   TimerTick - when the requested time interval has passed
/// Note that there is no guarantee that the exact interval has occurred between each TimerTick event, and
/// depending on the computing system being used, there is a lower limit to how fast an event loop can
/// be served.
/// 
/// Finally, the state can be any value, and thus the system offers much flexibility in terms of the
/// communication between the draw and the react function. However, since the programmer (and the user) are only
/// indirectly in control of their communication, it may be useful to think of draw and react as isolated
/// functions. E.g., a call by <c>interact</c> to <c>draw j</c> should produce a Picture for state <c>j</c> regardless of the previous
/// picture or the possible next. Likewise, a call to <c>read j ev</c> should react to the situation specified
/// by <c>j</c> and <c>ev</c> only, and the programmer should concentrate only on what the next event should be
/// given said input.
/// </remarks>
val interact : t:string -> w:int -> h:int -> interval:int option -> draw: ('s -> Picture) -> react: ('s -> Event -> 's option) -> s:'s-> unit
