using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core.Tests.Helpers
{
    public class RequiredKeys
    {
        public int IGNORE_1 { get; set; }
        public string SAMC_KEY { get; }
        public string API_KEY { get; }
        public string JWT_TOKEN { get; }
        public string JWT_TOKEN_ID { get; }
        public string SERVICE_ID { get; }
        public double IGNORE_2 { get; }
    }

    [TestClass]
    public class EnvValidatorTests
    {
        [TestMethod]
        public void Validate_WhenRequiredKeysAreNotPresent_ShouldThrowEnvVariableNotFoundException()
        {
            var validator = new EnvValidator()
                        .AddRequiredKeys("SAMC_KEY", "API_KEY", "JWT_TOKEN", "JWT_TOKEN_ID", "SERVICE_ID");

            void action() => validator.Validate();

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void Validate_WhenRequiredKeysArePresent_ShouldNotThrowEnvVariableNotFoundException()
        {
            SetEnvironmentVariable("JWT_TOKEN", "123");
            SetEnvironmentVariable("API_KEY", "123");
            var validator = new EnvValidator()
                        .AddRequiredKeys("JWT_TOKEN", "API_KEY")
                        .IgnoreException();          

            validator.Validate(out var result);

            Assert.AreEqual(expected: false, actual: result.HasError());
            Assert.AreEqual(expected: 0, actual: result.Count);
            SetEnvironmentVariable("JWT_TOKEN", null);
            SetEnvironmentVariable("API_KEY", null);
        }

        [TestMethod]
        public void Validate_WhenErrorsAreFound_ShouldReadTheErrors()
        {
            string msg;
            var validator = new EnvValidator()
                        .AddRequiredKeys("SAMC_KEY", "API_KEY", "JWT_TOKEN", "JWT_TOKEN_ID", "SERVICE_ID")
                        .IgnoreException();

            validator.Validate(out var result);

            Assert.AreEqual(expected: true, actual: result.HasError());
            Assert.AreEqual(expected: 5, actual: result.Count);

            msg = result.ErrorMessages;
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Variable Name: SAMC_KEY)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Variable Name: API_KEY)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Variable Name: JWT_TOKEN)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Variable Name: JWT_TOKEN_ID)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Variable Name: SERVICE_ID)");
        }

        [TestMethod]
        public void Validate_WhenRequiredKeysAreAddedByMeansOfClass_ShouldThrowEnvVariableNotFoundException()
        {
            var validator = new EnvValidator().AddRequiredKeys<RequiredKeys>();

            void action() => validator.Validate();

            Assert.ThrowsException<EnvVariableNotFoundException>(action);
        }

        [TestMethod]
        public void Validate_WhenRequiredKeysAreNotAdded_ShouldThrowInvalidOperationException()
        {
            var validator = new EnvValidator();

            void action() => validator.Validate();

            Assert.ThrowsException<InvalidOperationException>(action);
        }

        [TestMethod]
        public void SetRequiredKeys_WhenLengthOfTheKeysListIsZero_ShouldThrowArgumentException()
        {
            var validator = new EnvValidator();

            void action() => validator.AddRequiredKeys();

            Assert.ThrowsException<ArgumentException>(action);
        }
    }
}
