#r "nuget:DIKU.Canvas, 2.0.2-alpha1"
open Canvas
open Color
open System

let w,h = 600,400 // The size of the canvas
let rand = Random() // Initialize a random number generator
let num_lines = 100_000

// Produce a list of random (color, stroke-width, list of a start and an end coordinate pair)
let lines = [
    for i in 1..num_lines do yield (
        fromRgba (rand.Next(255)) (rand.Next(255)) (rand.Next(255)) (rand.Next(255)),
        (rand.NextDouble()+0.1)*10.0,
        let p1 = rand.NextDouble()*float w, rand.NextDouble()*float h
        let p2 = rand.NextDouble()*float w, rand.NextDouble()*float h
        [p1; p2])]

// Stack all lines onto each other
let fig = List.fold (fun acc (col,sw,lst) -> onto (piecewiseAffine col sw lst) acc) emptyTree lines

/// Prepare a Picture
let draw = make fig

// Render the picture to the screen
render "Random lines" w h draw
