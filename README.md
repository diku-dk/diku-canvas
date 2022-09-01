# The Canvas library

<img src="https://raw.githubusercontent.com/kfl/img-util-fs/main/images/turtle.png" border="2" width="250" align="right">

This library features a number of utility functions for drawing simple
2d graphics on a canvas, including features for running simple
user-interactive apps that display such canvas. 
A small DSL for drawing using [turtle graphics](https://en.wikipedia.org/wiki/Turtle_graphics) is included.

Features for loading a canvas from an image file and saving canvas to `.png` are included.
They *should* work out of the box on Windows, but on Linux and MacOS they require the user to have `SDL2` installed locally.

The library, which is based on SDL2, is portable,
in the sense that applications built using the library can execute on
Linux, macOS, and Windows using .NET6.0.

The library is intended to be built (and consumed) as a NuGet package.

## The API

The library API is available in the file [`img_util.fsi`](img_util.fsi).

## How to build

If you want to build the library and NuGet package yourself, you will need the `.NET6.0 SDK` and development versions of `SDL2` and `SDL2_image` for your platform. 

First install [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) for your
platform.

Then install [SDL2 and SDL2_image](https://www.libsdl.org/index.php):

  * On **macOS** with homebrew:

        brew install sdl2 sdl2_image

  * On **Debian** and **Ubuntu**:

        apt install libsdl2-dev libdl2-image-dev

  * On **Arch** (and probably **Manjaro**), `sdl2` is available in `extra`:

        sudo pacman -S sdl2 sdl2_image

  * On **Windows** you need `SDL2.dll` and `SDL2_image.dll` in a search path available to dotnet.
    The `.dll`'s in the `lib/` folder should be sufficient for building. 
	(Better instructions are a welcome contribution if anyone builds on Windows)


Finally, compile the library:

    dotnet build

And pack it into a NuGet package:

    dotnet pack

The package will be available in `bin/Debug/Canvas.X.y.z.nupkg`.

## Using the NuGet package from a local repository

Start by creating a directory to use as a local NuGet repository. Add it to the `NuGet` sources list:

    dotnet nuget add source /full/path/to/the/directory

Ensure the repository was added with

    dotnet nuget list source 

Place `Canvas.X.y.z.nupkg` in the local repository.
Now clear the nuget cache:

    dotnet nuget locals all -c

Test that everything worked with `dotnet fsi`

```
dotnet fsi
>#r "nuget:Canvas";;
>open Canvas;;
```

Canvas is now available to use in projects through NuGet, both in the interpreter and in projects with e.g. `.fsproj` files. 

## Examples

<img src="images/applespiral.png" border="2" width="250" align="right">

A number of examples are available in the `examples` folder.

The best show-cases for using the library are
- `examples/color_boxes.fsx`
- `examples/keyboard_example.fsx`
- `examples/turtle.fsx`

Note that not all the examples are currently executable.

## License

MIT license

## Copyright

Copyright 2018-2021 - Martin Elsman

## Contributions

The following individuals have contributed to the ImgUtil library:

- Ken Friis Larsen
- Mads Dyrvig Obitsø Thomsen
- Jan Rolandsen
- Jon Sporring
- Chris Pritchard (SDL P/Invoke)
