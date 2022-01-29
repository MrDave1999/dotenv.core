using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotEnv.Core.ExceptionMessages;
using static System.IO.Path;

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
            string basePath = $"Loader{DirectorySeparatorChar}env_files{DirectorySeparatorChar}";
            new EnvLoader()
                .SetBasePath(basePath)
                .IgnoreParserExceptions()
                .AddEnvFile(".env.validation.result1")
                .AddEnvFile(".env.validation.result2")
                .AddEnvFile(".env.validation.result3")
                .AddEnvFile(".env.validation.result4")
                .AddEnvFiles(".env.not.found3", ".env.not.found4", ".env.not.found5", ".env.not.found6")
                .Load(out result);

            msg = result.ErrorMessages;
            Assert.AreEqual(result.HasError(), true);

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is an error, Line: 1, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND, Line: 3, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND_2, Line: 3, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableIsAnEmptyStringMessage} (Line: 5, FileName: {basePath}.env.validation.result1)");
            StringAssert.Contains(msg, $"{VariableIsAnEmptyStringMessage} (Line: 5, FileName: {basePath}.env.validation.result1)");

            StringAssert.Contains(msg, $"{InputIsEmptyOrWhitespaceMessage} (FileName: {basePath}.env.validation.result2)");

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
    }
}
