//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Canvas
open System

type state = int
let N = 360
let next d = (d % N) + 1

let w,h = 600,400
let rand = Random()

let lines = [
    for i in 1..1000 do yield (
        fromRgba (rand.Next(255)) (rand.Next(255)) (rand.Next(255)) (rand.Next(255)),
        rand.NextDouble()*10.0,
        [(rand.NextDouble()*float w,rand.NextDouble()*float h);
        (rand.NextDouble()*float w,rand.NextDouble()*float h)])]

let fig = List.fold (fun acc (col,sw,lst) -> ontop (piecewiseaffine col sw lst) acc) emptyTree lines

let draw (): Picture = 
    make fig

runApp "Random lines" w h draw
