#r "nuget:DIKU.Canvas"
open Canvas


let canvas = create 600 600

let center = (300, 300) 

for i, angle in [-2.0 * System.Math.PI .. 0.1 * System.Math.PI .. 4.0 * System.Math.PI] |> List.indexed do
    strokeArc canvas black center (5*(i+1)) (angle) (angle + 1.5 * System.Math.PI)


fillArc canvas black (300, 300) 150 0.0 (1.0 * System.Math.PI)

show canvas "Circle test"