open ImgUtil

let rec triangle bm len (x,y) =
  if len < 20.0 then
    setBox blue (int(round x),int(round y))
                (int(round(x+len)),
                 int(round(y+len))) bm
  else let half = len / 2.0
       do triangle bm half (x+half/2.0,y)
       do triangle bm half (x,y+half)
       do triangle bm half (x+half,y+half)

do runSimpleApp "Sierpinski" 600 600
      (fun w h -> let bm = mk w h
                  in triangle bm (float(min w h)) (0.0,0.0);
                     bm)
