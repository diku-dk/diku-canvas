#r "nuget:DIKU.Canvas, 2.0.0-beta1"
open Canvas
open Color

type state = int // index into a list

/// generate n linear list of samples from x0 to x1 both included
let linspace (x0:float) (x1:float) (n:int) : float list =
    let d = x1-x0
    [0..n] |> List.map (fun i ->x0+d*(float i)/(float n))

// The size of the canvas, the radius of pacman and the absolut degree-span of the mouth motion
let w,h,R,N = 512,512,100.0,20.0

// The degree-list from -N to N
let degLstOneWay = linspace 0.0 (N*System.Math.PI/180.0) (int N)
let degLst = degLstOneWay@List.rev degLstOneWay

/// Update state function
let next d = (d+1) % degLst.Length

/// Given an index into the degree list, generate a linear samples of angles of the head except the mouth
let theta i = 
    let pi2 = 2.0*System.Math.PI
    linspace degLst[i] (pi2-degLst[i]) (int (pi2*R)) 

/// Prepare a Picture by the present state whenever needed
let draw (i:state): Picture = 
    // coordinates of the head with mouth added
    let lst = (0.0,0.0)::(List.map (fun j -> (R*cos(j),R*sin(j))) (theta i))@[(0.0, 0.0)]
    // build tree
    let cake = filledPolygon yellow lst |> translate (float w/2.0) (float h/2.0)
    make cake

/// React to whenever an event happens
let react (s:state) (ev:Event) : state option =
    match ev with
        | Event.TimerTick ->
            s |> next |> Some
        | _ -> None

// Start interaction session
let initialState = 0 // First state drawn by draw
let delayTime = (Some 10) // microseconds (as an option type)
interact "Pacman" w h delayTime draw react initialState
