# epidemic-simulation
## Detailed description
The application is C# object-oritented implementation of the epidemic simulation employing make building system. WinForms and XNA (FNA) frameworks were used to created graphical user interface. The user can set up different initial parameters of simulation including disease lethality, communicability, duration and simulated population. There are three different scenarios enriching the user experience: 1) single community simulation, 2) shopping community simulation and 3) multigroup community simulation. The user is able to observe the spread of the germ in real time as well as follow graphs depicting progress of the disease. At the very end of simulation, the file containg detalied statistics is being generated. The project has been created and developed as university project for the 'Object-Oriented Programming' course. It contains unit tests and automatically generated documentation thanks to doxygen.

## Available commands
- `make` - builds the entire project (application, tests and documentation)
- `make compile` - builds only the application
- `make run` - runs the application
- `make test` - compiles and runs unit tests
- `make compile_tests` - compiles only unit tests
- `make documentation` - updates the project documentation
- `make clean` - cleans the project's auxiliary and temporary files

## Requirements:
- C# implementation - dotnet or mono
- XNA or FNA framework
- make command
- doxygen command

## Information for lecturer
Project's github page is available at: https://github.com/Iamhexi/epidemic-simulation
Unit tests are present.
