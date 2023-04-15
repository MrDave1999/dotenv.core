namespace DotEnv.Core.Tests.Loader;

public partial class EnvLoaderTests
{
    [TestMethod]
    public void LoadEnv_WhenEnvFileNotFound_ShouldThrowFileNotFoundException()
    {
        // Arrange
        Env.CurrentEnvironment = "dev";
        var loader = new EnvLoader()
                        .SetBasePath("environment/files")
                        .SetDefaultEnvFileName(".env.example")
                        .AddEnvFiles(".env.example1", "foo/")
                        .EnableFileNotFoundException();

        // Act
        Action act = () => loader.LoadEnv();

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }

    [TestMethod]
    public void LoadEnv_WhenEnvironmentIsNotDefined_ShouldLoadFourEnvFilesForDevelopmentEnvironment()
    {
        // Arrange

        // Environment is not defined.
        Env.CurrentEnvironment = null;
        
        // Act
        new EnvLoader()
            .SetBasePath("Loader/env_files/environment/dev")
            // It should load four .env files: 
            // .env.dev.local, .env.local, .env.dev, .env
            .LoadEnv();

        new EnvLoader()
            .SetBasePath("Loader/env_files/environment/development")
            // It should load four .env files: 
            // .env.development.local, .env.local, .env.development, .env
            .LoadEnv();

        // Asserts
        Env.CurrentEnvironment.Should().Be("development");
        GetEnvironmentVariable("DEV_ENV").Should().NotBeNull();
        GetEnvironmentVariable("DEV_ENV_DEV").Should().NotBeNull();
        GetEnvironmentVariable("DEV_ENV_DEV_LOCAL").Should().NotBeNull();
        GetEnvironmentVariable("DEV_ENV_LOCAL").Should().NotBeNull();
        GetEnvironmentVariable("DEVELOPMENT_ENV").Should().NotBeNull();
        GetEnvironmentVariable("DEVELOPMENT_ENV_DEV").Should().NotBeNull();
        GetEnvironmentVariable("DEVELOPMENT_ENV_DEV_LOCAL").Should().NotBeNull();
        GetEnvironmentVariable("DEVELOPMENT_ENV_LOCAL").Should().NotBeNull();
    }

    [TestMethod]
    public void LoadEnv_WhenEnvironmentIsDefined_ShouldLoadFourEnvFilesForCurrentEnvironment()
    {
        // Arrange
        var loader = new EnvLoader();
        Env.CurrentEnvironment = "test";

        // Act
        loader
            .SetBasePath("Loader/env_files/environment/test")
            // It should load four .env files: 
            // .env.test.local, .env.local, .env.test, .env
            .LoadEnv();

        // Asserts
        GetEnvironmentVariable("TEST_ENV").Should().NotBeNull();
        GetEnvironmentVariable("TEST_ENV_TEST").Should().NotBeNull();
        GetEnvironmentVariable("TEST_ENV_TEST_LOCAL").Should().NotBeNull();
        GetEnvironmentVariable("TEST_ENV_LOCAL").Should().NotBeNull();
    }

    [TestMethod]
    public void LoadEnv_WhenErrorsAreFound_ShouldThrowParserException()
    {
        // Arrange
        var loader = new EnvLoader().SetBasePath("Loader/env_files/environment/production");
        Env.CurrentEnvironment = "production";

        // Act
        Action act = () => loader.LoadEnv();

        // Assert
        act.Should().Throw<ParserException>();
    }

    [TestMethod]
    public void LoadEnv_WhenSetsTheEnvironmentName_ShouldLoadFourEnvFilesForCurrentEnvironment()
    {
        // Arrange
        Env.CurrentEnvironment = null;
        var loader = new EnvLoader()
                        .AvoidModifyEnvironment()
                        .SetEnvironmentName("test")
                        .SetBasePath("Loader/env_files/environment/test");

        // Act
        // It should load four .env files: 
        // .env.test.local, .env.local, .env.test, .env
        var keyValuePairs = loader.LoadEnv();

        // Asserts
        Env.CurrentEnvironment.Should().Be("test");
        keyValuePairs["TEST_ENV"].Should().NotBeNull();
        keyValuePairs["TEST_ENV_TEST"].Should().NotBeNull();
        keyValuePairs["TEST_ENV_TEST_LOCAL"].Should().NotBeNull();
        keyValuePairs["TEST_ENV_LOCAL"].Should().NotBeNull();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void SetEnvironmentName_WhenEnvironmentNameIsAnEmptyStringOrWhiteSpace_ShouldThrowArgumentException(string envName)
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        Action act = () => loader.SetEnvironmentName(envName);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void LoadEnv_WhenAddsEnvFiles_ShouldMaintainThePriorityOfTheEnvFiles()
    {
        // Arrange
        Env.CurrentEnvironment = "dev";
        var loader = new EnvLoader()
                        .SetBasePath("Loader/env_files/local")
                        .AddEnvFile(".env.example1")
                        .AddEnvFile(".env.example2");

        // Act
        loader.LoadEnv();

        // Asserts
        GetEnvironmentVariable("MAX_PRIORITY").Should().Be(".env.dev.local");
        GetEnvironmentVariable("PRIORITY_2").Should().Be(".env.local");
        GetEnvironmentVariable("PRIORITY_3").Should().Be(".env.dev");
        GetEnvironmentVariable("PRIORITY_4").Should().Be(".env");
    }

    [TestMethod]
    public void LoadEnv_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        // Arrange
        var loader = new EnvLoader();
        var basePath = "Loader/env_files/environment/production/";
        EnvValidationResult result;
        Env.CurrentEnvironment = "production";

        // Act
        loader
            .SetBasePath(basePath)
            .IgnoreParserException()
            .LoadEnv(out result);

        // Asserts
        result.HasError().Should().BeTrue();
        result.Should().HaveCount(7);

        var fileName = $"{basePath}.env.production.local";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "==", 
            lineNumber: 2, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "PROD", 
            lineNumber: 5, 
            column: 1, 
            envFileName: fileName
        ));

        var value = "This is an error";
        fileName = $"{basePath}.env.local";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: value, 
            lineNumber: 4, 
            column: 1, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.production";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL1", 
            lineNumber: 3, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: value, 
            lineNumber: 5, 
            column: 1, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: value, 
            lineNumber: 4, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: value, 
            lineNumber: 6, 
            column: 1, 
            envFileName: fileName
        ));
    }

    [TestMethod]
    public void LoadEnv_WhenLocalEnvFilesNotExistAndEnvironmentIsNotDefined_ShouldGenerateAnError()
    {
        // Arrange
        Env.CurrentEnvironment = null;
        var loader = new EnvLoader();

        // Act
        loader
            .SetBasePath("environment/env_files")
            .LoadEnv(out var result);

        // Asserts
        result.HasError().Should().BeTrue();
        result.Count.Should().Be(1);
        Env.CurrentEnvironment.Should().Be("development");
        result.ErrorMessages.Should().Contain(FormatLocalFileNotPresentMessage());
    }

    [TestMethod]
    public void LoadEnv_WhenLocalEnvFilesNotExistAndEnvironmentIsDefined_ShouldGenerateAnError()
    {
        // Arrange
        var loader = new EnvLoader();
        Env.CurrentEnvironment = "test";

        // Act
        loader
            .SetBasePath("environment/env_files")
            .LoadEnv(out var result);

        // Asserts
        result.HasError().Should().BeTrue();
        result.Count.Should().Be(1);
        result.ErrorMessages
              .Should()
              .Contain(FormatLocalFileNotPresentMessage(environmentName: Env.CurrentEnvironment));
    }
}
