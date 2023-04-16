namespace DotEnv.Core.Tests.Reader;

public partial class EnvReaderTests
{
    [TestMethod]
    public void GetBoolValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BOOL", "true");
        reader.GetBoolValue("KEY_BOOL").Should().BeTrue();
        SetEnvironmentVariable("KEY_BOOL", "false");
        reader.GetBoolValue("KEY_BOOL").Should().BeFalse();
        Action act = () => reader.GetBoolValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetByteValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BYTE", "2");
        reader.GetByteValue("KEY_BYTE").Should().Be(2);
        Action act = () => reader.GetByteValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetCharValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_CHAR", "A");
        reader.GetCharValue("KEY_CHAR").Should().Be('A');
        Action act = () => reader.GetCharValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetDecimalValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DECIMAL", "12.5");
        reader.GetDecimalValue("KEY_DECIMAL").Should().Be(12.5M);
        SetEnvironmentVariable("KEY_DECIMAL", "12,5");
        reader.GetDecimalValue("KEY_DECIMAL").Should().Be(125M);
        Action act = () => reader.GetDecimalValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetDoubleValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DOUBLE", "12.5");
        reader.GetDoubleValue("KEY_DOUBLE").Should().Be(12.5D);
        SetEnvironmentVariable("KEY_DOUBLE", "12,5");
        reader.GetDoubleValue("KEY_DOUBLE").Should().Be(125D);
        Action act = () => reader.GetDoubleValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetFloatValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_FLOAT", "12.5");
        reader.GetFloatValue("KEY_FLOAT").Should().Be(12.5F);
        SetEnvironmentVariable("KEY_FLOAT", "12,5");
        reader.GetFloatValue("KEY_FLOAT").Should().Be(125F);
        Action act = () => reader.GetFloatValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetIntValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_INT", "3");
        reader.GetIntValue("KEY_INT").Should().Be(3);
        Action act = () => reader.GetIntValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetLongValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_LONG", "3");
        reader.GetLongValue("KEY_LONG").Should().Be(3L);
        Action act = () => reader.GetLongValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetSByteValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SBYTE", "3");
        reader.GetSByteValue("KEY_SBYTE").Should().Be(3);
        Action act = () => reader.GetSByteValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetShortValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SHORT", "3");
        reader.GetShortValue("KEY_SHORT").Should().Be(3);
        Action act = () => reader.GetShortValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetStringValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_STRING", "This is a string.");
        reader.GetStringValue("KEY_STRING").Should().Be("This is a string.");
        Action act = () => reader.GetStringValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetUIntValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_UINT", "2");
        reader.GetUIntValue("KEY_UINT").Should().Be(2U);
        Action act = () => reader.GetUIntValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetULongValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_ULONG", "2");
        reader.GetULongValue("KEY_ULONG").Should().Be(2UL);
        Action act = () => reader.GetULongValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }

    [TestMethod]
    public void GetUShortValue_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_USHORT", "2");
        reader.GetUShortValue("KEY_USHORT").Should().Be(2);
        Action act = () => reader.GetUShortValue(VariableNotFound);
        act.Should().Throw<VariableNotSetException>();
    }
}
