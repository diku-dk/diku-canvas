The DIKU-Canvas library: A Functional Graphics Library for F#
==============

[![Nuget](https://img.shields.io/nuget/v/DIKU.Canvas)](https://www.nuget.org/packages/DIKU.Canvas/)
<img src="https://raw.githubusercontent.com/kfl/diku-canvas/main/images/Sierpinski.png.png" border="2" width="250" align="right">
DIKU-Canvas is an advanced graphics library developed specifically for functional programming in F#. Rooted in computational geometry and functional paradigms, DIKU-Canvas provides computer scientists, researchers, and developers with an intuitive and mathematical approach to graphical programming.

# Overview

## Primitives

DIKU-Canvas offers a collection of primitives that serve as the foundation for complex geometric shapes:

### Piecewise Affine Lines
Represented as sequences of connected line segments, allowing for intricate paths and outlines.

### Circular Arcs
Defined by a center, radius, and angle, enabling precise circular structures.

### Cubic Bezier Curves
Provide control over curve definition and complexity, facilitating the design of smooth and customizable curves.

### Rectangles
Utilize coordinates for position, width, and height to draw various rectangular shapes.

### Ellipses
Create ellipses by specifying parameters that control shape and orientation.

## Transformations

DIKU-Canvas includes several transformation functions, empowering users to modify and control shapes:

### Translation
Translate objects across the 2D plane with user-defined x and y offsets.

### Rotation
Rotate objects around a specific point, providing the angle in degrees or radians.

### Scaling
Resize objects by a given scaling factor, either uniformly or non-uniformly.

## Composition and Alignment

In DIKU-Canvas, primitives and transformations can be synthesized to construct complex graphical entities:

### Horizontal and Vertical Alignment
Utilize alignment functions to organize shapes horizontally or vertically, aiding in layout design.

### Layering Shapes
Combine shapes by drawing them on top of each other, allowing for the creation of intricate designs.

## Animation and interaction with the user

DIKU-Canvas has an interactive mode allowing for producing animations, and reacting to input from the user via the keyboard and the mouse

## File support

DIKU-Canvas supports writing files in a large variety of image file formats and animated gifs.

## Functional Paradigm

Leveraging F#'s functional programming capabilities, DIKU-Canvas emphasizes:

- **Immutability**: All shapes and transformations are immutable, promoting a pure functional approach.
- **Higher-Order Functions**: Utilize functional constructs to create and manipulate complex shapes.
- **Type Safety**: Benefit from F#'s strong type system to ensure correctness and robustness.

## Conclusion

DIKU-Canvas extends the boundaries of functional graphics programming, offering computer scientists a robust and mathematically sound platform for visualization, geometric computations, and artistic creations. Its integration with the principles of functional programming makes DIKU-Canvas not just a graphics library but a comprehensive environment for geometric exploration and creativity. Start experimenting with DIKU-Canvas today, and unlock a new dimension in functional graphical programming.

-----------

## The API

The library API is available in the file [`canvas.fsi`](canvas.fsi).

## How to use Canvas in a F# script (.fsx)

Make an F# script, say `example.fsx` and make a NuGet reference:

```fsharp
#r "nuget:DIKU.Canvas";;
```

Or, if you want a specific version:

```fsharp
#r "nuget:DIKU.Canvas, 1.0.1";;
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
let draw width height =
    let canvas = Canvas.create width height
    Canvas.setFillBox canvas Canvas.blue (0,0) (width, height)
    canvas

do Canvas.runSimpleApp "Hello from F#" 400 400 draw
```

Run your app with the command:

    dotnet run

This should result in a window with a blue background.


## Examples

<img src="images/applespiral.png" border="2" width="250" align="right">

A number of examples are available in the `examples` folder.

The best show-cases for using the library are
- `examples/color_boxes.fsx`
- `examples/keyboard_example.fsx`
- `examples/spiral.fsx`
- `examples/turtle.fsx` (eventually)

Note that it is not all the examples in the `examples` directory that
have been ported to the current version of the Canvas library.


## How to build the Canvas library itself (if you want to contribute)

If you want to build the library and NuGet package yourself, you will
need the `.NET6.0 SDK` and development versions of `SDL2` and
`SDL2_image` for your platform.

First install [.NET
6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) for your
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

