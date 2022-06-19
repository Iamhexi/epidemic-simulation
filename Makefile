build:
	mcs src/ui/*.cs src/backend/*.cs vendor/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:bin/main.exe && doxygen && cd latex && make && mv refman.pdf ../documentation.pdf
compile:
	mcs src/ui/*.cs src/backend/*.cs vendor/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:bin/main.exe
run: bin/main.exe
	mono bin/main.exe
documentation:
	doxygen && cd latex && make && mv refman.pdf ../documentation.pdf
compile_tests:
	mcs src/backend/*.cs src/ui/ChartManager.cs vendor/*.cs test/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:bin/test.exe
test: compile_tests
	mono bin/test.exe
make clean:
	rm log/* latex html bin/*.exe -rf
