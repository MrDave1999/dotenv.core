using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Environment;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

namespace DotEnv.Core.Tests.Loader
{
    public partial class EnvLoaderTests
    {
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

        [TestMethod]
        public void LoadEnv_WhenErrorsAreFound_ShouldReadTheErrors()
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
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 2, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: "PROD", lineNumber: 5, envFileName: fileName));

            var value = "This is an error";
            fileName = $"{basePath}.env.local";
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: value, lineNumber: 4, envFileName: fileName));

            fileName = $"{basePath}.env.production";
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 3, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: value, lineNumber: 5, envFileName: fileName));

            fileName = $"{basePath}.env";
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: value, lineNumber: 4, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: value, lineNumber: 6, envFileName: fileName));
            Env.CurrentEnvironment = null;
        }

        [TestMethod]
        public void LoadEnv_WhenLocalEnvFilesNotExistAndEnvironmentIsNotDefined_ShouldReadTheErrors()
        {
            var loader = new EnvLoader();

            loader
                .SetBasePath("environment/env_files")
                .LoadEnv(out var result);

            Assert.AreEqual(expected: true, actual: result.HasError());
            Assert.AreEqual(expected: 1, actual: result.Count);
            StringAssert.Contains(result.ErrorMessages, FormatFileNotPresentLoadEnvMessage(FileNotPresentLoadEnvMessage));
        }

        [TestMethod]
        public void LoadEnv_WhenLocalEnvFilesNotExistAndEnvironmentIsDefined_ShouldReadTheErrors()
        {
            var loader = new EnvLoader();
            var environment = "test";
            Env.CurrentEnvironment = environment;

            loader
                .SetBasePath("environment/env_files")
                .LoadEnv(out var result);

            Assert.AreEqual(expected: true, actual: result.HasError());
            Assert.AreEqual(expected: 1, actual: result.Count);
            StringAssert.Contains(result.ErrorMessages, FormatFileNotPresentLoadEnvMessage(FileNotPresentLoadEnvMessage, environment));
            Env.CurrentEnvironment = null;
        }
    }
}
