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

**As a side note**, the `EnvLoader` class has a parameterized constructor in which you can specify a custom parser. You can inherit from `EnvParser` and inject the instance into the constructor.