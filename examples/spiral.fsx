#r "../bin/Debug/net6.0/img_util.dll"
open ImgUtil

let rec spiral C s i x y =
  if i >= 350 then ()
  else let p1 = (x,y)
       let p2 = (x+i,y)
       let p3 = (x+i,y+i)
       let p4 = (x-s,y+i)
       let p5 = (x-s,y-s)
       do setLine red p1 p2 C
       do setLine red p2 p3 C
       do setLine red p3 p4 C
       do setLine red p4 p5 C
       spiral C s (i+2*s) (x-s) (y-s)

let C = mk 400 400
do spiral C 10 10 200 200
do show "Spiral" C
