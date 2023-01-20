using System.Text;
using System;
using System.Collections.Generic;
using System.Reflection;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core;

/// <inheritdoc cref="IEnvBinder" />
public class EnvBinder : IEnvBinder
{
    /// <summary>
    /// Allows access to the configuration options for the binder.
    /// </summary>
    private readonly EnvBinderOptions _configuration = new();

    /// <summary>
    /// Allows access to the errors container of the binder.
    /// </summary>
    private readonly EnvValidationResult _validationResult = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvBinder" /> class.
    /// </summary>
    public EnvBinder()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvBinder" /> class with environment variables provider.
    /// </summary>
    /// <param name="provider">The environment variables provider.</param>
    public EnvBinder(IEnvironmentVariablesProvider provider)
    {
        _configuration.EnvVars = provider;
    }

    /// <inheritdoc />
    public TSettings Bind<TSettings>() where TSettings : new()
        => Bind<TSettings>(out _);

    /// <inheritdoc />
    public TSettings Bind<TSettings>(out EnvValidationResult result) where TSettings : new()
    {
        // Save a new key with the UpperCaseSnakeCase convention.
        var newKey     = new StringBuilder(capacity: 40);
        var envVars    = _configuration.EnvVars;
        var settings   = new TSettings();
        var type       = typeof(TSettings);
        var properties = _configuration.BindNonPublicProperties ? type.GetPublicAndNonPublicProperties() : type.GetProperties();
        result         = _validationResult;
        foreach (PropertyInfo property in properties)
        {
            if(IsReadOnlyOrWriteOnly(property)) 
                continue;

            var envKeyAttribute = (EnvKeyAttribute)Attribute.GetCustomAttribute(property, typeof(EnvKeyAttribute));
            var variableName    = envKeyAttribute is not null ? envKeyAttribute.Name : property.Name;
            var retrievedValue  = envVars[variableName];
            retrievedValue    ??= envKeyAttribute is not null ? retrievedValue : envVars[variableName.ToUpperCaseSnakeCase(newKey)];
            newKey.Clear();
            if (retrievedValue is null)
            {
                string errorMsg;
                if(envKeyAttribute is null)
                {
                    errorMsg = string.Format(
                        PropertyDoesNotMatchConfigKeyMessage,
                        type.Name,
                        property.Name
                    );
                }
                else 
                {
                    errorMsg = string.Format(
                        KeyAssignedToPropertyIsNotSetMessage,
                        type.Name,
                        property.Name,
                        envKeyAttribute.Name
                    );
                }
                _validationResult.Add(errorMsg);
                continue;
            }

            try
            {
                property.SetValue(settings, DotEnvHelper.ChangeType(retrievedValue, property.PropertyType));
            }
            catch (FormatException)
            {
                _validationResult.Add(errorMsg: string.Format(
                    FailedConvertConfigurationValueMessage, 
                    variableName, 
                    property.PropertyType.Name, 
                    retrievedValue, 
                    property.PropertyType.Name
                ));
            }
        }

        if(_validationResult.HasError() && _configuration.ThrowException)
            throw new BinderException(message: _validationResult.ErrorMessages);

        return settings;
    }

    /// <summary>
    /// Checks whether the property is read-only or write-only.
    /// </summary>
    /// <returns><c>true</c> if the property is read-only or write-only, or <c>false</c> if the property is read-write.</returns>
    private bool IsReadOnlyOrWriteOnly(PropertyInfo property)
        => !property.CanRead || !property.CanWrite;
        
    /// <inheritdoc />
    public IEnvBinder IgnoreException()
    {
        _configuration.ThrowException = false;
        return this;
    }

    /// <inheritdoc />
    public IEnvBinder AllowBindNonPublicProperties()
    {
        _configuration.BindNonPublicProperties = true;
        return this;
    }
}