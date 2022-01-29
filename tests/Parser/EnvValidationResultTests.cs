using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotEnv.Core.ExceptionMessages;

namespace DotEnv.Core.Tests.Parser
{
    [TestClass]
    public class EnvValidationResultTests
    {
        [TestMethod]
        public void Parse_WhenErrorsAreFound_ShouldReadTheErrors()
        {
            var parser = new EnvParser().IgnoreParserExceptions();
            string env, msg;
            EnvValidationResult result;

            env = @"This is an error
                =VAL1
                ERRORS_ARE_FOUND_1 = ${VARIABLE_NOT_FOUND} World! ${VARIABLE_NOT_FOUND_2}
                ERRORS_ARE_FOUND_3 = VAL
                ERRORS_ARE_FOUND_2 = ${} World! Hello ${   }
            ";
            parser.Parse(env, out _);

            env = null;
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
            Assert.AreEqual(result.HasError(), true);

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is an error, Line: 1)");
            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND, Line: 3)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND_2, Line: 3)");
            StringAssert.Contains(msg, $"{VariableIsAnEmptyStringMessage} (Line: 5)");
            StringAssert.Contains(msg, $"{VariableIsAnEmptyStringMessage} (Line: 5)");

            StringAssert.Contains(msg, InputIsEmptyOrWhitespaceMessage);

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a line, Line: 1)");
            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2)");
            StringAssert.Contains(msg, $"{VariableNotFoundMessage} (Actual Value: VARIABLE_NOT_FOUND, Line: 3)");

            StringAssert.Contains(msg, $"{LineHasNoKeyValuePairMessage} (Actual Value: This is a message, Line: 1)");
            StringAssert.Contains(msg, $"{KeyIsAnEmptyStringMessage} (Line: 2)");
        }
    }
}
