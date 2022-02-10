using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;

namespace DotEnv.Core.Tests.Helpers
{
    [TestClass]
    public class EnvTests
    {
        [TestMethod]
        public void IsDevelopment_WhenCurrentEnvironmentIsDevelopmentOrDev_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "development");
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", "DEVELOPMENT");
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", "dev");
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", "DEV");
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(expected: false, actual: Env.IsDevelopment());
        }

        [TestMethod]
        public void IsTest_WhenCurrentEnvironmentIsTest_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "test");
            Assert.AreEqual(expected: true, actual: Env.IsTest());
            SetEnvironmentVariable("DOTNET_ENV", "TEST");
            Assert.AreEqual(expected: true, actual: Env.IsTest());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(expected: false, actual: Env.IsTest());
        }

        [TestMethod]
        public void IsStaging_WhenCurrentEnvironmentIsStaging_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "staging");
            Assert.AreEqual(expected: true, actual: Env.IsStaging());
            SetEnvironmentVariable("DOTNET_ENV", "STAGING");
            Assert.AreEqual(expected: true, actual: Env.IsStaging());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(expected: false, actual: Env.IsStaging());
        }

        [TestMethod]
        public void IsProduction_WhenCurrentEnvironmentIsProductionOrProd_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "production");
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", "PRODUCTION");
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", "prod");
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", "PROD");
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(expected: false, actual: Env.IsProduction());
        }

        [TestMethod]
        public void IsEnvironment_WhenCurrentEnvironmentIsProductionAndValueSpecifiedIsProduction_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "production");
            Assert.AreEqual(expected: true, actual: Env.IsEnvironment("production"));
            Assert.AreEqual(expected: true, actual: Env.IsEnvironment("PRODUCTION"));
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(expected: false, actual: Env.IsEnvironment("production"));
        }
    }
}
