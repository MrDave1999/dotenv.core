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
                .SetBasePath("Loader/env_files")
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
                 .AddEnvFile(".env.not.found")
                 .EnableFileNotFoundException()
                 .Load();
            };

            Assert.ThrowsException<FileNotFoundException>(action);
        }
    }
}
