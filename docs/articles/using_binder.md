# Bind the model instance with the configuration keys

## Introduction

Start by creating the model that will represent the application configuration:
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

In case the key names of a .env file match the properties, then it is not necessary to use the attribute (*or decorator*):
```cs
class AppSettings
{
    public string ConnectionString { get; set; }
    public string SecretKey { get; set; }
}
```
Then call the `Bind` method to bind the `AppSettings` class to the keys of a .env file:
```cs
// You must first load the .env file.
new EnvLoader().Load();
var settings = new EnvBinder().Bind<AppSettings>();
string key1 = settings.ConnectionString;
string key2 = settings.SecretKey;
```
The `AppSettings` class must follow the following rules:
- Each property must be `public`.
- Each property must be read-write.

## Observation

If you use the `EnvBinder` class, then you no longer need to use the [EnvValidator](required_keys.md) class, and this is because the `EnvBinder.Bind` method already throws an exception in case the key is not present in the application, so it would be redundant to use both classes.

## Configuration options

### IgnoreException

It tells the `Bind` method not to throw an exception when it encounters one or more errors:
```cs
var settings = new EnvBinder()
    .IgnoreException()
    .Bind<AppSettings>();
```

### AllowBindNonPublicProperties

It tells the `Bind` method that it can bind non-public properties:
```cs
class AppSettings
{
    public string ConnectionString { get; set; }
    internal string SecretKey { get; set; }
}

var settings = new EnvBinder()
    .AllowBindNonPublicProperties()
    .Bind<AppSettings>();
```

## Error handling

You can handle the error through the `EnvValidationResult` class instead of throwing an exception:
```cs
new EnvLoader().Load();
var settings = new EnvBinder()
    // To ignore the exception thrown by the binder.
    .IgnoreException()
    .Bind<AppSettings>(out EnvValidationResult result);

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

## Injecting a Provider

The `EnvBinder` class is flexible, it adapts to any provider, so you can use it to bind the model instance to any environment variables provider (such as the current environment or a dictionary).

Example:
```cs
var envVars = new EnvLoader()
    .AvoidModifyEnvironment()
    .Load();

var settings = new EnvBinder(provider: envVars).Bind<AppSettings>();
```
In the above example, the `Load` method does not modify the environment, so the environment variables are obtained from a dictionary.

The `Load` method will return an instance that implements the `IEnvironmentVariablesProvider` interface and the instance is then injected into the constructor of the `EnvBinder` class. Therefore, the `Bind` method is binding the instance of the model with the keys that are in the provider (which in this case is a `dictionary<string, string>`).

## Register model as a service

If you want to use a DI Container as [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection), you must register the model as a service so that the container takes care of injecting the service when required:
```cs
// In Program.cs:
// You must first load the .env file.
new EnvLoader().Load();
var settings = new EnvBinder().Bind<AppSettings>();
var services = new ServiceCollection();
// Register the model as a service.
services.AddSingleton<AppSettings>(settings);
```
If you are using **ASP.NET Core 3.1**, you must register the service in the `ConfigureServices` method of the `Startup` class:
```cs
public void ConfigureServices(IServiceCollection services)
{
    // You must first load the .env file.
    new EnvLoader().Load();
    var settings = new EnvBinder().Bind<AppSettings>();
    // Register the model as a service.
    services.AddSingleton<AppSettings>(settings);
}
```
In **ASP.NET Core 6**, you must register the service in `Program.cs`:
```cs
var builder = WebApplication.CreateBuilder(args);
// You must first load the .env file.
new EnvLoader().Load();
var settings = new EnvBinder().Bind<AppSettings>();
// Register the model as a service.
builder.Services.AddSingleton<AppSettings>(settings);
```