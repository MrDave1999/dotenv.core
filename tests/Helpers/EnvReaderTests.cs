using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;

namespace DotEnv.Core.Tests.Helpers
{
    [TestClass]
    public class EnvReaderTests
    {
        private const string VariableNotFound = nameof(VariableNotFound);

        [TestMethod]
        public void Exists_WhenVariableExistsInTheCurrentProcess_ShouldReturnTrue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("VARIABLE_NAME", "1");
            Assert.AreEqual(true, reader.Exists("VARIABLE_NAME"));
            SetEnvironmentVariable("VARIABLE_NAME", null);
            Assert.AreEqual(false, reader.Exists("VARIABLE_NAME"));
            Assert.AreEqual(false, reader.Exists(""));
        }

        [TestMethod]
        [DataRow(true, "true")]
        [DataRow(false, "false")]
        public void GetBoolValue_WhenTheVariableIsFound_ShouldReturnValue(bool expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_BOOL", value);

            bool actual = reader.GetBoolValue("KEY_BOOL");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetBoolValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetBoolValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetByteValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            byte expected = 2;
            SetEnvironmentVariable("KEY_BYTE", "2");

            byte actual = reader.GetByteValue("KEY_BYTE");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetByteValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetByteValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow('A', "A")]
        [DataRow('a', "a")]
        public void GetCharValue_WhenTheVariableIsFound_ShouldReturnValue(char expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_CHAR", value);

            char actual = reader.GetCharValue("KEY_CHAR");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetCharValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetCharValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow(12.5D, "12.5")]
        [DataRow(-12.5D, "-12.5")]
        [DataRow(125D, "12,5")]
        [DataRow(-125D, "-12,5")]
        public void GetDecimalValue_WhenTheVariableIsFound_ShouldReturnValue(double input, string value)
        {
            var reader = new EnvReader();
            decimal expected = Convert.ToDecimal(input);
            SetEnvironmentVariable("KEY_DECIMAL", value);

            decimal actual = reader.GetDecimalValue("KEY_DECIMAL");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDecimalValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetDecimalValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow(12.5D, "12.5")]
        [DataRow(-12.5D, "-12.5")]
        [DataRow(125D, "12,5")]
        [DataRow(-125D, "-12,5")]
        public void GetDoubleValue_WhenTheVariableIsFound_ShouldReturnValue(double expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_DOUBLE", value);

            double actual = reader.GetDoubleValue("KEY_DOUBLE");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDoubleValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetDoubleValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow(12.5F, "12.5")]
        [DataRow(-12.5F, "-12.5")]
        [DataRow(125F, "12,5")]
        [DataRow(-125F, "-12,5")]
        public void GetFloatValue_WhenTheVariableIsFound_ShouldReturnValue(float expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_FLOAT", value);

            float actual = reader.GetFloatValue("KEY_FLOAT");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetFloatValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetFloatValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void GetIntValue_WhenTheVariableIsFound_ShouldReturnValue(int expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_INT", value);

            int actual = reader.GetIntValue("KEY_INT");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetIntValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetIntValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow(3L, "3")]
        [DataRow(-3L, "-3")]
        public void GetLongValue_WhenTheVariableIsFound_ShouldReturnValue(long expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_LONG", value);

            long actual = reader.GetLongValue("KEY_LONG");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetLongValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetLongValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void GetSByteValue_WhenTheVariableIsFound_ShouldReturnValue(int input, string value)
        {
            var reader = new EnvReader();
            sbyte expected = Convert.ToSByte(input);
            SetEnvironmentVariable("KEY_SBYTE", value);

            sbyte actual = reader.GetSByteValue("KEY_SBYTE");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSByteValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetSByteValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void GetShortValue_WhenTheVariableIsFound_ShouldReturnValue(int input, string value)
        {
            var reader = new EnvReader();
            short expected = Convert.ToInt16(input);
            SetEnvironmentVariable("KEY_SHORT", value);

            short actual = reader.GetShortValue("KEY_SHORT");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetShortValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetShortValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetStringValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            string expected = "This is a string.";
            SetEnvironmentVariable("KEY_STRING", "This is a string.");

            string actual = reader.GetStringValue("KEY_STRING");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStringValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetStringValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetUIntValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            uint expected = 2U;
            SetEnvironmentVariable("KEY_UINT", "2");

            uint actual = reader.GetUIntValue("KEY_UINT");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUIntValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetUIntValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetULongValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            ulong expected = 2UL;
            SetEnvironmentVariable("KEY_ULONG", "2");

            ulong actual = reader.GetULongValue("KEY_ULONG");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetULongValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetULongValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetUShortValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            ushort expected = 2;
            SetEnvironmentVariable("KEY_USHORT", "2");

            ushort actual = reader.GetUShortValue("KEY_USHORT");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetUShortValue_WhenTheVariableIsNotFound_ShouldThrowEnvVariableNotFound()
        {
            var reader = new EnvReader();

            Action action = () => reader.GetUShortValue(VariableNotFound);

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }
    }
}
