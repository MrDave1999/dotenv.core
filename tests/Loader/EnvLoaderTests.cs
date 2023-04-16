namespace DotEnv.Core.Tests.Loader;

[TestClass]
public partial class EnvLoaderTests
{
    [TestMethod]
    public void Load_WhenErrorsAreFound_ShouldThrowParserException()
    {
        // Arrange
        var loader = new EnvLoader()
                         .SetBasePath("Loader/env_files/validation")
                         .AddEnvFile(".env.validation.result1")
                         .AddEnvFile(".env.validation.result2")
                         .AddEnvFile(".env.validation.result3")
                         .AddEnvFile(".env.validation.result4");

        // Act
        Action act = () => loader.Load();

        // Assert
        act.Should().Throw<ParserException>();
    }

    [TestMethod]
    public void Load_WhenEnvFilesAreOptional_ShouldNotGenerateAnErrorIfTheFileDoesNotExist()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader
            .AddEnvFile(".env.example1", Encoding.UTF8, optional: true)
            .AddEnvFile(".env.example2", encodingName: "UTF-8", optional: true)
            .AddEnvFile(".env.example3", optional: true)
            .Load(out var result);

        // Asserts
        result.HasError().Should().BeFalse();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Load_WhenAllEnvFilesAreOptional_ShouldNotGenerateAnErrorIfTheFileDoesNotExist()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
       loader
            .AddEnvFile(".env.example1", Encoding.UTF8)
            .AddEnvFile(".env.example2", encodingName: "UTF-8")
            .AddEnvFile(".env.example3")
            .AddEnvFiles(".env.example4", ".env.example5")
            .AddEnvFile(".env.example6", Encoding.UTF8, optional: true)
            .AddEnvFile(".env.example7", encodingName: "UTF-8", optional: true)
            .AddEnvFile(".env.example8", optional: true)
            .AllowAllEnvFilesOptional()
            .Load(out var result);

        // Asserts
        result.HasError().Should().BeFalse();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Load_WhenDefaultEnvFileIsOptional_ShouldNotGenerateAnErrorIfTheFileDoesNotExist()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader
            .SetBasePath("dotnet/files")
            .AllowAllEnvFilesOptional()
            // Loads a default file: .env
            .Load(out var result);

