#r "nuget:SixLabors.ImageSharp"
#r "nuget:SixLabors.ImageSharp.Drawing, 1.0.0"

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.Drawing


let makeImage width height =
    let image = new Image<Rgba32>(int width, int height)
    let c = Color.Blue
    let star = new Star(x = 100.0f, y = 100.0f, prongs = 5, innerRadii = 20.0f, outerRadii = 60.0f)
    let star2 = Star(x = 0.0f, y = 0.0f, prongs = 6, innerRadii = 20.0f, outerRadii = 60.0f)
    let star3 = Star(x = 50.0f, y = 0.0f, prongs = 6, innerRadii = 20.0f, outerRadii = 60.0f)
    printfn "%A" star.Bounds
    //let brush = ...
    image.Mutate(fun x -> x.BackgroundColor(c) |> ignore)
    image.Mutate(fun ctx -> ctx.Fill(Color.Yellow, star)
                               .Draw(Color.Black, 1.0f, star.Bounds)
                               .Fill(Color.Red, star2)
                               .Draw(Color.Black, 1.0f, star2.Bounds)
                               .Fill(Color.Green, star3)
                               .Draw(Color.Black, 1.0f, star3.Bounds)
                               .Fill(Color.Tan, EllipsePolygon(0.0f, 0.0f, 10.0f))
                               .Fill(Color.Tan, EllipsePolygon(100.0f, 100.0f, 10.0f))
                               .Fill(Color.Tan, EllipsePolygon(200.0f, 200.0f, 10.0f))
                            |> ignore)
    image.Mutate(fun ctx ->
                 ctx.Crop(min 100 width, min 100 height)
                    .Resize(ResizeOptions(//TargetRectangle = Rectangle(0, 0, 100, 100),
                                          Position = AnchorPositionMode.TopLeft,
                                          Size = Size(100, 100), Mode = ResizeMode.BoxPad)) |> ignore)
    image.Save("blue-imagesharp.png")
