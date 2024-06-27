namespace DotEnv.Extensions.Microsoft.DI.Tests;

[TestClass]
public class ServiceCollectionExtensionsTests
{
    private const string Summaries       = "SUMMARIES";
    private const string Expected        = "Cool";
    private const string ConfigEnvPath   = "env_files/config.env";

    [TestMethod]
    public void AddDotEnv_WhenDefaultEnvFileNameIsUsed_ShouldReadKeyValuePairs()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv();
        using var serviceProvider = services.BuildServiceProvider();
        var reader = serviceProvider.GetRequiredService<IEnvReader>();

        // Assert
        reader[Summaries].Should().Be(Expected);
    }

    [TestMethod]
    public void AddDotEnv_WhenCustomConfigurationIsUsed_ShouldReadKeyValuePairs()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv(ConfigEnvPath);
        using var serviceProvider = services.BuildServiceProvider();
        var reader = serviceProvider.GetRequiredService<IEnvReader>();

        // Assert
        reader[Summaries].Should().Be(Expected);
    }

    [TestMethod]
    public void AddDotEnv_WhenPathCollectionIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();
        string[] paths = [];

        // Act
        Action act = () => services.AddDotEnv(paths);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithParameterName(nameof(paths));
    }

    [TestMethod]
    public void AddDotEnvOfT_WhenDefaultEnvFileNameIsUsed_ShouldReadKeyValuePairs()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv<AppSettings>();
        using var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetRequiredService<AppSettings>();

        // Assert
        settings.Summaries.Should().Be(Expected);
    }

    [TestMethod]
    public void AddDotEnvOfT_WhenCustomConfigurationIsUsed_ShouldReadKeyValuePairs()
    {
        // Arrange
        var services = new ServiceCollection();
        SetEnvironmentVariable(Summaries, null);

        // Act
        services.AddDotEnv<AppSettings>(ConfigEnvPath);
        using var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetRequiredService<AppSettings>();

        // Assert
        settings.Summaries.Should().Be(Expected);
    }

    [TestMethod]
    public void AddDotEnvOfT_WhenPathCollectionIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();
        string[] paths = [];

        // Act
        Action act = () => services.AddDotEnv<AppSettings>(paths);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithParameterName(nameof(paths));
    }

    [TestMethod]
    public void AddCustomEnv_WhenCustomConfigurationIsUsed_ShouldReadKeyValuePairs()
    {
        // Arrange
        var services = new ServiceCollection();
        Env.CurrentEnvironment = null;

        // Act
        services.AddCustomEnv(
            basePath: "env_files/environment/dev", 
            environmentName: "dev"
        );
        using var serviceProvider = services.BuildServiceProvider();
        var reader = serviceProvider.GetRequiredService<IEnvReader>();

        // Asserts
        reader["DEV_ENV"].Should().Be("1");
        reader["DEV_ENV_DEV"].Should().Be("1");
        reader["DEV_ENV_DEV_LOCAL"].Should().Be("1");
        reader["DEV_ENV_LOCAL"].Should().Be("1");
    }

    [TestMethod]
    public void AddCustomEnvOfT_WhenCustomConfigurationIsUsed_ShouldReadKeyValuePairs()
    {
        // Arrange
        var services = new ServiceCollection();
        Env.CurrentEnvironment = null;

        // Act
        services.AddCustomEnv<SettingsProduction>(
            basePath: "env_files/environment/production", 
            environmentName: "production"
        );
        using var serviceProvider = services.BuildServiceProvider();
        var settings = serviceProvider.GetRequiredService<SettingsProduction>();

        // Asserts
        settings.ProdEnv.Should().Be("1");
        settings.ProdEnvProd.Should().Be("1");
        settings.ProdEnvProdLocal.Should().Be("1");
        settings.ProdEnvLocal.Should().Be("1");
    }
}