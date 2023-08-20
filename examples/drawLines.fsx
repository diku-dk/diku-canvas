#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
//#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha7"
open Canvas
open System

let w,h = 600,400 // The size of the canvas
let rand = Random() // Initialize a random number generator

// Produce a list of random (color, stroke-width, list of a start and an end coordinate pair)
let lines = [
    for i in 1..1000 do yield (
        fromRgba (rand.Next(255)) (rand.Next(255)) (rand.Next(255)) (rand.Next(255)),
        rand.NextDouble()*10.0,
        [(rand.NextDouble()*float w,rand.NextDouble()*float h);
        (rand.NextDouble()*float w,rand.NextDouble()*float h)])]
// Stack all lines onto each other
let fig = List.fold (fun acc (col,sw,lst) -> onto (piecewiseAffine col sw lst) acc) emptyTree lines

/// Prepare a Picture by the present state whenever needed
let draw (): Picture = 
    make fig

// Render the picture to the screen
render "Random lines" w h draw
