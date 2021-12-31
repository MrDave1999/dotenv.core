# Using Parser

You can also use the parser directly, as in this example:
```cs
string envFile = @"
    KEY1=VAL1
    KEY2=VAL2
    KEY3=VAL3
";
new EnvParser().Parse(envFile);

System.Console.WriteLine(EnvReader.Instance["KEY1"]); // Print "VAL1".
System.Console.WriteLine(EnvReader.Instance["KEY2"]); // Print "VAL2".
System.Console.WriteLine(EnvReader.Instance["KEY3"]); // Print "VAL3".
```
**As an additional note**, if a key already exists as an environment variable, its value will not be overwritten. For example, if the key `KEY1` exists in the current process as an environment variable, then its value will not be overwritten by `VAL1`.

For example:
```cs
System.Environment.SetEnvironmentVariable("KEY1", "1");
new EnvParser().Parse("KEY1=VAL1");
System.Console.WriteLine(EnvReader.Instance["KEY1"]); // Print "1".
```
In this case, the parser does not overwrite the variable `KEY1`, so its current value is maintained.

You can also retrieve the keys from a file and pass it to the parser:
```cs
string envFile = System.IO.File.ReadAllText("./.env");
new EnvParser().Parse(envFile);
```

## Configuring parser behavior

You can also select options for the parser in order to change its behavior:
```cs
string envFile = System.IO.File.ReadAllText("./.env");
new EnvParser()
    .DisableTrimStartKeys() 
    .DisableTrimStartValues() 
    .Parse(envFile);
```
In the above case, the parser is instructed not to remove leading spaces from keys and values.

Don't forget to look up in the [API documentation](xref:DotEnv.Core.EnvParserOptions) what each configuration option means.

## Customizing the parser algorithm

You can also create a class that inherits from `EnvParser` and then you can override its methods (each of these methods are used by the [parser algorithm](xref:DotEnv.Core.EnvParser.Parse(System.String))):
```cs
class CustomEnvParser : EnvParser
{
    protected override bool IsComment(string line)
    {
        // Here you can add your own implementation.
    }

    protected override string ExtractKey(string line)
    {
        // Here you can add your own implementation.
    }

    protected override string ExtractValue(string line)
    {
        // Here you can add your own implementation.
    }

    protected override bool HasNoKeyValuePair(string line)
    {
        // Here you can add your own implementation.
    }

    protected override void SetEnvironmentVariable(string key, string value)
    {
        // Here you can add your own implementation.
    }

    protected override string ConcatValues(string currentValue, string value)
    {
        // Here you can add your own implementation.
    }

    protected override string ExpandEnvironmentVariables(string value, int lineNumber)
    {
        // Here you can add your own implementation.
    }
}
```
This is useful when you need to customize the parser and change its internal behavior. 

You can also tell the `Load` method of the `EnvLoader` class what type of parser it should use:
```cs
new EnvLoader(new CustomEnvParser())
    .Load();
```

**Note:** If you don't know what each class does, don't forget to check the [API documentation](xref:DotEnv.Core.EnvParser).

## Parser rules

- Each line beginning with the `#` character is a comment.
- White-spaces at the beginning of each comment will be ignored.
- Empty lines or lines with white-spaces will be ignored.
- If the data source (probably an .env file) is empty or consists only white-spaces, an exception is thrown.
- The key-value format must be as follows: `KEY=VAL`.
- There is no special handling of quotation marks. This means that **they are part of the VAL.**
- If the key is an empty string, an exception is thrown.
- If the value of a key is an empty string, it will be converted to a white-space.
- White-spaces at both ends of the key/value are ignored.
