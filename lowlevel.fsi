module Lowlevel

/// Represents a color with predefined static members corresponding to various colors.
type Color =
    struct
        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure with the specified red, green, blue, and alpha components.
        /// </summary>
        /// <param name="red">The red component of the color, ranging from 0 to 255.</param>
        /// <param name="green">The green component of the color, ranging from 0 to 255.</param>
        /// <param name="blue">The blue component of the color, ranging from 0 to 255.</param>
        /// <param name="a">The alpha (transparency) component of the color, ranging from 0 to 255.</param>
        new : red:int * green:int * blue:int * a:int -> Color

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> structure with the specified red, green, and blue components.
        /// </summary>
        /// <param name="red">The red component of the color, ranging from 0 to 255.</param>
        /// <param name="green">The green component of the color, ranging from 0 to 255.</param>
        /// <param name="blue">The blue component of the color, ranging from 0 to 255.</param>
        new : red:int * green:int * blue:int -> Color

        /// <summary>Gets the color AliceBlue.</summary>
        static member AliceBlue : Color

        /// <summary>Gets the color AntiqueWhite.</summary>
        static member AntiqueWhite : Color

        /// <summary>Gets the color Aqua.</summary>
        static member Aqua : Color

        /// <summary>Gets the color Aquamarine.</summary>
        static member Aquamarine : Color

        /// <summary>Gets the color Azure.</summary>
        static member Azure : Color

        /// <summary>Gets the color Beige.</summary>
        static member Beige : Color

        /// <summary>Gets the color Bisque.</summary>
        static member Bisque : Color

        /// <summary>Gets the color Black.</summary>
        static member Black : Color

        /// <summary>Gets the color BlanchedAlmond.</summary>
        static member BlanchedAlmond : Color

        /// <summary>Gets the color Blue.</summary>
        static member Blue : Color

        /// <summary>Gets the color BlueViolet.</summary>
        static member BlueViolet : Color

        /// <summary>Gets the color Brown.</summary>
        static member Brown : Color

        /// <summary>Gets the color BurlyWood.</summary>
        static member BurlyWood : Color

        /// <summary>Gets the color CadetBlue.</summary>
        static member CadetBlue : Color

        /// <summary>Gets the color Chartreuse.</summary>
        static member Chartreuse : Color

        /// <summary>Gets the color Chocolate.</summary>
        static member Chocolate : Color

        /// <summary>Gets the color Coral.</summary>
        static member Coral : Color

        /// <summary>Gets the color CornflowerBlue.</summary>
        static member CornflowerBlue : Color

        /// <summary>Gets the color Cornsilk.</summary>
        static member Cornsilk : Color

        /// <summary>Gets the color Crimson.</summary>
        static member Crimson : Color

        /// <summary>Gets the color Cyan.</summary>
        static member Cyan : Color

        /// <summary>Gets the color DarkBlue.</summary>
        static member DarkBlue : Color

        /// <summary>Gets the color DarkCyan.</summary>
        static member DarkCyan : Color

        /// <summary>Gets the color DarkGoldenrod.</summary>
        static member DarkGoldenrod : Color

        /// <summary>Gets the color DarkGray.</summary>
        static member DarkGray : Color

        /// <summary>Gets the color DarkGreen.</summary>
        static member DarkGreen : Color

        /// <summary>Gets the color DarkGrey.</summary>
        static member DarkGrey : Color

        /// <summary>Gets the color DarkKhaki.</summary>
        static member DarkKhaki : Color

        /// <summary>Gets the color DarkMagenta.</summary>
        static member DarkMagenta : Color

        /// <summary>Gets the color DarkOliveGreen.</summary>
        static member DarkOliveGreen : Color

        /// <summary>Gets the color DarkOrange.</summary>
        static member DarkOrange : Color

        /// <summary>Gets the color DarkOrchid.</summary>
        static member DarkOrchid : Color

        /// <summary>Gets the color DarkRed.</summary>
        static member DarkRed : Color

        /// <summary>Gets the color DarkSalmon.</summary>
        static member DarkSalmon : Color

        /// <summary>Gets the color DarkSeaGreen.</summary>
        static member DarkSeaGreen : Color

        /// <summary>Gets the color DarkSlateBlue.</summary>
        static member DarkSlateBlue : Color

        /// <summary>Gets the color DarkSlateGray.</summary>
        static member DarkSlateGray : Color

        /// <summary>Gets the color DarkSlateGrey.</summary>
        static member DarkSlateGrey : Color

        /// <summary>Gets the color DarkTurquoise.</summary>
        static member DarkTurquoise : Color

        /// <summary>Gets the color DarkViolet.</summary>
        static member DarkViolet : Color

        /// <summary>Gets the color DeepPink.</summary>
        static member DeepPink : Color

        /// <summary>Gets the color DeepSkyBlue.</summary>
        static member DeepSkyBlue : Color

        /// <summary>Gets the color DimGray.</summary>
        static member DimGray : Color

        /// <summary>Gets the color DimGrey.</summary>
        static member DimGrey : Color

        /// <summary>Gets the color DodgerBlue.</summary>
        static member DodgerBlue : Color

        /// <summary>Gets the color Firebrick.</summary>
        static member Firebrick : Color

        /// <summary>Gets the color FloralWhite.</summary>
        static member FloralWhite : Color

        /// <summary>Gets the color ForestGreen.</summary>
        static member ForestGreen : Color

        /// <summary>Gets the color Fuchsia.</summary>
        static member Fuchsia : Color

        /// <summary>Gets the color Gainsboro.</summary>
        static member Gainsboro : Color

        /// <summary>Gets the color GhostWhite.</summary>
        static member GhostWhite : Color

        /// <summary>Gets the color Gold.</summary>
        static member Gold : Color

        /// <summary>Gets the color Goldenrod.</summary>
        static member Goldenrod : Color

        /// <summary>Gets the color Gray.</summary>
        static member Gray : Color

        /// <summary>Gets the color Green.</summary>
        static member Green : Color

        /// <summary>Gets the color GreenYellow.</summary>
        static member GreenYellow : Color

        /// <summary>Gets the color Grey.</summary>
        static member Grey : Color

        /// <summary>Gets the color Honeydew.</summary>
        static member Honeydew : Color

        /// <summary>Gets the color HotPink.</summary>
        static member HotPink : Color

        /// <summary>Gets the color IndianRed.</summary>
        static member IndianRed : Color

        /// <summary>Gets the color Indigo.</summary>
        static member Indigo : Color

        /// <summary>Gets the color Ivory.</summary>
        static member Ivory : Color

        /// <summary>Gets the color Khaki.</summary>
        static member Khaki : Color

        /// <summary>Gets the color Lavender.</summary>
        static member Lavender : Color

        /// <summary>Gets the color LavenderBlush.</summary>
        static member LavenderBlush : Color

        /// <summary>Gets the color LawnGreen.</summary>
        static member LawnGreen : Color

        /// <summary>Gets the color LemonChiffon.</summary>
        static member LemonChiffon : Color

        /// <summary>Gets the color LightBlue.</summary>
        static member LightBlue : Color

        /// <summary>Gets the color LightCoral.</summary>
        static member LightCoral : Color

        /// <summary>Gets the color LightCyan.</summary>
        static member LightCyan : Color

        /// <summary>Gets the color LightGoldenrodYellow.</summary>
        static member LightGoldenrodYellow : Color

        /// <summary>Gets the color LightGray.</summary>
        static member LightGray : Color

        /// <summary>Gets the color LightGreen.</summary>
        static member LightGreen : Color

        /// <summary>Gets the color LightGrey.</summary>
        static member LightGrey : Color

        /// <summary>Gets the color LightPink.</summary>
        static member LightPink : Color

        /// <summary>Gets the color LightSalmon.</summary>
        static member LightSalmon : Color

        /// <summary>Gets the color LightSeaGreen.</summary>
        static member LightSeaGreen : Color

        /// <summary>Gets the color LightSkyBlue.</summary>
        static member LightSkyBlue : Color

        /// <summary>Gets the color LightSlateGray.</summary>
        static member LightSlateGray : Color

        /// <summary>Gets the color LightSlateGrey.</summary>
        static member LightSlateGrey : Color

        /// <summary>Gets the color LightSteelBlue.</summary>
        static member LightSteelBlue : Color

        /// <summary>Gets the color LightYellow.</summary>
        static member LightYellow : Color

        /// <summary>Gets the color Lime.</summary>
        static member Lime : Color

        /// <summary>Gets the color LimeGreen.</summary>
        static member LimeGreen : Color

        /// <summary>Gets the color Linen.</summary>
        static member Linen : Color

        /// <summary>Gets the color Magenta.</summary>
        static member Magenta : Color

        /// <summary>Gets the color Maroon.</summary>
        static member Maroon : Color

        /// <summary>Gets the color MediumAquamarine.</summary>
        static member MediumAquamarine : Color

        /// <summary>Gets the color MediumBlue.</summary>
        static member MediumBlue : Color

        /// <summary>Gets the color MediumOrchid.</summary>
        static member MediumOrchid : Color

        /// <summary>Gets the color MediumPurple.</summary>
        static member MediumPurple : Color

        /// <summary>Gets the color MediumSeaGreen.</summary>
        static member MediumSeaGreen : Color

        /// <summary>Gets the color MediumSlateBlue.</summary>
        static member MediumSlateBlue : Color

        /// <summary>Gets the color MediumSpringGreen.</summary>
        static member MediumSpringGreen : Color

        /// <summary>Gets the color MediumTurquoise.</summary>
        static member MediumTurquoise : Color

        /// <summary>Gets the color MediumVioletRed.</summary>
        static member MediumVioletRed : Color

        /// <summary>Gets the color MidnightBlue.</summary>
        static member MidnightBlue : Color

        /// <summary>Gets the color MintCream.</summary>
        static member MintCream : Color

        /// <summary>Gets the color MistyRose.</summary>
        static member MistyRose : Color

        /// <summary>Gets the color Moccasin.</summary>
        static member Moccasin : Color

        /// <summary>Gets the color NavajoWhite.</summary>
        static member NavajoWhite : Color

        /// <summary>Gets the color Navy.</summary>
        static member Navy : Color

        /// <summary>Gets the color OldLace.</summary>
        static member OldLace : Color

        /// <summary>Gets the color Olive.</summary>
        static member Olive : Color

        /// <summary>Gets the color OliveDrab.</summary>
        static member OliveDrab : Color

        /// <summary>Gets the color Orange.</summary>
        static member Orange : Color

        /// <summary>Gets the color OrangeRed.</summary>
        static member OrangeRed : Color

        /// <summary>Gets the color Orchid.</summary>
        static member Orchid : Color

        /// <summary>Gets the color PaleGoldenrod.</summary>
        static member PaleGoldenrod : Color

        /// <summary>Gets the color PaleGreen.</summary>
        static member PaleGreen : Color

        /// <summary>Gets the color PaleTurquoise.</summary>
        static member PaleTurquoise : Color

        /// <summary>Gets the color PaleVioletRed.</summary>
        static member PaleVioletRed : Color

        /// <summary>Gets the color PapayaWhip.</summary>
        static member PapayaWhip : Color

        /// <summary>Gets the color PeachPuff.</summary>
        static member PeachPuff : Color

        /// <summary>Gets the color Peru.</summary>
        static member Peru : Color

        /// <summary>Gets the color Pink.</summary>
        static member Pink : Color

        /// <summary>Gets the color Plum.</summary>
        static member Plum : Color

        /// <summary>Gets the color PowderBlue.</summary>
        static member PowderBlue : Color

        /// <summary>Gets the color Purple.</summary>
        static member Purple : Color

        /// <summary>Gets the color RebeccaPurple.</summary>
        static member RebeccaPurple : Color

        /// <summary>Gets the color Red.</summary>
        static member Red : Color

        /// <summary>Gets the color RosyBrown.</summary>
        static member RosyBrown : Color

        /// <summary>Gets the color RoyalBlue.</summary>
        static member RoyalBlue : Color

        /// <summary>Gets the color SaddleBrown.</summary>
        static member SaddleBrown : Color

        /// <summary>Gets the color Salmon.</summary>
        static member Salmon : Color

        /// <summary>Gets the color SandyBrown.</summary>
        static member SandyBrown : Color

        /// <summary>Gets the color SeaGreen.</summary>
        static member SeaGreen : Color

        /// <summary>Gets the color SeaShell.</summary>
        static member SeaShell : Color

        /// <summary>Gets the color Sienna.</summary>
        static member Sienna : Color

        /// <summary>Gets the color Silver.</summary>
        static member Silver : Color

        /// <summary>Gets the color SkyBlue.</summary>
        static member SkyBlue : Color

        /// <summary>Gets the color SlateBlue.</summary>
        static member SlateBlue : Color

        /// <summary>Gets the color SlateGray.</summary>
        static member SlateGray : Color

        /// <summary>Gets the color SlateGrey.</summary>
        static member SlateGrey : Color

        /// <summary>Gets the color Snow.</summary>
        static member Snow : Color

        /// <summary>Gets the color SpringGreen.</summary>
        static member SpringGreen : Color

        /// <summary>Gets the color SteelBlue.</summary>
        static member SteelBlue : Color

        /// <summary>Gets the color Tan.</summary>
        static member Tan : Color

        /// <summary>Gets the color Teal.</summary>
        static member Teal : Color

        /// <summary>Gets the color Thistle.</summary>
        static member Thistle : Color

        /// <summary>Gets the color Tomato.</summary>
        static member Tomato : Color

        /// <summary>Gets the color Transparent.</summary>
        static member Transparent : Color

        /// <summary>Gets the color Turquoise.</summary>
        static member Turquoise : Color

        /// <summary>Gets the color Violet.</summary>
        static member Violet : Color

        /// <summary>Gets the color Wheat.</summary>
        static member Wheat : Color

        /// <summary>Gets the color White.</summary>
        static member White : Color

        /// <summary>Gets the color WhiteSmoke.</summary>
        static member WhiteSmoke : Color

        /// <summary>Gets the color Yellow.</summary>
        static member Yellow : Color

        /// <summary>Gets the color YellowGreen.</summary>
        static member YellowGreen : Color
    end

