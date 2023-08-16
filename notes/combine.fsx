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
let points =[|PointF(0f,0f);PointF(50f,0f);PointF(0f,50f);PointF(0f,0f)|]
let N = 90
let pc = Array.map (fun i ->
    let R = Matrix3x2.CreateRotation(float32 ((float i)*(360.0/float N)*System.Math.PI/180.0))
    let T1 = Matrix3x2.CreateTranslation(PointF(56f,56f))
    let T2 = Matrix3x2.CreateTranslation(PointF(256f,256f))
    let M = T1*R*T2
    PathBuilder(M).AddLines(points).Build()) [|0..(N-1)|]
let path = PathCollection(pc).Translate(PointF(-56f,-56f))
img.Mutate(fun ctx -> ctx.Fill(Color.White).Draw(Color.Red, 3f, path)|>ignore)
img.Save("combine.png")
