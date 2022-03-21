[![dotenv-logo](images/dotenv-logo.png)](https://github.com/mrdave1999/dotenv.core)

[![dotenv.core](https://img.shields.io/badge/.NET%20Standard-2.0-red)](https://github.com/mrdave1999/dotenv.core)
[![dotenv.core](https://img.shields.io/badge/License-MIT-green)](https://raw.githubusercontent.com/MrDave1999/dotenv.core/master/LICENSE)
[![dotenv.core](https://img.shields.io/badge/Project-Class%20Library-yellow)](https://github.com/mrdave1999/dotenv.core)
[![Nuget-Badges](https://buildstats.info/nuget/dotenv.core)](https://www.nuget.org/packages/dotenv.core/)

**dotenv.core** is a class library for read and parsing .env files in .NET Core and also provides a mechanism to retrieve the value of an environment variable in a simple and easy way.

## Features

- It has a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface), which makes it simple and easy to use.
- Support for load multiple .env files.
- Searches in parent directories when it does not find the .env file in the current directory.
- You can set the base path for a set of .env files.
- You can change the default .env file name, so it does not necessarily have to be `.env`.
- Support for the variables interpolation.

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

## File Format

- Empty lines or lines with white-spaces will be ignored.
- The key-value format must be as follows: `KEY=VAL`.
- There is no special handling of quotation marks. This means that **they are part of the VAL.**
- If the value of a key is an empty string, it will be converted to a white-space.
- White-spaces at both ends of the key and value are ignored.

### Comments

Each line beginning with the `#` character is a comment. White-spaces at the beginning of each comment will be ignored.

Example:
```
# comment without white spaces
   # comment with white spaces
KEY=VALUE
```

### Interpolating variables

Sometimes you will need to interpolate variables within a value, for example:
```
MYSQL_USER=root
MYSQL_ROOT_PASSWORD=1234
CONNECTION_STRING=username=${MYSQL_USER};password=${MYSQL_ROOT_PASSWORD};database=testdb;
```
If the variable embedded in the value is not set, the parser will throw an exception, for example:
```
CONNECTION_STRING=username=${MYSQL_USER};password=${MYSQL_ROOT_PASSWORD};database=testdb;
MYSQL_USER=root
MYSQL_ROOT_PASSWORD=1234
```
In the above example, the parser should throw an exception because the `MYSQL_USER` variable is not set.

## Deployment in Production

In production, you should not add sensitive data (such as passwords) to an .env file, as it would be unencrypted! Instead, you should use a secrets manager such as [Azure Key Vault](https://docs.microsoft.com/en-us/azure/key-vault/general/basic-concepts) or [AWS Secrets Manager](https://docs.aws.amazon.com/secretsmanager/latest/userguide/intro.html).