/// <summary>
/// Represents an internal SDL event.
/// </summary>
type InternalEvent = internal InternalEvent of SDL.SDL_Event

/// <summary>
/// Represents a 2D point with integer coordinates.
/// </summary>
type point = int * int

/// <summary>
/// Represents a 2D point with floating-point coordinates.
/// </summary>
type pointF = float * float

/// <summary>
/// Represents a 2D vector.
/// </summary>
type Vector2 = System.Numerics.Vector2

/// <summary>
/// Represents a 3x2 matrix for 2D transformations.
/// </summary>
type Matrix3x2 = System.Numerics.Matrix3x2

/// <summary>
/// Represents a 4x4 matrix for 3D transformations.
/// </summary>
type Matrix4x4 = System.Numerics.Matrix4x4

/// <summary>
/// Represents text rendering options.
/// </summary>
type TextOptions = internal TextOptions of SixLabors.Fonts.TextOptions

/// <summary>
/// Creates a color from RGBA components.
/// </summary>
/// <param name="red">The red component (0-255).</param>
/// <param name="green">The green component (0-255).</param>
/// <param name="blue">The blue component (0-255).</param>
/// <param name="a">The alpha (opacity) component (0-255).</param>
/// <returns>A color instance with the specified RGBA values.</returns>
val fromRgba : red:int -> green:int -> blue:int -> a:int -> Color

