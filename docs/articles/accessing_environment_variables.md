# Accessing environment variables

## Helper methods

The `EnvReader` class has multiple **helper methods** that are used to access the environment variables of a specific provider.

### Get{Type}Value

The helper methods that begin with the word *Get* will throw an exception when the environment variable is not set:
```cs
GetStringValue
GetIntValue
GetLongValue
GetFloatValue
// And so on...
```
For example:
```cs
var reader = new EnvReader();
try
{
    string key = reader["KEY"];
    string key1 = reader.GetStringValue("KEY1");
    int key2 = reader.GetIntValue("KEY2");
    long key3 = reader.GetLongValue("KEY3");
    float key4 = reader.GetFloatValue("KEY4");
}
catch(VariableNotSetException ex)
{
    System.Console.WriteLine(ex.Message);
}
```
Don't forget to consult the [API documentation](xref:DotEnv.Core.EnvReader.GetBoolValue(System.String)) for more helper methods starting with the word *Get*.

### TryGet{Type}Value

The helper methods that begin with the word *Try* do not throw an exception, but return a `false` value when the environment variable is not set:
```cs
TryGetStringValue
TryGetIntValue
TryGetLongValue
TryGetFloatValue
// And so on...
```

For example:
```cs
var reader = new EnvReader();
if(reader.TryGetStringValue("KEY1", out string value1))
{
    System.Console.WriteLine(value1);
}
else 
{
    System.Console.WriteLine("Variable not found!");
}

if(reader.TryGetIntValue("KEY2", out int value2))
{
    System.Console.WriteLine(value2);
}
else 
{
    System.Console.WriteLine("Variable not found!");
}
```
Don't forget to consult the [API documentation](xref:DotEnv.Core.EnvReader.TryGetBoolValue(System.String,System.Boolean@)) for more helper methods starting with the word *Try*.

### Env{Type}

The helper methods that begin with the word *Env* return a default value when the environment variable is not set:
```cs
EnvString
EnvInt
EnvLong
EnvFloat
// And so on...
```

For example:
```cs
var reader = new EnvReader();
string key1 = reader.EnvString("KEY1", "Variable Not Found!");
int key2 = reader.EnvInt("KEY2", -1);
long key3 = reader.EnvLong("KEY3", -1);
float key4 = reader.EnvFloat("KEY4", -1.0F);
```
Then, if for example, the variable `KEY1` is not set, then the method returns `Variable Not Found!` (default value).

The second parameter is optional:
```cs
var reader = new EnvReader();
// Default value: null.
string key1 = reader.EnvString("KEY1");
// Default value: 0.
int key2 = reader.EnvInt("KEY2");
// Default value: 0.
long key3 = reader.EnvLong("KEY3");
// Default value: 0.0
float key4 = reader.EnvFloat("KEY4");
```
Here for example, if `KEY1` is not set, the method returns `null` (default value).

Don't forget to consult the [API documentation](xref:DotEnv.Core.EnvReader.EnvBool(System.String,System.Boolean)) for more helper methods starting with the word *Env*.

### HasValue

You can use this method to check if an environment variable has a value:
```cs
var reader = new EnvReader();
Console.WriteLine(reader.HasValue("VARIABLE_NAME")); 
Console.WriteLine(EnvReader.Instance.HasValue("VARIABLE_NAME"));
```

In fact, if you look at the source code of this method, what it actually does is to check if the variable is in a specific provider (it could be the environment of the current process or a simple dictionary).

## Customize EnvReader

You can also create a class that inherits from the `EnvReader` class and can add new methods or override existing methods:
```cs
class CustomEnvReader : EnvReader
{
    public CustomEnvReader() { }
    public CustomEnvReader(IEnvironmentVariablesProvider provider) : base(provider) { }

    public override string GetStringValue(string variable)
    {
        // Here you can write your own implementation.
    }

    public override bool TryGetStringValue(string variable, out string value)
    {
        // Here you can write your own implementation.
    }

    public override string EnvString(string variable, string defaultValue = default)
    {
        // Here you can write your own implementation.
    }

    // And so on...
}
```

## Iterate

You can also access all environment variables using the iterator:
```cs
var reader = new EnvReader();
foreach(string(key, value) in reader)
    System.Console.WriteLine($"{key}, {value}");
```

## Injecting a Provider

The `EnvReader` class has no relationship to the environment variables provider. So you can use the methods of this class to access environment variables, regardless of whether the variables are in the environment of the current process or in a `dictionary<string, string>`.

Example:
```cs
var envVars = new EnvLoader()
        .AvoidModifyEnvironment()
        .Load();

var reader = new EnvReader(provider: envVars);
// We access from a dictionary instead of the current environment.
string key1 = reader["KEY1"]; 
```
In the previous example we load the .env file without altering the environment, so the environment variables are in a dictionary.

Later, we inject the instance of type `IEnvironmentVariablesProvider` that returns the `Load` method in the constructor of the `EnvReader` class, this way we can access the environment variables from a dictionary.