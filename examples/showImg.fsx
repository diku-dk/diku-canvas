#r "nuget:DIKU.Canvas, 1.0.1-alpha"

let main =
    if Array.length fsi.CommandLineArgs < 2 then
        printfn "Please provide a file name"
    else
        let file = fsi.CommandLineArgs[1]
        do printfn "Reading %A" file
        let canvas = Canvas.fromFile file
        do printfn "height = %A width = %A" (Canvas.height canvas) (Canvas.width canvas)
        Canvas.runApp file (Canvas.height canvas) (Canvas.width canvas) (fun _ _ _ -> canvas) (fun _ _ -> None) ()
