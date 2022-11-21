using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DotEnv.Core;

/// <summary>
/// Represents the options for configuring various behaviors of the loader.
/// </summary>
internal class EnvLoaderOptions
{
    /// <summary>
    /// Gets or sets the default name of an .env file.
    /// </summary>
    public string DefaultEnvFileName { get; set; } = ".env";

    /// <summary>
    /// Gets or sets the base path for the .env files.
    /// </summary>
    public string BasePath { get; set; } = "";

    /// <summary>
    /// Gets or sets the collection of .env files.
    /// </summary>
    public List<EnvFile> EnvFiles { get; set; } = new List<EnvFile>();

    /// <summary>
    /// Gets or sets the encoding for the .env files. Its default value is UTF-8.
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// A value indicating whether <see cref="FileNotFoundException" /> may be thrown when the .env file is not found. Its default value is <c>false</c>.
    /// </summary>
    public bool ThrowFileNotFoundException { get; set; }

    /// <summary>
    /// Gets or sets the name of the environment.
    /// </summary>
    public string EnvironmentName { get; set; }

    /// <summary>
    /// A value indicating whether the .env files should be optional. Its default value is <c>false</c>.
    /// </summary>
    public bool Optional { get; set; }

    /// <summary>
    /// A value indicating whether the loader should search the parent directories when it does not find the .env file in a specific directory.
    /// Its default value is <c>true</c>.
    /// </summary>
    public bool SearchParentDirectories { get; set; } = true;
}