/// <summary>
/// Creates a color from RGB components.
/// </summary>
/// <param name="red">The red component (0-255).</param>
/// <param name="green">The green component (0-255).</param>
/// <param name="blue">The blue component (0-255).</param>
/// <returns>A color instance with the specified RGB values.</returns>
val fromRgb : red:int -> green:int -> blue:int -> Color

/// <summary>
/// Represents a drawing context for image processing.
/// </summary>
type DrawingContext = internal DrawingContext of SixLabors.ImageSharp.Processing.IImageProcessingContext

/// <summary>
/// Represents a function that takes and returns a `DrawingContext`.
/// </summary>
type drawing_fun = DrawingContext -> DrawingContext

/// <summary>
/// Represents a font.
/// </summary>
type Font = internal Font of SixLabors.Fonts.Font

/// <summary>
/// Represents a font family.
/// </summary>
type FontFamily = internal FontFamily of SixLabors.Fonts.FontFamily

/// <summary>
/// Provides a list of system font names available.
/// </summary>
val systemFontNames : string list

/// <summary>
/// Retrieves a font family by name.
/// </summary>
/// <param name="name">The name of the font family.</param>
/// <returns>The font family corresponding to the given name.</returns>
val getFamily : string -> FontFamily

/// <summary>
/// Creates font families from a collection of streams containing font data.
/// </summary>
/// <param name="streams">A collection of font streams.</param>
/// <returns>A collection of `FontFamily` instances created from the streams.</returns>
val createFontFamilies : streams:System.Collections.Generic.IEnumerable<System.IO.Stream> -> System.Collections.Generic.IEnumerable<FontFamily>

