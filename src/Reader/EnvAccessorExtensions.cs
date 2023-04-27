using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// Represents an accessor of environment variables for the <see cref="string" /> class.
/// </summary>
public static class EnvAccessorExtensions
{
    /// <summary>
    /// Gets the value of an environment variable from the current process.
    /// </summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    /// <exception cref="ArgumentNullException"><c>variable</c> is <c>null</c>.</exception>
    /// <exception cref="VariableNotSetException"><c>variable</c> is not set in current process.</exception>
    public static string GetEnv(this string variable)
        => EnvReader.Instance[variable];
}
