namespace DotEnv.Core.Tests.Common;

[TestClass]
public class EnvTests
{
    [TestMethod]
    [DataRow("development", true)]
    [DataRow("DEVELOPMENT", true)]
    [DataRow("dev",         true)]
    [DataRow("DEV",         true)]
    [DataRow(default,       false)]
    public void IsDevelopment_WhenCurrentEnvironmentIsDevelopmentOrDev_ShouldReturnsTrue(
        string currentEnvironment, bool expected)
    {
        // Arrange
        Env.CurrentEnvironment = currentEnvironment;

        // Act
        bool actual = Env.IsDevelopment();

        // Assert
        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("test",  true)]
    [DataRow("TEST",  true)]
    [DataRow(default, false)]
    public void IsTest_WhenCurrentEnvironmentIsTest_ShouldReturnsTrue(
        string currentEnvironment, bool expected)
    {
        // Arrange
        Env.CurrentEnvironment = currentEnvironment;

        // Act
        bool actual = Env.IsTest();

        // Assert
        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("staging", true)]
    [DataRow("STAGING", true)]
    [DataRow(default,   false)]
    public void IsStaging_WhenCurrentEnvironmentIsStaging_ShouldReturnsTrue(
        string currentEnvironment, bool expected)
    {
        // Arrange
        Env.CurrentEnvironment = currentEnvironment;

        // Act
        bool actual = Env.IsStaging();

        // Assert
        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("production", true)]
    [DataRow("PRODUCTION", true)]
    [DataRow("prod",       true)]
    [DataRow("PROD",       true)]
    [DataRow(default,      false)]
    public void IsProduction_WhenCurrentEnvironmentIsProductionOrProd_ShouldReturnsTrue(
        string currentEnvironment, bool expected)
    {
        // Arrange
        Env.CurrentEnvironment = currentEnvironment;

        // Act
        bool actual = Env.IsProduction();

        // Assert
        actual.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("production", "Production", true)]
    [DataRow("production", "PRODUCTION", true)]
    [DataRow(default,      "production", false)]
    public void IsEnvironment_WhenCurrentEnvironmentIsProductionAndValueSpecifiedIsProduction_ShouldReturnsTrue(
        string currentEnvironment,
        string environmentName,
        bool expected)
    {
        // Arrange
        Env.CurrentEnvironment = currentEnvironment;

        // Act
        bool actual = Env.IsEnvironment(environmentName);

        // Assert
        actual.Should().Be(expected);
    }
}
