/// Wrappers and PInvoke of SDL2
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
let SDL_KEYUP = 0x301u
let SDL_MOUSEMOTION = 0x400u;
let SDL_MOUSEBUTTONDOWN = 0x401u;
let SDL_MOUSEBUTTONUP = 0x402u;

// Define keycodes for SDL
// https://wiki.libsdl.org/SDLKeycodeLookup
let SDLK_ESCAPE = 27u
let SDLK_SPACE = 32u
let SDLK_RIGHT = 1073741903u
let SDLK_LEFT = 1073741904u
let SDLK_DOWN = 1073741905u
let SDLK_UP = 1073741906u
let SDL_QUIT = 0x100u

[<type:StructLayout(LayoutKind.Sequential, Size=16)>]
type SDL_Keysym = {
    scancode: uint32 //SDL_Scancode
    sym: uint32
    ``mod``: uint32 //SDL_Keymod
    unicode: uint32
}
and SDL_Scancode =
| SDL_SCANCODE_ESCAPE = 41
and SDL_Keymod =
| KMOD_NONE = 0x0000


// dotnet is not fond of the SDL_Event which is a union of different structs
// So instead, we write this beautiful struct of chunks
// And process the individual bytes into records
[<type:StructLayout(LayoutKind.Explicit, Size=32)>]
type SDL_Event =
    struct
        [<FieldOffset(0)>]
        val ``type``: uint32
        [<FieldOffset(4)>]
        val timestamp: uint32
        [<FieldOffset(8)>]
        val chunk1: uint32
        [<FieldOffset(12)>]
        val chunk2: uint32
        [<FieldOffset(16)>]
        val chunk3: uint32
        [<FieldOffset(20)>]
        val chunk4: uint32
        [<FieldOffset(24)>]
        val chunk5: uint32
        [<FieldOffset(28)>]
        val chunk6: uint32
        [<FieldOffset(32)>]
        val chunk7: uint32
        [<FieldOffset(36)>]
        val chunk8: uint32
        [<FieldOffset(40)>]
        val chunk9: uint32
        [<FieldOffset(44)>]
        val chunk10: uint32
        [<FieldOffset(48)>]
        val chunk11: uint32
        [<FieldOffset(52)>]
        val chunk12: uint32
        
        new (t) = {
                    ``type`` = t;
                    timestamp = 0u;
                    chunk1 = 0u;
                    chunk2 = 0u;
                    chunk3 = 0u;
                    chunk4 = 0u;
                    chunk5 = 0u;
                    chunk6 = 0u;
                    chunk7 = 0u;
                    chunk8 = 0u;
                    chunk9 = 0u;
                    chunk10 = 0u;
                    chunk11 = 0u;
                    chunk12 = 0u;
                  }
    end


type SDL_KeyboardEvent =
    {
        ``type``: uint32
        timestamp: uint32
        windowID: uint32
        state: byte
        repeat: byte
        padding2: byte
        padding3: byte
        keysym: SDL_Keysym
    }

type SDL_MouseButtonEvent =
    {
        ``type``: uint32
        timestamp: uint32
        windowID: uint32
        which: uint32
        button: byte
        state: byte
        clicks: byte
        padding1: byte
        x: int32
        y: int32
    }


type SDL_MouseMotionEvent =
    {
        ``type``: uint32
        timestamp: uint32
        windowID: uint32
        which: uint32
        state: uint32
        x: int32
        y: int32
        xrel: int32
        yrel: int32
    }
    

    

let toKeyboardEvent (event:SDL_Event) =    
    { ``type`` = event.``type``;
      timestamp = event.timestamp;
      windowID = event.chunk1;
      state = byte (event.chunk2 &&& 255u);
      repeat = byte ((event.chunk2 &&& 0xff00u) >>> 8);
      padding2 = 0uy;
      padding3 = 0uy;
      keysym = {
                   scancode = event.chunk3;
                   sym = event.chunk4;
                   ``mod`` = event.chunk5 &&& 0xFFFFu;
                   unicode = event.chunk6;
               }
    }


let toMouseButtonEvent (event:SDL_Event) =    
    { ``type`` = event.``type``;
      timestamp = event.timestamp;
      windowID = event.chunk1;
      which = event.chunk2;
      button = byte (event.chunk3 &&& 255u);
      state = byte ((event.chunk3 &&& 0xff00u) >>> 8);
      clicks = byte ((event.chunk3 &&& 0xff0000u) >>> 16);
      padding1 = 0uy;
      x = int event.chunk4;
      y = int event.chunk5;      
    }

let toMouseMotionEvent (event:SDL_Event) =    
    { ``type`` = event.``type``;
      timestamp = event.timestamp;
      windowID = event.chunk1;
      which = event.chunk2;
      state = event.chunk3;
      x = int event.chunk4;
      y = int event.chunk5;
      xrel = int event.chunk6;
      yrel = int event.chunk7;
    }



[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_Init(uint32 flags)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int SDL_CreateWindowAndRenderer (int width, int height, SDL_WindowFlags flags, IntPtr& window, IntPtr& renderer)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern unit SDL_SetWindowTitle (IntPtr window, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string title)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern uint32 SDL_GetTicks();

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
// extern int SDL_PollEvent(SDL_KeyboardEvent& _event)
extern int SDL_PollEvent(SDL_Event& _event)

[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
// extern int SDL_WaitEvent(SDL_KeyboardEvent& _event)
extern int SDL_WaitEvent(SDL_Event& _event)

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
