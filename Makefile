
FSC=fsharpc --nologo
#macOS:
INCL=-I /Library/Frameworks/Mono.framework/Versions/Current/lib/mono/gtk-sharp-2.0
#Linux:
#INCL=-I /usr/lib/cli/gtk-sharp-2.0 -I /usr/lib/cli/gdk-sharp-2.0 -I /usr/lib/cli/glib-sharp-2.0 -I /usr/lib/cli/gtk-dotnet-2.0
GTKDLLS=-r gdk-sharp.dll -r gtk-sharp.dll

.PHONY: all
all: img_util.dll

.PHONY: examples
examples:
	$(MAKE) -C examples all

.PHONY: clean
clean:
	rm -rf *~ *.exe *.png *.dll
	$(MAKE) -C examples clean

%.exe: %.fs
	$(FSC) $<

img_util.dll: img_util.fsi img_util.fs
	$(FSC) $(INCL) $(GTKDLLS) -a $^
