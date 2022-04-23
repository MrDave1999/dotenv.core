namespace DotEnv.Core.Tests.Helpers
{
    [TestClass]
    public class EnvTests
    {
        [TestMethod]
        public void IsDevelopment_WhenCurrentEnvironmentIsDevelopmentOrDev_ShouldReturnsTrue()
        {
            Env.CurrentEnvironment = "development";
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            Env.CurrentEnvironment = "DEVELOPMENT";
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            Env.CurrentEnvironment = "dev";
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            Env.CurrentEnvironment = "DEV";
            Assert.AreEqual(expected: true, actual: Env.IsDevelopment());
            Env.CurrentEnvironment = null;
            Assert.AreEqual(expected: false, actual: Env.IsDevelopment());
        }

        [TestMethod]
        public void IsTest_WhenCurrentEnvironmentIsTest_ShouldReturnsTrue()
        {
            Env.CurrentEnvironment = "test";
            Assert.AreEqual(expected: true, actual: Env.IsTest());
            Env.CurrentEnvironment = "TEST";
            Assert.AreEqual(expected: true, actual: Env.IsTest());
            Env.CurrentEnvironment = null;
            Assert.AreEqual(expected: false, actual: Env.IsTest());
        }

        [TestMethod]
        public void IsStaging_WhenCurrentEnvironmentIsStaging_ShouldReturnsTrue()
        {
            Env.CurrentEnvironment = "staging";
            Assert.AreEqual(expected: true, actual: Env.IsStaging());
            Env.CurrentEnvironment = "STAGING";
            Assert.AreEqual(expected: true, actual: Env.IsStaging());
            Env.CurrentEnvironment = null;
            Assert.AreEqual(expected: false, actual: Env.IsStaging());
        }

        [TestMethod]
        public void IsProduction_WhenCurrentEnvironmentIsProductionOrProd_ShouldReturnsTrue()
        {
            Env.CurrentEnvironment = "production";
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            Env.CurrentEnvironment = "PRODUCTION";
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            Env.CurrentEnvironment = "prod";
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            Env.CurrentEnvironment = "PROD";
            Assert.AreEqual(expected: true, actual: Env.IsProduction());
            Env.CurrentEnvironment = null;
            Assert.AreEqual(expected: false, actual: Env.IsProduction());
        }

        [TestMethod]
        public void IsEnvironment_WhenCurrentEnvironmentIsProductionAndValueSpecifiedIsProduction_ShouldReturnsTrue()
        {
            Env.CurrentEnvironment = "production";
            Assert.AreEqual(expected: true, actual: Env.IsEnvironment("production"));
            Assert.AreEqual(expected: true, actual: Env.IsEnvironment("PRODUCTION"));
            Env.CurrentEnvironment = null;
            Assert.AreEqual(expected: false, actual: Env.IsEnvironment("production"));
        }
    }
}
