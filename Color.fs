/// Utility module for working with colors
module Color

/// Type representing colors
type color = internal Color of Lowlevel.Color

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
let fromRgba r g b a = Lowlevel.fromRgba r g b a |> Color

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
let fromRgb r g b = Lowlevel.fromRgb r g b |> Color


/// The color aliceBlue
let aliceBlue : color = Color Lowlevel.Color.AliceBlue
/// The color antiqueWhite
let antiqueWhite : color = Color Lowlevel.Color.AntiqueWhite
/// The color aqua
let aqua : color = Color Lowlevel.Color.Aqua
/// The color aquamarine
let aquamarine : color = Color Lowlevel.Color.Aquamarine
/// The color azure
let azure : color = Color Lowlevel.Color.Azure
/// The color beige
let beige : color = Color Lowlevel.Color.Beige
/// The color bisque
let bisque : color = Color Lowlevel.Color.Bisque
/// The color black
let black : color = Color Lowlevel.Color.Black
/// The color blanchedAlmond
let blanchedAlmond : color = Color Lowlevel.Color.BlanchedAlmond
/// The color blue
let blue : color = Color Lowlevel.Color.Blue
/// The color blueViolet
let blueViolet : color = Color Lowlevel.Color.BlueViolet
/// The color brown
let brown : color = Color Lowlevel.Color.Brown
/// The color burlyWood
let burlyWood : color = Color Lowlevel.Color.BurlyWood
/// The color cadetBlue
let cadetBlue : color = Color Lowlevel.Color.CadetBlue
/// The color chartreuse
let chartreuse : color = Color Lowlevel.Color.Chartreuse
/// The color chocolate
let chocolate : color = Color Lowlevel.Color.Chocolate
/// The color coral
let coral : color = Color Lowlevel.Color.Coral
/// The color cornflowerBlue
let cornflowerBlue : color = Color Lowlevel.Color.CornflowerBlue
/// The color cornsilk
let cornsilk : color = Color Lowlevel.Color.Cornsilk
/// The color crimson
let crimson : color = Color Lowlevel.Color.Crimson
/// The color cyan
let cyan : color = Color Lowlevel.Color.Cyan
/// The color darkBlue
let darkBlue : color = Color Lowlevel.Color.DarkBlue
/// The color darkCyan
let darkCyan : color = Color Lowlevel.Color.DarkCyan
/// The color darkGoldenrod
let darkGoldenrod : color = Color Lowlevel.Color.DarkGoldenrod
/// The color darkGray
let darkGray : color = Color Lowlevel.Color.DarkGray
/// The color darkGreen
let darkGreen : color = Color Lowlevel.Color.DarkGreen
/// The color darkGrey
let darkGrey : color = Color Lowlevel.Color.DarkGrey
/// The color darkKhaki
let darkKhaki : color = Color Lowlevel.Color.DarkKhaki
/// The color darkMagenta
let darkMagenta : color = Color Lowlevel.Color.DarkMagenta
/// The color darkOliveGreen
let darkOliveGreen : color = Color Lowlevel.Color.DarkOliveGreen
/// The color darkOrange
let darkOrange : color = Color Lowlevel.Color.DarkOrange
/// The color darkOrchid
let darkOrchid : color = Color Lowlevel.Color.DarkOrchid
/// The color darkRed
let darkRed : color = Color Lowlevel.Color.DarkRed
/// The color darkSalmon
let darkSalmon : color = Color Lowlevel.Color.DarkSalmon
/// The color darkSeaGreen
let darkSeaGreen : color = Color Lowlevel.Color.DarkSeaGreen
/// The color darkSlateBlue
let darkSlateBlue : color = Color Lowlevel.Color.DarkSlateBlue
/// The color darkSlateGray
let darkSlateGray : color = Color Lowlevel.Color.DarkSlateGray
/// The color darkSlateGrey
let darkSlateGrey : color = Color Lowlevel.Color.DarkSlateGrey
/// The color darkTurquoise
let darkTurquoise : color = Color Lowlevel.Color.DarkTurquoise
/// The color darkViolet
let darkViolet : color = Color Lowlevel.Color.DarkViolet
/// The color deepPink
let deepPink : color = Color Lowlevel.Color.DeepPink
/// The color deepSkyBlue
let deepSkyBlue : color = Color Lowlevel.Color.DeepSkyBlue
/// The color dimGray
let dimGray : color = Color Lowlevel.Color.DimGray
/// The color dimGrey
let dimGrey : color = Color Lowlevel.Color.DimGrey
/// The color dodgerBlue
let dodgerBlue : color = Color Lowlevel.Color.DodgerBlue
/// The color firebrick
let firebrick : color = Color Lowlevel.Color.Firebrick
/// The color floralWhite
let floralWhite : color = Color Lowlevel.Color.FloralWhite
/// The color forestGreen
let forestGreen : color = Color Lowlevel.Color.ForestGreen
/// The color fuchsia
let fuchsia : color = Color Lowlevel.Color.Fuchsia
/// The color gainsboro
let gainsboro : color = Color Lowlevel.Color.Gainsboro
/// The color ghostWhite
let ghostWhite : color = Color Lowlevel.Color.GhostWhite
/// The color gold
let gold : color = Color Lowlevel.Color.Gold
/// The color goldenrod
let goldenrod : color = Color Lowlevel.Color.Goldenrod
/// The color gray
let gray : color = Color Lowlevel.Color.Gray
/// The color green
let green : color = Color Lowlevel.Color.Green
/// The color greenYellow
let greenYellow : color = Color Lowlevel.Color.GreenYellow
/// The color grey
let grey : color = Color Lowlevel.Color.Grey
/// The color honeydew
let honeydew : color = Color Lowlevel.Color.Honeydew
/// The color hotPink
let hotPink : color = Color Lowlevel.Color.HotPink
/// The color indianRed
let indianRed : color = Color Lowlevel.Color.IndianRed
/// The color indigo
let indigo : color = Color Lowlevel.Color.Indigo
/// The color ivory
let ivory : color = Color Lowlevel.Color.Ivory
/// The color khaki
let khaki : color = Color Lowlevel.Color.Khaki
/// The color lavender
let lavender : color = Color Lowlevel.Color.Lavender
/// The color lavenderBlush
let lavenderBlush : color = Color Lowlevel.Color.LavenderBlush
/// The color lawnGreen
let lawnGreen : color = Color Lowlevel.Color.LawnGreen
/// The color lemonChiffon
let lemonChiffon : color = Color Lowlevel.Color.LemonChiffon
/// The color lightBlue
let lightBlue : color = Color Lowlevel.Color.LightBlue
/// The color lightCoral
let lightCoral : color = Color Lowlevel.Color.LightCoral
/// The color lightCyan
let lightCyan : color = Color Lowlevel.Color.LightCyan
/// The color lightGoldenrodYellow
let lightGoldenrodYellow : color = Color Lowlevel.Color.LightGoldenrodYellow
/// The color lightGray
let lightGray : color = Color Lowlevel.Color.LightGray
/// The color lightGreen
let lightGreen : color = Color Lowlevel.Color.LightGreen
/// The color lightGrey
let lightGrey : color = Color Lowlevel.Color.LightGrey
/// The color lightPink
let lightPink : color = Color Lowlevel.Color.LightPink
/// The color lightSalmon
let lightSalmon : color = Color Lowlevel.Color.LightSalmon
/// The color lightSeaGreen
let lightSeaGreen : color = Color Lowlevel.Color.LightSeaGreen
/// The color lightSkyBlue
let lightSkyBlue : color = Color Lowlevel.Color.LightSkyBlue
/// The color lightSlateGray
let lightSlateGray : color = Color Lowlevel.Color.LightSlateGray
/// The color lightSlateGrey
let lightSlateGrey : color = Color Lowlevel.Color.LightSlateGrey
/// The color lightSteelBlue
let lightSteelBlue : color = Color Lowlevel.Color.LightSteelBlue
/// The color lightYellow
let lightYellow : color = Color Lowlevel.Color.LightYellow
/// The color lime
let lime : color = Color Lowlevel.Color.Lime
/// The color limeGreen
let limeGreen : color = Color Lowlevel.Color.LimeGreen
/// The color linen
let linen : color = Color Lowlevel.Color.Linen
/// The color magenta
let magenta : color = Color Lowlevel.Color.Magenta
/// The color maroon
let maroon : color = Color Lowlevel.Color.Maroon
/// The color mediumAquamarine
let mediumAquamarine : color = Color Lowlevel.Color.MediumAquamarine
/// The color mediumBlue
let mediumBlue : color = Color Lowlevel.Color.MediumBlue
/// The color mediumOrchid
let mediumOrchid : color = Color Lowlevel.Color.MediumOrchid
/// The color mediumPurple
let mediumPurple : color = Color Lowlevel.Color.MediumPurple
/// The color mediumSeaGreen
let mediumSeaGreen : color = Color Lowlevel.Color.MediumSeaGreen
/// The color mediumSlateBlue
let mediumSlateBlue : color = Color Lowlevel.Color.MediumSlateBlue
/// The color mediumSpringGreen
let mediumSpringGreen : color = Color Lowlevel.Color.MediumSpringGreen
/// The color mediumTurquoise
let mediumTurquoise : color = Color Lowlevel.Color.MediumTurquoise
/// The color mediumVioletRed
let mediumVioletRed : color = Color Lowlevel.Color.MediumVioletRed
/// The color midnightBlue
let midnightBlue : color = Color Lowlevel.Color.MidnightBlue
/// The color mintCream
let mintCream : color = Color Lowlevel.Color.MintCream
/// The color mistyRose
let mistyRose : color = Color Lowlevel.Color.MistyRose
/// The color moccasin
let moccasin : color = Color Lowlevel.Color.Moccasin
/// The color navajoWhite
let navajoWhite : color = Color Lowlevel.Color.NavajoWhite
/// The color navy
let navy : color = Color Lowlevel.Color.Navy
/// The color oldLace
let oldLace : color = Color Lowlevel.Color.OldLace
/// The color olive
let olive : color = Color Lowlevel.Color.Olive
/// The color oliveDrab
let oliveDrab : color = Color Lowlevel.Color.OliveDrab
/// The color orange
let orange : color = Color Lowlevel.Color.Orange
/// The color orangeRed
let orangeRed : color = Color Lowlevel.Color.OrangeRed
/// The color orchid
let orchid : color = Color Lowlevel.Color.Orchid
/// The color paleGoldenrod
let paleGoldenrod : color = Color Lowlevel.Color.PaleGoldenrod
/// The color paleGreen
let paleGreen : color = Color Lowlevel.Color.PaleGreen
/// The color paleTurquoise
let paleTurquoise : color = Color Lowlevel.Color.PaleTurquoise
/// The color paleVioletRed
let paleVioletRed : color = Color Lowlevel.Color.PaleVioletRed
/// The color papayaWhip
let papayaWhip : color = Color Lowlevel.Color.PapayaWhip
/// The color peachPuff
let peachPuff : color = Color Lowlevel.Color.PeachPuff
/// The color peru
let peru : color = Color Lowlevel.Color.Peru
/// The color pink
let pink : color = Color Lowlevel.Color.Pink
/// The color plum
let plum : color = Color Lowlevel.Color.Plum
/// The color powderBlue
let powderBlue : color = Color Lowlevel.Color.PowderBlue
/// The color purple
let purple : color = Color Lowlevel.Color.Purple
/// The color rebeccaPurple
let rebeccaPurple : color = Color Lowlevel.Color.RebeccaPurple
/// The color red
let red : color = Color Lowlevel.Color.Red
/// The color rosyBrown
let rosyBrown : color = Color Lowlevel.Color.RosyBrown
/// The color royalBlue
let royalBlue : color = Color Lowlevel.Color.RoyalBlue
/// The color saddleBrown
let saddleBrown : color = Color Lowlevel.Color.SaddleBrown
/// The color salmon
let salmon : color = Color Lowlevel.Color.Salmon
/// The color sandyBrown
let sandyBrown : color = Color Lowlevel.Color.SandyBrown
/// The color seaGreen
let seaGreen : color = Color Lowlevel.Color.SeaGreen
/// The color seaShell
let seaShell : color = Color Lowlevel.Color.SeaShell
/// The color sienna
let sienna : color = Color Lowlevel.Color.Sienna
/// The color silver
let silver : color = Color Lowlevel.Color.Silver
/// The color skyBlue
let skyBlue : color = Color Lowlevel.Color.SkyBlue
/// The color slateBlue
let slateBlue : color = Color Lowlevel.Color.SlateBlue
/// The color slateGray
let slateGray : color = Color Lowlevel.Color.SlateGray
/// The color slateGrey
let slateGrey : color = Color Lowlevel.Color.SlateGrey
/// The color snow
let snow : color = Color Lowlevel.Color.Snow
/// The color springGreen
let springGreen : color = Color Lowlevel.Color.SpringGreen
/// The color steelBlue
let steelBlue : color = Color Lowlevel.Color.SteelBlue
/// The color tan
let tan : color = Color Lowlevel.Color.Tan
/// The color teal
let teal : color = Color Lowlevel.Color.Teal
/// The color thistle
let thistle : color = Color Lowlevel.Color.Thistle
/// The color tomato
let tomato : color = Color Lowlevel.Color.Tomato
/// The color transparent
let transparent : color = Color Lowlevel.Color.Transparent
/// The color turquoise
let turquoise : color = Color Lowlevel.Color.Turquoise
/// The color violet
let violet : color = Color Lowlevel.Color.Violet
/// The color wheat
let wheat : color = Color Lowlevel.Color.Wheat
/// The color white
let white : color = Color Lowlevel.Color.White
/// The color whiteSmoke
let whiteSmoke : color = Color Lowlevel.Color.WhiteSmoke
/// The color yellow
let yellow : color = Color Lowlevel.Color.Yellow
/// The color yellowGreen
let yellowGreen : color = Color Lowlevel.Color.YellowGreen
