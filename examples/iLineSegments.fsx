#r "nuget: SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

open System
open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.Drawing.Processing
open System.Numerics;

let img = new Image<Rgba32>(512, 512)
let points =[|PointF(0f,0f);PointF(256f,0f);PointF(0f,256f);PointF(0f,0f)|]
let path = Path(LinearLineSegment(points)).Rotate(float32 (10.0*System.Math.PI/180.0)).Translate(256f,256f)
img.Mutate(fun ctx -> ctx.Fill(Color.White).Draw(Color.Red, 3f, path)|>ignore)
img.Save("ils.png")
