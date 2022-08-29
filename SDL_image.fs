/// Wrappers and PInvoke of SDL2_image

module SDLImage

open System.Runtime.InteropServices
open System

[<Literal>]
let libName = "SDL2_image"


// https://wiki.libsdl.org/SDL_Surface
[<type:StructLayout(LayoutKind.Sequential)>]
type SDL_Surface =
    struct
        val flags : uint32
        val pixelFormat: IntPtr
        val w : int
        val h : int
        val pitch : int
        val pixels : IntPtr
        val userdata : IntPtr
        val locked : int
        val lock_data : IntPtr
        val clip_rect : IntPtr
        val map : IntPtr
        val refcount : int
        new (unused) = {
                  flags = 0u;
                  pixelFormat = IntPtr.Zero;
                  w = 0;
                  h = 0;
                  pitch = 0;
                  pixels = IntPtr.Zero;
                  userdata = IntPtr.Zero;
                  locked = 0;
                  lock_data = IntPtr.Zero;
                  clip_rect = IntPtr.Zero;
                  map = IntPtr.Zero;
                  refcount = 0;
                  
                 }
    end


// Currently not used, but might be valuable at some later point
[<type:StructLayout(LayoutKind.Sequential)>]
type SDL_PixelFormat =
    struct
        val palette : IntPtr
        val bitsPerPixel : uint8
        val bytesPerPixel : uint8
        val rMask : uint32
        val gMask : uint32
        val bMask : uint32
        val aMask : uint32
        val rLoss : uint8
        val gLoss : uint8        
        val bLoss : uint8
        val aLoss : uint8        
        val rShift : uint8
        val gShift : uint8        
        val bShift : uint8
        val aShift : uint8        
        val refcount : int
        val next : IntPtr
        new (unused) = {
                     palette = IntPtr.Zero;
                     bitsPerPixel = uint8 0;
                     bytesPerPixel = uint8 0;
                     rMask = 0u;
                     gMask = 0u;
                     bMask = 0u;
                     aMask = 0u;                     
                     rLoss = uint8 0;
                     gLoss = uint8 0;
                     bLoss = uint8 0;
                     aLoss = uint8 0;                     
                     rShift = uint8 0;
                     gShift = uint8 0;
                     bShift = uint8 0;
                     aShift = uint8 0;                     
                     refcount = 0;
                     next = IntPtr.Zero;
                     }
    end

// Only tested on linux
// I assume LPUTF8Str will work fine on MacOs
// But we might run into problems on windows
[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern int IMG_SavePNG(IntPtr surface, [<MarshalAs(UnmanagedType.LPUTF8Str)>] string file)


// We load images as Surfaces, then convert them to our own canvas type
// When saving to png, we need a surface, when rendering we need a texture
// The difference is that textures are stored in GPU-memory
[<DllImport(libName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr IMG_Load([<MarshalAs(UnmanagedType.LPUTF8Str)>] string file)

