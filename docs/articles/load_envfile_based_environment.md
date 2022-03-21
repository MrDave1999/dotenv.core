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

The behavior of the `LoadEnv` method is simple. Imagine that you open the shell in Linux and type the following command:
```bash
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
    .IgnoreParserException() // To ignore the exception thrown by the parser.
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

System.Console.WriteLine(Env.IsDevelopment()); // output: false
System.Console.WriteLine(Env.IsTest()); // output: true
System.Console.WriteLine(Env.IsStaging()); // output: false
System.Console.WriteLine(Env.IsProduction()); // output: false
System.Console.WriteLine(Env.IsEnvironment("test")); // output: true
```