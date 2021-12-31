# More options for the Loader

There are some options that are for the parser, but we can also use it through the `EnvLoader` class.

## DisableTrimStartKeys

Disable the trim at the beginning of the keys:
```cs
new EnvLoader()
    .DisableTrimStartKeys()
    .Load();
```
This method will tell the parser not to remove leading white-spaces from the keys.

## DisableTrimEndKeys

Disable the trim at the end of the keys:
```cs
new EnvLoader()
    .DisableTrimEndKeys()
    .Load();
```
This method will tell the parser not to remove trailing white-spaces from the keys.

## DisableTrimKeys

Disables the trim at the start and end of the keys:
```cs
new EnvLoader()
    .DisableTrimKeys()
    .Load();
```
This method will tell the parser not to remove leading and trailing white-spaces from the keys.

## DisableTrimStartValues

Disable the trim at the beginning of the values:
```cs
new EnvLoader()
    .DisableTrimStartValues()
    .Load();
```
This method will tell the parser not to remove leading white-spaces from the values.

## DisableTrimEndValues

Disable the trim at the end of the values:
```cs
new EnvLoader()
    .DisableTrimEndValues()
    .Load();
```
This method will tell the parser not to remove trailing white-spaces from the values.

## DisableTrimValues

Disables the trim at the start and end of the values:
```cs
new EnvLoader()
    .DisableTrimValues()
    .Load();
```
This method will tell the parser not to remove leading and trailing white-spaces from the values.

## DisableTrimStartComments

Disable the trim at the beginning of the comments:
```cs
new EnvLoader()
    .DisableTrimStartComments()
    .Load();
```
This method will tell the parser not to remove leading white-spaces from the comments.

## AllowOverwriteExistingVars

Allows to overwrite the current value of an existing environment variable:
```cs
new EnvLoader()
    .AllowOverwriteExistingVars()
    .Load();
```
Imagine that if there is an environment variable called `KEY1` whose value is `1`, then in an .env file there can be a key named `KEY1` whose value is `2`, if this option is enabled, then, when the parser reads `KEY1` from the .env file, it will overwrite the value of `KEY1` by `2`.

## AllowConcatDuplicateKeys

Allows to concatenate the values of the duplicate keys:
```cs
new EnvLoader()
    .AllowConcatDuplicateKeys()
    .Load();
```
This method will by default concatenate to the end of the value, for example, imagine you have the following .env file:
```
KEY1 = Hello
KEY1 = World
KEY1 = !
```
The parser will concatenate the duplicate keys in this way:
```
KEY1 = HelloWorld!
```
But we can also tell the parser to concatenate at the beginning of the value using `ConcatKeysOptions` enum:
```cs
new EnvLoader()
    .AllowConcatDuplicateKeys(ConcatKeysOptions.Start)
    .Load();
```
So if we follow the above example, the parser will concatenate the duplicate keys in this way:
```
KEY1 = !WorldHello
```

## IgnoreParserExceptions

Ignores parser exceptions. By calling this method the parser will not throw any exceptions when it encounters an error:
```cs
new EnvLoader()
    .IgnoreParserExceptions()
    .Load();
```

## SetCommentChar

Sets the character that will define the beginning of a comment:
```cs
new EnvLoader()
    .SetCommentChar(';')
    .Load();
```
So the .env file could look like this:
```
; comment (1)
KEY1=VAL1
; comment (2)
```

## SetDelimiterKeyValuePair

Sets the delimiter that separates an assigment of a value to a key:
```cs
new EnvLoader()
    .SetDelimiterKeyValuePair(':')
    .Load();
```
So the .env file could look like this:
```
KEY1: VAL1
KEY2: VAL2
```

**As a side note**, the `EnvLoader` class has a parameterized constructor in which you can specify a custom parser. You can inherit from `EnvParser` and inject the instance into the constructor.