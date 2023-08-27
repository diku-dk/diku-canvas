The DIKU-Canvas library: A Functional Graphics Library for F#
==============

[![Nuget](https://img.shields.io/nuget/v/DIKU.Canvas)](https://www.nuget.org/packages/DIKU.Canvas/)

<img src="https://raw.githubusercontent.com/diku-dk/diku-canvas/main/images/Sierpinski.png" border="2" width="250" align="right">
DIKU-Canvas is a simple graphics library developed specifically for teaching functional programming in F#. Rooted in computational geometry and functional paradigms, DIKU-Canvas provides computer scientists, researchers, and developers with an intuitive and mathematical approach to graphical programming.

Leveraging F#'s functional programming capabilities, DIKU-Canvas emphasizes:

- **Immutability**: All shapes and transformations are immutable, promoting a purely functional approach.
- **Higher-order functions**: Utilize functional constructs to create and manipulate complex shapes.
- **Type Safety**: Benefit from F#'s strong type system to ensure correctness and robustness.

# Introduction

Graphic primitives may be transformed and combined in a tree structure, and the trees may be rendered to the screen or to file as a still image or an animation. DIKU-Canvas also has an interactive mode that accepts input from the keyboard and the mouse.

## Primitives

The collection of primitives serves as the foundation for complex geometric shapes:
- **Piecewise Affine Lines**: Represented as sequences of connected line segments, allowing for intricate paths and outlines.
- **Circular Arcs**: Defined by a center, radius, and angle, enabling precise circular structures.
- **Cubic Bezier Curves**: Provide control over curve definition and complexity, facilitating the design of smooth and customizable curves.
- **Rectangles**: Utilize coordinates for position, width, and height to draw various rectangular shapes.
- **Ellipses**: Create ellipses by specifying parameters that control shape and orientation.

## Transformations and composition

The primitives can be transformed and combined with:

- **Translation**: Translate objects across the 2D plane with user-defined x and y offsets.
- **Rotation**: Rotate objects around a specific point, providing the angle in degrees or radians.
- **Scaling**: Resize objects by a given scaling factor, either uniformly or non-uniformly.
- **Horizontal and Vertical Alignment**: Utilize alignment functions to organize shapes horizontally or vertically, aiding in layout design.
- **Layering Shapes**: Combine shapes by drawing them on top of each other, allowing for the creation of intricate designs.

## Rendering and interaction with the user

DIKU-Canvas has several rendering and interaction options:
- **Render**: Graphic trees may be rendered to the screen or a file
- **Animation**: Sequences of graphic trees may be rendered as an animation to the screen or a file
- **Interaction**: DIKU-Canvas has an interactive mode, which reacts to the user input from the keyboard or mouse and allows the programmer to update the graphic tree and render the result to the screen.

-----------

## Overview

Canvas is a system for combining simple graphics primitives into new figures organized as a tree. For example, 3 boxes can be combined to a single figure as
```fsharp
let box1 = rectangle goldenrod 1.0 20.0 80.0
let box2 = rectangle yellow 1.0 30.0 30.0
let tree = alignH (alignV box1 Right box2) Center box1  
printfn "%s" (toString tree)
```
results in
<img src="https://raw.githubusercontent.com/diku-dk/diku-canvas/main/images/Sierpinski.png" border="2" width="250" align="right">
```fsharp
AlignH position=0.5
∟>AlignV position=1
  ∟>Rectangle (color,stroke)=(Color DAA520FF, 1.0) coordinates=(0.0, 0.0, 20.0, 80.0)
  ∟>Rectangle (color,stroke)=(Color FFFF00FF, 1.0) coordinates=(0.0, 0.0, 30.0, 30.0)
∟>Rectangle (color,stroke)=(Color DAA520FF, 1.0) coordinates=(0.0, 0.0, 20.0, 80.0)
``</code>``
which demonstrates that alignV encloses box1 and box2 including data about each element.



The interact function can render still images in a window, show animations in a window, and to interact with the user via the keyboard and the mouse. For example,
```fsharp
let tree = ellipse darkCyan 2.0 85.0 64.0 |> translate 128.0 128.0
let draw _ = make tree
let react _ _ = None
interact "Render an image" 256 256 None draw react 0
```
The main workhorses of <c>interact</c> are the draw and react functions, which communicate via a user-defined state value. In the above example, the state value is implicitly defined as an integer, since the last argument of interact is of integer type, but both draw and react ignore the state. To make an animation with interact, the react function must react on TimerTicks. Consider the following example,
```fsharp
type state = float
let tree (i:state) : PrimitiveTree = 
    ellipse darkCyan 2.0 85.0 64.0 |> translate i i
let draw (j: state) : Picture = make (tree j)
let react (j: state) (ev:Event) : state option = 
    match ev with
        | Event.TimerTick -> Some ((j+1.0)%128.0)
        | _ -> None
let interval = Some 100
let initialState = 0.0
interact "Render an image" 256 256 interval draw react initialState
```
Here, we define a state type as float, which is the value controlling what to draw. The draw function is then a function, which takes a state and produces a Picture in this case the make of an tree containing an ellipse. The react function is set to listen for TimerTick events. When one such event occurs, it returns the next value of the state wrapped in an option type (Some). Other events may happen, but they are all ignored by returning None. Note that there is no mutable value, which contains the present value of the state. Further, note that the draw function is called inside interact, whenever interact deems it necessary, such as when react has been called to produce a new value of the state.

The function react is called when the following events occur:
  Key of char - when the user presses a regular key
  DownArrow - when the user presses the down arrow
  UpArrow - when the user presses the up arrow
  LeftArrow - when the user presses the left arrow
  RightArrow - when the user presses the right arrow
  Return - when the user presses the return key
  MouseButtonDown(x,y)- when the user presses the left mouse button
  MouseButtonUp(x,y) - when the user releases the left mouse button
  MouseMotion(x,y,relx,rely) - when the user moves the mouse
  TimerTick - when the requested time interval has passed
Note that there is no guarantee that the exact interval has occurred between each TimerTick event, and depending on the computing system being used, there is a lower limit to how fast an event loop can be served.

Finally, the state can be any value, and thus the system offers much flexibility in terms of the communication between the draw and the react function. However, since the programmer (and the user) are only indirectly in control of their communication, it may be useful to think of draw and react as isolated functions. E.g., a call by <c>interact</c> to <c>draw j</c> should produce a Picture for state <c>j</c> regardless of the previous
picture or the possible next. Likewise, a call to <c>read j ev</c> should react to the situation specified by <c>j</c> and <c>ev</c> only, and the programmer should concentrate only on what the next event should be given said input.





-----------

## The application programming interface (API)

The API is described in the file [`canvas.fsi`](canvas.fsi). There you will find a precise declaration of all available values and functions and their documentation using the [XML](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/xml-documentation) standard.

## How to use Canvas in an F# script (.fsx)

Make an F# script, say `myFirstCanvas.fsx` with a NuGet reference:

```fsharp
#r "nuget:DIKU.Canvas, 2.0"
open Canvas

let w,h = 256,256
let tree = filledRectangle green ((float w)/2.0) ((float h)/2.0)
let draw = make tree
render "My first canvas" w h draw
```

and run it from the command line using

    dotnet fsi myFirstCanvas.fsx

This should result in a window with a green square in the top left corner on a black background.

If you want a specific version you edit the reference to be, e.g.,:

```fsharp
#r "nuget:DIKU.Canvas, 2.0.1-alpha8"
```


## How to use Canvas in a F# project (that uses .fsproj)

Make a new directory, say `mycanvasapp`, in that directory start an F#
"Console App" project with the command:

    dotnet new console -lang "F#"

(This will give you both a `Program.fs` file and a `mycanvasapp.fsproj` file.)

Add a reference to the `DIKU.Canvas` package with the command:

    dotnet add package DIKU.Canvas

Edit `Program.fs` to have the content:

```fsharp
open Canvas

let w,h = 256,256
let tree = filledRectangle green ((float w)/2.0) ((float h)/2.0)
let draw = make tree
render "My first canvas" w h draw
```

Run your app with the command:

    dotnet run

This should result in a window with a green square in the top left corner on a black background.

## Examples

Several examples are available in the `examples` folder:

- [`examples/animate.fsx`](./examples/animate.fsx)

    demonstrates how to make an animation

- [`examples/animateGif.fsx`](./examples/animateGif.fsx)

    demonstrates how to save an animation as an animated gif

- [`examples/basic.fsx`](./examples/basic.fsx)

    demonstrates all DIKU-Canvas graphics primitives, transformations, and combinators using arguments from the command line

- [`examples/colortest.fsx`](./examples/colortest.fsx)

    demonstrates how to get and react to keyboard input

- [`examples/drawLines.fsx`](./examples/drawLines.fsx)

    demonstrates how to render many lines using the onto combination
    
- [`examples/miniGame.fsx`](./examples/miniGame.fsx)

    demonstrates how to make a small catch-the-monster game with the arrow keys

- [`examples/mouseTest.fsx`](./examples/mouseTest.fsx)

    demonstrates how to get and react to mouse input
    
- [`examples/myFirstCanvas.fsx`](./examples/myFirstCanvas.fsx)

    demonstrates how to render an image on the screen
    
- [`examples/pacman.fsx`](./examples/pacman.fsx)

    an animation demonstration
    
- [`examples/renderToFile.fsx`](./examples/renderToFile.fsx)

    demonstrates how to render a graphics tree to a file
    
- [`examples/sierpinski.fsx`](./examples/sierpinski.fsx)

    demonstrates how to recursively build a graphics tree
    
- [`examples/spiral.fsx`](./examples/spiral.fsx)

    demonstrates how to recursively build a graphics tree


## How to build the Canvas library itself (if you want to contribute)

If you want to build the library and NuGet package yourself, you will
need the `.NET7.0 SDK` and development versions of `SDL2` and
`SDL2_image` for your platform.

First, install [.NET
7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) for your
platform.

Then install [SDL2 and SDL2_image](https://www.libsdl.org/index.php):

  * On **macOS** with homebrew:

        brew install sdl2 sdl2_image

  * On **Debian** and **Ubuntu**:

        apt install libsdl2-dev libdl2-image-dev

  * On **Arch** (and probably **Manjaro**), `sdl2` is available in `extra`:

        sudo pacman -S sdl2 sdl2_image

  * On **Windows** you need `SDL2.dll` and `SDL2_image.dll` in a
    search path available to dotnet.  The `.dll`'s in the `lib/`
    folder should be sufficient for building.  (Better instructions
    are a welcome contribution if anyone builds on Windows)


Finally, compile the library:

    dotnet build

And pack it into a NuGet package:

    dotnet pack

The package will be available in `bin/Debug/Canvas.X.y.z.nupkg`.

## Using the NuGet package from a local repository

Start by creating a directory to use as a local NuGet repository. Add
it to the `NuGet` sources list:

    dotnet nuget add source /full/path/to/the/directory

Ensure the repository was added with:

    dotnet nuget list source

Place `Canvas.X.y.z.nupkg` in the local repository.
Now clear the nuget cache:

    dotnet nuget locals all -c

Test that everything worked with `dotnet fsi`

```fsharp
dotnet fsi
>#r "nuget:DIKU.Canvas";;
>open Canvas;;
```

Canvas is now available to use in projects through NuGet, both in the
interpreter and in projects with e.g. `.fsproj` files.

## License

MIT license

## Copyright

Copyright 2018-2021: Martin Elsman

Copyright 2022-2023: Ken Friis Larsen

Copyright 2023- : Ken Friis Larsen and Jon Sporring

## Contributions

The following individuals have contributed to the DIKU.Canvas (previously ImgUtil) library:

- Ken Friis Larsen
- Martin Elsman
- Mads Dyrvig Obitsø Thomsen
- Jon Sporring
- Jan Rolandsen
- Chris Pritchard (original SDL P/Invoke)