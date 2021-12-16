# dotenv.core

[![dotenv-logo](https://raw.githubusercontent.com/MrDave1999/dotenv.core/main/docs/images/dotenv-logo.png)](https://github.com/mrdave1999/dotenv.core)

[![dotenv.core](https://img.shields.io/badge/.NET%20Standard-2.0-red)](https://github.com/mrdave1999/dotenv.core)
[![dotenv.core](https://img.shields.io/badge/License-MIT-green)](https://raw.githubusercontent.com/MrDave1999/dotenv.core/main/LICENSE)
[![dotenv.core](https://img.shields.io/badge/Project-Class%20Library-yellow)](https://github.com/mrdave1999/dotenv.core)
[![Nuget-Badges](https://buildstats.info/nuget/dotenv.core)](https://www.nuget.org/packages/dotenv.core/)


**dotenv.core** is a class library for read .env files and also provides a mechanism to retrieve the value of an environment variable in a simple and easy way.

## Features

- It has a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface), which makes it simple and easy to use.
- Support for load multiple .env files.
- Searches in parent directories when it does not find the .env file in the current directory.
- You can customize the parser algorithm through inheritance.
- You can set the base path for a set of .env files.
- You can change the default .env file name, so it does not necessarily have to be `.env`.

Don't forget to visit the official library [website](https://mrdave1999.github.io/dotenv.core) where you can find [API documentation](https://mrdave1999.github.io/dotenv.core/api/DotEnv.Core.html), [articles](https://mrdave1999.github.io/dotenv.core/articles/getting_started.html) and [diagrams](https://mrdave1999.github.io/dotenv.core/diagrams/class_diagram.html).

## Basic Concepts
### What is a .env file?

A .env file or dotenv file is a simple text configuration file for controlling your Applications environment constants.

### What do .env files look like?

.env files are line delimitated text files, meaning that each new line represents a single variable. By convention .env variable names are uppercase words separated by underscores. Variable names are followed directly by an = which, in turn is followed directly by the value, for example:
```
VARIABLE_NAME=value
```

### What is environment variable?

An environment variable is a dynamic variable that can affect the behavior of running processes on a computer. They are part of the environment in which a process runs.

## Installation

If you're an hardcore and want to do it manually, you must add the following to the `csproj` file:
```xml
<PackageReference Include="DotEnv.Core" Version="1.0.0" />
```
If you're want to install the package from Visual Studio, you must open the project/solution in Visual Studio, and open the console using the **Tools** > **NuGet Package Manager** > **Package Manager Console** command and run the install command:
```
Install-Package DotEnv.Core
```
If you are making use of the dotnet CLI, then run the following in your terminal:
```
dotnet add package DotEnv.Core
```

## Usage

You must import the namespace types at the beginning of your class file:
```cs
using DotEnv.Core;
```

Then you can load the .env file with the `Load` method of the `EnvLoader` class:
```cs
new EnvLoader().Load();
```
By default, the `Load` method will look for a file called `.env` in the current directory and if it does not find it, it will look for it in the parent directories of the current directory.

The current directory is where the executable with its dependencies is located.

Remember that if no encoding is specified to the `Load` method, the default will be `UTF-8`.

After you have loaded the .env file with the `Load` method, you can access the environment variables using the indexer of the `EnvReader` class:
```cs
var reader = new EnvReader();
string key1 = reader["KEY1"];
string key2 = reader["KEY2"];
```
Or you can also access the environment variables using the static property `Instance`:
```cs
string key1 = EnvReader.Instance["KEY1"];
string key2 = EnvReader.Instance["KEY2"];
```
For more information, see the [articles](https://mrdave1999.github.io/dotenv.core/articles/getting_started.html).

## Deployment in Production

In production, you should not add sensitive data (such as passwords) to an .env file, as it would be unencrypted! Instead, you should use a secrets manager such as [Azure Key Vault](https://docs.microsoft.com/en-us/azure/key-vault/general/basic-concepts) or [AWS Secrets Manager](https://docs.aws.amazon.com/secretsmanager/latest/userguide/intro.html).

## Contribution

If you want to contribute in this project, simply fork the repository, make changes and then create a [pull request](https://github.com/MrDave1999/dotenv.core/pulls).