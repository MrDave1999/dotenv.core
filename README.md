# dotenv.core

[![dotenv-logo](https://raw.githubusercontent.com/MrDave1999/dotenv.core/master/docs/images/dotenv-logo.png)](https://github.com/mrdave1999/dotenv.core)

[![dotenv.core](https://img.shields.io/badge/.NET%20Standard-2.0-red)](https://github.com/mrdave1999/dotenv.core)
[![dotenv.core](https://img.shields.io/badge/License-MIT-green)](https://raw.githubusercontent.com/MrDave1999/dotenv.core/master/LICENSE)
[![dotenv.core](https://img.shields.io/badge/Project-Class%20Library-yellow)](https://github.com/mrdave1999/dotenv.core)
[![Nuget-Badges](https://buildstats.info/nuget/dotenv.core)](https://www.nuget.org/packages/dotenv.core/)
[![PayPal-donate-button](https://img.shields.io/badge/paypal-donate-orange)](https://www.paypal.com/paypalme/DavidRomanAmariles)


**dotenv.core** is a class library for read and parsing .env files in .NET Core and also provides a mechanism to retrieve the value of an environment variable in a simple and easy way.

The advantage of using this library is that you do not need to set the environment variable from the operating system shell (**dotenv** sets environment variables from a .env file).

- [Features](#features)
- [Basic Concepts](#basic-concepts)
  * [What is a .env file?](#what-is-a-env-file)
  * [What do .env files look like?](#what-do-env-files-look-like)
  * [What is environment variable?](#what-is-environment-variable)
- [Installation](#installation)
- [Overview](#overview)
  * [Load .env file](#load-env-file)
  * [Accessing environment variables](#accessing-environment-variables)
  * [Bind the model instance with the configuration keys](#bind-the-model-instance-with-the-configuration-keys)
  * [Load .env file without altering the environment](#load-env-file-without-altering-the-environment)
  * [Required Keys](#required-keys)
  * [Load .env file based on environment](#load-env-file-based-on-environment)
  * [Parsing .env files](#parsing-env-files)
  * [Using DotEnv in ASP.NET Core](#using-dotenv-in-aspnet-core)
- [Copying .env file to the output directory](#copying-env-file-to-the-output-directory)
- [Extensions](#extensions)
- [File Format](#file-format)
  * [Comments](#comments)
  * [Interpolating variables](#interpolating-variables)
  * [Export variables](#export-variables)
  * [Multiline values](#multiline-values)
- [Frequently Answered Questions](#frequently-answered-questions)
  * [Can I use an .env file in a production environment?](#can-i-use-an-env-file-in-a-production-environment)
  * [Should I commit my .env file?](#should-i-commit-my-env-file)
  * [Why is it not overriding existing environment variables?](#why-is-it-not-overriding-existing-environment-variables)
- [Contribution](#contribution)

## Features

- It has a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface), which makes it simple and easy to use.
- Support for load multiple .env files.
- Support to load the .env file depending on the environment (development, test, staging, or production).
- Searches in parent directories when it does not find the .env file in the current directory.
- You can set the base path for a set of .env files.
- You can define which keys should be required by the application.
- You can change the default .env file name, so it does not necessarily have to be `.env`.
- Support for the variables interpolation.
- And much more.

Don't forget to visit the official library [website](https://mrdave1999.github.io/dotenv.core) where you can find [API documentation](https://mrdave1999.github.io/dotenv.core/api/DotEnv.Core.html), [articles](https://mrdave1999.github.io/dotenv.core/articles/getting_started.html) and [diagrams](https://mrdave1999.github.io/dotenv.core/diagrams/class_diagram.html).

## Basic Concepts
### What is a .env file?

A .env file or dotenv file is a simple text configuration file for controlling your Applications environment constants.

### What do .env files look like?

.env files are line delimitated text files, meaning that each new line represents a single variable. By convention .env variable names are uppercase words separated by underscores. Variable names are followed directly by an = which, in turn is followed directly by the value, for example:
```bash
VARIABLE_NAME=value
```

### What is environment variable?

An environment variable is a dynamic variable that can affect the behavior of running processes on a computer. They are part of the environment in which a process runs.

## Installation

If you're want to install the package from Visual Studio, you must open the project/solution in Visual Studio, and open the console using the **Tools** > **NuGet Package Manager** > **Package Manager Console** command and run the install command:
```
Install-Package DotEnv.Core
```
If you are making use of the dotnet CLI, then run the following in your terminal:
```
dotnet add package DotEnv.Core
```

## Overview

The first thing you need to do is create a `.env` file in the root directory of your project.

### Load .env file

You must import the namespace types at the beginning of your class file:
```cs
using DotEnv.Core;
```

Then you can load the .env file with the `Load` method of the `EnvLoader` class:
```cs
new EnvLoader().Load();
```
By default, the `Load` method will search for a file called `.env` in the current directory and if it does not find it, it will search for it in the parent directories of the current directory. Generally, the current directory is where the executable (your application itself) with its dependencies is located.

Remember that if no encoding is specified to the `Load` method, the default will be `UTF-8`. Also, by default, the `Load` method does not overwrite the value of an existing environment variable.

You can also load your own .env file using the `AddEnvFile` method:
```cs
new EnvLoader()
   .AddEnvFile("config.env")
   .Load();
```
In this case, the `Load` method will search for a file called `config.env` instead of `.env`.

### Accessing environment variables

After you have loaded the .env file with the `Load` method, you can access the environment variables using the `EnvReader` class:
```cs
var reader = new EnvReader();
string value = reader["CONNECTION_STRING"];
int dbPort = reader.GetIntValue("DB_PORT");
```
Or you can also access the environment variables using the static property `Instance`:
```cs
string value = EnvReader.Instance["CONNECTION_STRING"];
int dbPort = EnvReader.Instance.GetIntValue("DB_PORT");
```
If you don't want to use the `EnvReader` class to access environment variables, you can use the `string` class:
```cs
string value = "CONNECTION_STRING".GetEnv();
int dbPort = "DB_PORT".GetEnv<int>();
```
You can also use the [Environment](https://learn.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable?view=net-7.0) class or the [Configuration API](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration) to access the environment variables. 

**Suggestion:** It is recommended to use constants to avoid hard-coded keys in the application logic. A complete example can be found [here](https://github.com/MrDave1999/dotenv.core/blob/master/example/Program.cs).

### Bind the model instance with the configuration keys

In case you do not want to use the `EnvReader` or `Environment` class, you can bind your own instance of the model with the keys of a .env file.

Create the model representing the setting class of the application:
```cs
class AppSettings
{
    [EnvKey("CONNECTION_STRING")]
    public string ConnectionString { get; set; }

    [EnvKey("SECRET_KEY")]
    public string SecretKey { get; set; }
}
```
The `EnvKey` attribute is used in case the key names do not match the properties and this is because the key names in a .env file usually follow this convention: `KEY_NAME=VALUE` (UpperCase + SnakeCase).

> Note: As of version 2.3.0, it is no longer necessary to use the `EnvKey` attribute, because the binder performs an additional step: It converts the property name to UpperCaseSnakeCase and then checks if it exists in the environment. Note that this additional step only occurs if the `EnvKey` attribute is not used.

Then call the `EnvBinder.Bind` method to bind the `AppSettings` class with the configuration keys:
```cs
new EnvLoader().Load();
var settings = new EnvBinder().Bind<AppSettings>();
string key1 = settings.ConnectionString;
string key2 = settings.SecretKey;
```

### Load .env file without altering the environment

You can load an .env file without having to modify the environment:
```cs
var envVars = new EnvLoader()
        .AvoidModifyEnvironment()
        .Load();

string key1 = envVars["KEY1"];
string key2 = envVars["KEY2"];
```
The `Load` method will return an instance that implements the `IEnvironmentVariablesProvider` interface and through this instance we can access the environment variables. In fact, the environment variables are obtained from a dictionary, instead of the current process.

### Required Keys

You can specify which keys should be required by the application, in case they are missing, throw an error:
```cs
new EnvValidator()
    .SetRequiredKeys("SERVICE_APP_ID", "SERVICE_KEY", "SERVICE_SECRET")
    .Validate();
```
The `Validate` method will check if the keys "SERVICE_APP_ID", "SERVICE_KEY", "SERVICE_SECRET" are present in the application, otherwise it throws an exception.

### Load .env file based on environment

You can load an .env file based on the environment (dev, test, staging or production) with the `LoadEnv` method. The environment is defined by the actual environment variable as `DOTNET_ENV`:
```cs
new EnvLoader().LoadEnv();
```
The `LoadEnv` method will search for these .env files in the following order:
- `.env.[environment].local` (has the highest priority)
- `.env.local`
- `.env.[environment]`
- `.env` (has the lowest priority)

The `environment` is specified by the actual environment variable `DOTNET_ENV`.

It should be noted that the default environment will be `development` or `dev` if the environment is never specified with `DOTNET_ENV`.

### Parsing .env files

You can analyze key-value pairs from any data source (a .env file, a database, a web service, etc):
```cs
string myDataSource = @"
    SERVICE_APP_ID=1
    SERVICE_KEY=1234$
    SERVICE_SECRET=1234example$
";
new EnvParser().Parse(myDataSource);
```
Then you can access the environment variables with the `EnvReader` or `System.Environment` class.

### Using DotEnv in ASP.NET Core

Open the `Startup.cs` file and add this code in the constructor:
```cs
new EnvLoader().Load();
Configuration = new ConfigurationBuilder()
          .AddEnvironmentVariables()
          .Build();
```
Once the environment variables have been set from an .env file, we call the `AddEnvironmentVariables` method to take care of adding the environment variables in the **configuration object** managed by **ASP.NET Core**. Then, the keys can be accessed with the `IConfiguration` interface, for example:
```cs
class HomeController
{
    public HomeController(IConfiguration configuration)
    {
        string key = configuration["KEY_NAME"];
    }
}
```
If you are using **ASP.NET Core 6**, you will not need to add anything in a `Startup.cs` file. Simply go to `Program.cs` and add the following code after the `WebApplication.CreateBuilder` method:
```cs
new EnvLoader().Load();
builder.Configuration.AddEnvironmentVariables();
```

For more information, see the [articles](https://mrdave1999.github.io/dotenv.core/articles/getting_started.html).

## Copying .env file to the output directory

If you want to copy the .env file to the output directory, you have to add the following to your .csproj file:
```xml
<ItemGroup>
  <Content Include=".env">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```
**NOTE:** Your .env file must be in the same directory as the .csproj file.

## Extensions

- [Dotenv.Extensions.Microsoft.DependencyInjection](https://github.com/MrDave1999/Dotenv.Extensions.Microsoft.DependencyInjection)

  - This package adds extension methods for [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection), which can be accessed from the `IServiceCollection` interface.

## File Format

- Empty lines or lines with white-spaces will be ignored.
- The key-value format must be as follows: `KEY=VAL`.
- Single or double quotes in a value are removed.
- If the value of a key is an empty string, it will be converted to a white-space.
- White-spaces at both ends of the key and value are ignored.

### Comments

Each line beginning with the `#` character is a comment. White-spaces at the beginning of each comment will be ignored.

Example:
```bash
# This is a comment without white-spaces
   # This is a comment with white-spaces
KEY=VALUE
```
A comment may begin anywhere on a line after a space (inline comments):
```bash
KEY=VALUE # This is an inline comment
VAR=VALUE    # This is another inline comment
```

### Interpolating variables

Sometimes you will need to interpolate variables within a value, for example:
```bash
MYSQL_USER=root
MYSQL_ROOT_PASSWORD=1234
CONNECTION_STRING=username=${MYSQL_USER};password=${MYSQL_ROOT_PASSWORD};database=testdb;
```
If the variable embedded in the value is not set, the parser will throw an exception, for example:
```bash
MYSQL_ROOT_PASSWORD=1234
CONNECTION_STRING=username=${MYSQL_USER};password=${MYSQL_ROOT_PASSWORD};database=testdb;
MYSQL_USER=root
```
In the above example, the parser should throw an exception because the `MYSQL_USER` variable is not set.

### Export variables

Lines can start with the `export` prefix, which has no effect on their interpretation.
```bash
export VAR=VALUE
export KEY=VALUE
```
The `export` prefix makes it possible to export environment variables from a file using the `source` command:
```bash
source .env
```

### Multiline values

It is possible for single- or double-quoted values to span multiple lines. The following examples are equivalent:
```bash
KEY="first line
second line"

VAR='first line
second line'
```

```bash
KEY="first line\nsecond line"
VAR='first line\nsecond line'
```

## Frequently Answered Questions

### Can I use an `.env file` in a production environment?

Generally, you should not add sensitive data (such as passwords) to a .env file, as it would be unencrypted! Instead, you could use a secrets manager such as [Azure Key Vault](https://docs.microsoft.com/en-us/azure/key-vault/general/basic-concepts) or [AWS Secrets Manager](https://docs.aws.amazon.com/secretsmanager/latest/userguide/intro.html).

If you are going to use .env files in production, make sure you have good security at the infrastructure level and also grant read/write permissions to a specific user (such as admin), so that not just anyone can access your .env file.

### Should I commit my .env file?

Credentials should only be accessible on the machines that need access to them. Never commit sensitive information to a repository that is not needed by every development machine.

### Why is it not overriding existing environment variables?

By default, it won't overwrite existing environment variables as dotenv assumes the deployment environment has more knowledge about configuration than the application does.

## Contribution

Any contribution is welcome, the **parser** is still VERY dumb, so if you can improve it, do it.

Follow the steps below:

1. Fork it
2. Create your feature branch (git checkout -b my-new-feature)
3. Commit your changes (git commit -am 'Added some feature')
4. Push to the branch (git push origin my-new-feature)
5. Create new [Pull Request](https://github.com/MrDave1999/dotenv.core/pulls)
