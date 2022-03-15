using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;
using System.Text;

namespace DotEnv.Core.Tests.Loader
{
    [TestClass]
    public class EnvLoaderTests
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
        public void Load_WhenEnvFilesAreOptional_ShouldNotGenerateErrors()
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
        public void Load_WhenAllEnvFilesAreOptional_ShouldNotGenerateErrors()
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
        public void Load_WhenDefaultEnvFileIsOptional_ShouldNotGenerateErrors()
        {
            new EnvLoader()
                .SetBasePath("dotnet/files")
                .AllowAllEnvFilesOptional()
                .Load(out var result);

            Assert.AreEqual(expected: false, actual: result.HasError());
            Assert.AreEqual(expected: 0, actual: result.Count);
        }

        [TestMethod]
        public void Load_WhenIgnoresSearchInParentDirectories_ShouldNotLoadTheEnvFile()
        {
            new EnvLoader()
                .SetBasePath("Loader/env_files")
                .AddEnvFile(".env.parent.directories")
                .IgnoreParentDirectories()
                .Load();

            Assert.IsNull(GetEnvironmentVariable("PARENT_DIRECTORIES"));
        }

        [TestMethod]
        public void Load_WhenLoadEnvFileWithDefaultConfig_ShouldBeAbleToReadEnvironmentVariables()
        {
            var loader = new EnvLoader();

            loader.Load();

            Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("CONFIG_DEFAULT_1"));
            Assert.AreEqual(expected: "VAL2", actual: GetEnvironmentVariable("CONFIG_DEFAULT_2"));
        }

        [TestMethod]
        public void Load_WhenLoadEnvFileWithCustomConfig_ShouldBeAbleToReadEnvironmentVariables()
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
        public void Load_WhenSetsEncoding_ShouldBeAbleToReadEnvironmentVariables()
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
        public void Load_WhenLoadMultiEnvFiles_ShouldBeAbleToReadEnvironmentVariables()
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
        public void Load_WhenEnvFilesHasNoExtension_ShouldBeAbleToReadEnvironmentVariables()
        {
            new EnvLoader()
                .SetBasePath("Loader/env_files")
                .SetDefaultEnvFileName(".env.dev")
                .AddEnvFiles("./foo", "./bar")
                .Load();

            Assert.IsNotNull(GetEnvironmentVariable("HAS_NOT_EXTENSION_1"));
            Assert.IsNotNull(GetEnvironmentVariable("HAS_NOT_EXTENSION_2"));
        }

        [TestMethod]
        public void Load_WhenEnvFileIsInTheCurrentDirectory_ShouldBeAbleToReadEnvironmentVariables()
        {
            new EnvLoader()
                .SetDefaultEnvFileName(".env.local")
                .Load();

            Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("CURRENT_DIRECTORY"));
        }

        [TestMethod]
        public void Load_WhenPathIsAbsolute_ShouldBeAbleToReadEnvironmentVariables()
        {
            string absolutePath = Directory.GetCurrentDirectory();

            new EnvLoader()
                .SetBasePath(absolutePath)
                .AddEnvFile(".env.absolute")
                .Load();

            Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("PATH_ABSOLUTE"));
        }

        [TestMethod]
        public void Load_WhenPathIsRelative_ShouldBeAbleToReadEnvironmentVariables()
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
        public void LoadEnv_WhenEnvFileNotFound_ShouldThrowFileNotFoundException()
        {
            var loader = new EnvLoader()
                            .SetBasePath("environment/files")
                            .SetDefaultEnvFileName(".env.example")
                            .AddEnvFiles(".env.example1", "foo/")
                            .EnableFileNotFoundException();

            void action() => loader.LoadEnv();

            Assert.ThrowsException<FileNotFoundException>(action);
        }

        [TestMethod]
        public void LoadEnv_WhenEnvironmentIsNotDefined_ShouldBeAbleToReadEnvironmentVariables()
        {
            new EnvLoader()
                .SetBasePath("Loader/env_files/environment/dev")
                .LoadEnv();

            new EnvLoader()
                .SetBasePath("Loader/env_files/environment/development")
                .LoadEnv();

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
        public void LoadEnv_WhenEnvironmentIsDefined_ShouldBeAbleToReadEnvironmentVariables()
        {
            Env.CurrentEnvironment = "test";

            new EnvLoader()
                .SetBasePath("Loader/env_files/environment/test")
                .LoadEnv();

            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV"));
            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_TEST"));
            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_TEST_LOCAL"));
            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_LOCAL"));
            Env.CurrentEnvironment = null;
        }

        [TestMethod]
        public void LoadEnv_WhenErrorsAreFound_ShouldThrowParserException()
        {
            var loader = new EnvLoader().SetBasePath("Loader/env_files/environment/production");
            Env.CurrentEnvironment = "production";

            void action() => loader.LoadEnv();

            Assert.ThrowsException<ParserException>(action);
            Env.CurrentEnvironment = null;
        }

        [TestMethod]
        public void LoadEnv_WhenSetsTheEnvironmentName_ShouldBeAbleToReadVariables()
        {
            var loader = new EnvLoader()
                            .AvoidModifyEnvironment()
                            .SetEnvironmentName("test")
                            .SetBasePath("Loader/env_files/environment/test");

            var keyValuePairs = loader.LoadEnv();

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
    }
}
