# Using Helpers

## Get{Type}Value

The `EnvReader class` has multiple helper methods, so methods starting with the word *Get* will throw an exception when the variable does not found in the current process:
```cs
GetStringValue
GetIntValue
GetLongValue
GetFloatValue
//and so on...
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
catch(EnvVariableNotFoundException ex)
{
    System.Console.WriteLine(ex.Message);
}
```
Don't forget to consult the [API documentation](xref:DotEnv.Core.EnvReader.GetBoolValue(System.String)) for more helper methods starting with the word *Get*.

## TryGet{Type}Value

The helper methods that begin with the word *Try* do not throw an exception, but return a `false` value when the variable does not exist in the current process:
```cs
TryGetStringValue
TryGetIntValue
TryGetLongValue
TryGetFloatValue
//and so on...
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

## Env{Type}

The helper methods starting with the word *Env* return a default value when the variable does not exist in the current process:
```cs
EnvString
EnvInt
EnvLong
EnvFloat
//and so on...
```

For example:
```cs
var reader = new EnvReader();
string key1 = reader.EnvString("KEY1", "Variable Not Found!");
int key2 = reader.EnvInt("KEY2", -1);
long key3 = reader.EnvLong("KEY3", -1);
float key4 = reader.EnvFloat("KEY4", -1.0F);
```
Then, if for example, the variable `KEY1` does not exist in the current process, then the method returns `Variable Not Found!` (default value).

The second parameter is optional:
```cs
var reader = new EnvReader();
string key1 = reader.EnvString("KEY1"); // default value: null
int key2 = reader.EnvInt("KEY2"); // default value: 0
long key3 = reader.EnvLong("KEY3"); // default value: 0
float key4 = reader.EnvFloat("KEY4"); // default value: 0.0
```
Here for example, if `KEY1` does not exist, the method returns `null` (default value).

Don't forget to consult the [API documentation](xref:DotEnv.Core.EnvReader.EnvBool(System.String,System.Boolean)) for more helper methods starting with the word *Env*.

## Customize EnvReader

You can also create a class that inherits from the `EnvReader` class and can add new methods or override existing methods:
```cs
class CustomEnvReader : EnvReader
{
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

    //and so on...
}
```

## Iterate

You can also access all environment variables of the current process using the iterator:
```cs
var reader = new EnvReader();
foreach(string(key, value) in reader)
    System.Console.WriteLine($"{key}, {value}");
```