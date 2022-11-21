using System;
using System.Collections.Generic;

namespace DotEnv.Core;

/// <summary>
/// Allows binding strongly typed objects to configuration values.
/// </summary>
public interface IEnvBinder
{
    /// <param name="result">The result contains the errors found by the binder.</param>
    /// <inheritdoc cref="Bind()" />
    TSettings Bind<TSettings>(out EnvValidationResult result) where TSettings : new();

    /// <summary>
    /// Binds the instance of the environment variables provider to a new instance of type TSettings.
    /// </summary>
    /// <typeparam name="TSettings">The type of the new instance to bind.</typeparam>
    /// <exception cref="BinderException">
    /// If the binder encounters one or more errors.
    /// </exception>
    /// <remarks>Binds only read-write properties and, moreover, only sets public properties.</remarks>
    /// <returns>The new instance of TSettings.</returns>
    TSettings Bind<TSettings>() where TSettings : new();

    /// <summary>
    /// Disables/ignores <see cref="BinderException" />. This method tells the binder not to throw an exception when it encounters one or more errors.
    /// </summary>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvBinder IgnoreException();

    /// <summary>
    /// Allows binding of non-public properties.
    /// This method tells the binder that it can set non-public properties.
    /// </summary>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvBinder AllowBindNonPublicProperties();
}