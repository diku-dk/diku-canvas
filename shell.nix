{ pkgs ? import <nixpkgs> {} }:

with pkgs;
mkShell {
  nativeBuildInputs =
    with buildPackages; [
      dotnetCorePackages.sdk_8_0
      fsautocomplete
      SDL2
      SDL2_ttf   
      stdenv
      libglvnd
    ] ++ (with xorg; [ libX11 libXext libXinerama libXi libXrandr ]);
  
  LD_LIBRARY_PATH =
    with xorg; "${libX11}/lib:${libXext}/lib:${libXinerama}/lib:${libXi}/lib:${libXrandr}/lib:${libglvnd}/lib:${pkgs.SDL2}/lib";
}
