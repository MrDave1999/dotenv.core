using System;
using System.Collections.Generic;
using System.Text;
using DotEnv.Core;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Represents an ENV file as an <see cref="IConfigurationSource"/>.
/// </summary>
/// <examples>
/// KEY1=VALUE1
/// # comment
/// </examples>
public class EnvConfigurationSource : IConfigurationSource
{
    /// <summary>
    /// The path to the file.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Determines if loading the file is optional.
    /// </summary>
    public bool Optional { get; set; }

    /// <summary>
    /// Builds the <see cref="EnvConfigurationProvider"/> for this source.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
    /// <returns>An <see cref="EnvConfigurationProvider"/></returns>
    /// <exception cref="InvalidOperationException">
    /// <see cref="Path"/> is null, empty or consists only of white-space characters.
    /// </exception>
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        if (string.IsNullOrWhiteSpace(Path))
            throw new InvalidOperationException(ExceptionMessages.PathIsInvalid);

        return new EnvConfigurationProvider(this);
    }
}
