using System;
using System.Collections.Generic;
using System.Text;
using DotEnv.Core;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Extension methods for adding <see cref="EnvConfigurationProvider"/>.
/// </summary>
public static class EnvConfigurationExtensions
{
    /// <summary>
    /// Adds the ENV configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">The path to the file.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the <paramref name="path"/> is null, empty or consists only of white-space characters.
    /// </exception>
    public static IConfigurationBuilder AddEnvFile(this IConfigurationBuilder builder, string path)
        => builder.AddEnvFile(path, optional: false);

    /// <summary>
    /// Adds a ENV configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="path">The path to the file.</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the <paramref name="path"/> is null, empty or consists only of white-space characters.
    /// </exception>
    public static IConfigurationBuilder AddEnvFile(
        this IConfigurationBuilder builder,
        string path,
        bool optional)
    {
        ThrowHelper.ThrowIfNull(builder, nameof(builder));
        ThrowHelper.ThrowIfNullOrWhiteSpace(path, nameof(path));
        return builder.AddEnvFile(source =>
        {
            source.Path = path;
            source.Optional = optional;
        });
    }

    /// <summary>
    /// Adds a ENV configuration source to <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="configureSource">Configures the source.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/>, or <paramref name="configureSource"/> is <c>null</c>.
    /// </exception>
    public static IConfigurationBuilder AddEnvFile(
        this IConfigurationBuilder builder,
        Action<EnvConfigurationSource> configureSource)
    {
        ThrowHelper.ThrowIfNull(builder, nameof(builder));
        ThrowHelper.ThrowIfNull(configureSource, nameof(configureSource));
        return builder.Add(configureSource);
    }
}
