namespace DotEnv.Extensions.Microsoft.DI.Tests;

[TestClass]
public class ServiceCollectionExtensionsTests
{
    private const string Summaries       = "SUMMARIES";
    private const string Expected        = "Cool";
    private const string ConfigEnvPath   = "env_files/config.env";

    [TestMethod]
    public void AddDotEnvWithDefaultEnvFileName()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv();
        var reader = services.BuildServiceProvider().GetRequiredService<IEnvReader>();

        // Assert
        reader[Summaries].Should().Be(Expected);
    }

    [TestMethod]
    public void AddDotEnvWithCustomConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv(ConfigEnvPath);
        var reader = services.BuildServiceProvider().GetRequiredService<IEnvReader>();

        // Assert
        reader[Summaries].Should().Be(Expected);
    }

    [TestMethod]
    public void AddDotEnvGenericWithDefaultEnvFileName()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv<AppSettings>();
        var settings = services.BuildServiceProvider().GetRequiredService<AppSettings>();

        // Assert
        settings.Summaries.Should().Be(Expected);
    }

    [TestMethod]
    public void AddDotEnvGenericWithCustomConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv<AppSettings>(ConfigEnvPath);
        var settings = services.BuildServiceProvider().GetRequiredService<AppSettings>();

        // Assert
        settings.Summaries.Should().Be(Expected);
    }

    [TestMethod]
    public void AddCustomEnvWithCustomConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        Env.CurrentEnvironment = null;

        // Act
        services.AddCustomEnv(
            basePath: "env_files/environment/dev", 
            environmentName: "dev"
        );
        var reader = services.BuildServiceProvider().GetRequiredService<IEnvReader>();

        // Asserts
        reader["DEV_ENV"].Should().Be("1");
        reader["DEV_ENV_DEV"].Should().Be("1");
        reader["DEV_ENV_DEV_LOCAL"].Should().Be("1");
        reader["DEV_ENV_LOCAL"].Should().Be("1");
    }

    [TestMethod]
    public void AddCustomEnvGenericWithCustomConfiguration()
    {
        // Arrange
        var services = new ServiceCollection();
        Env.CurrentEnvironment = null;

        // Act
        services.AddCustomEnv<SettingsProduction>(
            basePath: "env_files/environment/production", 
            environmentName: "production"
        );
        var settings = services.BuildServiceProvider().GetRequiredService<SettingsProduction>();

        // Asserts
        settings.ProdEnv.Should().Be("1");
        settings.ProdEnvProd.Should().Be("1");
        settings.ProdEnvProdLocal.Should().Be("1");
        settings.ProdEnvLocal.Should().Be("1");
    }
}