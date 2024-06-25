# Dotenv.Extensions.Microsoft.Configuration

[![dotenv.core](https://img.shields.io/badge/.NET%20Standard-2.0-red)](https://github.com/MrDave1999/dotenv.core)
[![Nuget-Badges](https://buildstats.info/nuget/Dotenv.Extensions.Microsoft.Configuration)](https://www.nuget.org/packages/Dotenv.Extensions.Microsoft.Configuration/)

ENV configuration provider implementation for [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration).

This library adds extension methods for the [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) package, which can be accessed through the [IConfigurationBuilder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.iconfigurationbuilder) interface. So, this library is just a wrapper and behind the scenes what happens is that it uses the classes and methods from the [DotEnv.Core](https://github.com/MrDave1999/dotenv.core) project.

This project was created to integrate the [DotEnv.Core](https://www.nuget.org/packages/DotEnv.Core) package into the .NET configuration system.

Refer to the [API documentation](https://mrdave1999.github.io/dotenv.core/api/Microsoft.Extensions.Configuration.html).

## Installation

If you're want to install the package from Visual Studio, you must open the project/solution in Visual Studio, and open the console using the **Tools** > **NuGet Package Manager** > **Package Manager Console** command and run the install command:
```
Install-Package Dotenv.Extensions.Microsoft.Configuration
```
If you are making use of the dotnet CLI, then run the following in your terminal:
```
dotnet add package Dotenv.Extensions.Microsoft.Configuration
```

## Usage

The following example shows how to read the application configuration from ENV file.
```cs
using System;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        // Build a configuration object from ENV file.
        IConfiguration config = new ConfigurationBuilder()
            .AddEnvFile("appsettings.env", optional: true)
            .Build();

        // Get a configuration section.
        IConfigurationSection section = config.GetSection("Settings");

        // Read configuration values.
        Console.WriteLine($"Server: {section["Server"]}");
        Console.WriteLine($"Database: {section["Database"]}");
    }
}
```
To run this example, include an `appsettings.env` file with the following content in your project:
```.env
Settings__Server=example.com
Settings__Database=Northwind
```
It doesn't matter if your .env file is in the root directory of your project, the configuration provider will start searching from the current directory and go up the parent directories until it finds it.