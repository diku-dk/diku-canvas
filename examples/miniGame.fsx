#r "nuget:DIKU.Canvas, 2.0"
open Canvas
open Color

type state = {monster : int * int;  // x-pos and direction
              player : int * int }  // x-pos and y-pos

// The size of the window
let w, h = 1024, 800

// The size of the monster
let monsterSz = 200

// The size of the player
let playerSz = 50
let playerSpeed = 20

/// Update monster part of state function
let moveMonster (s:state) =
    let x, dir = s.monster
    let dir = 
        if x + dir < 0 || x + dir > w - monsterSz then
            -dir
        else
            dir
    { s with monster = x + dir, dir }

let clamp maximum value = max 0 (min value maximum)

let movePlayer (dx, dy) s =
    let x, y = s.player
    { s with player = clamp (w-playerSz) (x+dx), clamp (h-playerSz) (y+dy) }

let collision s =
    let px, py = s.player
    let mx, _ = s.monster
    let my = (h-monsterSz)/2
    px < mx + monsterSz &&
    px + playerSz > mx  &&
    py < my + monsterSz &&
    py + playerSz > my

let boxAt color w h x y =
    (filledRectangle color (float w) (float h))
    |> translate (float x) (float y)

let drawMonster (s:state) =
    let x, _ = s.monster
    boxAt Color.green monsterSz monsterSz x ((h-monsterSz)/2)

let drawPlayer (s:state) =
    let x, y = s.player
    boxAt Color.skyBlue 50 50 x y

let drawEndScreen s =
    if collision s then boxAt Color.red w h 0 0
    else emptyTree

let (++) p1 p2 = onto p1 p2

let draw (s:state) =
    drawEndScreen s ++
    drawPlayer s ++
    drawMonster s
    |> make

/// React to whenever an event happens
let react (s:state) (ev:Event) : state option =
    match ev with
        | _ when collision s -> None
        | Event.TimerTick ->
            s |> moveMonster |> Some
        | LeftArrow ->
            s |> movePlayer (- playerSpeed, 0) |> Some
        | RightArrow ->
            s |> movePlayer (playerSpeed, 0) |> Some
        | DownArrow ->
            s |> movePlayer (0, playerSpeed) |> Some
        | UpArrow ->
            s |> movePlayer (0, - playerSpeed) |> Some
        | _ -> None

// Start interaction session
let initialState = {player = 0, 0; monster = 0, 5}// First state drawn by draw
let delayTime = Some 20 // microseconds (as an option type)
interact "Minigame" w h delayTime draw react initialState
