using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;

namespace DotEnv.Core.Tests.Helpers
{
    [TestClass]
    public class EnvReaderWithTryTests
    {
        private const string VariableNotFound = nameof(VariableNotFound);

        [TestMethod]
        [DataRow(true, "true")]
        [DataRow(false, "false")]
        public void TryGetBoolValue_WhenTheVariableIsFound_ShouldReturnValue(bool expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_BOOL", value);

            reader.TryGetBoolValue("KEY_BOOL", out bool actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetBoolValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetBoolValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void TryGetByteValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            byte expected = 2;
            SetEnvironmentVariable("KEY_BYTE", "2");

            reader.TryGetByteValue("KEY_BYTE", out byte actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetByteValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetByteValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow('A', "A")]
        [DataRow('a', "a")]
        public void TryGetCharValue_WhenTheVariableIsFound_ShouldReturnValue(char expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_CHAR", value);

            reader.TryGetCharValue("KEY_CHAR", out char actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetCharValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetCharValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(12.5D, "12.5")]
        [DataRow(-12.5D, "-12.5")]
        [DataRow(125D, "12,5")]
        [DataRow(-125D, "-12,5")]
        public void TryGetDecimalValue_WhenTheVariableIsFound_ShouldReturnValue(double input, string value)
        {
            var reader = new EnvReader();
            decimal expected = Convert.ToDecimal(input);
            SetEnvironmentVariable("KEY_DECIMAL", value);

            reader.TryGetDecimalValue("KEY_DECIMAL", out decimal actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetDecimalValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetDecimalValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(12.5D, "12.5")]
        [DataRow(-12.5D, "-12.5")]
        [DataRow(125D, "12,5")]
        [DataRow(-125D, "-12,5")]
        public void TryGetDoubleValue_WhenTheVariableIsFound_ShouldReturnValue(double expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_DOUBLE", value);

            reader.TryGetDoubleValue("KEY_DOUBLE", out double actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetDoubleValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetDoubleValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(12.5F, "12.5")]
        [DataRow(-12.5F, "-12.5")]
        [DataRow(125F, "12,5")]
        [DataRow(-125F, "-12,5")]
        public void TryGetFloatValue_WhenTheVariableIsFound_ShouldReturnValue(float expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_FLOAT", value);

            reader.TryGetFloatValue("KEY_FLOAT", out float actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetFloatValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetFloatValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void TryGetIntValue_WhenTheVariableIsFound_ShouldReturnValue(int expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_INT", value);

            reader.TryGetIntValue("KEY_INT", out int actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetIntValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetIntValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(3L, "3")]
        [DataRow(-3L, "-3")]
        public void TryGetLongValue_WhenTheVariableIsFound_ShouldReturnValue(long expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_LONG", value);

            reader.TryGetLongValue("KEY_LONG", out long actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetLongValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetLongValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void TryGetSByteValue_WhenTheVariableIsFound_ShouldReturnValue(int input, string value)
        {
            var reader = new EnvReader();
            sbyte expected = Convert.ToSByte(input);
            SetEnvironmentVariable("KEY_SBYTE", value);

            reader.TryGetSByteValue("KEY_SBYTE", out sbyte actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetSByteValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetSByteValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void TryGetShortValue_WhenTheVariableIsFound_ShouldReturnValue(int input, string value)
        {
            var reader = new EnvReader();
            short expected = Convert.ToInt16(input);
            SetEnvironmentVariable("KEY_SHORT", value);

            reader.TryGetShortValue("KEY_SHORT", out short actual);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetShortValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetShortValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetStringValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            string expected = "This is a string.";
            SetEnvironmentVariable("KEY_STRING", "This is a string.");

            reader.TryGetStringValue("KEY_STRING", out string actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetStringValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetStringValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetUIntValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            uint expected = 2U;
            SetEnvironmentVariable("KEY_UINT", "2");

            reader.TryGetUIntValue("KEY_UINT", out uint actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetUIntValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetUIntValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetULongValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            ulong expected = 2UL;
            SetEnvironmentVariable("KEY_ULONG", "2");

            reader.TryGetULongValue("KEY_ULONG", out ulong actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetULongValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetULongValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetUShortValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            ushort expected = 2;
            SetEnvironmentVariable("KEY_USHORT", "2");

            reader.TryGetUShortValue("KEY_USHORT", out ushort actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryGetUShortValue_WhenTheVariableIsNotFound_ShouldReturnFalse()
        {
            var reader = new EnvReader();
            bool expected = false;

            bool actual = reader.TryGetUShortValue(VariableNotFound, out _);

            Assert.AreEqual(expected, actual);
        }
    }
}
