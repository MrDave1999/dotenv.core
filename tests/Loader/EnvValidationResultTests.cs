using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotEnv.Core.ExceptionMessages;
using static System.IO.Path;
using static System.Environment;

namespace DotEnv.Core.Tests.Loader
{
    [TestClass]
    public class EnvValidationResultTests
    {
        [TestMethod]
        public void Load_WhenErrorsAreFound_ShouldReadTheErrors()
        {
            string msg;
            EnvValidationResult result;
            char sep = DirectorySeparatorChar;
            string basePath = $"Loader{sep}env_files{sep}validation{sep}";
            new EnvLoader()
                .SetBasePath(basePath)
                .DisableParserException()
                .AddEnvFile(".env.validation.result1")
                .AddEnvFile(".env.validation.result2")
                .AddEnvFile(".env.validation.result3")
                .AddEnvFile(".env.validation.result4")
                .AddEnvFiles(".env.not.found3", ".env.not.found4", ".env.not.found5", ".env.not.found6")
                .Load(out result);

            msg = result.ErrorMessages;
            Assert.AreEqual(true, result.HasError());
            Assert.AreEqual(16, result.Count);

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is an error, Line: 1, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND, Line: 3, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND_2, Line: 3, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableIsAnEmptyStringMessage} (Line: 5, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableIsAnEmptyStringMessage} (Line: 5, FileName: {basePath}.env.validation.result1)");

            StringAssert.Contains(msg, $"{DataSourceIsEmptyOrWhitespaceMessage} (FileName: {basePath}.env.validation.result2)");

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a line, Line: 1, FileName: {basePath}.env.validation.result3)");
            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2, FileName: {basePath}.env.validation.result3)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND, Line: 3, FileName: {basePath}.env.validation.result3)");

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a message, Line: 1, FileName: {basePath}.env.validation.result4)");
            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2, FileName: {basePath}.env.validation.result4)");

            StringAssert.Contains(msg, $"{FileNotFoundMessage} (FileName: {basePath}.env.not.found3)");
            StringAssert.Contains(msg, $"{FileNotFoundMessage} (FileName: {basePath}.env.not.found4)");
            StringAssert.Contains(msg, $"{FileNotFoundMessage} (FileName: {basePath}.env.not.found5)");
            StringAssert.Contains(msg, $"{FileNotFoundMessage} (FileName: {basePath}.env.not.found6)");
        }

        [TestMethod]
        public void LoadEnv_WhenErrorsAreFound_ShouldReadTheErrors()
        {
            string msg;
            EnvValidationResult result;
            char sep = DirectorySeparatorChar;
            string basePath = $"Loader{sep}env_files{sep}environment{sep}production{sep}";
            SetEnvironmentVariable("DOTNET_ENV", "production");

            new EnvLoader()
                .SetBasePath(basePath)
                .DisableParserException()
                .LoadEnv(out result);

            msg = result.ErrorMessages;
            Assert.AreEqual(true, result.HasError());
            Assert.AreEqual(7, result.Count);

            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2, FileName: {basePath}.env.production.local)");
            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: PROD, Line: 5, FileName: {basePath}.env.production.local)");

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a error, Line: 4, FileName: {basePath}.env.local)");

            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 3, FileName: {basePath}.env.production)");
            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a error, Line: 5, FileName: {basePath}.env.production)");

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a error, Line: 4, FileName: {basePath}.env)");
            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a error, Line: 6, FileName: {basePath}.env)");
            SetEnvironmentVariable("DOTNET_ENV", null);
        }

        [TestMethod]
        public void LoadEnv_WhenLocalEnvFilesNotExistAndEnvironmentIsNotDefined_ShouldReadTheErrors()
        {
            var loader = new EnvLoader();

            loader
                .SetBasePath("environment/env_files")
                .LoadEnv(out var result);

            Assert.AreEqual(true, result.HasError());
            Assert.AreEqual(1, result.Count);
            StringAssert.Contains(result.ErrorMessages, $"{FileNotPresentLoadEnvMessage}: .env.development.local or .env.dev.local or .env.local");
        }

        [TestMethod]
        public void LoadEnv_WhenLocalEnvFilesNotExistAndEnvironmentIsDefined_ShouldReadTheErrors()
        {
            var loader = new EnvLoader();
            SetEnvironmentVariable("DOTNET_ENV", "test");

            loader
                .SetBasePath("environment/env_files")
                .LoadEnv(out var result);

            Assert.AreEqual(true, result.HasError());
            Assert.AreEqual(1, result.Count);
            StringAssert.Contains(result.ErrorMessages, $"{FileNotPresentLoadEnvMessage}: .env.test.local or .env.local");
            SetEnvironmentVariable("DOTNET_ENV", null);
        }
    }
}
