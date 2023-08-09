#r "nuget:SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.Fonts

let J = Image<Rgba32>(50,30,Color.LightGray)
J.Save("empty.jpg")

let imageInfo = Image.Identify("foo.jpg")
printfn "Width: %A" imageInfo.Width
printfn "Height: %A" imageInfo.Height

let im = Image.Load("foo.jpg")
printfn "foo.jpg: %A %A" im.Width im.Height
let imageMetaData = im.Metadata;
printfn "%A" imageMetaData

let jpegData = imageMetaData.GetJpegMetadata()
printfn "%A" jpegData
im.Save("bar.jpg")
im.Mutate(fun x -> x.Resize(im.Width / 2, im.Height / 2) |>ignore)
im.Save("small.jpg")

let image = Image<Rgba32>(512,512)
let poly = Star(PointF(100F, 100F), 5, 20F, 30F, 0F)
image.Mutate(fun x -> x.Fill(Color.Red, poly) |> ignore)
image.Save("star.jpg")

let brush = Brushes.Solid(Color.Red)
let pen = Pens.DashDot(Color.Green, 5f)
let yourPolygon = new Star(PointF(100f, 100f), 5, 20f, 30f, 0f)
image.Mutate(fun x -> x.Fill(brush, yourPolygon)|>ignore; x.Draw(pen, yourPolygon)|>ignore);
image.Save("star2.jpg")

let yourText = "this is some sample text";
let font = SystemFonts.CreateFont("Microsoft Sans Serif", 12f)
image.Mutate(fun x -> x.DrawText(yourText, font, Color.White, PointF(10f, 10f))|>ignore);
image.Save("text.jpg")

let A = Matrix3x2Extensions.CreateTranslation(PointF(30f,100f))
let atb = AffineTransformBuilder()
atb.AppendMatrix(A)
image.Mutate(fun x -> x.Transform(atb).Crop(512,512)|>ignore)
image.Save("textT.jpg")
