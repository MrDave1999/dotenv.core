using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;
using System.Linq;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvValidator" />
    public class EnvValidator : IEnvValidator
    {
        /// <summary>
        /// Allows access to the configuration options for the validator.
        /// </summary>
        private readonly EnvValidatorOptions _configuration = new EnvValidatorOptions();

        /// <summary>
        /// Allows access to the errors container of the validator.
        /// </summary>
        private readonly EnvValidationResult _validationResult = new EnvValidationResult();

        /// <inheritdoc />
        public void Validate()
            => Validate(out _);

        /// <inheritdoc />
        public void Validate(out EnvValidationResult result)
        {
            result = _validationResult;
            if (_configuration.RequiredKeys == null)
                throw new InvalidOperationException(RequiredKeysNotSetMessage);

            foreach(var key in _configuration.RequiredKeys)
            {
                var retrievedValue = Environment.GetEnvironmentVariable(key);
                if (retrievedValue == null)
                    _validationResult.Add(FormatEnvVariableNotFoundExceptionMessage(VariableNotFoundMessage, key));
            }

            if (_validationResult.HasError() && _configuration.ThrowException)
                throw new EnvVariableNotFoundException(message: _validationResult.ErrorMessages);
        }

        /// <inheritdoc />
        public IEnvValidator SetRequiredKeys(params string[] keys)
        {
            _ = keys ?? throw new ArgumentNullException(nameof(keys));
            if (keys.IsEmpty())
                throw new ArgumentException(LengthOfParamsListIsZeroMessage, nameof(keys));

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
            _ = keysType ?? throw new ArgumentNullException(nameof(keysType));
            var readablePropertyNames =
                from propertyInfo in keysType.GetProperties()
                where propertyInfo.CanRead && propertyInfo.PropertyType == typeof(string)
                select propertyInfo.Name;
            _configuration.RequiredKeys = readablePropertyNames;
            return this;
        }
    }
}