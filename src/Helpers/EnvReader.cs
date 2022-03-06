using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;
using static System.Environment;

namespace DotEnv.Core
{
    /// <inheritdoc cref="IEnvReader" />
    public partial class EnvReader : IEnvReader
    {
        private static readonly EnvReader s_instance = new EnvReader();
        private readonly IEnvironmentVariablesProvider _envVars = new DefaultEnvironmentProvider();

        /// <summary>
        /// Gets an instance of type <see cref="EnvReader" />.
        /// </summary>
        /// <remarks>This method is thread-safe.</remarks>
        public static EnvReader Instance => s_instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvReader" /> class.
        /// </summary>
        public EnvReader()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvReader" /> class with environment variables provider.
        /// </summary>
        /// <param name="envVars">The environment variables provider.</param>
        public EnvReader(IEnvironmentVariablesProvider envVars)
        {
            _envVars = envVars;
        }

        /// <inheritdoc />
        public bool Exists(string variable)
        {
            _ = variable ?? throw new ArgumentNullException(nameof(variable));
            var retrievedValue = _envVars[variable];
            return retrievedValue != null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the variables.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the variables.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            => _envVars.GetEnumerator();

        /// <inheritdoc cref="GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        /// <inheritdoc />
        public virtual string this[string variable] => GetStringValue(variable);

        /// <inheritdoc />
        public virtual string GetStringValue(string variable)
        {
            if (TryGetStringValue(variable, out string value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual bool GetBoolValue(string variable)
        {
            if (TryGetBoolValue(variable, out bool value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual byte GetByteValue(string variable)
        {
            if (TryGetByteValue(variable, out byte value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual sbyte GetSByteValue(string variable)
        {
            if (TryGetSByteValue(variable, out sbyte value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual char GetCharValue(string variable)
        {
            if (TryGetCharValue(variable, out char value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual int GetIntValue(string variable)
        {
            if (TryGetIntValue(variable, out int value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual uint GetUIntValue(string variable)
        {
            if (TryGetUIntValue(variable, out uint value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual long GetLongValue(string variable)
        {
            if (TryGetLongValue(variable, out long value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual ulong GetULongValue(string variable)
        {
            if (TryGetULongValue(variable, out ulong value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual short GetShortValue(string variable)
        {
            if (TryGetShortValue(variable, out short value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual ushort GetUShortValue(string variable)
        {
            if (TryGetUShortValue(variable, out ushort value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual decimal GetDecimalValue(string variable)
        {
            if (TryGetDecimalValue(variable, out decimal value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual double GetDoubleValue(string variable)
        {
            if (TryGetDoubleValue(variable, out double value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }

        /// <inheritdoc />
        public virtual float GetFloatValue(string variable)
        {
            if (TryGetFloatValue(variable, out float value))
                return value;

            throw new EnvVariableNotFoundException(VariableNotFoundMessage, variable);
        }
    }
}