/// <summary>
/// Creates a font from a font family and size.
/// </summary>
/// <param name="family">The font family.</param>
/// <param name="size">The font size.</param>
/// <returns>The created font.</returns>
val makeFont : FontFamily -> size:float -> Font

/// <summary>
/// Creates a font from a font family and size (C# compatible).
/// </summary>
/// <param name="family">The font family.</param>
/// <param name="size">The font size.</param>
/// <returns>The created font.</returns>
val makeFontCSharp : FontFamily * float -> Font

/// <summary>
/// Provides a read-only collection of font families available.
/// </summary>
val fontFamilies : System.Collections.ObjectModel.ReadOnlyCollection<FontFamily>

/// <summary>
/// Measures the dimensions of a text string when rendered with a specific font.
/// </summary>
/// <param name="font">The font to use for measurement.</param>
/// <param name="txt">The text string to measure.</param>
/// <returns>The width and height of the text.</returns>
val measureText : Font -> txt:string -> float * float

/// <summary>
/// Measures the dimensions of a text string when rendered with a specific font (C# compatible).
/// </summary>
/// <param name="font">The font to use for measurement.</param>
/// <param name="txt">The text string to measure.</param>
/// <returns>The dimensions of the text as a `Vector2`.</returns>
val measureTextCSharp : Font * string -> Vector2

/// <summary>
/// Represents a drawing tool, either a pen or a brush.
/// </summary>
type Tool = 
    | Pen of SixLabors.ImageSharp.Drawing.Processing.Pen
    | Brush of SixLabors.ImageSharp.Drawing.Processing.Brush

/// <summary>
/// Creates a solid brush with the specified color.
/// </summary>
/// <param name="color">The color of the brush.</param>
/// <returns>A brush tool with the specified color.</returns>
val solidBrush : color:Color -> Tool

/// <summary>
/// Creates a solid pen with the specified color and width.
/// </summary>
/// <param name="color">The color of the pen.</param>
/// <param name="width">The width of the pen.</param>
/// <returns>A pen tool with the specified color and width.</returns>
val solidPen : color:Color -> width:float -> Tool

/// <summary>
/// Retrieves text rendering options for a specific font.
/// </summary>
/// <param name="font">The font to use for text rendering.</param>
/// <returns>The text options for the specified font.</returns>
val textOptions : Font -> TextOptions

/// <summary>
/// Represents different types of path definitions for drawing.
/// </summary>
type PathDefinition =
    | EmptyDef
    | ArcTo of float * float * float * bool * bool * pointF
    | CubicBezierTo of pointF * pointF * pointF
    | LineTo of pointF
    | MoveTo of pointF
    | QuadraticBezierTo of pointF * pointF
    | SetTransform of Matrix3x2
    | LocalTransform of Matrix3x2 * PathDefinition
    | StartFigure
    | CloseFigure
    | CloseAllFigures
    | Combine of PathDefinition * PathDefinition

/// <summary>
/// Concatenates two `PathDefinition` values into one.
/// </summary>
/// <param name="a">The first path definition.</param>
/// <param name="b">The second path definition.</param>
/// <returns>A new `PathDefinition` that represents the combination of `a` and `b`.</returns>
val (<++>) : PathDefinition -> PathDefinition -> PathDefinition

/// <summary>
/// Represents different types of primitive paths for drawing.
/// </summary>
type PrimPath =
    | Arc of pointF * float * float * float * float * float
    | CubicBezier of pointF * pointF * pointF * pointF
    | Line of pointF * pointF
    | Lines of pointF list

/// <summary>
/// Represents a collection of paths and drawing instructions.
/// </summary>
type PathTree =
    | Empty
    | Prim of Tool * PrimPath
    | PathAdd of PathTree * PathTree
    | Transform of Matrix3x2 * PathTree
    | Text of Tool * string * TextOptions
    | TextAlong of Tool * string * TextOptions * PrimPath

/// <summary>
/// Concatenates two `PathTree` values into one.
/// </summary>
/// <param name="a">The first path tree.</param>
/// <param name="b">The second path tree.</param>
/// <returns>A new `PathTree` that represents the combination of `a` and `b`.</returns>
val (<+>) : PathTree -> PathTree -> PathTree

/// <summary>
/// Adds two path trees together.
/// </summary>
/// <param name="a">The first path tree.</param>
/// <param name="b">The second path tree.</param>
/// <returns>A new `PathTree` that represents the union of `a` and `b`.</returns>
val pathAdd : PathTree -> PathTree -> PathTree

