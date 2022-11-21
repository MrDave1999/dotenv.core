using System;
using System.Collections.Generic;
using System.Text;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core;

// These methods throw an exception when the environment variable is not set.
public partial class EnvReader
{
    /// <inheritdoc />
    public virtual string this[string variable] => GetStringValue(variable);

    /// <inheritdoc />
    public virtual string GetStringValue(string variable)
    {
        if (TryGetStringValue(variable, out string value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual bool GetBoolValue(string variable)
    {
        if (TryGetBoolValue(variable, out bool value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual byte GetByteValue(string variable)
    {
        if (TryGetByteValue(variable, out byte value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual sbyte GetSByteValue(string variable)
    {
        if (TryGetSByteValue(variable, out sbyte value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual char GetCharValue(string variable)
    {
        if (TryGetCharValue(variable, out char value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual int GetIntValue(string variable)
    {
        if (TryGetIntValue(variable, out int value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual uint GetUIntValue(string variable)
    {
        if (TryGetUIntValue(variable, out uint value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual long GetLongValue(string variable)
    {
        if (TryGetLongValue(variable, out long value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual ulong GetULongValue(string variable)
    {
        if (TryGetULongValue(variable, out ulong value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual short GetShortValue(string variable)
    {
        if (TryGetShortValue(variable, out short value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual ushort GetUShortValue(string variable)
    {
        if (TryGetUShortValue(variable, out ushort value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual decimal GetDecimalValue(string variable)
    {
        if (TryGetDecimalValue(variable, out decimal value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual double GetDoubleValue(string variable)
    {
        if (TryGetDoubleValue(variable, out double value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }

    /// <inheritdoc />
    public virtual float GetFloatValue(string variable)
    {
        if (TryGetFloatValue(variable, out float value))
            return value;

        throw new VariableNotSetException(string.Format(VariableNotSetMessage, variable), nameof(variable));
    }
}
