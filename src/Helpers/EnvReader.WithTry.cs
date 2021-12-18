﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static System.Environment;

namespace DotEnv.Core
{
    // These methods return false if the environment variable does not exist.
    public partial class EnvReader
    {
        /// <inheritdoc />
        public virtual bool TryGetStringValue(string variable, out string value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if(variableValue == null)
            {
                value = default;
                return false;
            }
            value = variableValue;
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetBoolValue(string variable, out bool value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = bool.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetByteValue(string variable, out byte value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = byte.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetSByteValue(string variable, out sbyte value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = sbyte.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetCharValue(string variable, out char value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = char.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetIntValue(string variable, out int value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = int.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetUIntValue(string variable, out uint value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = uint.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetLongValue(string variable, out long value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = long.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetULongValue(string variable, out ulong value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = ulong.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetShortValue(string variable, out short value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = short.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetUShortValue(string variable, out ushort value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = ushort.Parse(variableValue);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetDecimalValue(string variable, out decimal value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = decimal.Parse(variableValue, CultureInfo.InvariantCulture);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetDoubleValue(string variable, out double value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = double.Parse(variableValue, CultureInfo.InvariantCulture);
            return true;
        }

        /// <inheritdoc />
        public virtual bool TryGetFloatValue(string variable, out float value)
        {
            string variableValue = GetEnvironmentVariable(variable);
            if (variableValue == null)
            {
                value = default;
                return false;
            }
            value = float.Parse(variableValue, CultureInfo.InvariantCulture);
            return true;
        }
    }
}