/// <summary>
/// Represents an empty path tree.
/// </summary>
val emptyPath : PathTree

/// <summary>
/// Applies a transformation matrix to a path tree.
/// </summary>
/// <param name="matrix">The transformation matrix.</param>
/// <param name="pathTree">The path tree to transform.</param>
/// <returns>A new `PathTree` that represents the transformed path tree.</returns>
val transform : Matrix3x2 -> PathTree -> PathTree

/// <summary>
/// Represents a window in the SDL application with rendering and event handling capabilities.
/// </summary>
/// <param name="t">The title of the window.</param>
/// <param name="w">The width of the window.</param>
/// <param name="h">The height of the window.</param>
type Window(t: string, w: int, h: int) =

    /// <summary>
    /// Cleans up resources associated with the window.
    /// </summary>
    member this.Cleanup() : unit

    /// <summary>
    /// Disposes of the window by cleaning up resources.
    /// </summary>
    interface IDisposable with
        member this.Dispose() : unit

    /// <summary>
    /// Finalizer to ensure cleanup if Dispose wasn't called.
    /// </summary>
    override this.Finalize() : unit

    /// <summary>
    /// Clears the window by setting the background color.
    /// </summary>
    member this.Clear() : unit

    /// <summary>
    /// Renders the current image using the specified drawing context action.
    /// </summary>
    /// <param name="draw">An action that performs drawing operations on the provided drawing context.</param>
    member this.Render(draw: Action<DrawingContext>) : unit

    /// <summary>
    /// Waits for an event to arrive and processes it using the provided function.
    /// </summary>
    /// <param name="f">A function that processes the event.</param>
    /// <returns>The result of processing the event.</returns>
    member this.WaitEvent(f: Func<InternalEvent, 'a>) : 'a

    /// <summary>
    /// Polls for events and processes them using the provided action.
    /// </summary>
    /// <param name="f">An action that processes each event.</param>
    member this.PollEvents(f: Action<InternalEvent>) : unit

    /// <summary>
    /// Hides the window from view.
    /// </summary>
    member this.HideWindow() : unit

    /// <summary>
    /// Sets the clear color for the window.
    /// </summary>
    /// <param name="r">The red component of the color.</param>
    /// <param name="g">The green component of the color.</param>
    /// <param name="b">The blue component of the color.</param>
    member this.SetClearColor(r: int, g: int, b: int) : unit

    /// <summary>
    /// Sets the clear color for the window.
    /// </summary>
    /// <param name="color">The color.</param>
    member this.SetClearColor(color: Color) : unit

    /// <summary>
    /// Gets the title of the window.
    /// </summary>
    /// <value>The title of the window.</value>
    member this.Title : string

    /// <summary>
    /// Gets the width of the window.
    /// </summary>
    /// <value>The width of the window.</value>
    member this.Width : int

    /// <summary>
    /// Gets the height of the window.
    /// </summary>
    /// <value>The height of the window.</value>
    member this.Height : int

/// <summary>
/// Converts an internal event into a keyboard event, which can be a quit action, key press, key release, or ignore.
/// </summary>
/// <param name="event">The internal event to be converted.</param>
/// <returns>The corresponding keyboard event action.</returns>
val toKeyboardEvent: event: InternalEvent -> KeyboardEvent

/// <summary>
/// Represents the type of keyboard action (key press or key release).
/// </summary>
type KeyAction =
    /// <summary>
    /// Indicates that a key was pressed.
    /// </summary>
    | KeyPress = 0
    
    /// <summary>
    /// Indicates that a key was released.
    /// </summary>
    | KeyRelease = 1

/// <summary>
/// Represents various keyboard keys with their corresponding SDL key values.
/// </summary>
type KeyboardKey =
    /// <summary>
    /// Matches the case of an invalid, non-existent, or unsupported keyboard key.
    /// </summary>
    | Unknown = 0
    
    /// <summary>
    /// The spacebar key.
    /// </summary>
    | Space = 1
    
    /// <summary>
    /// The apostrophe key (').
    /// </summary>
    | Apostrophe = 2
    
    /// <summary>
    /// The comma key (,).
    /// </summary>
    | Comma = 3
    
    /// <summary>
    /// The minus key (-).
    /// </summary>
    | Minus = 4
    
    /// <summary>
    /// The plus key (+).
    /// </summary>
    | Plus = 5
    
    /// <summary>
    /// The period/dot key (.).
    /// </summary>
    | Period = 6
    
    /// <summary>
    /// The slash key (/).
    /// </summary>
    | Slash = 7
    
    /// <summary>
    /// The 0 key.
    /// </summary>
    | Num0 = 8
    
    /// <summary>
    /// The 1 key.
    /// </summary>
    | Num1 = 9
    
    /// <summary>
    /// The 2 key.
    /// </summary>
    | Num2 = 10
    
    /// <summary>
    /// The 3 key.
    /// </summary>
    | Num3 = 11
    
    /// <summary>
    /// The 4 key.
    /// </summary>
    | Num4 = 12
    
    /// <summary>
    /// The 5 key.
    /// </summary>
    | Num5 = 13
    
    /// <summary>
    /// The 6 key.
    /// </summary>
    | Num6 = 14
    
    /// <summary>
    /// The 7 key.
    /// </summary>
    | Num7 = 15
    
    /// <summary>
    /// The 8 key.
    /// </summary>
    | Num8 = 16
    
    /// <summary>
    /// The 9 key.
    /// </summary>
    | Num9 = 17
    
    /// <summary>
    /// The semicolon key (;).
    /// </summary>
    | Semicolon = 18
    
    /// <summary>
    /// The equal key (=).
    /// </summary>
    | Equal = 19
    
    /// <summary>
    /// The A key.
    /// </summary>
    | A = 20
    
    /// <summary>
    /// The B key.
    /// </summary>
    | B = 21
    
    /// <summary>
    /// The C key.
    /// </summary>
    | C = 22
    
    /// <summary>
    /// The D key.
    /// </summary>
    | D = 23
    
    /// <summary>
    /// The E key.
    /// </summary>
    | E = 24
    
    /// <summary>
    /// The F key.
    /// </summary>
    | F = 25
    
    /// <summary>
    /// The G key.
    /// </summary>
    | G = 26
    
    /// <summary>
    /// The H key.
    /// </summary>
    | H = 27
    
    /// <summary>
    /// The I key.
    /// </summary>
    | I = 28
    
    /// <summary>
    /// The J key.
    /// </summary>
    | J = 29
    
    /// <summary>
    /// The K key.
    /// </summary>
    | K = 30
    
    /// <summary>
    /// The L key.
    /// </summary>
    | L = 31
    
    /// <summary>
    /// The M key.
    /// </summary>
    | M = 32
    
    /// <summary>
    /// The N key.
    /// </summary>
    | N = 33
    
    /// <summary>
    /// The O key.
    /// </summary>
    | O = 34
    
    /// <summary>
    /// The P key.
    /// </summary>
    | P = 35
    
    /// <summary>
    /// The Q key.
    /// </summary>
    | Q = 36
    
    /// <summary>
    /// The R key.
    /// </summary>
    | R = 37
    
    /// <summary>
    /// The S key.
    /// </summary>
    | S = 38
    
    /// <summary>
    /// The T key.
    /// </summary>
    | T = 39
    
    /// <summary>
    /// The U key.
    /// </summary>
    | U = 40
    
    /// <summary>
    /// The V key.
    /// </summary>
    | V = 41
    
    /// <summary>
    /// The W key.
    /// </summary>
    | W = 42
    
    /// <summary>
    /// The X key.
    /// </summary>
    | X = 43
    
    /// <summary>
    /// The Y key.
    /// </summary>
    | Y = 44
    
    /// <summary>
    /// The Z key.
    /// </summary>
    | Z = 45
    
    /// <summary>
    /// The left bracket (opening bracket) key ([).
    /// </summary>
    | LeftBracket = 46
    
    /// <summary>
    /// The backslash key (\).
    /// </summary>
    | Backslash = 47
    
    /// <summary>
    /// The right bracket (closing bracket) key (]).
    /// </summary>
    | RightBracket = 48
    
    /// <summary>
    /// The grave accent key (`).
    /// </summary>
    | GraveAccent = 49
    
    /// <summary>
    /// The acute accent key (´).
    /// </summary>
    | AcuteAccent = 50
    
    /// <summary>
    /// The escape key.
    /// </summary>
    | Escape = 51
    
    /// <summary>
    /// The enter key.
    /// </summary>
    | Enter = 52
    
    /// <summary>
    /// The tab key.
    /// </summary>
    | Tab = 53
    
    /// <summary>
    /// The backspace key.
    /// </summary>
    | Backspace = 54
    
    /// <summary>
    /// The insert key.
    /// </summary>
    | Insert = 55
    
    /// <summary>
    /// The delete key.
    /// </summary>
    | Delete = 56
    
    /// <summary>
    /// The right arrow key.
    /// </summary>
    | Right = 57
    
    /// <summary>
    /// The left arrow key.
    /// </summary>
    | Left = 58
    
    /// <summary>
    /// The down arrow key.
    /// </summary>
    | Down = 59
    
    /// <summary>
    /// The up arrow key.
    /// </summary>
    | Up = 60
    
    /// <summary>
    /// The page up key.
    /// </summary>
    | PageUp = 61
    
    /// <summary>
    /// The page down key.
    /// </summary>
    | PageDown = 62
    
    /// <summary>
    /// The home key.
    /// </summary>
    | Home = 63
    
    /// <summary>
    /// The end key.
    /// </summary>
    | End = 64
    
    /// <summary>
    /// The caps lock key.
    /// </summary>
    | CapsLock = 65
    
    /// <summary>
    /// The scroll lock key.
    /// </summary>
    | ScrollLock = 66
    
    /// <summary>
    /// The num lock key.
    /// </summary>
    | NumLock = 67
    
    /// <summary>
    /// The print screen key.
    /// </summary>
    | PrintScreen = 68
    
    /// <summary>
    /// The pause key.
    /// </summary>
    | Pause = 69
    
    /// <summary>
    /// The F1 key.
    /// </summary>
    | F1 = 70
    
    /// <summary>
    /// The F2 key.
    /// </summary>
    | F2 = 71
    
    /// <summary>
    /// The F3 key.
    /// </summary>
    | F3 = 72
    
    /// <summary>
    /// The F4 key.
    /// </summary>
    | F4 = 73
    
    /// <summary>
    /// The F5 key.
    /// </summary>
    | F5 = 74
    
    /// <summary>
    /// The F6 key.
    /// </summary>
    | F6 = 75
    
    /// <summary>
    /// The F7 key.
    /// </summary>
    | F7 = 76
    
    /// <summary>
    /// The F8 key.
    /// </summary>
    | F8 = 77
    
    /// <summary>
    /// The F9 key.
    /// </summary>
    | F9 = 78
    
    /// <summary>
    /// The F10 key.
    /// </summary>
    | F10 = 79
    
    /// <summary>
    /// The F11 key.
    /// </summary>
    | F11 = 80
    
    /// <summary>
    /// The F12 key.
    /// </summary>
    | F12 = 81
    
    /// <summary>
    /// The 0 key on the key pad.
    /// </summary>
    | KeyPad0 = 82
    
    /// <summary>
    /// The 1 key on the key pad.
    /// </summary>
    | KeyPad1 = 83
    
    /// <summary>
    /// The 2 key on the key pad.
    /// </summary>
    | KeyPad2 = 84
    
    /// <summary>
    /// The 3 key on the key pad.
    /// </summary>
    | KeyPad3 = 85
    
    /// <summary>
    /// The 4 key on the key pad.
    /// </summary>
    | KeyPad4 = 86
    
    /// <summary>
    /// The 5 key on the key pad.
    /// </summary>
    | KeyPad5 = 87
    
    /// <summary>
    /// The 6 key on the key pad.
    /// </summary>
    | KeyPad6 = 88
    
    /// <summary>
    /// The 7 key on the key pad.
    /// </summary>
    | KeyPad7 = 89
    
    /// <summary>
    /// The 8 key on the key pad.
    /// </summary>
    | KeyPad8 = 90
    
    /// <summary>
    /// The 9 key on the key pad.
    /// </summary>
    | KeyPad9 = 91
    
    /// <summary>
    /// The decimal key on the key pad.
    /// </summary>
    | KeyPadDecimal = 92
    
    /// <summary>
    /// The divide key on the key pad.
    /// </summary>
    | KeyPadDivide = 93
    
    /// <summary>
    /// The multiply key on the key pad.
    /// </summary>
    | KeyPadMultiply = 94
    
    /// <summary>
    /// The subtract key on the key pad.
    /// </summary>
    | KeyPadSubtract = 95
    
    /// <summary>
    /// The add key on the key pad.
    /// </summary>
    | KeyPadAdd = 96
    
    /// <summary>
    /// The enter key on the key pad.
    /// </summary>
    | KeyPadEnter = 97
    
    /// <summary>
    /// The equal key on the key pad.
    /// </summary>
    | KeyPadEqual = 98
    
    /// <summary>
    /// The left shift key.
    /// </summary>
    | LeftShift = 99
    
    /// <summary>
    /// The left control key.
    /// </summary>
    | LeftControl = 100
    
    /// <summary>
    /// The left alt key.
    /// </summary>
    | LeftAlt = 101
    
    /// <summary>
    /// The left super key.
    /// </summary>
    | LeftSuper = 102
    
    /// <summary>
    /// The right shift key.
    /// </summary>
    | RightShift = 103
    
    /// <summary>
    /// The right control key.
    /// </summary>
    | RightControl = 104
    
    /// <summary>
    /// The right alt key.
    /// </summary>
    | RightAlt = 105
    
    /// <summary>
    /// The right super key.
    /// </summary>
    | RightSuper = 106
    
    /// <summary>
    /// The menu key.
    /// </summary>
    | Menu = 107
    
    /// <summary>
    /// The Diaresis key (¨).
    /// </summary>
    | Diaresis = 108
    
    /// <summary>
    /// The less than sign key (<).
    /// </summary>
    | LessThan = 109
    
    /// <summary>
    /// The greater than sign key (>).
    /// </summary>
    | GreaterThan = 110
    
    /// <summary>
    /// The vulgar fraction one-half key (½).
    /// </summary>
    | FractionOneHalf = 111
    
    /// <summary>
    /// The Danish AA key (Å).
    /// </summary>
    | DanishAA = 112
    
    /// <summary>
    /// The Danish AE key (Æ).
    /// </summary>
    | DanishAE = 113
    
    /// <summary>
    /// The Danish OE key (Ø).
    /// </summary>
    | DanishOE = 114

/// <summary>
/// Represents an image object for drawing.
/// </summary>
type Image = internal Image of SixLabors.ImageSharp.Image

/// <summary>
/// Represents a collection of paths for drawing.
/// </summary>
type PathCollection = internal PathCollection of IPathCollection

/// <summary>
/// Creates a path collection from a given text and font.
/// </summary>
/// <param name="text">The text to be converted into paths.</param>
/// <param name="font">The font to be used for the text.</param>
/// <returns>A <see cref="PathCollection"/> representing the glyphs of the text.</returns>
val createText: text: string * font: Font -> PathCollection

/// <summary>
/// Creates an image from a byte buffer.
/// </summary>
/// <param name="buffer">The byte buffer containing image data.</param>
/// <returns>An <see cref="Image"/> object created from the byte buffer.</returns>
val createImage: buffer: ReadOnlySpan<byte> -> Image

/// <summary>
/// Renders an image onto a drawing context at a specified position.
/// </summary>
/// <param name="x">The x-coordinate where the image should be drawn.</param>
/// <param name="y">The y-coordinate where the image should be drawn.</param>
/// <param name="image">The <see cref="Image"/> to be rendered.</param>
/// <param name="ctx">The <see cref="DrawingContext"/> where the image will be drawn.</param>
val renderImage: x: int * y: int * Image: Image * ctx: DrawingContext -> unit

/// <summary>
/// Renders a path collection with a solid brush color onto a drawing context.
/// </summary>
/// <param name="color">The color of the brush to be used for filling the path.</param>
/// <param name="path">The <see cref="PathCollection"/> to be filled.</param>
/// <param name="ctx">The <see cref="DrawingContext"/> where the path will be rendered.</param>
val renderBrushPath: color: Color * PathCollection: PathCollection * ctx: DrawingContext -> unit

/// <summary>
/// Renders a path collection with a solid pen color onto a drawing context.
/// </summary>
/// <param name="width">The width of the pen used for drawing the path.</param>
/// <param name="color">The color of the pen to be used for drawing the path.</param>
/// <param name="path">The <see cref="PathCollection"/> to be drawn.</param>
/// <param name="ctx">The <see cref="DrawingContext"/> where the path will be rendered.</param>
val renderPenPath: width: float32 * color: Color * PathCollection: PathCollection * ctx: DrawingContext -> unit

/// <summary>
/// Applies a transformation matrix to a path collection.
/// </summary>
/// <param name="path">The <see cref="PathCollection"/> to be transformed.</param>
/// <param name="matrix">The transformation matrix to be applied.</param>
/// <returns>A new <see cref="PathCollection"/> with the applied transformation.</returns>
val transformPath: PathCollection: PathCollection * matrix: Matrix3x2 -> PathCollection

/// <summary>
/// Resizes an image to the specified width and height.
/// </summary>
/// <param name="image">The <see cref="Image"/> to be resized.</param>
/// <param name="x">The new width of the image.</param>
/// <param name="y">The new height of the image.</param>
/// <returns>A new <see cref="Image"/> object with the specified size.</returns>
val setSizeImage: Image: Image * x: int * y: int -> Image

/// <summary>
/// Crops an image to the specified rectangle.
/// </summary>
/// <param name="image">The <see cref="Image"/> to be cropped.</param>
/// <param name="x">The x-coordinate of the upper-left corner of the crop rectangle.</param>
/// <param name="y">The y-coordinate of the upper-left corner of the crop rectangle.</param>
/// <param name="w">The width of the crop rectangle.</param>
/// <param name="h">The height of the crop rectangle.</param>
/// <returns>A new <see cref="Image"/> object cropped to the specified rectangle.</returns>
val cropImage: Image: Image * x: int * y: int * w: int * h: int -> Image

/// <summary>
/// Measures the dimensions of an image.
/// </summary>
/// <param name="image">The <see cref="Image"/> to be measured.</param>
/// <returns>A tuple containing the width and height of the image.</returns>
val measureImage: Image: Image -> int * int

/// <summary>
/// Draws a path tree onto a drawing context by flattening the tree and drawing each collection of paths.
/// </summary>
/// <param name="paths">The <see cref="PathTree"/> containing the paths to be drawn.</param>
/// <param name="ctx">The <see cref="DrawingContext"/> where the paths will be drawn.</param>
/// <returns>The updated <see cref="DrawingContext"/> after drawing the path tree.</returns>
val drawPathTree: paths: PathTree * ctx: DrawingContext -> DrawingContext

/// <summary>
/// Draws content to a file by creating an image and applying the provided drawing function to it.
/// </summary>
/// <param name="width">The width of the image to be created.</param>
/// <param name="height">The height of the image to be created.</param>
/// <param name="filePath">The path to the file where the image will be saved.</param>
/// <param name="draw">A function that takes a <see cref="DrawingContext"/> and applies drawing operations.</param>
val drawToFile: width: int * height: int * filePath: string * draw: drawing_fun -> unit

/// <summary>
/// Creates an animated GIF from a list of drawing functions and saves it to a file.
/// </summary>
/// <param name="width">The width of each frame in the animated GIF.</param>
/// <param name="height">The height of each frame in the animated GIF.</param>
/// <param name="frameDelay">The delay between frames in hundredths of a second.</param>
/// <param name="repeatCount">The number of times the animation should repeat. Use 0 for infinite repetition.</param>
/// <param name="filePath">The path to the file where the animated GIF will be saved.</param>
/// <param name="drawLst">A list of drawing functions, each representing a frame in the animated GIF.</param>
val drawToAnimatedGif: width: int * height: int * frameDelay: int * repeatCount: int * filePath: string * drawLst: drawing_fun list -> unit

/// <summary>
/// Runs an application window with a specified timer and event handling mechanism.
/// </summary>
/// <param name="t">The title of the window.</param>
/// <param name="w">The width of the window.</param>
/// <param name="h">The height of the window.</param>
/// <param name="interval">An optional interval (in milliseconds) for the timer. If <c>None</c>, no timer is used.</param>
/// <param name="draw">A function that generates a drawing function based on the current state of type <c>'s</c>.</param>
/// <param name="react">A function that reacts to events and returns an updated state or <c>None</c> if no update is needed.</param>
/// <param name="s">The initial state of type <c>'s</c>.</param>
/// <returns>A unit value, indicating that the function performs actions without returning a value.</returns>
val runAppWithTimer: t: string * w: int * h: int * interval: int option * draw: 's -> drawing_fun * react: 's -> Event -> 's option * s: 's -> unit

/// <summary>
/// Runs an application window without a timer, using a static drawing function and no event reactions.
/// </summary>
/// <param name="t">The title of the window.</param>
/// <param name="w">The width of the window.</param>
/// <param name="h">The height of the window.</param>
/// <param name="draw">A function that returns a drawing function to be used for rendering.</param>
/// <returns>A unit value, indicating that the function performs actions without returning a value.</returns>
val runApp: t: string * w: int * h: int * draw: unit -> drawing_fun -> unit
