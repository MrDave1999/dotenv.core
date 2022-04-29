# Using DotEnv in ASP.NET Core

## Registering IEnvReader

Open the `Startup.cs` file and in the `ConfigureServices` method you must load the .env file and register the `IEnvReader` service:
```cs
public void ConfigureServices(IServiceCollection services)
{
    new EnvLoader().Load();
    services.AddSingleton<IEnvReader>(new EnvReader());
}
```
Then you can access the environment variables with `IEnvReader`, for example:
```cs
class HomeController
{
    public HomeController(IEnvReader reader)
    {
        string key = reader["KEY_NAME"];
    }
}
```
If you are using **ASP.NET Core 6** you do not need to add anything in the `Startup.cs` file, just go to `Program.cs` and load the .env file and register the service:
```cs
var builder = WebApplication.CreateBuilder(args);
new EnvLoader().Load();
builder.Services.AddSingleton<IEnvReader>(new EnvReader());
```

## Using IConfiguration

Open the `Startup.cs` file and in the constructor of the `Startup` class you will need to load the .env file and build a new configuration:
```cs
class Startup
{
    public Startup(IConfiguration configuration)
    {
        new EnvLoader().Load();
        Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
    }

    public IConfiguration Configuration { get; }
}
```
The `AddEnvironmentVariables` method must be called because it will add the environment variables in the **configuration object** managed by ASP.NET Core, so you can access the variables with `IConfiguration`:
```cs
class HomeController
{
    public HomeController(IConfiguration configuration)
    {
        string key = configuration["KEY_NAME"];
    }
}
```
If you are using **ASP.NET Core 6**, open the `Program.cs` file and load the .env file and call the `Configuration` property of the `WebApplicationBuilder` class:
```cs
var builder = WebApplication.CreateBuilder(args);
new EnvLoader().Load();
builder.Configuration.AddEnvironmentVariables();
```

## Using IOptions

If you do not want to use `IEnvReader` or `IConfiguration`, you can create your own configuration class and with this custom class you will be able to access the keys that are read from an .env file:
```cs
// My Custom Class
class AppSettings
{
    public string ConnectionString { get; set; }
    public string JwtSecret { get; set; }
}
```
Remember that each property must match the key name of the .env file.

Then you must open the `Startup.cs` file and load the .env file and build the configuration in the constructor of the `Startup` class:
```cs
class Startup
{
    public Startup(IConfiguration configuration)
    {
        new EnvLoader().Load();
        Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
    }

    public IConfiguration Configuration { get; }
}
```
Then go to `ConfigureServices` method and register the configuration instance. The idea is to bind the `AppSettings` class with the configuration instance referenced by the `Startup.Configuration` property:
```cs
class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AppSettings>(Configuration);
    }

    public IConfiguration Configuration { get; }
}
```
And ready, you can now use the `IOptions` interface to access the keys:
```cs
class HomeController
{
    public HomeController(IOptions<AppSettings> options)
    {
        AppSettings settings = options.Value;
        string key1 = settings.ConnectionString;
        string key2 = settings.JwtSecret;
    }
}
```
The above explanation is not difficult to adapt to **ASP.NET Core 6** if you are using it. I leave you the complete example so that you can guide you:
```cs
// In Program.cs file:
var builder = WebApplication.CreateBuilder(args);
new EnvLoader().Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<AppSettings>(builder.Configuration);
```

## Registering AppSettings

If you don't want to use the `IOptions` interface, you can register the `AppSettings` class as service:
```cs
class Startup
{
    public Startup(IConfiguration configuration)
    {
        // Load .env file
        new EnvLoader().Load();
        // Build Configuration
        Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Bind the 'AppSettings' class with the configuration instance.
        AppSettings settings = ConfigurationBinder.Get<AppSettings>(Configuration);
        // Register the `AppSettings` class as service.
        services.AddSingleton(settings);
    }

    public IConfiguration Configuration { get; }
}
```
Then you can directly use the `AppSettings` class to access the configuration values:
```cs
class HomeController
{
    public HomeController(AppSettings settings)
    {
        string key1 = settings.ConnectionString;
        string key2 = settings.JwtSecret;
    }
}
```
In case you are using **ASP.NET Core 6**:
```cs
// In Program.cs file:
var builder = WebApplication.CreateBuilder(args);
new EnvLoader().Load();
builder.Configuration.AddEnvironmentVariables();
AppSettings settings = ConfigurationBinder.Get<AppSettings>(builder.Configuration);
builder.Services.AddSingleton(settings);
```