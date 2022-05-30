namespace DotEnv.Core.Tests.Loader;

public partial class EnvLoaderTests
{
    [TestMethod]
    public void LoadEnv_WhenEnvFileNotFound_ShouldThrowFileNotFoundException()
    {
        Env.CurrentEnvironment = "dev";
        var loader = new EnvLoader()
                        .SetBasePath("environment/files")
                        .SetDefaultEnvFileName(".env.example")
                        .AddEnvFiles(".env.example1", "foo/")
                        .EnableFileNotFoundException();

        void action() => loader.LoadEnv();

        Assert.ThrowsException<FileNotFoundException>(action);
    }

    [TestMethod]
    public void LoadEnv_WhenEnvironmentIsNotDefined_ShouldLoadFourEnvFilesForDevelopmentEnvironment()
    {
        Env.CurrentEnvironment = null; // Environment is not defined
        
        new EnvLoader()
            .SetBasePath("Loader/env_files/environment/dev")
            .LoadEnv(); // It should load four .env files: 
                        // .env.dev.local, .env.local, .env.dev, .env

        new EnvLoader()
            .SetBasePath("Loader/env_files/environment/development")
            .LoadEnv(); // It should load four .env files: 
                        // .env.development.local, .env.local, .env.development, .env

        Assert.AreEqual(expected: "development", actual: Env.CurrentEnvironment);
        Assert.IsNotNull(GetEnvironmentVariable("DEV_ENV"));
        Assert.IsNotNull(GetEnvironmentVariable("DEV_ENV_DEV"));
        Assert.IsNotNull(GetEnvironmentVariable("DEV_ENV_DEV_LOCAL"));
        Assert.IsNotNull(GetEnvironmentVariable("DEV_ENV_LOCAL"));
        Assert.IsNotNull(GetEnvironmentVariable("DEVELOPMENT_ENV"));
        Assert.IsNotNull(GetEnvironmentVariable("DEVELOPMENT_ENV_DEV"));
        Assert.IsNotNull(GetEnvironmentVariable("DEVELOPMENT_ENV_DEV_LOCAL"));
        Assert.IsNotNull(GetEnvironmentVariable("DEVELOPMENT_ENV_LOCAL"));
    }

    [TestMethod]
    public void LoadEnv_WhenEnvironmentIsDefined_ShouldLoadFourEnvFilesForCurrentEnvironment()
    {
        Env.CurrentEnvironment = "test";

        new EnvLoader()
            .SetBasePath("Loader/env_files/environment/test")
            .LoadEnv(); // It should load four .env files: 
                        // .env.test.local, .env.local, .env.test, .env

        Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV"));
        Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_TEST"));
        Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_TEST_LOCAL"));
        Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_LOCAL"));
    }

    [TestMethod]
    public void LoadEnv_WhenErrorsAreFound_ShouldThrowParserException()
    {
        var loader = new EnvLoader().SetBasePath("Loader/env_files/environment/production");
        Env.CurrentEnvironment = "production";

        void action() => loader.LoadEnv();

        Assert.ThrowsException<ParserException>(action);
    }

    [TestMethod]
    public void LoadEnv_WhenSetsTheEnvironmentName_ShouldLoadFourEnvFilesForCurrentEnvironment()
    {
        Env.CurrentEnvironment = null;
        var loader = new EnvLoader()
                        .AvoidModifyEnvironment()
                        .SetEnvironmentName("test")
                        .SetBasePath("Loader/env_files/environment/test");

        // It should load four .env files: 
        // .env.test.local, .env.local, .env.test, .env
        var keyValuePairs = loader.LoadEnv();

        Assert.AreEqual(expected: "test", actual: Env.CurrentEnvironment);
        Assert.IsNotNull(keyValuePairs["TEST_ENV"]);
        Assert.IsNotNull(keyValuePairs["TEST_ENV_TEST"]);
        Assert.IsNotNull(keyValuePairs["TEST_ENV_TEST_LOCAL"]);
        Assert.IsNotNull(keyValuePairs["TEST_ENV_LOCAL"]);
    }

    [TestMethod]
    public void SetEnvironmentName_WhenEnvironmentNameIsAnEmptyStringOrWhiteSpace_ShouldThrowArgumentException()
    {
        var loader = new EnvLoader();
        Action action;

        action = () => loader.SetEnvironmentName("");
        Assert.ThrowsException<ArgumentException>(action);
        action = () => loader.SetEnvironmentName("   ");
        Assert.ThrowsException<ArgumentException>(action);
    }

    [TestMethod]
    public void LoadEnv_WhenAddsEnvFiles_ShouldMaintainThePriorityOfTheEnvFiles()
    {
        Env.CurrentEnvironment = "dev";
        var loader = new EnvLoader()
                        .SetBasePath("Loader/env_files/local")
                        .AddEnvFile(".env.example1")
                        .AddEnvFile(".env.example2");

        loader.LoadEnv();

        Assert.AreEqual(expected: ".env.dev.local", actual: GetEnvironmentVariable("MAX_PRIORITY"));
        Assert.AreEqual(expected: ".env.local", actual: GetEnvironmentVariable("PRIORITY_2"));
        Assert.AreEqual(expected: ".env.dev", actual: GetEnvironmentVariable("PRIORITY_3"));
        Assert.AreEqual(expected: ".env", actual: GetEnvironmentVariable("PRIORITY_4"));
    }

    [TestMethod]
    public void LoadEnv_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        string msg;
        EnvValidationResult result;
        string basePath = $"Loader/env_files/environment/production/";
        Env.CurrentEnvironment = "production";

        new EnvLoader()
            .SetBasePath(basePath)
            .IgnoreParserException()
            .LoadEnv(out result);

        msg = result.ErrorMessages;
        Assert.AreEqual(expected: true, actual: result.HasError());
        Assert.AreEqual(expected: 7, actual: result.Count);

        var fileName = $"{basePath}.env.production.local";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "==", 
            lineNumber: 2, 
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "PROD", 
            lineNumber: 5, 
            column: 1, 
            envFileName: fileName
        ));

        var value = "This is an error";
        fileName = $"{basePath}.env.local";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: value, 
            lineNumber: 4, 
            column: 1, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.production";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL1", 
            lineNumber: 3, 
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: value, 
            lineNumber: 5, 
            column: 1, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: value, 
            lineNumber: 4, 
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
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
        Env.CurrentEnvironment = null;
        var loader = new EnvLoader();

        loader
            .SetBasePath("environment/env_files")
            .LoadEnv(out var result);

        Assert.AreEqual(expected: true, actual: result.HasError());
        Assert.AreEqual(expected: 1, actual: result.Count);
        Assert.AreEqual(expected: "development", actual: Env.CurrentEnvironment);
        StringAssert.Contains(result.ErrorMessages, FormatLocalFileNotPresentMessage());
    }

    [TestMethod]
    public void LoadEnv_WhenLocalEnvFilesNotExistAndEnvironmentIsDefined_ShouldGenerateAnError()
    {
        var loader = new EnvLoader();
        Env.CurrentEnvironment = "test";

        loader
            .SetBasePath("environment/env_files")
            .LoadEnv(out var result);

        Assert.AreEqual(expected: true, actual: result.HasError());
        Assert.AreEqual(expected: 1, actual: result.Count);
        StringAssert.Contains(result.ErrorMessages, FormatLocalFileNotPresentMessage(environmentName: Env.CurrentEnvironment));
    }
}
