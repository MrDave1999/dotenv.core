namespace DotEnv.Core.Tests.Reader;

public partial class EnvReaderTests
{
    [TestMethod]
    public void EnvBool_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BOOL", "true");
        Assert.AreEqual(expected: true, actual: reader.EnvBool("KEY_BOOL"));
        SetEnvironmentVariable("KEY_BOOL", "false");
        Assert.AreEqual(expected: false, actual: reader.EnvBool("KEY_BOOL"));
        Assert.AreEqual(expected: default, actual: reader.EnvBool(VariableNotFound));
        Assert.AreEqual(expected: true, actual: reader.EnvBool(VariableNotFound, true));
    }

    [TestMethod]
    public void EnvByte_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_BYTE", "2");
        Assert.AreEqual(expected: (byte)2, actual: reader.EnvByte("KEY_BYTE"));
        Assert.AreEqual(expected: default, actual: reader.EnvByte(VariableNotFound));
        Assert.AreEqual(expected: (byte)2, actual: reader.EnvByte(VariableNotFound, 2));
    }

    [TestMethod]
    public void EnvChar_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_CHAR", "A");
        Assert.AreEqual(expected: 'A', actual: reader.EnvChar("KEY_CHAR"));
        Assert.AreEqual(expected: default, actual: reader.EnvChar(VariableNotFound));
        Assert.AreEqual(expected: 'B', actual: reader.EnvChar(VariableNotFound, 'B'));
    }

    [TestMethod]
    public void EnvDecimal_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DECIMAL", "12.5");
        Assert.AreEqual(expected: 12.5M, actual: reader.EnvDecimal("KEY_DECIMAL"));
        SetEnvironmentVariable("KEY_DECIMAL", "12,5");
        Assert.AreEqual(expected: 125M, actual: reader.EnvDecimal("KEY_DECIMAL"));
        Assert.AreEqual(expected: default, actual: reader.EnvDecimal(VariableNotFound));
        Assert.AreEqual(expected: 2.5M, actual: reader.EnvDecimal(VariableNotFound, 2.5M));
    }

    [TestMethod]
    public void EnvDouble_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_DOUBLE", "12.5");
        Assert.AreEqual(expected: 12.5D, actual: reader.EnvDouble("KEY_DOUBLE"));
        SetEnvironmentVariable("KEY_DOUBLE", "12,5");
        Assert.AreEqual(expected: 125D, actual: reader.EnvDouble("KEY_DOUBLE"));
        Assert.AreEqual(expected: default, actual: reader.EnvDouble(VariableNotFound));
        Assert.AreEqual(expected: 2.5D, actual: reader.EnvDouble(VariableNotFound, 2.5D));
    }

    [TestMethod]
    public void EnvFloat_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_FLOAT", "12.5");
        Assert.AreEqual(expected: 12.5F, actual: reader.EnvFloat("KEY_FLOAT"));
        SetEnvironmentVariable("KEY_FLOAT", "12,5");
        Assert.AreEqual(expected: 125F, actual: reader.EnvFloat("KEY_FLOAT"));
        Assert.AreEqual(expected: default, actual: reader.EnvFloat(VariableNotFound));
        Assert.AreEqual(expected: 2.5F, actual: reader.EnvFloat(VariableNotFound, 2.5F));
    }

    [TestMethod]
    public void EnvInt_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_INT", "3");
        Assert.AreEqual(expected: 3, actual: reader.EnvInt("KEY_INT"));
        Assert.AreEqual(expected: default, actual: reader.EnvInt(VariableNotFound));
        Assert.AreEqual(expected: 3, actual: reader.EnvInt(VariableNotFound, 3));
    }

    [TestMethod]
    public void EnvLong_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_LONG", "3");
        Assert.AreEqual(expected: 3L, actual: reader.EnvLong("KEY_LONG"));
        Assert.AreEqual(expected: default, actual: reader.EnvLong(VariableNotFound));
        Assert.AreEqual(expected: 3L, actual: reader.EnvLong(VariableNotFound, 3));
    }


    [TestMethod]
    public void EnvSByte_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SBYTE", "3");
        Assert.AreEqual(expected: (sbyte)3, actual: reader.EnvSByte("KEY_SBYTE"));
        Assert.AreEqual(expected: default, actual: reader.EnvSByte(VariableNotFound));
        Assert.AreEqual(expected: (sbyte)3, actual: reader.EnvSByte(VariableNotFound, 3));
    }

    [TestMethod]
    public void EnvShort_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_SHORT", "3");
        Assert.AreEqual(expected: (short)3, actual: reader.EnvShort("KEY_SHORT"));
        Assert.AreEqual(expected: default, actual: reader.EnvShort(VariableNotFound));
        Assert.AreEqual(expected: (short)3, actual: reader.EnvShort(VariableNotFound, 3));
    }

    [TestMethod]
    public void EnvString_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_STRING", "This is a string.");
        Assert.AreEqual(expected: "This is a string.", actual: reader.EnvString("KEY_STRING"));
        Assert.AreEqual(expected: default, actual: reader.EnvString(VariableNotFound));
        Assert.AreEqual(expected: VariableNotFound, actual: reader.EnvString(VariableNotFound, VariableNotFound));
    }

    [TestMethod]
    public void EnvUInt_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_UINT", "2");
        Assert.AreEqual(expected: 2U, actual: reader.EnvUInt("KEY_UINT"));
        Assert.AreEqual(expected: default, actual: reader.EnvUInt(VariableNotFound));
        Assert.AreEqual(expected: 2U, actual: reader.EnvUInt(VariableNotFound, 2));
    }

    [TestMethod]
    public void EnvULong_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_ULONG", "2");
        Assert.AreEqual(expected: 2UL, actual: reader.EnvULong("KEY_ULONG"));
        Assert.AreEqual(expected: default, actual: reader.EnvULong(VariableNotFound));
        Assert.AreEqual(expected: 2UL, actual: reader.EnvULong(VariableNotFound, 2));
    }

    [TestMethod]
    public void EnvUShort_WhenVariableIsSet_ShouldReturnValueOfTheVariable()
    {
        var reader = new EnvReader();
        SetEnvironmentVariable("KEY_USHORT", "2");
        Assert.AreEqual(expected: (ushort)2, actual: reader.EnvUShort("KEY_USHORT"));
        Assert.AreEqual(expected: default, actual: reader.EnvUShort(VariableNotFound));
        Assert.AreEqual(expected: (ushort)2, actual: reader.EnvUShort(VariableNotFound, 2));
    }
}
