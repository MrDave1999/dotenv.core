using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;
using System.Linq;

namespace DotEnv.Core;

/// <inheritdoc cref="IEnvValidator" />
public class EnvValidator : IEnvValidator
{
    /// <summary>
    /// Allows access to the configuration options for the validator.
    /// </summary>
    private readonly EnvValidatorOptions _configuration = new();

    /// <summary>
    /// Allows access to the errors container of the validator.
    /// </summary>
    private readonly EnvValidationResult _validationResult = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvValidator" /> class.
    /// </summary>
    public EnvValidator()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvValidator" /> class with environment variables provider.
    /// </summary>
    /// <param name="provider">The environment variables provider.</param>
    /// <exception cref="ArgumentNullException"><c>provider</c> is <c>null</c>.</exception>
    public EnvValidator(IEnvironmentVariablesProvider provider)
    {
        ThrowHelper.ThrowIfNull(provider, nameof(provider));
        _configuration.EnvVars = provider;
    }

    /// <inheritdoc />
    public void Validate()
        => Validate(out _);

    /// <inheritdoc />
    public void Validate(out EnvValidationResult result)
    {
        result = _validationResult;
        if (_configuration.RequiredKeys is null)
            throw new InvalidOperationException(RequiredKeysNotSpecifiedMessage);

        foreach(var key in _configuration.RequiredKeys)
        {
            var retrievedValue = _configuration.EnvVars[key];
            if (retrievedValue is null)
                _validationResult.Add(string.Format(RequiredKeysNotPresentMessage, key));
        }

        if (_validationResult.HasError() && _configuration.ThrowException)
            throw new RequiredKeysNotPresentException(message: _validationResult.ErrorMessages);
    }

    /// <inheritdoc />
    public IEnvValidator SetRequiredKeys(params string[] keys)
    {
        ThrowHelper.ThrowIfNull(keys, nameof(keys));
        ThrowHelper.ThrowIfEmptyCollection(keys, nameof(keys));
        _configuration.RequiredKeys = keys;
        return this;
    }

    /// <inheritdoc />
    public IEnvValidator IgnoreException()
    {
        _configuration.ThrowException = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvValidator SetRequiredKeys<TKeys>()
    {
        SetRequiredKeys(typeof(TKeys));
        return this;
    }

    /// <inheritdoc />
    public IEnvValidator SetRequiredKeys(Type keysType)
    {
        ThrowHelper.ThrowIfNull(keysType, nameof(keysType));
        var readablePropertyNames =
            from propertyInfo in keysType.GetProperties()
            where propertyInfo.CanRead && propertyInfo.PropertyType == typeof(string)
            select propertyInfo.Name;
        _configuration.RequiredKeys = readablePropertyNames;
        return this;
    }
}