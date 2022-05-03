# DocFX

## Installation

You must download the [memberpage](https://www.nuget.org/packages/memberpage/) package and then uncompresse the file in the `plugins/memberpage/` folder.

### Windows

Install the latest version of [DocFX](https://github.com/dotnet/docfx/releases/latest). Once the .zip file is downloaded and uncompressed, you must add the path where docfx.exe is located in the PATH environment variable, in order to be able to run docfx.exe from any location.

If you do not know how to modify the PATH environment variable in Windows, it is very easy, go to the Start button, then type "Environment variable" and there you will be able to edit an environment variable. Here is a good [tutorial](https://www.architectryan.com/2018/08/31/how-to-change-environment-variables-on-windows-10/).

### Linux

You must also install the latest version of [DocFX](https://github.com/dotnet/docfx/releases/latest). Uncompress the .ZIP file to the path: `$HOME/docfx` (if you do not have the docfx folder created, do so). 

Remember that DocFX is a .NET Framework application and it is not possible to run it on Linux, therefore, you must install [mono](https://www.mono-project.com/download/stable/#download-lin) to be able to run DocFX.

## Build/Deploy

### Windows

Open cmd.exe and build/deploy the DotEnv.Core site:
```cmd
docfx --serve
```

### Linux

Open bash and build/deploy the DotEnv.Core site:
```sh
./docfx.sh --serve
```
Go to https://localhost:8080 to view the DotEnv.Core site.

## Observation

Each command must be executed in the directory where the `docfx.json` file (i.e. in the `docs` folder) is located, otherwise, you must pass the path to DocFX.

### Example in Windows:

**Build/Deploy:**
```cmd
docfx C:\Program Files\MyApp\docs\docfx.json --serve
```

### Example in Linux:

**Build/Deploy:**
```sh
./docfx.sh $HOME/MyApp/docs/docfx.json --serve
```
