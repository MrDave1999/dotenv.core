namespace DotEnv.Extensions.Microsoft.Config.Tests;

[TestClass]
public class EnvConfigurationExtensionsTests
{
    private const string DefaultName = ".env.default";

    [TestMethod]
    public void Build_WhenEnvFileIsNotOptional_ShouldThrowFileNotFoundExceptionIfNotExist()
    {
        // Arrange
        var builder = new ConfigurationBuilder()
            .AddEnvFile(DefaultName, optional: false);

        // Act
        Action act = () => builder.Build();

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }

    [TestMethod]
    public void Build_WhenEnvFileIsOptional_ShouldNotThrowFileNotFoundExceptionIfNotExist()
    {
        // Arrange
        var builder = new ConfigurationBuilder()
            .AddEnvFile(DefaultName, optional: true);

        // Act
        Action act = () => builder.Build();

        // Assert
        act.Should().NotThrow<FileNotFoundException>();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("  ")]
    [DataRow(null)]
    public void AddEnvFile_WhenPathIsNullOrEmptyStringOrWhitespaces_ShouldThrowArgumentException(string path)
    {
        // Arrange
        var builder = new ConfigurationBuilder();

        // Act
        Action act = () => builder.AddEnvFile(path);

        // Assert
        act.Should()
           .Throw<ArgumentException>()
           .WithParameterName(nameof(path));
    }

    [TestMethod]
    [DataRow("MARIADB")]
    [DataRow("mariadb")]
    [DataRow("MariaDb")]
    public void Build_WhenPathIsValid_ShouldLoadKeyValuePairsFromValidEnvFile(string key)
    {
        // Arrange
        var path = Path.Combine("env_files", ".env");
        var builder = new ConfigurationBuilder()
            .AddEnvFile(path);

        // Act
        IConfigurationRoot config = builder.Build();

        // Asserts
        config["BASE_URL"].Should().Be("localhost");
        config["PORT"].Should().Be("3306");
        config["MARIADB:USER"].Should().Be("user123");
        config["MARIADB:PASSWORD"].Should().Be("1234");
        config["mariadb:user"].Should().Be("user123");
        config["mariadb:password"].Should().Be("1234");

        IConfigurationSection section = config.GetSection(key);
        section["USER"].Should().Be("user123");
        section["PASSWORD"].Should().Be("1234");
        section["user"].Should().Be("user123");
        section["password"].Should().Be("1234");
    }
}
