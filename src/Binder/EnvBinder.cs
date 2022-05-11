using System;
using System.Collections.Generic;
using System.Reflection;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core
{
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
            var settings = new TSettings();
            var type = typeof(TSettings);
            result = _validationResult;
            foreach (PropertyInfo property in type.GetProperties())
            {
                var envKeyAttribute = (EnvKeyAttribute)Attribute.GetCustomAttribute(property, typeof(EnvKeyAttribute));
                var variableName = envKeyAttribute is not null ? envKeyAttribute.Name : property.Name;
                var retrievedValue = _configuration.EnvVars[variableName];

                if (retrievedValue is null)
                {
                    var errorMsg = envKeyAttribute is not null ? string.Format(KeyAssignedToPropertyIsNotSetMessage, type.Name, property.Name, envKeyAttribute.Name) 
                        : string.Format(PropertyDoesNotMatchConfigKeyMessage, property.Name);
                    _validationResult.Add(errorMsg);
                    continue;
                }

                try
                {
                    property.SetValue(settings, Convert.ChangeType(retrievedValue, property.PropertyType));
                }
                catch (FormatException)
                {
                    _validationResult.Add(errorMsg: string.Format(FailedConvertConfigurationValueMessage, variableName, property.PropertyType.Name, retrievedValue, property.PropertyType.Name));
                }
            }

            if(_validationResult.HasError() && _configuration.ThrowException)
                throw new BinderException(message: _validationResult.ErrorMessages);

            return settings;
        }

        /// <inheritdoc />
        public IEnvBinder IgnoreException()
        {
            _configuration.ThrowException = false;
            return this;
        }
    }
}