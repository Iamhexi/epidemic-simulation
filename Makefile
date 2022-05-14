compile:
	mcs src/UI/main.cs src/backend/*.cs -out:main.exe -pkg:mono-cairo -pkg:gtk-sharp-2.0
run: main.exe
	mono main.exe
