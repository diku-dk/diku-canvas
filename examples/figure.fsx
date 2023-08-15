#r "nuget: SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

open System
open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.Drawing.Processing

let img = new Image<Rgba32>(500, 500)
let pathBuilder = new PathBuilder()

for i in 0..35 do
    pathBuilder.StartFigure() |> ignore

    let M = Matrix3x2Extensions.CreateRotation((float32 i)*10f*3.1415f/180f,PointF(256f,256f))
    pathBuilder.SetTransform(M) |> ignore// Transform everything after
    pathBuilder.SetOrigin(PointF(256f, 256f)) |> ignore
    pathBuilder.AddLine(0f,0f,256f,0f) |> ignore
    pathBuilder.CloseFigure() |> ignore
let path = pathBuilder.Build()

img.Mutate(fun ctx -> ctx.Fill(Color.White).Draw(Color.Red, 3f, path)|>ignore)
img.Save("figure.png")
