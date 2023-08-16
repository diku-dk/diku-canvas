#r "nuget: SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

open System
open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.Drawing.Processing

let img = new Image<Rgba32>(512, 512)
let pathBuilder = new PathBuilder()
let points =[|PointF(0f,0f);PointF(256f,0f)|]

// Seems that only the last added line or array of lines is drawn for each figure. New figures can be made by either starting a figure or closing the previous.
for i in 0..35 do
    let M = Matrix3x2Extensions.CreateRotation((float32 i)*10f*3.1415f/180f,PointF(256f,256f))
    pathBuilder.StartFigure() |> ignore
    pathBuilder.SetTransform(M) |> ignore
    pathBuilder.SetOrigin(PointF(256f, 256f)) |> ignore
    pathBuilder.AddLines(points) |> ignore
//    pathBuilder.AddLine(points[0],points[1]) |> ignore
//    pathBuilder.CloseFigure() |> ignore

let path = pathBuilder.Build()
img.Mutate(fun ctx -> ctx.Fill(Color.White).Draw(Color.Red, 3f, path)|>ignore)
img.Save("linesFromPath.png")
