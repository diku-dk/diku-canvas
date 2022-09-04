#r "nuget:DIKU.Canvas, 1.0.0-alpha3"
open Canvas

let rec spiral C s i x y =
  if i >= 350 then ()
  else let p1 = (x,y)
       let p2 = (x+i,y)
       let p3 = (x+i,y+i)
       let p4 = (x-s,y+i)
       let p5 = (x-s,y-s)
       do setLine C red p1 p2
       do setLine C red p2 p3
       do setLine C red p3 p4
       do setLine C red p4 p5
       spiral C s (i+2*s) (x-s) (y-s)

let C = create 400 400
do spiral C 10 10 200 200
do show C "Spiral"
