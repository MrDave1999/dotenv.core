[![dotenv-logo](images/dotenv-logo.png)](https://github.com/mrdave1999/dotenv.core)

[![dotenv.core](https://img.shields.io/badge/.NET%20Standard-2.0-red)](https://github.com/mrdave1999/dotenv.core)
[![dotenv.core](https://img.shields.io/badge/License-MIT-green)](https://raw.githubusercontent.com/MrDave1999/dotenv.core/master/LICENSE)
[![dotenv.core](https://img.shields.io/badge/Project-Class%20Library-yellow)](https://github.com/mrdave1999/dotenv.core)
[![PayPal-donate-button](https://img.shields.io/badge/paypal-donate-orange)](https://www.paypal.com/paypalme/DavidRomanAmariles)

[![DotEnv.Core](https://img.shields.io/nuget/vpre/DotEnv.Core?label=DotEnv.Core%20-%20nuget&color=red)](https://www.nuget.org/packages/DotEnv.Core)
[![downloads](https://img.shields.io/nuget/dt/DotEnv.Core?color=yellow)](https://www.nuget.org/packages/DotEnv.Core)

[![DotEnv.Core.Props](https://img.shields.io/nuget/vpre/DotEnv.Core.Props?label=DotEnv.Core.Props%20-%20nuget&color=red)](https://www.nuget.org/packages/DotEnv.Core.Props)
[![downloads](https://img.shields.io/nuget/dt/DotEnv.Core.Props?color=yellow)](https://www.nuget.org/packages/DotEnv.Core.Props)

[![Dotenv.Microsoft.DI](https://img.shields.io/nuget/vpre/Dotenv.Extensions.Microsoft.DependencyInjection?label=Dotenv.Extensions.Microsoft.DependencyInjection%20-%20nuget&color=red)](https://www.nuget.org/packages/Dotenv.Extensions.Microsoft.DependencyInjection)
[![downloads](https://img.shields.io/nuget/dt/Dotenv.Extensions.Microsoft.DependencyInjection?color=yellow)](https://www.nuget.org/packages/Dotenv.Extensions.Microsoft.DependencyInjection)

**DotEnv.Core** is a class library for read and parsing .env files in .NET Core and also provides a mechanism to retrieve the value of an environment variable in a simple and easy way.

The advantage of using this library is that you do not need to set the environment variable from the operating system shell (**dotenv** sets environment variables from a .env file).

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

## Extensions

- [Dotenv.Extensions.Microsoft.DependencyInjection](https://mrdave1999.github.io/dotenv.core/extensions/microsoft_extensions_DI.html)

  - This package adds extension methods for [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection), which can be accessed from the `IServiceCollection` interface.

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

