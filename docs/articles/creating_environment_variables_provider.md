# Creating environment variables provider

## Introduction

**An environment variables provider** (*or also called key-value pairs provider*) is an entity that represents a storage of environment variables and provides a service to access them. In the context of this library, the term "*environment variable*" means a "*pair of key-value*".

The **DotEnv library** has two providers of environment variables:
- Environment of the current process.
- A dictionary of key/value pairs (this provider is used when using [AvoidModifyEnvironment](xref:DotEnv.Core.IEnvLoader.AvoidModifyEnvironment) method).

In fact, if you look at the source code of the library, you will notice that it has two classes that represent the provider: `DefaultEnvironmentProvider` and `DictionaryProvider`.

The library exposes an interface to access the provider: `IEnvironmentVariablesProvider`.

Example:
```cs
var envVars = new EnvLoader().Load();
```
In the example, the `Load` method returns an instance that implements the `IEnvironmentVariablesProvider` interface, through this interface the environment variables of the provider can be accessed:
```cs
string key1 = envVars["KEY1"];
```
In this case the environment variables are obtained from the environment of the current process.

## Customized Provider

Let's start creating our environment variables provider:
```cs
class CustomProvider : IEnvironmentVariablesProvider
{
    private Dictionary<string, string> _keyValuePairs = new();

    public string this[string variable] 
    {
        get => _keyValuePairs.ContainsKey(variable) ? _keyValuePairs[variable] : null;
        set => _keyValuePairs[variable] = value;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _keyValuePairs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
```
In this provider is where our environment variables will be stored.

Then we will use the `SetEnvironmentVariablesProvider` method to configure our custom provider:
```cs
var envVars = new EnvLoader()
        .SetEnvironmentVariablesProvider(new CustomProvider())
        .Load();

string key1 = envVars["KEY1"];
```
In this case the environment variables are obtained from the custom provider.

## Extension methods

The `IEnvironmentVariablesProvider` interface has its own extension methods:

### CreateReader

Creates an instance that implements the `IEnvReader` interface:
```cs
var envVars = new EnvLoader()
        .SetEnvironmentVariablesProvider(new CustomProvider())
        .Load();

// Equivalent to: var reader = new EnvReader(envVars);
IEnvReader reader = envVars.CreateReader();
string key1 = envVars["KEY1"];
```

### CreateValidator

Creates an instance that implements the `IEnvValidator` interface:
```cs
var envVars = new EnvLoader()
        .SetEnvironmentVariablesProvider(new CustomProvider())
        .Load();

// Equivalent to: var validator = new EnvValidator(envVars);
IEnvValidator validator = envVars.CreateValidator();
```

### CreateBinder

Creates an instance that implements the `IEnvBinder` interface:
```cs
var envVars = new EnvLoader()
        .SetEnvironmentVariablesProvider(new CustomProvider())
        .Load();

// Equivalent to: var binder = new EnvBinder(envVars);
IEnvBinder binder = envVars.CreateBinder();
```

### ToDictionary

Converts the provider instance to a dictionary:
```cs
var envVars = new EnvLoader().Load();
var dict = envVars.ToDictionary();
```

More information can be found in the [API Documentation](xref:DotEnv.Core.EnvironmentVariablesProviderExtensions).



