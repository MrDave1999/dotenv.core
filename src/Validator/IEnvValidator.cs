using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

/// <summary>
/// Represents the validator of the required keys.
/// </summary>
public interface IEnvValidator
{
    /// <param name="result">The result contains the errors found by the validator.</param>
    /// <inheritdoc cref="Validate()" />
    void Validate(out EnvValidationResult result);

    /// <summary>
    /// Validates whether the required keys are present in the application.
    /// </summary>
    /// <exception cref="InvalidOperationException">The required keys are not specified with the <c>SetRequiredKeys</c> method.</exception>
    /// <exception cref="RequiredKeysNotPresentException">
    /// If the required keys are not present in the application.
    /// This exception is not thrown if the <see cref="IgnoreException" /> method is invoked.
    /// </exception>
    void Validate();

    /// <summary>
    /// Sets the required keys by means of a string collection.
    /// </summary>
    /// <param name="keys">The required keys to set.</param>
    /// <exception cref="ArgumentNullException"><c>keys</c> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The length of the <c>keys</c> list is zero.</exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvValidator SetRequiredKeys(params string[] keys);

    /// <summary>
    /// Sets the required keys by means of the properties of a class or struct.
    /// </summary>
    /// <typeparam name="TKeys">The type with the required keys.</typeparam>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvValidator SetRequiredKeys<TKeys>();

    /// <summary>
    /// Sets the required keys by means of the properties of a class or struct.
    /// </summary>
    /// <param name="keysType">The type with the required keys.</param>
    /// <exception cref="ArgumentNullException"><c>keysType</c> is <c>null</c>.</exception>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvValidator SetRequiredKeys(Type keysType);

    /// <summary>
    /// Disables/ignores <see cref="RequiredKeysNotPresentException" />. This method tells the validator not to throw an exception when it encounters one or more errors.
    /// </summary>
    /// <returns>An instance implementing the fluent interface.</returns>
    IEnvValidator IgnoreException();
}
