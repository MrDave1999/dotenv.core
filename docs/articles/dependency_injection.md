# Dependency Injection

## Introduction

Dependency injection is an object-oriented design pattern, in which objects are supplied to a class instead of the class itself creating those objects. The containing class is the one that supplies the object to our class.

In some cases, classes should not directly create an instance of the `EnvReader` class, because if you change the implementation, you will have to make changes in the classes that depend on `EnvReader`.

**For example:**
```cs
class Foo
{
    private EnvReader _reader;

    public Foo()
    {
        _reader = new EnvReader();
    }
}

class Bar
{
    private EnvReader _reader;

    public Bar()
    {
        _reader = new EnvReader();
    }
}
```

The classes `Foo` and `Bar` create the instance in the constructor, this makes it difficult to reverse the dependency. In the future you could create a new class that inherits from `EnvReader` and this would cause changes in two classes: `Foo` and `Bar`:
```cs
class CustomEnvReader : EnvReader
{
    // Here we can override some methods.
}

class Foo
{
    private EnvReader _reader;

    public Foo()
    {
        // change #1
        _reader = new CustomEnvReader();
    }
}

class Bar
{
    private EnvReader _reader;

    public Bar()
    {
        // change #2
        _reader = new CustomEnvReader();
    }
}
```
So to avoid those changes in the future, you can make use of dependency injection pattern:
```cs
class Foo
{
    private EnvReader _reader;

    public Foo(EnvReader reader)
    {
        _reader = reader;
    }
}

class Bar
{
    private EnvReader _reader;

    public Bar(EnvReader reader)
    {
        _reader = reader;
    }
}
```
You can also make use of the `IEnvReader` interface instead of the base `EnvReader` class:
```cs
class Foo
{
    private IEnvReader _reader;

    public Foo(IEnvReader reader)
    {
        _reader = reader;
    }
}

class Bar
{
    private IEnvReader _reader;

    public Bar(IEnvReader reader)
    {
        _reader = reader;
    }
}
```
You probably don't want to inject the dependency manually, as in this example:
```cs
class Program
{
    static void Main(string[] args)
    {
        var foo = new Foo(new CustomEnvReader());
        var bar = new Bar(new CustomEnvReader());
        // more code...
    }
}
```

## DI Container

So in the end we would consider using a service container to handle dependency injection. Microsoft has created a package in NuGet specifically for this: [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection).

The above example could be done in this way using the DI Container:
```cs
// Import all types.
using Microsoft.Extensions.DependencyInjection;
using DotEnv.Core;

class Program
{
    static void Main(string[] args)
    {
        // Load the .env file.
        new EnvLoader().Load();
        // Creates the service collection.
        var services = new ServiceCollection();
        // Register services.
        services.AddSingleton<IEnvReader>(new EnvReader())
            .AddTransient<Foo>()
            .AddTransient<Bar>();
        // Creates the service container.
        using(var serviceProvider = services.BuildServiceProvider())
        {
            // Retrieves an instance of the service and the container resolves the dependencies.
            var foo = serviceProvider.GetRequiredService<Foo>();
            var bar = serviceProvider.GetRequiredService<Bar>();
        }
    }
}
```

### Another example
```cs
// Import all types.
using Microsoft.Extensions.DependencyInjection;
using DotEnv.Core;

class Program
{
    static void Main(string[] args)
    {
        // Load the .env file.
        var envVars = new EnvLoader().Load();
        // Creates the service collection.
        var services = new ServiceCollection();
        // Register services.
        services.AddSingleton<IEnvReader>(envVars.CreateReader())
            .AddSingleton<IEnvironmentVariablesProvider>(envVars);
    }
}
```