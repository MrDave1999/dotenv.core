# Parsing

## Introduction

You can also use the parser directly, as in this example:
```cs
string dataSource = @"
    KEY1=VAL1
    KEY2=VAL2
    KEY3=VAL3
";
new EnvParser().Parse(dataSource);

System.Console.WriteLine(EnvReader.Instance["KEY1"]); // Print "VAL1".
System.Console.WriteLine(EnvReader.Instance["KEY2"]); // Print "VAL2".
System.Console.WriteLine(EnvReader.Instance["KEY3"]); // Print "VAL3".
```
By default, the `Parse` method does not overwrite the value of the environment variable.

For example:
```cs
System.Environment.SetEnvironmentVariable("KEY1", "1");
new EnvParser().Parse("KEY1=VAL1");
System.Console.WriteLine(EnvReader.Instance["KEY1"]); // Print "1".
```
In this case, the parser does not overwrite the variable `KEY1`, so its current value is maintained.

You can also retrieve keys from any data source and pass it to the parser:
```cs
string dataSource = System.IO.File.ReadAllText("./.env");
new EnvParser().Parse(dataSource);
```

## Configuring parser behavior

There are configuration options that allow you to change the behavior of the parser, one of them are:
```cs
string dataSource = System.IO.File.ReadAllText("./.env");
new EnvParser()
    .DisableTrimStartKeys()
    .DisableTrimEndKeys() 
    .DisableTrimStartValues()
    .DisableTrimEndValues()
    .DisableTrimStartComments()
    .AllowOverwriteExistingVars()
    .Parse(dataSource);
```
Don't forget to look up in the [API documentation](xref:DotEnv.Core.IEnvParser) what each configuration option means.

## Error handling

We can handle errors with the `EnvValidationResult` class instead of throwing an exception:
```cs
string dataSource = System.IO.File.ReadAllText("./.env");
new EnvParser()
    .IgnoreParserException() // To ignore the exception thrown by the parser.
    .Parse(dataSource, out EnvValidationResult result);

if(result.HasError())
{
    System.Console.WriteLine(result.ErrorMessages);
}
else 
{
    // Execute some action when there is no error.
}
```

## Avoid modifying the environment

You can tell the parser not to modify the environment:
```cs
string dataSource = System.IO.File.ReadAllText("./.env");
var envVars = new EnvParser()
        .AvoidModifyEnvironment()
        .Parse(dataSource);
```
As the environment cannot be modified, the `Parse` method will return an instance that implements the `IEnvironmentVariablesProvider` interface, through this returned instance, we can access the environment variables that have been set in a dictionary:
```cs
// The value of the variable is obtained from a dictionary and not from the current environment:
string key1 = envVars["KEY1"];
```