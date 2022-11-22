# Getting Started with `dotenv.core`

## Installation

If you're want to install the package from Visual Studio, you must open the project/solution in Visual Studio, and open the console using the **Tools** > **NuGet Package Manager** > **Package Manager Console** command and run the install command:
```
Install-Package DotEnv.Core
```
If you are making use of the dotnet CLI, then run the following in your terminal:
```
dotnet add package DotEnv.Core
```

## Usage

The first thing you need to do is create a `.env` file in the root directory of your project.

### Loading .env file

You must import the namespace types at the beginning of your class file:
```cs
using DotEnv.Core;
```

Then you can load the .env file with the `Load` method of the `EnvLoader` class:
```cs
new EnvLoader().Load();
```
By default, the `Load` method will search for a file called `.env` in the current directory and if it does not find it, it will search for it in the parent directories of the current directory. Generally, the current directory is where the executable (your application itself) with its dependencies is located.

Remember that if no encoding is specified to the `Load` method, the default will be `UTF-8`. Also, by default, the `Load` method does not overwrite the value of an existing environment variable.

### Accessing variables

After you have loaded the .env file with the `Load` method, you can access the environment variables using the indexer of the `EnvReader` class:
```cs
var reader = new EnvReader();
string key1 = reader["KEY1"];
string key2 = reader["KEY2"];
```
Or you can also access the environment variables using the static property `Instance`:
```cs
string key1 = EnvReader.Instance["KEY1"];
string key2 = EnvReader.Instance["KEY2"];
```
If you don't want to use the `EnvReader` class to access environment variables, you can use the `System.Environment` class:
```cs
string key1 = System.Environment.GetEnvironmentVariable("KEY1");
string key2 = System.Environment.GetEnvironmentVariable("KEY2");
```

### Changing default name

You can also change the default name of the .env file using the `SetDefaultEnvFileName` method:
```cs
new EnvLoader()
    .SetDefaultEnvFileName(".env.dev")
    .Load();
```
Now the `Load` method will search for the **.env.dev** file in the current directory and in the parent directories if it is not found in the current directory.

Another case would be:
```cs
new EnvLoader()
    .SetDefaultEnvFileName(".env.dev")
    .AddEnvFiles("/foo/foo2", "/bar/bar2")
    .Load();
```
The `Load` method will search for two `.env.dev` files in the paths `/foo/foo2` and `/bar/bar2`.

### Specifying path absolute

You can also specify the absolute path to the .env file:
```cs
new EnvLoader()
    .AddEnvFile("/home/MyProject/App/src/.env.dev")
    .Load();
```
In this case the `Load` method will search for the `.env.dev` file in the path `/home/MyProject/App/src/`, if it does not find it, the method will search for the `.env.dev` file in the parent directories. In other words, the `Load` method will search for the file in the parent directories of `src` such as: `App`, `MyProject`, `home`.

It is recommended not to use absolute paths, instead use relative paths. Remember that an absolute path can be different in each operating system, so your application could lose portability.

### Loading multiple .env files

You can also load multiple .env files in a single call:
```cs
new EnvLoader()
    .AddEnvFiles("env.example", "env.example2")
    .Load();
```
Or you can use the `AddEnvFile` method:
```cs
new EnvLoader()
    .AddEnvFile("env.example")
    .AddEnvFile("env.example2")
    .Load();
```

If you need to specify an encoding type for all .env files, you can do it like this:
```cs
new EnvLoader()
    .AddEnvFiles("env.example", "env.example2")
    // Or you can also use: SetEncoding("Unicode")
    .SetEncoding(Encoding.Unicode)
    .Load();
```

You can also specify an encoding type for each .env file using the `AddEnvFile` method:
```cs
new EnvLoader()
    .AddEnvFile("env.example",  Encoding.Unicode)
    .AddEnvFile("env.example2", Encoding.ASCII)
    .AddEnvFile("env.example3", "Unicode")
    .AddEnvFile("env.example4", "ASCII")
    .Load();
```

### Optional .env files

You can indicate that the existence of an .env file is optional by means of the `AddEnvFile` method:
```cs
new EnvLoader()
    .AddEnvFile(".env.example", optional: true)
    .Load();
```
At the end the `Load` method will not generate any error in case the `.env.example` file is not in a directory, since it is optional.

You can also mark all .env files as optional using the `AllowAllEnvFilesOptional` method:
```cs
new EnvLoader()
    .AddEnvFile(".env.example1") 
    .AddEnvFile(".env.example2") 
    .AddEnvFile(".env.example3")
    .AllowAllEnvFilesOptional()
    .Load();
```

### Specifying path relative

You can also specify a relative path using the `AddEnvFile` method:
```cs
new EnvLoader()
    .AddEnvFile("./dotenv/files")
    .Load();
```
In this case, the `.env` file is inside a directory, i.e. in `files`. The `Load` method will search for the file `dotenv/files/.env` in the current directory, if it does not find it, it will search for it in parent directories.

This is useful when the .env file is located in a different directory than the current one.

### Specifying base path

You can specify one base path for all .env files:
```cs
new EnvLoader()
    .SetBasePath("./dotenv/files")
    .AddEnvFiles(".env.example", ".env.example2")
    .Load();
```
In this case, the **Base Path** is a relative path, so the `.env.example` and `.env.example2` files are inside `dotenv/files`.

### Error handling

By default, the `Load` method does not throw any exception if it does not found the .env file but you can change this behavior if you use the `EnableFileNotFoundException` method:
```cs
try 
{
    new EnvLoader()
        .EnableFileNotFoundException()
        .Load();
}
catch(FileNotFoundException ex)
{
    System.Console.WriteLine(ex.Message);
}
```

You can handle the error without throwing an exception by means of the `EnvValidationResult` class:
```cs
new EnvLoader()
    // To ignore the exception thrown by the parser.
    .IgnoreParserException()
    .Load(out EnvValidationResult result);

if(result.HasError())
{
    string msg = result.ErrorMessages;
    System.Console.WriteLine(msg);
    // Or you can also iterate over the errors.
    foreach(string errorMsg in result)
        System.Console.WriteLine(errorMsg); 
}
else 
{
    // Execute some action when there is no error.
}
```

**Note:** If you don't know what each class does, don't forget to check the [API documentation](xref:DotEnv.Core).

## Copying .env file to the output directory

If you want to copy the .env file to the output directory, you have to add the following to your .csproj file:
```xml
<ItemGroup>
  <Content Include=".env">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```
**NOTE:** Your .env file must be in the same directory as the .csproj file.