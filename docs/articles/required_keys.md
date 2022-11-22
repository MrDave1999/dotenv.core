# Required Keys

## Introduction

If a required key is not present in the application, an error should be generated.

To require configuration keys:
```cs
new EnvLoader().Load();
new EnvValidator()
    .SetRequiredKeys("SERVICE_APP_ID", "SERVICE_KEY", "SERVICE_SECRET")
    .Validate();
```
In the above example we first load the .env file and set the variables, then we call the `Validate` method to validate if the required keys exist in the current environment, otherwise an exception is thrown.

In other words, after calling the `Load` method, it will check if the keys `SERVICE_APP_ID`, `SERVICE_KEY`, `SERVICE_SECRET` exist in the environment of the current process.

## Other configuration options

### IgnoreException

With this method you can ignore the exception thrown by the `Validate` method:
```cs
new EnvLoader().Load();
new EnvValidator()
    .IgnoreException()
    .SetRequiredKeys("SERVICE_APP_ID", "SERVICE_KEY", "SERVICE_SECRET")
    .Validate();
```

### SetRequiredKeys

With this method we can specify the required keys to be validated.

This method has several overloads and one of them is to be able to specify the required keys by means of a class with properties:
```cs
class RequiredKeys
{
    public string SERVICE_APP_ID { get; }
    public string SERVICE_KEY    { get; }
    public string SERVICE_SECRET { get; }
}

new EnvLoader().Load();
new EnvValidator()
    .SetRequiredKeys<RequiredKeys>()
    .Validate();
```
You can also pass an instance of type `System.Type`:
```cs
new EnvValidator()
    .SetRequiredKeys(typeof(RequiredKeys))
    .Validate();
```
You should note that the `RequiredKeys` class must follow the following rules:
- Each required key is represented as a property.
- Each property represents a value of type `string`.
- Each property must be `public`.
- Each property can be read-only or read-write.

## Error handling

You can handle the error through the `EnvValidationResult` class instead of throwing an exception:
```cs
new EnvLoader().Load();
new EnvValidator()
    // To ignore the exception thrown by the validator.
    .IgnoreException()
    .SetRequiredKeys<RequiredKeys>()
    .Validate(out EnvValidationResult result);

if(result.HasError())
{
    string msg = result.ErrorMessages;
    System.Console.WriteLine(msg);
}
else 
{
    // Execute some action when there is no error.
}
```

## Injecting a Provider

The `EnvValidator` class is flexible and adapts to any provider, i.e., you can use this class to validate whether the required keys are set in the current process environment or in a dictionary or custom provider.

Example:
```cs
var envVars = new EnvLoader()
        .AvoidModifyEnvironment()
        .Load();

new EnvValidator(provider: envVars)
    .SetRequiredKeys<RequiredKeys>()
    .Validate();
```
In the above example, the `Load` method does not modify the environment, so the environment variables are obtained from a dictionary. 

The instance of type `IEnvironmentVariablesProvider` returned by `Load` method is injected into the constructor of the `EnvValidator` class, so the `Validate` method would be checking if the required keys are present in the dictionary, instead of the current environment.