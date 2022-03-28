# Load .env file without altering the environment

The `AvoidModifyEnvironment` method tells the loader not to modify the environment:
```cs
var envVars = new EnvLoader()
        .AvoidModifyEnvironment()
        .Load();

string key1 = envVars["KEY1"];
string key2 = envVars["KEY2"];
```
This way the .env file is loaded without touching the environment. The `Load` method will return an instance that implements the `IEnvironmentVariablesProvider` interface and through this instance we can access the environment variables. In fact, the environment variables are obtained from a dictionary, instead of the current process.

You can also iterate over the retrieved elements:
```cs
foreach(string(variable, value) in envVars)
    System.Console.WriteLine($"{variable}, {value}");
```

You can also convert the provider instance to a `dictionary<string, string>`:
```cs
var dict = envVars.ToDictionary();
```