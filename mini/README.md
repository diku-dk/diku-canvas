# Mini version of ImgUtil

This version of ImgUtil provides simple methods for reading,
manipulating and saving .png-files on a per-pixel basis.

The library API is available in the file [img_util.fsi](img_util.fsi).


## Compilation

You may make use of the included [Makefile](Makefile), or you can
compile and use the library, manually, as follows:

### Compile the library

```
fsharpc -a img_util.fsi img_util.fs
```

### Compile a program that uses the library

```
fsharpc -r img_util.dll PROGRAMNAME.fsx
```

Here  `PROGRAMNAME` is the name of your .fsx file.

#### Example

```
fsharpc -r img_util.dll simpletest.fsx
```

## Usage


```
open ImgUtil // make the library available to use without prepending ImgUtil.
// create a bitmap object with width=100 and height=200
let canvas = mk 100 200
// define red as a color
let (myRed:color) = fromRgb (255,0,0)
// Make three red dots. setPixel manipulates the given canvas. it returns unit.
// This is "weird" in f#. We are used to all functions having a result and all
// values being immutable.
// In this case, bmp is being manipulated but not returned. (in academic speak, setPixel is an impure function)
do setPixel myRed (49,99) canvas
do setPixel myRed (50,98) canvas
do setPixel myRed (51,99) canvas

// Save it as a png file - it will have three red dots
// even though we did NOT have code like "canvas <- setPixel myRed (49,99) canvas"
// the contents of canvas are still updated.
toPngFile "testfile.png" canvas
```
