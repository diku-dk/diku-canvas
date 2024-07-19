{ pkgs ? import <nixpkgs> {} }:

with pkgs;
buildDotnetModule rec {
  pname = "DIKU.Canvas";
  version = "2.0.4";

  src = ./.;

  projectFile = "canvas.fsproj";
  nugetDeps = ./deps.nix;

  projectReferences = [];

  dotnet-sdk = dotnetCorePackages.sdk_6_0;
  dotnet-runtime = dotnetCorePackages.runtime_6_0;

  executables = [];

  packNupkg = true;
  
  runtimeDeps = [
    SDL2
  ];

  buildInputs = [
    SDL2_ttf   
    stdenv
    libglvnd
  ] ++ (with xorg; [ libX11 libXext libXinerama libXi libXrandr ]);
  
  LD_LIBRARY_PATH = with xorg; "${libX11}/lib:${libXext}/lib:${libXinerama}/lib:${libXi}/lib:${libXrandr}/lib:${libglvnd}/lib";
}
