#r "nuget:DIKU.Canvas, 1.0.1"

let main =
    if Array.length fsi.CommandLineArgs < 2 then
        printfn "Please provide a file name"
    else
        let file = fsi.CommandLineArgs[1]
        let canvas = Canvas.fromFile file
        let width = Canvas.width canvas
        let height = Canvas.height canvas
        Canvas.show canvas file
