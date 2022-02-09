﻿using System;
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

            Action action = () => loader.Load();

            Assert.ThrowsException<ParserException>(action);
        }

        [TestMethod]
        public void Load_WhenLoadEnvFileWithDefaultConfig_ShouldBeAbleToReadEnvironmentVariables()
        {
            var loader = new EnvLoader();

            loader.Load();

            Assert.AreEqual("VAL1", GetEnvironmentVariable("CONFIG_DEFAULT_1"));
            Assert.AreEqual("VAL2", GetEnvironmentVariable("CONFIG_DEFAULT_2"));
        }

        [TestMethod]
        public void Load_WhenLoadEnvFileWithCustomConfig_ShouldBeAbleToReadEnvironmentVariables()
        {
            new EnvLoader(new CustomEnvParser())
                .SetBasePath("Loader/env_files")
                .DisableTrimStartValues()
                .DisableTrimEndValues()
                .DisableTrimStartKeys()
                .DisableTrimEndKeys()
                .DisableTrimStartComments()
                .AllowOverwriteExistingVars()
                .AddEnvFile(".env.custom.configuration")
                .Load();

            Assert.AreEqual(" VAL1 ", GetEnvironmentVariable(" CONFIG_CUSTOM_1 "));
            Assert.AreEqual(" VAL2 ", GetEnvironmentVariable(" CONFIG_CUSTOM_2 "));
        }

        [TestMethod]
        public void Load_WhenLoadMultiEnvFiles_ShouldBeAbleToReadEnvironmentVariables()
        {
            string absolutePath = Directory.GetCurrentDirectory();
            new EnvLoader(new CustomEnvParser())
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

            Assert.AreEqual("VAL1", GetEnvironmentVariable("CURRENT_DIRECTORY"));
        }

        [TestMethod]
        public void Load_WhenPathIsAbsolute_ShouldBeAbleToReadEnvironmentVariables()
        {
            string absolutePath = Directory.GetCurrentDirectory();

            new EnvLoader()
                .SetBasePath(absolutePath)
                .AddEnvFile(".env.absolute")
                .Load();

            Assert.AreEqual("VAL1", GetEnvironmentVariable("PATH_ABSOLUTE"));
        }

        [TestMethod]
        public void Load_WhenPathIsRelative_ShouldBeAbleToReadEnvironmentVariables()
        {
            string relativePath = "./dotenv/files";

            new EnvLoader()
                .SetBasePath(relativePath)
                .AddEnvFile(".env.relative")
                .Load();

            Assert.AreEqual("VAL1", GetEnvironmentVariable("PATH_RELATIVE"));
        }

        [TestMethod]
        public void Load_WhenEnvFileNotFound_ShouldThrowFileNotFoundException()
        {
            Action action = () =>
            {
                new EnvLoader()
                 .AddEnvFiles(
                        ".env.not.found",
                        ".env.not.found3", 
                        ".env.not.found4", 
                        ".env.not.found5", 
                        ".env.not.found6"
                    )
                 .EnableFileNotFoundException()
                 .Load();
            };

            Assert.ThrowsException<FileNotFoundException>(action);
        }

        [TestMethod]
        public void LoadEnv_WhenEnvFileNotFound_ShouldThrowFileNotFoundException()
        {
            Action action = () =>
            {
                new EnvLoader()
                 .SetBasePath("environment/files")
                 .SetDefaultEnvFileName(".env.example")
                 .AddEnvFiles(".env.example1", "foo/")
                 .EnableFileNotFoundException()
                 .LoadEnv();
            };

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
            SetEnvironmentVariable("DOTNET_ENV", "test");

            new EnvLoader()
                .SetBasePath("Loader/env_files/environment/test")
                .LoadEnv();

            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV"));
            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_TEST"));
            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_TEST_LOCAL"));
            Assert.IsNotNull(GetEnvironmentVariable("TEST_ENV_LOCAL"));
            SetEnvironmentVariable("DOTNET_ENV", null);
        }

        [TestMethod]
        public void LoadEnv_WhenErrorsAreFound_ShouldThrowParserException()
        {
            var loader = new EnvLoader().SetBasePath("Loader/env_files/environment/production");
            SetEnvironmentVariable("DOTNET_ENV", "production");

            Action action = () => loader.LoadEnv();

            Assert.ThrowsException<ParserException>(action);
            SetEnvironmentVariable("DOTNET_ENV", null);
        }

        [TestMethod]
        public void LoadEnv_WhenSetsTheEnvironmentName_ShouldBeAbleToReadVariables()
        {
            var loader = new EnvLoader()
                            .AvoidModifyEnvironment()
                            .SetEnvironmentName("test")
                            .SetBasePath("Loader/env_files/environment/test");

            var dict = loader.LoadEnv();

            Assert.IsNotNull(dict["TEST_ENV"]);
            Assert.IsNotNull(dict["TEST_ENV_TEST"]);
            Assert.IsNotNull(dict["TEST_ENV_TEST_LOCAL"]);
            Assert.IsNotNull(dict["TEST_ENV_LOCAL"]);
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
    }
}
