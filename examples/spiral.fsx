#r "nuget:DIKU.Canvas, 1.0.0-alpha3"
open Canvas

let rec spiral canvas s i x y =
  if i >= 350 then ()
  else let p1 = (x,y)
       let p2 = (x+i,y)
       let p3 = (x+i,y+i)
       let p4 = (x-s,y+i)
       let p5 = (x-s,y-s)
       do setLine canvas red p1 p2
       do setLine canvas red p2 p3
       do setLine canvas red p3 p4
       do setLine canvas red p4 p5
       spiral canvas s (i+2*s) (x-s) (y-s)

let canvas = create 400 400
do spiral canvas 10 10 200 200
do show canvas "Spiral"
