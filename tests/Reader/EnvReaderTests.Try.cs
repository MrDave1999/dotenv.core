namespace DotEnv.Core.Tests.Reader;

public partial class EnvReaderTests
{
    [TestMethod]
    public void TryGetBoolValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BOOL", "true");
        reader.TryGetBoolValue("KEY_BOOL", out bool actual);
        actual.Should().BeTrue();
        SetEnvironmentVariable("KEY_BOOL", "false");
        reader.TryGetBoolValue("KEY_BOOL", out actual);
        actual.Should().BeFalse();
        reader.TryGetBoolValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetByteValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BYTE", "2");
        reader.TryGetByteValue("KEY_BYTE", out byte actual);
        actual.Should().Be(2);
        reader.TryGetByteValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetCharValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_CHAR", "A");
        reader.TryGetCharValue("KEY_CHAR", out char actual);
        actual.Should().Be('A');
        reader.TryGetCharValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetDecimalValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DECIMAL", "12.5");
        reader.TryGetDecimalValue("KEY_DECIMAL", out decimal actual);
        actual.Should().Be(12.5M);
        SetEnvironmentVariable("KEY_DECIMAL", "12,5");
        reader.TryGetDecimalValue("KEY_DECIMAL", out actual);
        actual.Should().Be(125M);
        reader.TryGetDecimalValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetDoubleValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DOUBLE", "12.5");
        reader.TryGetDoubleValue("KEY_DOUBLE", out double actual);
        actual.Should().Be(12.5D);
        SetEnvironmentVariable("KEY_DOUBLE", "12,5");
        reader.TryGetDoubleValue("KEY_DOUBLE", out actual);
        actual.Should().Be(125D);
        reader.TryGetDoubleValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetFloatValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_FLOAT", "12.5");
        reader.TryGetFloatValue("KEY_FLOAT", out float actual);
        actual.Should().Be(12.5F);
        SetEnvironmentVariable("KEY_FLOAT", "12,5");
        reader.TryGetFloatValue("KEY_FLOAT", out actual);
        actual.Should().Be(125F);
        reader.TryGetFloatValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetIntValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_INT", "3");
        reader.TryGetIntValue("KEY_INT", out int actual);
        actual.Should().Be(3);
        reader.TryGetIntValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetLongValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_LONG", "3");
        reader.TryGetLongValue("KEY_LONG", out long actual);
        actual.Should().Be(3L);
        reader.TryGetLongValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetSByteValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SBYTE", "3");
        reader.TryGetSByteValue("KEY_SBYTE", out sbyte actual);
        actual.Should().Be(3);
        reader.TryGetSByteValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetShortValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SHORT", "3");
        reader.TryGetShortValue("KEY_SHORT", out short actual);
        actual.Should().Be(3);
        reader.TryGetShortValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetStringValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_STRING", "This is a string.");
        reader.TryGetStringValue("KEY_STRING", out string actual);
        actual.Should().Be("This is a string.");
        reader.TryGetStringValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetUIntValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_UINT", "2");
        reader.TryGetUIntValue("KEY_UINT", out uint actual);
        actual.Should().Be(2U);
        reader.TryGetUIntValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetULongValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_ULONG", "2");
        reader.TryGetULongValue("KEY_ULONG", out ulong actual);
        actual.Should().Be(2UL);
        reader.TryGetULongValue(VariableNotFound, out _).Should().BeFalse();
    }

    [TestMethod]
    public void TryGetUShortValue_WhenVariableIsSet_ShouldTryGetValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_USHORT", "2");
        reader.TryGetUShortValue("KEY_USHORT", out ushort actual);
        actual.Should().Be(2);
        reader.TryGetUShortValue(VariableNotFound, out _).Should().BeFalse();
    }
}
