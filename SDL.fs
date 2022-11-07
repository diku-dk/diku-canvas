﻿/// Wrappers and PInvoke of SDL2
/// Originally taken from https://github.com/ChrisPritchard/tiny-ray-caster
/// tiny-ray-caster uses Unlicense License
/// parts from tiny-ray-case used with permission. (Obtained by Mads Obitsø via email)
/// Extensions and modifications made by Ken Friis Larsen and Mads Obitsø
module SDL

open System.Runtime.InteropServices
open System

[<Literal>]
let libName = "SDL2"

let SDL_INIT_VIDEO = 0x00000020u

type SDL_WindowFlags =
| SDL_WINDOW_SHOWN = 0x00000004
| SDL_WINDOW_INPUT_FOCUS = 0x00000200

let SDL_WINDOWPOS_CENTERED = 0x2FFF0000u

let SDL_TEXTUREACCESS_STREAMING = 1
let SDL_PIXELFORMAT_ARGB8888 = 372645892u
let SDL_PIXELFORMAT_RGBA8888 = 373694468u
let SDL_PIXELFORMAT_ABGR8888 = 376840196u
let SDL_PIXELFORMAT_RGBA32 = if BitConverter.IsLittleEndian
                             then SDL_PIXELFORMAT_ABGR8888
                             else SDL_PIXELFORMAT_RGBA8888
let SDL_KEYDOWN = 0x300u
let SDL_KEYUP = 0x301u
let SDL_MOUSEMOTION = 0x400u
let SDL_MOUSEBUTTONDOWN = 0x401u
let SDL_MOUSEBUTTONUP = 0x402u
// Define keycodes for SDL
// https://wiki.libsdl.org/SDLKeycodeLookup
let SDLK_ESCAPE = 27u
let SDLK_SPACE = 32u
let SDLK_RIGHT = 1073741903u
let SDLK_LEFT = 1073741904u
let SDLK_DOWN = 1073741905u
let SDLK_UP = 1073741906u

let SDL_QUIT = 0x100u
let SDL_USEREVENT = 0x8000u

[<type:StructLayout(LayoutKind.Sequential)>]
type SDL_Keysym =
    struct
        val scancode: int32
        val sym: uint32
        val ``mod``: uint16
        val unicode: uint32
    end

let SDL_SCANCODE_ESCAPE : int32 = 41
let KMOD_NONE : uint16 = 0x0000us

[<type:StructLayout(LayoutKind.Sequential)>]
type SDL_KeyboardEvent =
    struct
        val ``type``: uint32
        val timestamp: uint32
        val windowID: uint32
        val state: byte
        val repeat: byte
        val private padding2: byte
        val private padding3: byte
        val keysym: SDL_Keysym
    end

[<StructLayout(LayoutKind.Sequential)>]
type SDL_UserEvent=
    struct
        val ``type``: uint32
        val timestamp: uint32
        val windowID: uint32
        val code: int32
        val data1: IntPtr
        val data2: IntPtr
    end

[<StructLayout(LayoutKind.Sequential)>]
type SDL_MouseButtonEvent=
    struct
        val ``type``: uint32
        val timestamp: uint32
        val windowID: uint32
        val which: uint32
        val button: byte
        val state: byte
        val clicks: byte
        val padding1: byte
        val x: int32
        val y: int32
    end

[<StructLayout(LayoutKind.Sequential)>]
type SDL_MouseMotionEvent=
    struct
        val ``type``: uint32
        val timestamp: uint32
        val windowID: uint32
        val which: uint32
        val state: uint32
        val x: int32
        val y: int32
        val xrel: int32
        val yrel: int32
    end

// Trick to encode a C union type
#nowarn "9"
[<StructLayout(LayoutKind.Explicit, Size=56)>]
type SDL_Event =
    struct
        [<FieldOffset(0)>]
        val mutable ``type``: uint32
        [<FieldOffset(0)>]
        val key: SDL_KeyboardEvent
        [<FieldOffset(0)>]
        val user: SDL_UserEvent
        [<FieldOffset(0)>]
        val button: SDL_MouseButtonEvent
        [<FieldOffset(0)>]
        val motion: SDL_MouseMotionEvent
    end

type Event =
    | Quit
    | KeyDown of SDL_KeyboardEvent
    | KeyUp of SDL_KeyboardEvent
    | MouseButtonDown of SDL_MouseButtonEvent
    | MouseButtonUp of SDL_MouseButtonEvent
    | MouseMotion of SDL_MouseMotionEvent
    | User of SDL_UserEvent
    | Raw of SDL_Event

let convertEvent (event: SDL_Event) =
    match event.``type`` with
        | c when c = SDL_QUIT -> Quit
        | c when c = SDL_KEYDOWN -> event.key |> KeyDown
        | c when c = SDL_KEYUP -> event.key |> KeyUp
        | c when c >= SDL_USEREVENT -> event.user |> User
        | c when c = SDL_MOUSEBUTTONDOWN -> event.button |> MouseButtonDown
        | c when c = SDL_MOUSEBUTTONUP -> event.button |> MouseButtonUp
        | c when c = SDL_MOUSEMOTION -> event.motion |> MouseMotion
        | _ -> event |> Raw

type SDL_RendererFlags =
    | SDL_RENDERER_SOFTWARE      = 0x00000001
    | SDL_RENDERER_ACCELERATED   = 0x00000002
    | SDL_RENDERER_PRESENTVSYNC  = 0x00000004
    | SDL_RENDERER_TARGETTEXTURE = 0x00000008


[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_Init(uint32 flags)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_CreateWindowAndRenderer (int width, int height, SDL_WindowFlags flags, IntPtr& window, IntPtr& renderer)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr SDL_CreateWindow([<MarshalAs(UnmanagedType.LPUTF8Str)>] string title,
                               uint x, uint y, int width, int height, SDL_WindowFlags flags)


[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr SDL_CreateRenderer(IntPtr window, int index, SDL_RendererFlags flags)


[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_SetWindowTitle (IntPtr window, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string title)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern uint32 SDL_GetTicks();

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_PollEvent(SDL_Event& _event)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_WaitEvent(SDL_Event& _event)

(* Allocate a set of user-defined events *)
[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern uint32 SDL_RegisterEvents(int numevents)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_PushEvent(SDL_Event& _event)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr SDL_CreateTexture (IntPtr renderer, uint32 format, int access, int width, int height)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_UpdateTexture(IntPtr texture, IntPtr rect, IntPtr pixels, int pitch);

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_RenderClear(IntPtr renderer)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_RenderCopy(IntPtr renderer, IntPtr texture, IntPtr srcrect, IntPtr destrect);

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_RenderPresent(IntPtr renderer)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_DestroyTexture(IntPtr texture)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_DestroyRenderer(IntPtr renderer)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_DestroyWindow(IntPtr window)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_Quit()

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr SDL_CreateRGBSurfaceFrom (IntPtr pixels, int width, int height, int depth, int pitch, uint32 Rmask, uint32 Gmask, uint32 Bmask, uint32 Amask)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_FreeSurface(IntPtr surface)
