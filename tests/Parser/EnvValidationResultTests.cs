using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotEnv.Core.ExceptionMessages;
using static DotEnv.Core.FormattingMessage;

namespace DotEnv.Core.Tests.Parser
{
    [TestClass]
    public class EnvValidationResultTests
    {
        [TestMethod]
        public void Parse_WhenErrorsAreFound_ShouldReadTheErrors()
        {
            var parser = new EnvParser().IgnoreParserException();
            string env, msg;
            EnvValidationResult result;

            env = @"This is an error
                =VAL1
                ERRORS_ARE_FOUND_1 = ${VARIABLE_NOT_FOUND} World! ${VARIABLE_NOT_FOUND_2}
                ERRORS_ARE_FOUND_3 = VAL
                ERRORS_ARE_FOUND_2 = ${} World! Hello ${   }
            ";
            parser.Parse(env, out _);

            env = "";
            parser.Parse(env, out _);

            env = @"This is a line
                =VAL2
                ERRORS_ARE_FOUND_4 = ${VARIABLE_NOT_FOUND}!!
            ";
            parser.Parse(env, out _);

            env = @"This is a message
                =VAL3
            ";
            parser.Parse(env, out result);

            msg = result.ErrorMessages;
            Assert.AreEqual(expected: true, actual: result.HasError());
            Assert.AreEqual(expected: 12, actual: result.Count);

            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: "This is an error", lineNumber: 1));
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 2));
            StringAssert.Contains(msg, FormatParserExceptionMessage(InterpolatedVariableNotFoundMessage, actualValue: "VARIABLE_NOT_FOUND", lineNumber: 3));
            StringAssert.Contains(msg, FormatParserExceptionMessage(InterpolatedVariableNotFoundMessage, actualValue: "VARIABLE_NOT_FOUND_2", lineNumber: 3));
            StringAssert.Contains(msg, FormatParserExceptionMessage(VariableIsAnEmptyStringMessage, lineNumber: 5));
            StringAssert.Contains(msg, FormatParserExceptionMessage(VariableIsAnEmptyStringMessage, lineNumber: 5));

            StringAssert.Contains(msg, FormatParserExceptionMessage(DataSourceIsEmptyOrWhitespaceMessage));

            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: "This is a line", lineNumber: 1));
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 2));
            StringAssert.Contains(msg, FormatParserExceptionMessage(InterpolatedVariableNotFoundMessage, actualValue: "VARIABLE_NOT_FOUND", lineNumber: 3));


            StringAssert.Contains(msg, FormatParserExceptionMessage(LineHasNoKeyValuePairMessage, actualValue: "This is a message", lineNumber: 1));
            StringAssert.Contains(msg, FormatParserExceptionMessage(KeyIsAnEmptyStringMessage, lineNumber: 2));
        }
    }
}
