.PHONY: all
all:
	@echo "Possible targets:"
	@echo "  - make test"
	@echo "  - make clean"

.PHONY: test
test: quicktest.exe
	mono ./$<

.PHONY: clean
clean:
	rm -rf *.exe *.dll *~ testfile*.png

img_util.dll: img_util.fsi img_util.fs
	fsharpc --nologo -a $^

quicktest.exe: img_util.dll quicktest.fsx
	fsharpc --nologo -r $^
