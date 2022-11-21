using System;
using System.Collections.Generic;
using System.Text;

namespace DotEnv.Core;

// These methods return a default value when the environment variable is not set.
public partial class EnvReader
{
    /// <inheritdoc />
    public virtual string EnvString(string variable, string defaultValue = default)
        => TryGetStringValue(variable, out string value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual bool EnvBool(string variable, bool defaultValue = default)
        => TryGetBoolValue(variable, out bool value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual byte EnvByte(string variable, byte defaultValue = default)
        => TryGetByteValue(variable, out byte value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual sbyte EnvSByte(string variable, sbyte defaultValue = default)
        => TryGetSByteValue(variable, out sbyte value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual char EnvChar(string variable, char defaultValue = default)
        => TryGetCharValue(variable, out char value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual int EnvInt(string variable, int defaultValue = default)
        => TryGetIntValue(variable, out int value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual uint EnvUInt(string variable, uint defaultValue = default)
        => TryGetUIntValue(variable, out uint value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual long EnvLong(string variable, long defaultValue = default)
        => TryGetLongValue(variable, out long value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual ulong EnvULong(string variable, ulong defaultValue = default)
        => TryGetULongValue(variable, out ulong value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual short EnvShort(string variable, short defaultValue = default)
        => TryGetShortValue(variable, out short value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual ushort EnvUShort(string variable, ushort defaultValue = default)
        => TryGetUShortValue(variable, out ushort value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual decimal EnvDecimal(string variable, decimal defaultValue = default)
        => TryGetDecimalValue(variable, out decimal value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual double EnvDouble(string variable, double defaultValue = default)
        => TryGetDoubleValue(variable, out double value) ? value : defaultValue;

    /// <inheritdoc />
    public virtual float EnvFloat(string variable, float defaultValue = default)
        => TryGetFloatValue(variable, out float value) ? value : defaultValue;
}
