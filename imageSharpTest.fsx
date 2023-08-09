#r "nuget:SixLabors.ImageSharp"
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

let J = Image<Rgba32>(50,30)
J.Save("empty.jpg")

let imageInfo = Image.Identify("foo.jpg")
printfn "Width: %A" imageInfo.Width
printfn "Height: %A" imageInfo.Height

let image = Image.Load("foo.jpg")
printfn "foo.jpg: %A %A" image.Width image.Height
let imageMetaData = image.Metadata;
printfn "%A" imageMetaData

let jpegData = imageMetaData.GetJpegMetadata()
printfn "%A" jpegData
image.Save("bar.jpg")
image.Mutate(fun x -> x.Resize(image.Width / 2, image.Height / 2) |>ignore)
image.Save("small.jpg")
