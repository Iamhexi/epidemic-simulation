compile:
	mcs src/ui/*.cs src/backend/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:bin/main.exe
run: bin/main.exe
	mono bin/main.exe
