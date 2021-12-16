using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;

namespace DotEnv.Core.Tests.Helpers
{
    [TestClass]
    public class EnvReaderWithDefaultParamsTests
    {
        private const string VariableNotFound = nameof(VariableNotFound);

        [TestMethod]
        [DataRow(true, "true")]
        [DataRow(false, "false")]
        public void EnvBool_WhenTheVariableIsFound_ShouldReturnValue(bool expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_BOOL", value);

            bool actual = reader.EnvBool("KEY_BOOL");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvBool_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            bool expected1 = default;
            bool expected2 = true;
            bool actual;

            actual = reader.EnvBool(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvBool(VariableNotFound, true);
            Assert.AreEqual(expected2, actual);

        }

        [TestMethod]
        public void EnvByte_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            byte expected = 2;
            SetEnvironmentVariable("KEY_BYTE", "2");

            byte actual = reader.EnvByte("KEY_BYTE");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvByte_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            byte expected1 = default;
            byte expected2 = 2;
            byte actual;

            actual = reader.EnvByte(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvByte(VariableNotFound, 2);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow('A', "A")]
        [DataRow('a', "a")]
        public void EnvChar_WhenTheVariableIsFound_ShouldReturnValue(char expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_CHAR", value);

            char actual = reader.EnvChar("KEY_CHAR");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvChar_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            char expected1 = default;
            char expected2 = 'B';
            char actual;

            actual = reader.EnvChar(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvChar(VariableNotFound, 'B');
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow(12.5D, "12.5")]
        [DataRow(-12.5D, "-12.5")]
        [DataRow(125D, "12,5")]
        [DataRow(-125D, "-12,5")]
        public void EnvDecimal_WhenTheVariableIsFound_ShouldReturnValue(double input, string value)
        {
            var reader = new EnvReader();
            decimal expected = Convert.ToDecimal(input);
            SetEnvironmentVariable("KEY_DECIMAL", value);

            decimal actual = reader.EnvDecimal("KEY_DECIMAL");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvDecimal_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            decimal expected1 = default;
            decimal expected2 = 2.5M;
            decimal actual;

            actual = reader.EnvDecimal(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvDecimal(VariableNotFound, 2.5M);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow(12.5D, "12.5")]
        [DataRow(-12.5D, "-12.5")]
        [DataRow(125D, "12,5")]
        [DataRow(-125D, "-12,5")]
        public void EnvDouble_WhenTheVariableIsFound_ShouldReturnValue(double expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_DOUBLE", value);

            double actual = reader.EnvDouble("KEY_DOUBLE");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvDouble_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            double expected1 = default;
            double expected2 = 2.5D;
            double actual;

            actual = reader.EnvDouble(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvDouble(VariableNotFound, 2.5D);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow(12.5F, "12.5")]
        [DataRow(-12.5F, "-12.5")]
        [DataRow(125F, "12,5")]
        [DataRow(-125F, "-12,5")]
        public void EnvFloat_WhenTheVariableIsFound_ShouldReturnValue(float expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_FLOAT", value);

            float actual = reader.EnvFloat("KEY_FLOAT");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvFloat_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            float expected1 = default;
            float expected2 = 2.5F;
            float actual;

            actual = reader.EnvFloat(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvFloat(VariableNotFound, 2.5F);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void EnvInt_WhenTheVariableIsFound_ShouldReturnValue(int expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_INT", value);

            int actual = reader.EnvInt("KEY_INT");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvInt_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            int expected1 = default;
            int expected2 = 3;
            int actual;

            actual = reader.EnvInt(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvInt(VariableNotFound, 3);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow(3L, "3")]
        [DataRow(-3L, "-3")]
        public void EnvLong_WhenTheVariableIsFound_ShouldReturnValue(long expected, string value)
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_LONG", value);

            long actual = reader.EnvLong("KEY_LONG");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvLong_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            long expected1 = default;
            long expected2 = 3;
            long actual;

            actual = reader.EnvLong(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvLong(VariableNotFound, 3);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void EnvSByte_WhenTheVariableIsFound_ShouldReturnValue(int input, string value)
        {
            var reader = new EnvReader();
            sbyte expected = Convert.ToSByte(input);
            SetEnvironmentVariable("KEY_SBYTE", value);

            sbyte actual = reader.EnvSByte("KEY_SBYTE");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvSByte_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            sbyte expected1 = default;
            sbyte expected2 = 3;
            sbyte actual;

            actual = reader.EnvSByte(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvSByte(VariableNotFound, 3);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        [DataRow(3, "3")]
        [DataRow(-3, "-3")]
        public void EnvShort_WhenTheVariableIsFound_ShouldReturnValue(int input, string value)
        {
            var reader = new EnvReader();
            short expected = Convert.ToInt16(input);
            SetEnvironmentVariable("KEY_SHORT", value);

            short actual = reader.EnvShort("KEY_SHORT");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvShort_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            short expected1 = default;
            short expected2 = 3;
            short actual;

            actual = reader.EnvShort(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvShort(VariableNotFound, 3);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        public void EnvString_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            string expected = "This is a string.";
            SetEnvironmentVariable("KEY_STRING", "This is a string.");

            string actual = reader.EnvString("KEY_STRING");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvString_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            string expected1 = default;
            string expected2 = VariableNotFound;
            string actual;

            actual = reader.EnvString(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvString(VariableNotFound, VariableNotFound);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        public void EnvUInt_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            uint expected = 2U;
            SetEnvironmentVariable("KEY_UINT", "2");

            uint actual = reader.EnvUInt("KEY_UINT");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvUInt_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            uint expected1 = default;
            uint expected2 = 2;
            uint actual;

            actual = reader.EnvUInt(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvUInt(VariableNotFound, 2);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        public void EnvULong_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            ulong expected = 2UL;
            SetEnvironmentVariable("KEY_ULONG", "2");

            ulong actual = reader.EnvULong("KEY_ULONG");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvULong_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            ulong expected1 = default;
            ulong expected2 = 2;
            ulong actual;

            actual = reader.EnvULong(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvULong(VariableNotFound, 2);
            Assert.AreEqual(expected2, actual);
        }

        [TestMethod]
        public void EnvUShort_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            ushort expected = 2;
            SetEnvironmentVariable("KEY_USHORT", "2");

            ushort actual = reader.EnvUShort("KEY_USHORT");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnvUShort_WhenTheVariableIsNotFound_ShouldReturnDefaultValue()
        {
            var reader = new EnvReader();
            ushort expected1 = default;
            ushort expected2 = 2;
            ushort actual;

            actual = reader.EnvUShort(VariableNotFound);
            Assert.AreEqual(expected1, actual);

            actual = reader.EnvUShort(VariableNotFound, 2);
            Assert.AreEqual(expected2, actual);
        }
    }
}
