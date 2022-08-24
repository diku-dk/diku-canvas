open ImgUtil

let rec triangle bm len (x,y) =
  if len < 12 then
    setBox blue (x,y) (x+len,y+len) bm
  else let half = len / 2
       do triangle bm half (x+half/2,y)
       do triangle bm half (x,y+half)
       do triangle bm half (x+half,y+half)

do runSimpleApp "Sierpinski" 600 600
      (fun w h -> let bm = mk w h
                  in triangle bm 512 (44,44);
                     bm)
