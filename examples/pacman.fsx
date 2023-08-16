//#i "nuget:/Users/kfl/projects/fsharp-experiments/diku-canvas/bin/Release"
#i "nuget:/Users/jrh630/repositories/diku-canvas/bin/Release/"
#r "nuget:DIKU.Canvas, 2.0.0-alpha4"
open Canvas

type state = int
let linspace (x0:float) (x1:float) (n:int) : float list =
    let d = x1-x0
    [0..n] |> List.map (fun i ->x0+d*(float i)/(float n))

let w,h,R,N = 512,512,100.0,20.0
let degLstOneWay = linspace 0.0 (N*System.Math.PI/180.0) (int N)
let degLst = degLstOneWay@List.rev degLstOneWay
let next d = (d+1) % degLst.Length

let pi2 = 2.0*System.Math.PI
let theta i = linspace degLst[i] (pi2-degLst[i]) (int (pi2*R)) 
let bck = filledrectangle color.Black w h

let draw (i:state): Picture = 
    let lst = (0.0,0.0)::(List.map (fun j -> (R*cos(j),R*sin(j))) (theta i))@[(0.0, 0.0)]
    let cake = filledpolygon color.Yellow lst |> translate (float w/2.0) (float h/2.0)
    let d = ontop bck cake 
    make d

let react (s:state) (ev:Lowlevel.Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

runAppWithTimer "Pacman" w h (Some 10) draw react 0
