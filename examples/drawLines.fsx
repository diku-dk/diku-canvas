#r "nuget: SixLabors.ImageSharp"
#r "nuget: SixLabors.ImageSharp.Drawing, 1.0.0-beta15"

open System
open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.ImageSharp.Drawing.Processing

let w,h = 600,400
let rand = Random()
let image = new Image<Rgba32>(w, h)
image.Mutate(fun imageContext ->
    // draw background
    imageContext.BackgroundColor(Color.Black)|>ignore

    let lineCount = 1000
    for i in 0 .. (lineCount-1) do
        // create an array of two points to make the straight line
        let points = [|
            PointF((float32<|rand.NextDouble()) * float32 w, (float32<|rand.NextDouble()) * float32 h);
            PointF((float32<|rand.NextDouble()) * float32 w, (float32<|rand.NextDouble()) * float32 h)|]

        // create a pen unique to this line
        let lineColor = 
            Color.FromRgba(byte<|rand.Next(255),byte<|rand.Next(255),byte<|rand.Next(255),byte<|rand.Next(255))
        let lineWidth = rand.Next(1, 10)
        let linePen = Pen(lineColor, float32 lineWidth)

        // draw the line
        imageContext.DrawLines(linePen, points)|>ignore
    )
image.Save("drawLines.png")
