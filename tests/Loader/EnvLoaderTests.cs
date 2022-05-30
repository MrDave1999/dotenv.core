namespace DotEnv.Core.Tests.Loader;

[TestClass]
public partial class EnvLoaderTests
{
    [TestMethod]
    public void Load_WhenErrorsAreFound_ShouldThrowParserException()
    {
        var loader = new EnvLoader()
            .SetBasePath("Loader/env_files/validation")
            .AddEnvFile(".env.validation.result1")
            .AddEnvFile(".env.validation.result2")
            .AddEnvFile(".env.validation.result3")
            .AddEnvFile(".env.validation.result4");

        void action() => loader.Load();

        Assert.ThrowsException<ParserException>(action);
    }

    [TestMethod]
    public void Load_WhenEnvFilesAreOptional_ShouldNotGenerateAnErrorIfTheFileDoesNotExist()
    {
        new EnvLoader()
            .AddEnvFile(".env.example1", Encoding.UTF8, optional: true)
            .AddEnvFile(".env.example2", encodingName: "UTF-8", optional: true)
            .AddEnvFile(".env.example3", optional: true)
            .Load(out var result);

        Assert.AreEqual(expected: false, actual: result.HasError());
        Assert.AreEqual(expected: 0, actual: result.Count);
    }

    [TestMethod]
    public void Load_WhenAllEnvFilesAreOptional_ShouldNotGenerateAnErrorIfTheFileDoesNotExist()
    {
        new EnvLoader()
            .AddEnvFile(".env.example1", Encoding.UTF8)
            .AddEnvFile(".env.example2", encodingName: "UTF-8")
            .AddEnvFile(".env.example3")
            .AddEnvFiles(".env.example4", ".env.example5")
            .AddEnvFile(".env.example6", Encoding.UTF8, optional: true)
            .AddEnvFile(".env.example7", encodingName: "UTF-8", optional: true)
            .AddEnvFile(".env.example8", optional: true)
            .AllowAllEnvFilesOptional()
            .Load(out var result);

        Assert.AreEqual(expected: false, actual: result.HasError());
        Assert.AreEqual(expected: 0, actual: result.Count);
    }

    [TestMethod]
    public void Load_WhenDefaultEnvFileIsOptional_ShouldNotGenerateAnErrorIfTheFileDoesNotExist()
    {
        new EnvLoader()
            .SetBasePath("dotnet/files")
            .AllowAllEnvFilesOptional()
            .Load(out var result); // Loads a default file: .env

        Assert.AreEqual(expected: false, actual: result.HasError());
        Assert.AreEqual(expected: 0, actual: result.Count);
    }

    [TestMethod]
    public void Load_WhenIgnoresSearchInParentDirectories_ShouldOnlySearchInTheCurrentDirectory()
    {
        new EnvLoader()
            .SetBasePath("Loader/env_files")
            .AddEnvFile(".env.parent.directories")
            .IgnoreParentDirectories()
            .Load();

        Assert.IsNull(GetEnvironmentVariable("PARENT_DIRECTORIES"));
    }

