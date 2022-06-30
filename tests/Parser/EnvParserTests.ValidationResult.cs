namespace DotEnv.Core.Tests.Parser;

public partial class EnvParserTests
{
    [TestMethod]
    public void Parse_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
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
        parser.Parse(env, out _);

        env = @"
                MULTI_UNENDED=""THIS
                LINE HAS ${VARIABLE_NOT_FOUND}
                NO END QUOTE ${VARIABLE_NOT_FOUND_2}'
            ";
        parser.Parse(env, out result);

        msg = result.ErrorMessages;
        Assert.AreEqual(expected: true, actual: result.HasError());
        Assert.AreEqual(expected: 15, actual: result.Count);

        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is an error", 
            lineNumber: 1, 
            column: 1
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "                =VAL1",
            lineNumber: 2, 
            column: 1
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 40
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND_2", 
            lineNumber: 3, 
            column: 69
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${}", 
            lineNumber: 5, 
            column: 38
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${   }", 
            lineNumber: 5, 
            column: 55
        ));

        StringAssert.Contains(msg, FormatParserExceptionMessage(DataSourceIsEmptyOrWhitespaceMessage));

        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a line", 
            lineNumber: 1, 
            column: 1
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "                =VAL2", 
            lineNumber: 2, 
            column: 1
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 40
        ));

        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a message", 
            lineNumber: 1, 
            column: 1
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "                =VAL3", 
            lineNumber: 2, 
            column: 1
        ));

        StringAssert.Contains(msg, FormatParserExceptionMessage(
            LineHasNoEndDoubleQuoteMessage, 
            lineNumber: 2, 
            column: 1
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 28
        ));
        StringAssert.Contains(msg, FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND_2", 
            lineNumber: 4, 
            column: 32
        ));
    }
}
