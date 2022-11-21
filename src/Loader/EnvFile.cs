using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// Represents an env file.
/// </summary>
internal class EnvFile
{
    /// <summary>
    /// Gets or sets the path of an .env file.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the encoding of an .env file.
    /// </summary>
    public Encoding Encoding { get; set; }

    /// <summary>
    /// A value indicating whether the .env file is present in any directory. Its default value is true.
    /// </summary>
    public bool Exists { get; set; } = true;

    /// <summary>
    /// A value indicating whether the existence of the .env file is optional or not.
    /// </summary>
    public bool Optional { get; set; }
}
