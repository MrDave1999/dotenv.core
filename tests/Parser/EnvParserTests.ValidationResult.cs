namespace DotEnv.Core.Tests.Parser;

public partial class EnvParserTests
{
    [TestMethod]
    public void Parse_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        // Arrange
        var parser = new EnvParser().IgnoreParserException();
        string env;
        EnvValidationResult result;

        // Act
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
                KEY #comment=VAL
            ";
        parser.Parse(env, out _);

        env = @"
                MULTI_UNENDED=""THIS
                LINE HAS ${VARIABLE_NOT_FOUND}
                NO END QUOTE ${VARIABLE_NOT_FOUND_2}'
            ";
        parser.Parse(env, out result);

        // Asserts
        result.HasError().Should().BeTrue();
        result.Should().HaveCount(16);

        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is an error", 
            lineNumber: 1, 
            column: 1
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "                =VAL1",
            lineNumber: 2, 
            column: 1
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 40
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND_2", 
            lineNumber: 3, 
            column: 69
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${}", 
            lineNumber: 5, 
            column: 38
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableIsAnEmptyStringMessage, 
            actualValue: "${   }", 
            lineNumber: 5, 
            column: 55
        ));

        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            DataSourceIsEmptyOrWhitespaceMessage
        ));

        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a line", 
            lineNumber: 1, 
            column: 1
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "                =VAL2", 
            lineNumber: 2, 
            column: 1
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 40
        ));

        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "This is a message", 
            lineNumber: 1, 
            column: 1
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "                =VAL3", 
            lineNumber: 2, 
            column: 1
        ));

        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoKeyValuePairMessage, 
            actualValue: "                KEY", 
            lineNumber: 3, 
            column: 1
        ));

        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            LineHasNoEndDoubleQuoteMessage, 
            lineNumber: 2, 
            column: 1
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND", 
            lineNumber: 3, 
            column: 28
        ));
        result.ErrorMessages.Should().Contain(FormatParserExceptionMessage(
            VariableNotSetMessage, 
            actualValue: "VARIABLE_NOT_FOUND_2", 
            lineNumber: 4, 
            column: 32
        ));
    }
}
