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
            Assert.AreEqual(true, Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", "DEVELOPMENT");
            Assert.AreEqual(true, Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", "dev");
            Assert.AreEqual(true, Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", "DEV");
            Assert.AreEqual(true, Env.IsDevelopment());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(false, Env.IsDevelopment());
        }

        [TestMethod]
        public void IsTest_WhenCurrentEnvironmentIsTest_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "test");
            Assert.AreEqual(true, Env.IsTest());
            SetEnvironmentVariable("DOTNET_ENV", "TEST");
            Assert.AreEqual(true, Env.IsTest());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(false, Env.IsTest());
        }

        [TestMethod]
        public void IsStaging_WhenCurrentEnvironmentIsStaging_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "staging");
            Assert.AreEqual(true, Env.IsStaging());
            SetEnvironmentVariable("DOTNET_ENV", "STAGING");
            Assert.AreEqual(true, Env.IsStaging());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(false, Env.IsStaging());
        }

        [TestMethod]
        public void IsProduction_WhenCurrentEnvironmentIsProductionOrProd_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "production");
            Assert.AreEqual(true, Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", "PRODUCTION");
            Assert.AreEqual(true, Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", "prod");
            Assert.AreEqual(true, Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", "PROD");
            Assert.AreEqual(true, Env.IsProduction());
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(false, Env.IsProduction());
        }

        [TestMethod]
        public void IsEnvironment_WhenCurrentEnvironmentIsProductionAndValueSpecifiedIsProduction_ShouldReturnsTrue()
        {
            SetEnvironmentVariable("DOTNET_ENV", "production");
            Assert.AreEqual(true, Env.IsEnvironment("production"));
            Assert.AreEqual(true, Env.IsEnvironment("PRODUCTION"));
            SetEnvironmentVariable("DOTNET_ENV", null);
            Assert.AreEqual(false, Env.IsEnvironment("production"));
        }
    }
}
