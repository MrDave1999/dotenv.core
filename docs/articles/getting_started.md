# Getting Started with `dotenv.core`

## Installation

If you're an hardcore and want to do it manually, you must add the following to the `csproj` file:
```xml
<PackageReference Include="DotEnv.Core" Version="1.1.2" />
```
If you're want to install the package from Visual Studio, you must open the project/solution in Visual Studio, and open the console using the **Tools** > **NuGet Package Manager** > **Package Manager Console** command and run the install command:
```
Install-Package DotEnv.Core
```
If you are making use of the dotnet CLI, then run the following in your terminal:
```
dotnet add package DotEnv.Core
```

## Usage

### Loading .env file

You must import the namespace types at the beginning of your class file:
```cs
using DotEnv.Core;
```

Then you can load the .env file with the `Load` method of the `EnvLoader` class:
```cs
new EnvLoader().Load();
```
By default, the `Load` method will look for a file called `.env` in the current directory and if it does not find it, it will look for it in the parent directories of the current directory.

The current directory is where the executable with its dependencies is located.

Remember that if no encoding is specified to the `Load` method, the default will be `UTF-8`.

### Accessing the variables

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

### Changing the default name

You can also change the default name of the .env file using the `SetDefaultEnvFileName` method:
```cs
new EnvLoader()
    .SetDefaultEnvFileName(".env.dev")
    .Load();
```
Now the `Load` method will look for the **.env.dev** file in the current directory and in the parent directories if it is not found in the current directory.

Another case would be:
```cs
new EnvLoader()
    .SetDefaultEnvFileName(".env.dev")
    .AddEnvFiles("/foo/foo2", "/bar/bar2")
    .Load();
```
The `Load` method will look for two `.env.dev` files in the paths `/foo/foo2` and `/bar/bar2`.

### Specifying the path absolute

You can also specify the absolute path to the .env file:
```cs
new EnvLoader()
    .AddEnvFile("/home/App/.env.dev")
    .Load();
```
In this case the `Load` method will search for the file `.env.dev` in the path `/home/App`, if it does not find it, the method will not search in the current directory or parent directories.

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
    .SetEncoding(Encoding.Unicode)
    .Load();
```

You can also specify an encoding type for each .env file using the `AddEnvFile` method:
```cs
new EnvLoader()
    .AddEnvFile("env.example", Encoding.Unicode)
    .AddEnvFile("env.example2", Encoding.ASCII)
    .Load();
```

### Specifying the path relative

You can also specify a relative path using the `AddEnvFile` method:
```cs
new EnvLoader()
    .AddEnvFile("./dotenv/files")
    .Load();
```
In this case, the `.env` file is inside a directory, i.e. in `files`. The `Load` method will look for the file `dotenv/files/.env` in the current directory, if it does not find it, it will look for it in parent directories.

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

### Throw FileNotFoundException

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

### Interpolating variables

Sometimes you will need to interpolate variables within a value, for example:
```
MYSQL_USER=root
MYSQL_ROOT_PASSWORD=1234
CONNECTION_STRING=username=${MYSQL_USER};password=${MYSQL_ROOT_PASSWORD};database=testdb;
```
If the variable embedded in the value does not exist in the current process, the parser will throw an exception, for example:
```
CONNECTION_STRING=username=${MYSQL_USER};password=${MYSQL_ROOT_PASSWORD};database=testdb;
MYSQL_USER=root
MYSQL_ROOT_PASSWORD=1234
```
In the above example, the parser should throw an exception because the `MYSQL_USER` variable does not exist yet.

**Note:** If you don't know what each class does, don't forget to check the [API documentation](xref:DotEnv.Core).