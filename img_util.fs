module ImgUtil

open Gtk
open System

// some utility functions
let max x y = if x > y then x else y
let abs a = if a < 0 then -a else a

// colors
type color = {a:byte;r:byte;g:byte;b:byte}

let fromArgb (a:int,r:int,g:int,b:int) : color =
  {a=byte(a); r=byte(r); g=byte(g); b=byte(b)}

let fromRgb (r:int,g:int,b:int) : color = fromArgb (255,r,g,b)

let red : color = fromRgb(255,0,0)
let green : color = fromRgb(0,255,0)
let blue : color = fromRgb(0,0,255)

let fromColor (c: color) : int * int * int * int =
  (int(c.a),int(c.r),int(c.g),int(c.b))

// canvas - 4 bytes for each pixel (r,g,b,a)
type canvas = {w:int;h:int;data:byte[]}

// create white opaque canvas
let mk w h : canvas =
  {w=w; h=h;
   data=Array.create (h*w*4) 0xffuy}

let height (C:canvas) = C.h
let width (C:canvas) = C.w

// draw a single pixel if it is inside the bitmap
let setPixel (c: color) (x,y) (C:canvas) : unit =
  if x < 0 || y < 0 || x >= C.w || y >= C.h then ()
  else let i = 4*(y*C.w+x)
       in (C.data.[i] <- c.r; C.data.[i+1] <- c.g;
           C.data.[i+2] <- c.b; C.data.[i+3] <- c.a)

// draw a line by linear interpolation
let setLine (c: color) (x1:int,y1:int) (x2:int,y2:int) C : unit =
  let m = max (abs(y2-y1)) (abs(x2-x1))
  in if m = 0 then ()
     else for i in [0..m] do
            let x = ((m-i)*x1 + i*x2) / m
            let y = ((m-i)*y1 + i*y2) / m
            in setPixel c (x,y) C

// draw a box
let setBox c (x1,y1) (x2,y2) C =
  do setLine c (x1,y1) (x2,y1) C
  do setLine c (x2,y1) (x2,y2) C
  do setLine c (x2,y2) (x1,y2) C
  do setLine c (x1,y2) (x1,y1) C

// get a pixel color from a bitmap
let getPixel (C:canvas) (x:int,y:int) : color =   // rgba
  let i = 4*(y*C.w+x)
  in {r=C.data.[i]; g=C.data.[i+1];
      b=C.data.[i+2]; a=C.data.[i+3]}

// initialize a new bitmap
let init (w:int) (h:int) (f:int*int->color) : canvas =    // rgba
  let data = Array.create (h*w*4) 0xffuy
  for y in [0..h-1] do
    for x in [0..w-1] do
      let c = f (x,y)
      let i = 4*(y*w+x)
      in (data.[i] <- c.r;
          data.[i+1] <- c.g;
          data.[i+2] <- c.b;
          data.[i+3] <- c.a)
  {h=h;w=w;data=data}

// scale a bitmap
let scale (C:canvas) w2 h2 : canvas =
  let scale_x x = (x * C.w) / w2
  let scale_y y = (y * C.h) / h2
  in init w2 h2 (fun (x,y) -> getPixel C (scale_x x, scale_y y))

// read a bitmap file
let fromFile (fname : string) : canvas =
  use pb0 = new Gdk.Pixbuf(fname)
  use pb = pb0.AddAlpha(false,0x0uy,0x0uy,0x0uy)
  let h = pb.Height
  let w = pb.Width
  let len = h*w*4
  let data = Array.create len 0xffuy
  in (System.Runtime.InteropServices.Marshal.Copy(pb.Pixels,data,0,len);
      {h=h;w=w;data=data})

// create a pixbuf from a bitmap
let toPixbuf (C:canvas) =
  new Gdk.Pixbuf(C.data,true,8,C.w,C.h,4*C.w)

// save a bitmap as a png file
let toPngFile (fname : string) (C:canvas) : unit =
  use pb = toPixbuf C
  in pb.Save(fname,"png") |> ignore

// start and run an application with an action
let runApplication (action:unit -> unit) =
  (Application.Init();
   Application.Invoke(fun _ _ -> action());
   Application.Run())

type Key = Gdk.Key

// start an app that can listen to key-events
let runApp (t:string) (w:int) (h:int)
           (f:int -> int -> 's -> canvas)
           (onKeyDown: 's -> Key -> 's option) (s:'s) : unit =
  runApplication (
    fun () ->
     let window = new Window(t)
     in (window.SetDefaultSize(w,h)
         window.DeleteEvent.Add(fun e -> (window.Hide();
                                          Application.Quit();
                                          e.RetVal <- true))
         let state = ref s
         let drawing = new Gtk.DrawingArea()
         let draw () =
           let gc = drawing.Style.BaseGC(StateType.Normal)
           let (w,h) = window.GetSize()
           let C = f w h (!state)
           use pb = toPixbuf C
           use pm = new Gdk.Pixmap(null, w, h, 24)
           in (pb.RenderToDrawable(pm,gc,0,0,0,0,w,h,Gdk.RgbDither.None,0,0);
               drawing.GdkWindow.DrawDrawable(gc,pm,0,0,0,0,-1,-1);
               gc.Dispose()
              )
         in (drawing.ExposeEvent.Add( fun _ -> draw() );
             window.KeyPressEvent.Add( fun (e:KeyPressEventArgs) ->
                                       match e.Event.Key with
                                           | Gdk.Key.Escape -> Application.Quit()
                                           | k -> (match onKeyDown (!state) k with
                                                   | Some s -> (state := s;
                                                                window.QueueDraw())
                                                   | None -> ())
                                     );
             window.Add(drawing);
             window.ShowAll();
             window.Show()))
  )

let runSimpleApp t w h (f:int->int->canvas) : unit =
  runApp t w h (fun w h () -> f w h)
               (fun _ _ -> None) ()

let show t (C:canvas) : unit =
  runApp t (width C) (height C) (fun w h () -> scale C w h) (fun _ _ -> None) ()
