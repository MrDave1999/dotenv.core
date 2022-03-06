using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;
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
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: "This is an error", lineNumber: 1, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 2, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(InterpolatedVariableNotSetMessage, actualValue: "VARIABLE_NOT_FOUND", lineNumber: 3, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(InterpolatedVariableNotSetMessage, actualValue: "VARIABLE_NOT_FOUND_2", lineNumber: 3, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(VariableIsAnEmptyStringMessage, lineNumber: 5, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(VariableIsAnEmptyStringMessage, lineNumber: 5, envFileName: fileName));

            fileName = $"{basePath}.env.validation.result2";
            StringAssert.Contains(msg, FormatParserExceptionMessage(DataSourceIsEmptyOrWhitespaceMessage, envFileName: fileName));

            fileName = $"{basePath}.env.validation.result3";
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: "This is a line", lineNumber: 1, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 2, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(InterpolatedVariableNotSetMessage, actualValue: "VARIABLE_NOT_FOUND", lineNumber: 3, envFileName: fileName));

            fileName = $"{basePath}.env.validation.result4";
            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: "This is a message", lineNumber: 1, envFileName: fileName));
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 2, envFileName: fileName));

            StringAssert.Contains(msg, FormatFileNotFoundExceptionMessage(FileNotFoundMessage, envFileName: $"{basePath}.env.not.found3"));
            StringAssert.Contains(msg, FormatFileNotFoundExceptionMessage(FileNotFoundMessage, envFileName: $"{basePath}.env.not.found4"));
            StringAssert.Contains(msg, FormatFileNotFoundExceptionMessage(FileNotFoundMessage, envFileName: $"{basePath}.env.not.found5"));
            StringAssert.Contains(msg, FormatFileNotFoundExceptionMessage(FileNotFoundMessage, envFileName: $"{basePath}.env.not.found6"));
        }

        [TestMethod]
        public void LoadEnv_WhenErrorsAreFound_ShouldReadTheErrors()
        {
            string msg;
            EnvValidationResult result;
            char sep = DirectorySeparatorChar;
            string basePath = $"Loader{sep}env_files{sep}environment{sep}production{sep}";
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
