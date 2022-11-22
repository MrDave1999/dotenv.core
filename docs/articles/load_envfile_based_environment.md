# Load .env file based on environment

## Introduction

You can load an .env file based on the environment (dev, test, staging or production) with the `LoadEnv` method. The environment is defined by the actual environment variable as `DOTNET_ENV`:
```cs
System.Environment.SetEnvironmentVariable("DOTNET_ENV", "test");
new EnvLoader().LoadEnv();
```
The `LoadEnv` method will search for these .env files in the following order:
- `.env.[environment].local` (has the highest priority)
- `.env.local`
- `.env.[environment]`
- `.env` (has the lowest priority)

The `environment` is specified by the actual environment variable `DOTNET_ENV`.

It should be noted that the default environment will be `development` or `dev` if the environment is never specified with `DOTNET_ENV`.

## Concepts

- `.env`: defines the default values for all environments and machines.
- `.env.local`: defines the configuration values for all environments but only on the machine which contains the file. This file should not be committed to the repository.
- `.env.[environment]` (e.g. `.env.test`): defines the default values for one environment, but for all machines (these files are committed).
- `.env.[environment].local` (e.g. `.env.test.local`): defines configuration values that are machine-specific but only for one environment.

Real environment variables always win over env vars created by any of the `.env` files.

The `.env` and `.env.[environment]` files should be committed to the repository because they are the same for all developers and machines. However, the env files ending in `.local` (`.env.local` and `.env.[environment].local`) should not be committed because only you will use them. 

## Functioning

The behavior of the `LoadEnv` method is simple. Imagine that you open the shell in Linux and type the following command:
```sh
export DOTNET_ENV=production;
```
Then we run the application and the following code is executed:
```cs
new EnvLoader().LoadEnv();
```
The `LoadEnv` method loads the following .env files:
- .env.production.local
- .env.local
- .env.production
- .env

In this example, at least file `.env.production.local` or `.env.local` must be present, otherwise the `LoadEnv` method generates an error. There must be at least one local file.

## Configuration option

The `SetEnvironmentName` method can be used to set the environment name from source code:
```cs
new EnvLoader()
    .SetEnvironmentName("test")
    .LoadEnv();
```
If the actual environment variable `DOTNET_ENV` is set, the `SetEnvironmentName` method will have no effect, because the `LoadEnv` method will give higher priority to the value of `DOTNET_ENV`.

## Error handling

You can handle the error without throwing an exception by means of the `EnvValidationResult` class:
```cs
new EnvLoader()
    // To ignore the exception thrown by the parser.
    .IgnoreParserException()
    .LoadEnv(out EnvValidationResult result);

if(result.HasError())
{
    string msg = result.ErrorMessages;
    System.Console.WriteLine(msg);
}
else
{
    // Execute some action when there is no error.
}
```

## Helper methods

The `Env` class has helper methods to check if the **current environment** is development, test, staging, or production. The **current environment** is defined by the actual environment variable `DOTNET_ENV`.

Example:
```cs
// Equivalent to: Env.CurrentEnvironment = "test";
System.Environment.SetEnvironmentVariable("DOTNET_ENV", "test");

System.Console.WriteLine(Env.IsDevelopment());
System.Console.WriteLine(Env.IsTest());
System.Console.WriteLine(Env.IsStaging());
System.Console.WriteLine(Env.IsProduction());
System.Console.WriteLine(Env.IsEnvironment("test"));
// The example displays the following output to the console:
// false
// true
// false
// false
// true
```