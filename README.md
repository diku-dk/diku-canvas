The DIKU-Canvas library: A Functional Graphics Library for F#
==============

[![Nuget](https://img.shields.io/nuget/v/DIKU.Canvas)](https://www.nuget.org/packages/DIKU.Canvas/)

<img src="https://raw.githubusercontent.com/diku-dk/diku-canvas/main/docs/images/Sierpinski.png" border="2" width="250" align="right">
DIKU-Canvas is a simple graphics library developed specifically for teaching functional programming in F#. Rooted in computational geometry and functional paradigms, DIKU-Canvas provides computer scientists, researchers, and developers with an intuitive and mathematical approach to graphical programming.

Leveraging F#'s functional programming capabilities, DIKU-Canvas emphasizes:

- **Immutability**: All shapes and transformations are immutable, promoting a pure functional approach.
- **Higher-Order Functions**: Utilize functional constructs to create and manipulate complex shapes.
- **Type Safety**: Benefit from F#'s strong type system to ensure correctness and robustness.

# Overview

Graphic primitives may be transformed and combined in a tree structure, and the trees may be rendered to the screen or to file as a still-image or an animation. DIKU-Canvas also has an interactive mode which accepts intput from the keyboard and the mouse.

## Primitives

The collection of primitives serve as the foundation for complex geometric shapes:
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
- **Render**: Graphic trees may be rendered to the screen or to a file
- **Animation**: Sequences of graphic trees may be rendered as an animation to the screen or to a file
- **Interaction**: DIKU-Canvas has an interactive mode, which reacts to the user input from the keyboard or mouse and allows the programmer to update the graphic tree and render the result to the screen.

-----------

## The application programming interface (API)

The API is described in the file [`canvas.fsi`](canvas.fsi). There you will find a precise declaration of all available values and functions and their documentation using the [XML](https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/xml-documentation) standard.

## How to use Canvas in a F# script (.fsx)

Make an F# script, say `myFirstCanvas.fsx` with a NuGet reference:

```fsx
#r "nuget:DIKU.Canvas, 2.0"
open Canvas

let w,h = 256,256
let tree = filledRectangle Color.green ((float w)/2.0) ((float h)/2.0)
let draw = fun _ -> make tree
render "My first canvas" w h draw
```

and run it from the commandline using

    dotnet fsi myFirstCanvas.fsx

This should result in a window with a green square in the top left corner on a black background.

If you want a specific version you edit the reference to be, e.g.,:

```fsx
#r "nuget:DIKU.Canvas, 2.0.1-alpha8"
```


## How to use Canvas in a F# project (that uses .fsproj)

Make an new directory, say `mycanvasapp`, in that directory start a F#
"Console App" project with the command:

    dotnet new console -lang "F#"

(This will give you both a `Program.fs` file and a `mycanvasapp.fsproj` file.)

Add a reference to the `DIKU.Canvas` package with the command:

    dotnet add package DIKU.Canvas

Edit `Program.fs` to have the content:

```fsharp
open Canvas

let w,h = 256,256
let tree = filledRectangle Color.green ((float w)/2.0) ((float h)/2.0)
let draw = fun _ -> make tree
render "My first canvas" w h draw
```

Run your app with the command:

    dotnet run

This should result in a window with a green square in the top left corner on a black background.

## Examples

A number of examples are available in the `examples` folder:

- [`examples/animate.fsx`](./examples/animate.fsx)

    demonstrates how to make an animation

- [`examples/animateGif.fsx`](./examples/animateGif.fsx)

    demonstrates how to save an animation as an animated gif

- [`examples/basic.fsx`](./examples/basic.fsx)

    demonstrates all DIKU-Canvas graphics primitives, transformations, and combinators using argument from the command line

- [`examples/colortest.fsx`](./examples/colortest.fsx)

    demonstrates how to get and react to keyboard input

- [`examples/drawLines.fsx`](./examples/drawLines.fsx)

    demonstrates how to render many lines using the onto combination
    
- [`examples/mouseTest.fsx`](./examples/mouseTest.fsx)

    demonstrates how to get and react to mouse input
    
- [`examples/miniGame.fsx`](./examples/miniGame.fsx)

    a simple catch-the-monster game
    
- [`examples/myFirstCanvas.fsx`](./examples/myFirstCanvas.fsx)

    demonstrates how to render an image to the screen
    
- [`examples/pacman.fsx`](./examples/pacman.fsx)

    an animation demonstration
    
- [`examples/renderToFile.fsx`](./examples/renderToFile.fsx)

    demonstrates how to render a graphics tree to a file
    
- [`examples/sierpinski.fsx`](./examples/sierpinski.fsx)

    demonstrates ow to recursively build a graphics tree
    
- [`examples/spiral.fsx`](./examples/spiral.fsx)

    demonstrates how to recursively build a graphics tree


## How to build the Canvas library itself (if you want to contribute)

If you want to build the library and NuGet package yourself, you will
need the `.NET7.0 SDK` and development versions of `SDL2` and
`SDL2_image` for your platform.

First install [.NET
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

```
dotnet fsi
>#r "nuget:DIKU.Canvas";;
>open Canvas;;
```

Canvas is now available to use in projects through NuGet, both in the
interpreter and in projects with e.g. `.fsproj` files.

## License

MIT license

## Copyright

Copyright 2018-2021 - Martin Elsman

Copyright 2022-2023 - Ken Friis Larsen

## Contributions

The following individuals have contributed to the DIKU.Canvas (previosly ImgUtil) library:

- Ken Friis Larsen
- Martin Elsman
- Mads Dyrvig Obitsø Thomsen
- Jon Sporring
- Jan Rolandsen
- Chris Pritchard (original SDL P/Invoke)

