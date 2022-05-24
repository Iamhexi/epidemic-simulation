compile:
	mcs src/UI/main.cs src/backend/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll && mv main.exe bin/
run: main.exe
	mono bin/main.exe
