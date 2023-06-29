**DotEnv.Core.Props** is a file that represents a set of properties that will tell MSBuild to copy the .env files from the project folder to the publish directory.

This .props file excludes the following sample .env files:
- `.env.example`
- `example.env`
- `.env.sample`
- `sample.env`
- `.env.template`
- `template.env`

## Installation

Run the following command from the terminal:
```
dotnet add package DotEnv.Core.Props
```

## How to use it?

The .props file does not need to be used directly, simply run the `dotnet publish` command and MSBuild will copy the .env files to the publish directory.