    [TestMethod]
    public void Load_WhenLoadEnvFileWithDefaultConfig_ShouldSetEnvironmentVariablesFromAnEnvFile()
    {
        var loader = new EnvLoader();

        loader.Load();

        Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("CONFIG_DEFAULT_1"));
        Assert.AreEqual(expected: "VAL2", actual: GetEnvironmentVariable("CONFIG_DEFAULT_2"));
    }

    [TestMethod]
    public void Load_WhenLoadEnvFileWithCustomConfig_ShouldSetEnvironmentVariablesFromAnEnvFile()
    {
        new EnvLoader()
            .SetBasePath("Loader/env_files")
            .DisableTrimStartValues()
            .DisableTrimEndValues()
            .DisableTrimStartKeys()
            .DisableTrimEndKeys()
            .DisableTrimStartComments()
            .AllowOverwriteExistingVars()
            .AddEnvFile(".env.custom.configuration")
            .Load();

        Assert.AreEqual(expected: " VAL1 ", actual: GetEnvironmentVariable(" CONFIG_CUSTOM_1 "));
        Assert.AreEqual(expected: " VAL2 ", actual: GetEnvironmentVariable(" CONFIG_CUSTOM_2 "));
    }

    [TestMethod]
    public void Load_WhenSetsEncoding_ShouldLoadTheEnvFileCorrectly()
    {
        new EnvLoader()
            .SetBasePath("Loader/env_files")
            .SetEncoding("UTF-8")
            .AddEnvFile(".env.unicode.chinese")
            .AddEnvFile(".env.unicode.russian")
            .Load();

        Assert.AreEqual(expected: "我们先 坐二号线% 然后 换一号线%", actual: GetEnvironmentVariable("UNICODE_CHINESE"));
        Assert.AreEqual(expected: "Привет мир! Привет, чем занимаешься", actual: GetEnvironmentVariable("UNICODE_RUSSIAN"));
    }

    [TestMethod]
    public void SetEncoding_WhenEncodingNameIsNotSupported_ShouldThrowArgumentException()
    {
        var loader = new EnvLoader();

        void action() => loader.SetEncoding("UTF-88");

        Assert.ThrowsException<ArgumentException>(action);
    }

    [TestMethod]
    public void AddEnvFile_WhenEncodingNameIsNotSupported_ShouldThrowArgumentException()
    {
        var loader = new EnvLoader();

        void action() => loader.AddEnvFile(".env", "UTF-88");

        Assert.ThrowsException<ArgumentException>(action);
    }

    [TestMethod]
    public void Load_WhenLoadMultiEnvFiles_ShouldSetEnvironmentVariablesFromAnEnvFile()
    {
        string absolutePath = Directory.GetCurrentDirectory();
        new EnvLoader()
            .SetBasePath("Loader/env_files/multi")
            .AddEnvFiles(".env.multi1", "./", ".env.multi2")
            .AddEnvFile(".env.multi3")
            .AddEnvFile(".env.multi4")
            .AddEnvFile(".env.multi5")
            .AddEnvFile($"{absolutePath}/.env.multi6.absolute")
            .Load();

        Assert.IsNotNull(GetEnvironmentVariable("ENV_MULTI"));
        Assert.IsNotNull(GetEnvironmentVariable("ENV_MULTI_1"));
        Assert.IsNotNull(GetEnvironmentVariable("ENV_MULTI_2"));
        Assert.IsNotNull(GetEnvironmentVariable("ENV_MULTI_3"));
        Assert.IsNotNull(GetEnvironmentVariable("ENV_MULTI_4"));
        Assert.IsNotNull(GetEnvironmentVariable("ENV_MULTI_5"));
        Assert.IsNotNull(GetEnvironmentVariable("ENV_MULTI_6"));
    }

    [TestMethod]
    public void Load_WhenEnvFileIsAddedAsDirectory_ShouldCombineDirectoryWithDefaultEnvFileName()
    {
        new EnvLoader()
            .SetBasePath("Loader/env_files")
            .SetDefaultEnvFileName(".env.dev")
            .AddEnvFiles("./foo", "./bar") // Equivalent to: ./foo/.env.dev, ./bar/.env.dev
            .Load();

        Assert.IsNotNull(GetEnvironmentVariable("HAS_NOT_EXTENSION_1"));
        Assert.IsNotNull(GetEnvironmentVariable("HAS_NOT_EXTENSION_2"));
    }

    [TestMethod]
    public void Load_WhenEnvFileIsInTheCurrentDirectory_ShouldReadContentsOfTheEnvFile()
    {
        new EnvLoader()
            .SetDefaultEnvFileName(".env.local")
            .Load();

        Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("CURRENT_DIRECTORY"));
    }

    [TestMethod]
    public void Load_WhenEnvFileIsInAnAbsolutePath_ShouldReadContentsOfTheEnvFile()
    {
        string absolutePath = Directory.GetCurrentDirectory();

        new EnvLoader()
            .SetBasePath(absolutePath)
            .AddEnvFile(".env.absolute")
            .AddEnvFile(Path.Combine(absolutePath, ".env.absolute2"))
            .Load();

        Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("PATH_ABSOLUTE"));
        Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("PATH_ABSOLUTE2"));
    }

    [TestMethod]
    public void Load_WhenEnvFileIsInAnRelativePath_ShouldReadContentsOfTheEnvFile()
    {
        string relativePath = "./dotenv/files";

        new EnvLoader()
            .SetBasePath(relativePath)
            .AddEnvFile(".env.relative")
            .Load();

        Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("PATH_RELATIVE"));
    }

    [TestMethod]
    public void Load_WhenEnvFileNotFound_ShouldThrowFileNotFoundException()
    {
        var loader = new EnvLoader()
                        .AddEnvFiles(".env.not.found", ".env.not.found3", ".env.not.found4", ".env.not.found5", ".env.not.found6")
                        .EnableFileNotFoundException();

        void action() => loader.Load();

        Assert.ThrowsException<FileNotFoundException>(action);
    }

    [TestMethod]
    public void Load_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        string msg;
        EnvValidationResult result;
        string basePath = $"Loader/env_files/validation/";
        new EnvLoader()
            .SetBasePath(basePath)
            .IgnoreParserException()
            .AddEnvFile(".env.validation.result1")
            .AddEnvFile(".env.validation.result2")
            .AddEnvFile(".env.validation.result3")
            .AddEnvFile(".env.validation.result4")
            .AddEnvFiles(".env.not.found3", ".env.not.found4", ".env.not.found5", ".env.not.found6")
            .Load(out result);

        msg = result.ErrorMessages;
        Assert.AreEqual(expected: true, actual: result.HasError());
        Assert.AreEqual(expected: 16, actual: result.Count);

        var fileName = $"{basePath}.env.validation.result1";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is an error", 
            lineNumber: 1, 
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL1", 
            lineNumber: 2, 
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 26, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND_2", 
            lineNumber: 3, 
            column: 55, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${}", 
            lineNumber: 5, 
            column: 24, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${   }", 
            lineNumber: 5, 
            column: 41, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.validation.result2";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            DataSourceIsEmptyOrWhitespaceMessage, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.validation.result3";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a line", 
            lineNumber: 1, 
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL2", 
            lineNumber: 2,
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 26, 
            envFileName: fileName
        ));

        fileName = $"{basePath}.env.validation.result4";
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a message", 
            lineNumber: 1, 
            column: 1, 
            envFileName: fileName
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "=VAL3", 
            lineNumber: 2, 
            column: 1, 
            envFileName: fileName
        ));

        StringAssert.Contains(msg, string.Format(FileNotFoundMessage, $"{basePath}.env.not.found3"));
        StringAssert.Contains(msg, string.Format(FileNotFoundMessage, $"{basePath}.env.not.found4"));
        StringAssert.Contains(msg, string.Format(FileNotFoundMessage, $"{basePath}.env.not.found5"));
        StringAssert.Contains(msg, string.Format(FileNotFoundMessage, $"{basePath}.env.not.found6"));
    }
}
