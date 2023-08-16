#load "../canvasNT.fsx"
open CanvasNT
/// Tests
/// Testing creation of simple Picture
let p = box Color.LightGray 30 50
printfn "\nAn empty box:\n %A" p
save (sharpDraw p) "p.jpg"
let q = box Color.Red 50 30
printfn "\nA full box: %A" q
save (sharpDraw q) "q.jpg"

/// Testing Crop
let w,h = getSize p in
  let u = Crop(p, w-10, h-5) in 
    printfn "\nCrop p smaller: %A" u;
    save (sharpDraw u) "cropSmall.jpg"
let w,h = getSize p in
  let u = Crop(p, w+10, h+5) in 
    printfn "\nCrop p larger: %A" u;
    save (sharpDraw u) "cropLarge.jpg"

/// Testing Translate
let w,h = getSize p in
  let u = Translate(p, -10, -5, w,h) in 
    printfn "\nTranslate left-up keep size: %A" u;
    save (sharpDraw u) "translatelus.jpg"
let w,h = getSize p in
  let u = Translate(p, 10, 5, w,h) in 
    printfn "\nTranslate right-down keep size: %A" u;
    save (sharpDraw u) "translaterds.jpg"
let w,h = getSize p in
  let u = Translate(p, -10, -5, w+10,h+5) in 
    printfn "\nTranslate left-up bigger size: %A" u;
    save (sharpDraw u) "translatelub.jpg"
let w,h = getSize p in
  let u = Translate(p, 10, 5, w+10,h+5) in 
    printfn "\nTranslate right-down bigger size: %A" u;
    save (sharpDraw u) "translaterdb.jpg"

/// Testing creation of Picture tree
let r = horizontal p Top q
printfn "\nhorizontal p Top q: %A" r
save (sharpDraw r) "r.jpg"
let s = vertical p Center q
printfn "\nvertical p Center q: %A" q
save (sharpDraw s) "s.jpg"
let t = horizontal r Bottom s;
printfn "\nhorizontal r Bottom s: %A" t
save (sharpDraw t) "nonTrivial.jpg"

/// Testing horizontal concatenation
let hPos = [Top; Center; Bottom];
let lstHorisontal0 = List.map (fun pos -> horizontal p pos p) hPos
printfn "\nHorizontally empty-empty: %A" (List.zip hPos lstHorisontal0)
List.iter (fun (v,p) -> save (sharpDraw p) (sprintf "horizontal%g.jpg" v)) (List.zip hPos lstHorisontal0)
let lstHorisontal1 = List.map (fun pos -> horizontal p pos q) hPos
printfn "\nHorizontally empty-full: %A" (List.zip hPos lstHorisontal1)
List.iter (fun (v,p) -> save (sharpDraw p) (sprintf "horizontal%g.jpg" v)) (List.zip hPos lstHorisontal1)
let lstHorisontal2 = List.map (fun pos -> horizontal q pos p) hPos
printfn "\nHorizontally full-empty: %A" (List.zip hPos lstHorisontal2)
List.iter (fun (v,p) -> save (sharpDraw p) (sprintf "horizontal%g.jpg" v)) (List.zip hPos lstHorisontal2)

/// Testing vertical concatenation
let vPos = [Left; Center; Right];
let lstVertical0 = List.map (fun pos -> vertical p pos p) vPos
printfn "\nVertically empty-empty: %A" (List.zip vPos lstVertical0)
List.iter (fun (v,p) -> save (sharpDraw p) (sprintf "vertical%g.jpg" v)) (List.zip hPos lstVertical0)
let lstVertical1 = List.map (fun pos -> vertical p pos q) vPos
printfn "\nVertically empty-full: %A" (List.zip vPos lstVertical1)
List.iter (fun (v,p) -> save (sharpDraw p) (sprintf "vertical%g.jpg" v)) (List.zip hPos lstVertical1)
let lstVertical2 = List.map (fun pos -> vertical q pos p) vPos
printfn "\nVertically full-empty: %A" (List.zip vPos lstVertical2)
List.iter (fun (v,p) -> save (sharpDraw p) (sprintf "vertical%g.jpg" v)) (List.zip hPos lstVertical2)

/// Testing stacking
let tPos = [0.0; 0.5; 1.0];
let lstOnTop0 = List.map (fun a -> List.map (fun b -> ontop p a b p) tPos) tPos
printfn "\nOnTop empty-empty: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop0))
List.iter (fun lst -> List.iter (fun p -> sharpDraw p|>ignore) lst) lstOnTop0
let lstOnTop1 = List.map (fun a -> List.map (fun b -> ontop p a b q) tPos) tPos
printfn "\nOnTop empty-full: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop1))
List.iter (fun lst -> List.iter (fun p -> sharpDraw p|>ignore) lst) lstOnTop1
let lstOnTop2 = List.map (fun a -> List.map (fun b -> ontop q a b p) tPos) tPos
printfn "\nOnTop full-empty: %A" (List.zip tPos (List.map (fun lst -> List.zip tPos lst) lstOnTop2))
List.iter (fun lst -> List.iter (fun p -> sharpDraw p|>ignore) lst) lstOnTop2

/// Testing rotation modification
let w,h = getSize t
let u = Rotate(t, 0, 0, 10.0*System.Math.PI/180.0, w, h)
printfn "\nRotate(t): %A" u
save (sharpDraw u) "rotate.jpg"

/// Testing rotation and animated gifs
let b = box Color.Black w h
let frames = List.map (fun i -> OnTop(b, Rotate(t, (float w)/2.0, (float h)/2.0, (float i)/60.0*2.0*System.Math.PI, w, h), w, h)) [0..59]
let movie = List.map sharpDraw frames
let frameDelay = 100
let repeatCount = 5
saveAnimatedGif frameDelay repeatCount movie "animated.gif"

/// Testing content creations
let pen = new Pen(Color.White, 1f) in
  let p = curve 50 30 pen [(10.0,10.0); (20.0, 25.0); (10.0, 25.0); (10.0, 10.0)] in 
    save (sharpDraw p) "curve.jpg"

let p = rectangle 50 30 pen (10.0,10.0) (30.0, 10.0) in 
  save (sharpDraw p) "rectangle.jpg"

let p = rectangle 50 30 pen (10.0,10.0) (30.0, 10.0) in 
  save (sharpDraw p) "rectangle.jpg"

let p = filledRectangle 50 30 Color.Red (10.0,10.0) (30.0, 10.0) in 
  save (sharpDraw p) "filledRectangle.jpg"

let p = filledPolygon 50 30 Color.White [(10.0,10.0); (20.0, 25.0); (10.0, 25.0); (10.0, 10.0)] in 
  save (sharpDraw p) "filledPolygon.jpg"

let p = ellipse 512 256 pen (256.0,128.0) (128.0,64.0) in 
  save (sharpDraw p) "circle.jpg"

let p = filledEllipse 512 256 Color.White (256.0,128.0) (128.0,64.0) in 
  save (sharpDraw p) "filledCircle.jpg"

printfn "Available fonts:\n%A" systemFonts
let font = createFont "Microsoft Sans Serif" 36.0 in
  let p = text Color.White font "Hello World" in 
    save (sharpDraw p) "text.jpg"
