compile:
	mcs src/ui/Program.cs src/backend/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -o:Program.exe && mv src/ui/Program.exe bin/main.exe
run: bin/main.exe
	mono bin/main.exe
