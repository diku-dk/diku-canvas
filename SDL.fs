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

let SDL_TEXTUREACCESS_STREAMING = 1
let SDL_PIXELFORMAT_ARGB8888 = 372645892u
let SDL_PIXELFORMAT_RGBA8888 = 373694468u
let SDL_PIXELFORMAT_ABGR8888 = 376840196u
let SDL_PIXELFORMAT_RGBA32 = if BitConverter.IsLittleEndian
                             then SDL_PIXELFORMAT_ABGR8888
                             else SDL_PIXELFORMAT_RGBA8888
let SDL_KEYDOWN = 0x300u
let SDL_KEYUP = 769u
// Define keycodes for SDL
// https://wiki.libsdl.org/SDLKeycodeLookup
let SDLK_ESCAPE = 27u
let SDLK_SPACE = 32u
let SDLK_RIGHT = 1073741903u
let SDLK_LEFT = 1073741904u
let SDLK_DOWN = 1073741905u
let SDLK_UP = 1073741906u
let SDL_QUIT = 0x100u

[<type:StructLayout(LayoutKind.Sequential)>]
type SDL_Keysym = {
    scancode: SDL_Scancode
    sym: uint32
    ``mod``: SDL_Keymod
    unicode: uint32
}
and SDL_Scancode =
| SDL_SCANCODE_ESCAPE = 41
and SDL_Keymod =
| KMOD_NONE = 0x0000

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
        new (t) =     { ``type`` = t ;
                       timestamp = 0u;
                       windowID = 0u;
                       state = 0uy;
                       repeat = 0uy;
                       padding2 = 0uy;
                       padding3 = 0uy;
                       keysym = { scancode = SDL_Scancode.SDL_SCANCODE_ESCAPE;
                                  sym = 0u;
                                  ``mod`` = SDL_Keymod.KMOD_NONE;
                                  unicode = 0u }}

    end


[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_Init(uint32 flags)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_CreateWindowAndRenderer (int width, int height, SDL_WindowFlags flags, IntPtr& window, IntPtr& renderer)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_SetWindowTitle (IntPtr window, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string title)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern uint32 SDL_GetTicks();

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_PollEvent(SDL_KeyboardEvent& _event)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_WaitEvent(SDL_KeyboardEvent& _event)


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
