open ImgUtil

let min x y = if x < y then x else y
let max x y = if x > y then x else y
let rec pow2 x = if x <= 1 then x else 2*pow2(x-1)

let rec triangle bmp tst (w,h) (x,y) =
  if w <= tst then setBox red (x,y) (x+w,y+h) bmp
  else let (w_half,h_half) = (w / 2, h / 2)
       do triangle bmp tst (w_half,h_half) (x+w_half/2,y)
       do triangle bmp tst (w_half,h_half) (x,y+h_half)
       do triangle bmp tst (w_half,h_half) (x+w_half,y+h_half)

type state = int

let margin = 30

let draw w h (s:state) =
  let bmp = ImgUtil.mk w h
  let tst = 512 * pow2 s / w
  let (w,h) = (w-2*margin,h-2*margin)
  let () = triangle bmp tst (w,h) (margin,margin)
  in bmp

let react (s:state) (k:Gdk.Key) : state option =
  if k = Gdk.Key.d then
    Some (min 10 (s+1))
  else if k = Gdk.Key.u then
    Some (max 2 (s-1))
  else None

let init = 5

do runApp "Sierp" 600 600 draw react init
