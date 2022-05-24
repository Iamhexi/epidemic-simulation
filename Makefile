compile:
	mcs src/UI/main.cs src/backend/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -o:main.exe && mv src/UI/main.exe bin/main.exe
run: bin/main.exe
	mono bin/main.exe
