namespace DotEnv.Core.Tests.Reader
{
    public partial class EnvReaderTests
    {
        [TestMethod]
        public void TryGetBoolValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_BOOL", "true");
            reader.TryGetBoolValue("KEY_BOOL", out bool actual);
            Assert.AreEqual(expected: true, actual);
            SetEnvironmentVariable("KEY_BOOL", "false");
            reader.TryGetBoolValue("KEY_BOOL", out actual);
            Assert.AreEqual(expected: false, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetBoolValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetByteValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_BYTE", "2");
            reader.TryGetByteValue("KEY_BYTE", out byte actual);
            Assert.AreEqual(expected: (byte)2, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetByteValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetCharValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_CHAR", "A");
            reader.TryGetCharValue("KEY_CHAR", out char actual);
            Assert.AreEqual(expected: 'A', actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetCharValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetDecimalValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_DECIMAL", "12.5");
            reader.TryGetDecimalValue("KEY_DECIMAL", out decimal actual);
            Assert.AreEqual(expected: 12.5M, actual);
            SetEnvironmentVariable("KEY_DECIMAL", "12,5");
            reader.TryGetDecimalValue("KEY_DECIMAL", out actual);
            Assert.AreEqual(expected: 125M, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetDecimalValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetDoubleValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_DOUBLE", "12.5");
            reader.TryGetDoubleValue("KEY_DOUBLE", out double actual);
            Assert.AreEqual(expected: 12.5D, actual);
            SetEnvironmentVariable("KEY_DOUBLE", "12,5");
            reader.TryGetDoubleValue("KEY_DOUBLE", out actual);
            Assert.AreEqual(expected: 125D, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetDoubleValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetFloatValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_FLOAT", "12.5");
            reader.TryGetFloatValue("KEY_FLOAT", out float actual);
            Assert.AreEqual(expected: 12.5F, actual);
            SetEnvironmentVariable("KEY_FLOAT", "12,5");
            reader.TryGetFloatValue("KEY_FLOAT", out actual);
            Assert.AreEqual(expected: 125F, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetFloatValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetIntValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_INT", "3");
            reader.TryGetIntValue("KEY_INT", out int actual);
            Assert.AreEqual(expected: 3, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetIntValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetLongValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_LONG", "3");
            reader.TryGetLongValue("KEY_LONG", out long actual);
            Assert.AreEqual(expected: 3L, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetLongValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetSByteValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_SBYTE", "3");
            reader.TryGetSByteValue("KEY_SBYTE", out sbyte actual);
            Assert.AreEqual(expected: (sbyte)3, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetSByteValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetShortValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_SHORT", "3");
            reader.TryGetShortValue("KEY_SHORT", out short actual);
            Assert.AreEqual(expected: (short)3, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetShortValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetStringValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_STRING", "This is a string.");
            reader.TryGetStringValue("KEY_STRING", out string actual);
            Assert.AreEqual(expected: "This is a string.", actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetStringValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetUIntValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_UINT", "2");
            reader.TryGetUIntValue("KEY_UINT", out uint actual);
            Assert.AreEqual(expected: 2U, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetUIntValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetULongValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_ULONG", "2");
            reader.TryGetULongValue("KEY_ULONG", out ulong actual);
            Assert.AreEqual(expected: 2UL, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetULongValue(VariableNotFound, out _));
        }

        [TestMethod]
        public void TryGetUShortValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_USHORT", "2");
            reader.TryGetUShortValue("KEY_USHORT", out ushort actual);
            Assert.AreEqual(expected: (ushort)2, actual);
            Assert.AreEqual(expected: false, actual: reader.TryGetUShortValue(VariableNotFound, out _));
        }
    }
}