        // Asserts
        result.HasError().Should().BeFalse();
        result.Should().BeEmpty();
    }

    [TestMethod]
    public void Load_WhenIgnoresSearchInParentDirectories_ShouldOnlySearchInTheCurrentDirectory()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader
            .AddEnvFile("Loader/env_files/.env.parent.directories")
            // This file if copied to the current directory.
            .AddEnvFile(".env.only.currentdirectory")
            .IgnoreParentDirectories()
            .Load();

        // Asserts
        GetEnvironmentVariable("PARENT_DIRECTORIES").Should().BeNull();
        GetEnvironmentVariable("ONLY_CURRENT_DIRECTORY").Should().NotBeNull();
    }

    [TestMethod]
    public void Load_WhenLoadEnvFileWithDefaultConfig_ShouldSetEnvironmentVariablesFromAnEnvFile()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader.Load();

        // Asserts
        GetEnvironmentVariable("CONFIG_DEFAULT_1").Should().Be("VAL1");
        GetEnvironmentVariable("CONFIG_DEFAULT_2").Should().Be("VAL2");
    }

    [TestMethod]
    public void Load_WhenLoadEnvFileWithCustomConfig_ShouldSetEnvironmentVariablesFromAnEnvFile()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader
            .SetBasePath("Loader/env_files")
            .DisableTrimStartValues()
            .DisableTrimEndValues()
            .DisableTrimStartKeys()
            .DisableTrimEndKeys()
            .DisableTrimStartComments()
            .AllowOverwriteExistingVars()
            .AddEnvFile(".env.custom.configuration")
            .Load();

        // Asserts
        GetEnvironmentVariable(" CONFIG_CUSTOM_1 ").Should().Be(" VAL1 ");
        GetEnvironmentVariable(" CONFIG_CUSTOM_2 ").Should().Be(" VAL2 ");
    }

    [TestMethod]
    public void Load_WhenSetsEncoding_ShouldLoadTheEnvFileCorrectly()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader
            .SetBasePath("Loader/env_files")
            .SetEncoding("UTF-8")
            .AddEnvFile(".env.unicode.chinese")
            .AddEnvFile(".env.unicode.russian")
            .Load();

        // Asserts
        GetEnvironmentVariable("UNICODE_CHINESE").Should().Be("我们先 坐二号线% 然后 换一号线%");
        GetEnvironmentVariable("UNICODE_RUSSIAN").Should().Be("Привет мир! Привет, чем занимаешься");
    }

    [TestMethod]
    public void SetEncoding_WhenEncodingNameIsNotSupported_ShouldThrowArgumentException()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        Action act = () => loader.SetEncoding("UTF-88");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void AddEnvFile_WhenEncodingNameIsNotSupported_ShouldThrowArgumentException()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        Action act = () => loader.AddEnvFile(".env", "UTF-88");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Load_WhenLoadMultiEnvFiles_ShouldSetEnvironmentVariablesFromAnEnvFile()
    {
        // Arrange
        var loader = new EnvLoader();
        string absolutePath = Directory.GetCurrentDirectory();

        // Act
        loader
            .SetBasePath("Loader/env_files/multi")
            .AddEnvFiles(".env.multi1", "./", ".env.multi2")
            .AddEnvFile(".env.multi3")
            .AddEnvFile(".env.multi4")
            .AddEnvFile(".env.multi5")
            .AddEnvFile($"{absolutePath}/.env.multi6.absolute")
            .Load();

        // Asserts
        GetEnvironmentVariable("ENV_MULTI").Should().NotBeNull();
        GetEnvironmentVariable("ENV_MULTI_1").Should().NotBeNull();
        GetEnvironmentVariable("ENV_MULTI_2").Should().NotBeNull();
        GetEnvironmentVariable("ENV_MULTI_3").Should().NotBeNull();
        GetEnvironmentVariable("ENV_MULTI_4").Should().NotBeNull();
        GetEnvironmentVariable("ENV_MULTI_5").Should().NotBeNull();
        GetEnvironmentVariable("ENV_MULTI_6").Should().NotBeNull();
    }

    [TestMethod]
    public void Load_WhenEnvFileIsAddedAsDirectory_ShouldCombineDirectoryWithDefaultEnvFileName()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader
            .SetBasePath("Loader/env_files")
            .SetDefaultEnvFileName(".env.dev")
            // Equivalent to: ./foo/.env.dev, ./bar/.env.dev
            .AddEnvFiles("./foo", "./bar")
            .Load();

        // Asserts
        GetEnvironmentVariable("HAS_NOT_EXTENSION_1").Should().NotBeNull();
        GetEnvironmentVariable("HAS_NOT_EXTENSION_2").Should().NotBeNull();
    }

    [TestMethod]
    public void Load_WhenEnvFileIsInTheCurrentDirectory_ShouldReadContentsOfTheEnvFile()
    {
        // Arrange
        var loader = new EnvLoader();

        // Act
        loader
            .SetDefaultEnvFileName(".env.local")
            .Load();

        // Assert
        GetEnvironmentVariable("CURRENT_DIRECTORY").Should().Be("VAL1");
    }

    [TestMethod]
    public void Load_WhenEnvFileIsInAnAbsolutePath_ShouldReadContentsOfTheEnvFile()
    {
        // Arrange
        var loader = new EnvLoader();
        string absolutePath = Directory.GetCurrentDirectory();

        // Act
        loader
            .SetBasePath(absolutePath)
            .AddEnvFile(".env.absolute")
            .AddEnvFile(Path.Combine(absolutePath, ".env.absolute2"))
            .Load();

        // Asserts
        GetEnvironmentVariable("PATH_ABSOLUTE").Should().Be("VAL1");
        GetEnvironmentVariable("PATH_ABSOLUTE2").Should().Be("VAL1");
    }

    [TestMethod]
    public void Load_WhenEnvFileIsInAnRelativePath_ShouldReadContentsOfTheEnvFile()
    {
        // Arrange
        var loader = new EnvLoader();
        string relativePath = "./dotenv/files";

        // Act
        loader
            .SetBasePath(relativePath)
            .AddEnvFile(".env.relative")
            .Load();

        // Assert
        GetEnvironmentVariable("PATH_RELATIVE").Should().Be("VAL1");
    }

    [TestMethod]
    public void Load_WhenEnvFileNotFound_ShouldThrowFileNotFoundException()
    {
        // Arrange
        var loader = new EnvLoader()
                         .AddEnvFiles(
                            ".env.not.found", 
                            ".env.not.found3", 
                            ".env.not.found4", 
                            ".env.not.found5", 
                            ".env.not.found6"
                           )
                         .EnableFileNotFoundException();

        // Act
        Action act = () => loader.Load();

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }

    [TestMethod]
    public void Load_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        // Arrange
        var loader = new EnvLoader();
        var basePath = "Loader/env_files/validation/";
        EnvValidationResult result;

        // Act
        loader
            .SetBasePath(basePath)
            .IgnoreParserException()
            .AddEnvFile(".env.validation.result1")
            .AddEnvFile(".env.validation.result2")
            .AddEnvFile(".env.validation.result3")
            .AddEnvFile(".env.validation.result4")
            .AddEnvFile(".env.validation.result5")
            .AddEnvFiles(
                ".env.not.found3", 
                ".env.not.found4", 
                ".env.not.found5", 
                ".env.not.found6"
              )
            .Load(out result);

        // Asserts
        result.HasError().Should().BeTrue();
        result.Should().HaveCount(20);

        var fileName = $"{basePath}.env.validation.result1";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is an error", 
            lineNumber: 1, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL1", 
            lineNumber: 2, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 26, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND_2", 
            lineNumber: 3, 
            column: 55, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${}", 
            lineNumber: 5, 
            column: 24, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${   }", 
            lineNumber: 5, 
            column: 41, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.validation.result2";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            DataSourceIsEmptyOrWhitespaceMessage, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.validation.result3";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a line", 
            lineNumber: 1, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL2", 
            lineNumber: 2,
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 26, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.validation.result4";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a message", 
            lineNumber: 1, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL3", 
            lineNumber: 2, 
            column: 1, 
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "KEY", 
            lineNumber: 4, 
            column: 1, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.validation.result5";
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoEndDoubleQuoteMessage, 
            lineNumber: 1, 
            column: 1,
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 2, 
            column: 12,
            envFileName: fileName
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND_2", 
            lineNumber: 3, 
            column: 16,
            envFileName: fileName
        ));

        result.ErrorMessages
              .Should()
              .Contain(string.Format(FileNotFoundMessage, $"{basePath}.env.not.found3"));
        result.ErrorMessages
              .Should()
              .Contain(string.Format(FileNotFoundMessage, $"{basePath}.env.not.found4"));
        result.ErrorMessages
              .Should()
              .Contain(string.Format(FileNotFoundMessage, $"{basePath}.env.not.found5"));
        result.ErrorMessages
              .Should()
              .Contain(string.Format(FileNotFoundMessage, $"{basePath}.env.not.found6"));
    }
}
