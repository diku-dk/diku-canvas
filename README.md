# The ImgUtil library (img_util.dll)

<img src="images/turtle.png" border="2" width="250" align="right">

This library features a number of utility functions for drawing simple
2d graphics on a canvas, including features for loading and saving
images and for running simple user-interactive apps that display
images. The library, which is based on Mono and Gtk, is portable, in
the sense that applications built using the library can execute on
Linux, macOS, and Windows.

## The API

The library API is available in the file [img_util.fsi](img_util.fsi).

## Quick and dirty port to SDL2

First install [.NET Core SDK
v3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) for your
platform.

Then install [SDL2](https://www.libsdl.org/index.php):

  * On **macOS** with homebrew:

        brew install sdl2

  * On **Debian** and **Ubuntu**

        apt install libsdl2-dev

  * On **Windows** we got you back covered and have added a copy of
    the SDL runtime in the file `SDL2.dll`

Finally, compile and run the `turtle` example:

    dotnet run

(To try an other example, edit the `img_util.fsproj` file.)

**Below are old mono instructions**



## Example compilation and use of the library

To compile and run an example program without using `make`, see the
section titled "Compilation without using make" below. Before you
start, be sure that you have the mono-mdk framework installed. On
macOS, mono-mdk can be installed as follows, using `brew`:

    $ brew install mono-mdk

Also, ensure that you're accessing the correct version of Mono by
adding the following line to your `~/.bash_profile` file:

    export PATH="/Library/Frameworks/Mono.framework/Versions/Current/Commands:$PATH"

Make sure that your `PATH` environment variable is updated:

    $ . ~/.bash_profile

To build the `img_util.dll` resource, execute the following command
in the present directory:

    $ make img_util.dll

If you are not on macOS, you probably need to adjust the Makefile or build the DLL without using make, as described below.

## Examples

<img src="images/applespiral.png" border="2" width="250" align="right">

A number of examples are available in the `examples` folder. To
compile the examples, execute the following commands:

    $ cd examples
	$ make all

Here is an overview of the examples.

### Sierpinski and Spiral

    $ make sierp.exe && mono sierp.exe
    $ make spiral.exe && mono spiral.exe

### Save png files

    $ make fig.exe && mono fig.exe

### Functional images

    $ make gui_wav.exe && mono gui_wav.exe

### Turtle graphics

    $ make turtle.exe && mono turtle.exe

## Compilation without using make

First copy the two files `img_util.fsi` and `img_util.fs` to a local
directory. Then, in a terminal, execute the following command:

    $ fsharpc --nologo -I /Library/Frameworks/Mono.framework/Versions/Current/lib/mono/gtk-sharp-2.0 -r gdk-sharp.dll -r gtk-sharp.dll -a img_util.fsi img_util.fs

This command should produce the file `img_util.dll`, which should now
be available in the present directory.

Now, to compile and run the Spiral example, for example, copy the
file `spiral.fs` to the present directory and execute the commands:

    $ fsharpc --nologo -r img_util.dll spiral.fs
    $ mono spiral.exe

You should now be able to find an open window showing a spiral (under
macOS, you may need to hit Command-Tab a couple of times to select the
newly opened window).

## License

MIT license

## Copyright

Copyright 2020 - Martin Elsman
