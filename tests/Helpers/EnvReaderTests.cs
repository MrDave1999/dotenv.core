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
            Assert.AreEqual(expected: true, actual: reader.Exists("VARIABLE_NAME"));
            SetEnvironmentVariable("VARIABLE_NAME", null);
            Assert.AreEqual(expected: false, actual: reader.Exists("VARIABLE_NAME"));
            Assert.AreEqual(expected: false, actual: reader.Exists(""));
        }

        [TestMethod]
        public void GetBoolValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_BOOL", "true");
            Assert.AreEqual(expected: true, actual: reader.GetBoolValue("KEY_BOOL"));
            SetEnvironmentVariable("KEY_BOOL", "false");
            Assert.AreEqual(expected: false, actual: reader.GetBoolValue("KEY_BOOL"));
            void action() => reader.GetBoolValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetByteValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_BYTE", "2");
            Assert.AreEqual(expected: (byte)2, actual: reader.GetByteValue("KEY_BYTE"));
            void action() => reader.GetByteValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetCharValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_CHAR", "A");
            Assert.AreEqual(expected: 'A', actual: reader.GetCharValue("KEY_CHAR"));
            void action() => reader.GetCharValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetDecimalValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_DECIMAL", "12.5");
            Assert.AreEqual(expected: 12.5M, actual: reader.GetDecimalValue("KEY_DECIMAL"));
            SetEnvironmentVariable("KEY_DECIMAL", "12,5");
            Assert.AreEqual(expected: 125M, actual: reader.GetDecimalValue("KEY_DECIMAL"));
            void action() => reader.GetDecimalValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetDoubleValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_DOUBLE", "12.5");
            Assert.AreEqual(expected: 12.5D, actual: reader.GetDoubleValue("KEY_DOUBLE"));
            SetEnvironmentVariable("KEY_DOUBLE", "12,5");
            Assert.AreEqual(expected: 125D, actual: reader.GetDoubleValue("KEY_DOUBLE"));
            void action() => reader.GetDoubleValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetFloatValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_FLOAT", "12.5");
            Assert.AreEqual(expected: 12.5F, actual: reader.GetFloatValue("KEY_FLOAT"));
            SetEnvironmentVariable("KEY_FLOAT", "12,5");
            Assert.AreEqual(expected: 125F, actual: reader.GetFloatValue("KEY_FLOAT"));
            void action() => reader.GetFloatValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetIntValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_INT", "3");
            Assert.AreEqual(expected: 3, actual: reader.GetIntValue("KEY_INT"));
            void action() => reader.GetIntValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetLongValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_LONG", "3");
            Assert.AreEqual(expected: 3L, actual: reader.GetLongValue("KEY_LONG"));
            void action() => reader.GetLongValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetSByteValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_SBYTE", "3");
            Assert.AreEqual(expected: (sbyte)3, actual: reader.GetSByteValue("KEY_SBYTE"));
            void action() => reader.GetSByteValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetShortValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_SHORT", "3");
            Assert.AreEqual(expected: (short)3, actual: reader.GetShortValue("KEY_SHORT"));
            void action() => reader.GetShortValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetStringValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_STRING", "This is a string.");
            Assert.AreEqual(expected: "This is a string.", actual: reader.GetStringValue("KEY_STRING"));
            void action() => reader.GetStringValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetUIntValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_UINT", "2");
            Assert.AreEqual(expected: 2U, actual: reader.GetUIntValue("KEY_UINT"));
            void action() => reader.GetUIntValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetULongValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_ULONG", "2");
            Assert.AreEqual(expected: 2UL, actual: reader.GetULongValue("KEY_ULONG"));
            void action() => reader.GetULongValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void GetUShortValue_WhenTheVariableIsFound_ShouldReturnValue()
        {
            var reader = new EnvReader();
            SetEnvironmentVariable("KEY_USHORT", "2");
            Assert.AreEqual(expected: (ushort)2, actual: reader.GetUShortValue("KEY_USHORT"));
            void action() => reader.GetUShortValue(VariableNotFound);
            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }
    }
}
