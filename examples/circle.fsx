#r "nuget:DIKU.Canvas"
open Canvas


setCircle canvas black (200, 200) 150
strokeArc canvas green (200, 200) 100 (1.0*System.Math.PI) (2.0*System.Math.PI) 
strokeArc canvas blue (200, 200) 100 (0.0*System.Math.PI) (1.0*System.Math.PI) 
strokeArc canvas red (200, 200) 125 (0.5*System.Math.PI) (1.5*System.Math.PI) 

// Show that arc can draw circle
strokeArc canvas black (200, 200) 50 (0) (2.0*System.Math.PI) 
show canvas "Circle test"

