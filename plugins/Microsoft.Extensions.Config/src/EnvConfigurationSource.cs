using System;
using System.Collections.Generic;
using System.Text;

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
    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => new EnvConfigurationProvider(this);
}
