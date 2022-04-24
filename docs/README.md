# Documentation

## Installation

### Windows

Install the latest version of [DocFX](https://github.com/dotnet/docfx/releases/latest).Once the file is downloaded and unzipped, you must add the path where docfx.exe is located in the PATH environment variable, in order to be able to run docfx.exe from any location.

If you do not know how to modify the PATH environment variable in Windows, it is very easy, go to the Start button, then type "Environment variable" and there you will be able to edit an environment variable. Here is a good [tutorial](https://www.architectryan.com/2018/08/31/how-to-change-environment-variables-on-windows-10/).

### Linux

You must also install the latest version of [DocFX](https://github.com/dotnet/docfx/releases/latest) but you must run it through [mono](https://www.mono-project.com/download/stable/#download-lin).

## Build

### Windows

Open cmd.exe and build documentation:
```cmd
docfx
```

### Linux

Open bash and build documentation:
```sh
./docfx.sh
```
You may need to grant permission to execute:
```sh
chmod u+x docfx.sh
```

## Deployment

### Windows

Start the server:
```cmd
docfx serve
```

### Linux

Start the server:
```sh
./docfx.sh serve
```

## Observation

Each command must be executed in the directory where the `docfx.json` file is located, otherwise, you must pass the path to DocFX.

### Examples in Windows:

**Build:**
```cmd
docfx C:\Program Files\MyApp\docs\docfx.json
```

**Deploy:**
```cmd
docfx serve C:\Program Files\MyApp\docs\docfx.json
```

### Examples in Linux:

**Build:**
```sh
./docfx.sh $HOME/MyApp/docs/docfx.json
```

**Deploy:**
```sh
./docfx.sh serve $HOME/MyApp/docs/docfx.json
```