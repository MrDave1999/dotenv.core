namespace DotEnv.Core.Tests.Reader;

public partial class EnvReaderTests
{
    [TestMethod]
    public void EnvBool_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BOOL", "true");
        reader.EnvBool("KEY_BOOL").Should().BeTrue();
        SetEnvironmentVariable("KEY_BOOL", "false");
        reader.EnvBool("KEY_BOOL").Should().BeFalse();
        reader.EnvBool(VariableNotFound).Should().Be(default);
        reader.EnvBool(VariableNotFound, true).Should().BeTrue();
    }

    [TestMethod]
    public void EnvByte_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BYTE", "2");
        reader.EnvByte("KEY_BYTE").Should().Be(2);
        reader.EnvByte(VariableNotFound).Should().Be(default);
        reader.EnvByte(VariableNotFound, 2).Should().Be(2);
    }

    [TestMethod]
    public void EnvChar_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_CHAR", "A");
        reader.EnvChar("KEY_CHAR").Should().Be('A');
        reader.EnvChar(VariableNotFound).Should().Be(default);
        reader.EnvChar(VariableNotFound, 'B').Should().Be('B');
    }

    [TestMethod]
    public void EnvDecimal_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DECIMAL", "12.5");
        reader.EnvDecimal("KEY_DECIMAL").Should().Be(12.5M);
        SetEnvironmentVariable("KEY_DECIMAL", "12,5");
        reader.EnvDecimal("KEY_DECIMAL").Should().Be(125M);
        reader.EnvDecimal(VariableNotFound).Should().Be(default);
        reader.EnvDecimal(VariableNotFound, 2.5M).Should().Be(2.5M);
    }

    [TestMethod]
    public void EnvDouble_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DOUBLE", "12.5");
        reader.EnvDouble("KEY_DOUBLE").Should().Be(12.5D);
        SetEnvironmentVariable("KEY_DOUBLE", "12,5");
        reader.EnvDouble("KEY_DOUBLE").Should().Be(125D);
        reader.EnvDouble(VariableNotFound).Should().Be(default);
        reader.EnvDouble(VariableNotFound, 2.5D).Should().Be(2.5D);
    }

    [TestMethod]
    public void EnvFloat_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_FLOAT", "12.5");
        reader.EnvFloat("KEY_FLOAT").Should().Be(12.5F);
        SetEnvironmentVariable("KEY_FLOAT", "12,5");
        reader.EnvFloat("KEY_FLOAT").Should().Be(125F);
        reader.EnvFloat(VariableNotFound).Should().Be(default);
        reader.EnvFloat(VariableNotFound, 2.5F).Should().Be(2.5F);
    }

    [TestMethod]
    public void EnvInt_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_INT", "3");
        reader.EnvInt("KEY_INT").Should().Be(3);
        reader.EnvInt(VariableNotFound).Should().Be(default);
        reader.EnvInt(VariableNotFound, 3).Should().Be(3);
    }

    [TestMethod]
    public void EnvLong_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_LONG", "3");
        reader.EnvLong("KEY_LONG").Should().Be(3L);
        reader.EnvLong(VariableNotFound).Should().Be(default);
        reader.EnvLong(VariableNotFound, 3L).Should().Be(3L);
    }


    [TestMethod]
    public void EnvSByte_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SBYTE", "3");
        reader.EnvSByte("KEY_SBYTE").Should().Be(3);
        reader.EnvSByte(VariableNotFound).Should().Be(default);
        reader.EnvSByte(VariableNotFound, 3).Should().Be(3);
    }

    [TestMethod]
    public void EnvShort_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SHORT", "3");
        reader.EnvShort("KEY_SHORT").Should().Be(3);
        reader.EnvShort(VariableNotFound).Should().Be(default);
        reader.EnvShort(VariableNotFound, 3).Should().Be(3);
    }

    [TestMethod]
    public void EnvString_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_STRING", "This is a string.");
        reader.EnvString("KEY_STRING").Should().Be("This is a string.");
        reader.EnvString(VariableNotFound).Should().Be(default);
        reader.EnvString(VariableNotFound, VariableNotFound).Should().Be(VariableNotFound);
    }

    [TestMethod]
    public void EnvUInt_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_UINT", "2");
        reader.EnvUInt("KEY_UINT").Should().Be(2U);
        reader.EnvUInt(VariableNotFound).Should().Be(default);
        reader.EnvUInt(VariableNotFound, 2U).Should().Be(2U);
    }

    [TestMethod]
    public void EnvULong_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_ULONG", "2");
        reader.EnvULong("KEY_ULONG").Should().Be(2UL);
        reader.EnvULong(VariableNotFound).Should().Be(default);
        reader.EnvULong(VariableNotFound, 2UL).Should().Be(2UL);
    }

    [TestMethod]
    public void EnvUShort_WhenVariableIsSet_ShouldReturnsValue()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_USHORT", "2");
        reader.EnvUShort("KEY_USHORT").Should().Be(2);
        reader.EnvUShort(VariableNotFound).Should().Be(default);
        reader.EnvUShort(VariableNotFound, 2).Should().Be(2);
    }
}
