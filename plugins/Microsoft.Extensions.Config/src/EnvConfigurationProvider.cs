using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DotEnv.Core;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// An ENV file based <see cref="ConfigurationProvider"/>.
/// </summary>
/// <examples>
/// KEY1=VALUE1
/// # comment
/// </examples>
public class EnvConfigurationProvider : ConfigurationProvider
{
    private readonly EnvConfigurationSource _source;

    /// <summary>
    /// Initializes a new instance with the specified source.
    /// </summary>
    /// <param name="source">The source settings.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> is <c>null</c>.
    /// </exception>
    public EnvConfigurationProvider(EnvConfigurationSource source)
    {
        ThrowHelper.ThrowIfNull(source, nameof(source));
        _source = source;
    }

    /// <summary>
    /// Loads the data for this provider.
    /// </summary>
    public override void Load()
    {
        var provider = new DictionaryProvider(StringComparer.OrdinalIgnoreCase);
        new EnvLoader()
            .AddEnvFile(_source.Path, _source.Optional)
            .SetEnvironmentVariablesProvider(provider)
            .EnableFileNotFoundException()
            .Load();

        Data = provider.ToDictionary(
            keyValuePair => NormalizeKey(keyValuePair.Key),
            keyValuePair => keyValuePair.Value,
            StringComparer.OrdinalIgnoreCase);
    }

    private static string NormalizeKey(string key) 
        => key.Replace("__", ConfigurationPath.KeyDelimiter);
}
