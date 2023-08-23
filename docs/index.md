---
title: Getting started
category: Examples
categoryindex: 1
index: 1
---

# DIKU.Canvas

This library features a number of utility functions for drawing simple
2D graphics on a canvas, including features for running simple
user-interactive apps that display such canvas


## How to use Canvas in a F# script (.fsx)

Make an F# script, say `myFirstCanvas.fsx` with a NuGet reference:

    #r "nuget:DIKU.Canvas, 2.0"
    open Canvas

    let w,h = 256,256
    let tree = filledRectangle green ((float w)/2.0) ((float h)/2.0)
    let draw = fun _ -> make tree
    render "My first canvas" w h draw

Run it from the commandline using the command:

    [lang=bash]
    dotnet fsi myFirstCanvas.fsx

This should result in a window with a green square in the top left corner on a black background.

If you want a specific version you edit the reference to be, e.g.,:

    #r "nuget:DIKU.Canvas, 2.0.1"


## How to use Canvas in a F# project (that uses .fsproj)

Make an new directory, say `mycanvasapp`, in that directory start a F#
"Console App" project with the command:

    [lang=bash]
    dotnet new console -lang "F#"

(This will give you both a `Program.fs` file and a `mycanvasapp.fsproj` file.)

Add a reference to the `DIKU.Canvas` package with the command:

    [lang=bash]
    dotnet add package DIKU.Canvas

Edit `Program.fs` to have the content:

    open Canvas

    let w,h = 256,256
    let tree = filledRectangle green ((float w)/2.0) ((float h)/2.0)
    let draw = fun _ -> make tree
    render "My first canvas" w h draw

Run your app with the command:

    [lang=bash]
    dotnet run

This should result in a window with a green square in the top left corner on a black background.
