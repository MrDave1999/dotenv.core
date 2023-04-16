namespace DotEnv.Core.Tests.Parser;

[TestClass]
public partial class EnvParserTests
{
    [TestMethod]
    public void Parse_WhenErrorsAreFound_ShouldThrowParserException()
    {
        // Arrange
        var parser = new EnvParser();
        var env = @"This is an error
            =VAL1
            ERRORS_ARE_FOUND_1 = ${VARIABLE_NOT_FOUND} World! ${VARIABLE_NOT_FOUND_2}
            ERRORS_ARE_FOUND_3 = VAL
            ERRORS_ARE_FOUND_2 = ${} World! Hello ${   }
        ";

        // Act
        Action act = () => parser.Parse(env);

        // Assert
        act.Should().Throw<ParserException>();
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("       ")]
    public void Parse_WhenDataSourceIsEmptyOrWhitespace_ShouldThrowParserException(string source)
    {
        // Arrange
        var parser = new EnvParser();
        var expectedMessage = DataSourceIsEmptyOrWhitespaceMessage;
        var expectedSubstring = $"*{expectedMessage}*";

        // Act
        Action act = () => parser.Parse(source);

        // Assert
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);
    }

    [TestMethod]
    public void Parse_WhenKeyIsAnEmptyString_ShouldThrowParserException()
    {
        // Arrange
        var env = "KEY_EMPTY_STRING=VAL1\n=VAL2";
        var parser = new EnvParser();
        var expectedMessage = string.Format(LineHasNoKeyValuePairMessage, "=VAL2");
        var expectedSubstring = $"*{expectedMessage}*";

        // Act
        Action act = () => parser.Parse(env);

        // Assert
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);
    }

    [TestMethod]
    [DataRow("LINE_HAS_NO_KEY_2;VAL2")]
    [DataRow("LINE_HAS_NO_KEY_3 VAL1")]
    [DataRow("This is a line.")]
    public void Parse_WhenLineHasNoKeyValuePair_ShouldThrowParserException(string source)
    {
        // Arrange
        var parser = new EnvParser();
        var expectedMessage = string.Format(LineHasNoKeyValuePairMessage, source);
        var expectedSubstring = $"*{expectedMessage}*";

        // Act
        Action act = () => parser.Parse(source);

        // Assert
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);;
    }

    [TestMethod]
    public void Parse_WhenLineIsComment_ShouldIgnoreComment()
    {
        // Arrange
        var env = @"
            #KEY1=VAL1
                #KEY2=VAL2
                    #KEY3=VAL3
            #KEY4=VAL4
        ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("#KEY1").Should().BeNull();
        GetEnvironmentVariable("#KEY2").Should().BeNull();
        GetEnvironmentVariable("#KEY3").Should().BeNull();
        GetEnvironmentVariable("#KEY4").Should().BeNull();
    }

    [TestMethod]
    public void Parse_WhenLineHasInlineComment_ShouldRemoveInlineComment()
    {
        // Arrange
        var env = @"
            INLINE_COMMENT_1=VAL # This is an inline comment.
            INLINE_COMMENT_2=VAL  # This is an inline comment # Other comment.
            INLINE_COMMENT_3=VAL ### This is an inline comment.
            INLINE_COMMENT_4=VAL# This isn't an inline comment.
            INLINE_COMMENT_5=#This isn't an inline comment.
            INLINE_COMMENT_6= #This is an inline comment.
            INLINE_COMMENT_TOKEN=6U+c'UDH""l`ZFDD5%/|'t{Ojt.5hzu+#wUBH#:9w*l_I2z{^m/7h-U&!qcLlXZ
        ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("INLINE_COMMENT_1").Should().Be("VAL");
        GetEnvironmentVariable("INLINE_COMMENT_2").Should().Be("VAL");
        GetEnvironmentVariable("INLINE_COMMENT_3").Should().Be("VAL");
        GetEnvironmentVariable("INLINE_COMMENT_4")
            .Should()
            .Be("VAL# This isn't an inline comment.");

        GetEnvironmentVariable("INLINE_COMMENT_5")
            .Should()
            .Be("#This isn't an inline comment.");

        GetEnvironmentVariable("INLINE_COMMENT_6").Should().Be(" ");
        GetEnvironmentVariable("INLINE_COMMENT_TOKEN")
            .Should()
            .Be("6U+c'UDH\"l`ZFDD5%/|'t{Ojt.5hzu+#wUBH#:9w*l_I2z{^m/7h-U&!qcLlXZ");
    }

    [TestMethod]
    public void Parse_WhenValueIsQuoted_ShouldRemoveSingleOrDoubleQuotes()
    {
        // Arrange
        var env = @"
            DOUBLE_QUOTES_1=""VAL""
            DOUBLE_QUOTES_2=""  VAL  ""
            DOUBLE_QUOTES_3=""""
            DOUBLE_QUOTES_4=""   ""
            DOUBLE_QUOTES_5=  "" VAL""  

            SINGLE_QUOTES_1='VAL'
            SINGLE_QUOTES_2='  VAL  '
            SINGLE_QUOTES_3=''
            SINGLE_QUOTES_4='   '
            SINGLE_QUOTES_5=  ' VAL'  

            INCOMPLETE_QUOTES_1=VAL'
            INCOMPLETE_QUOTES_2=VAL""
            INCOMPLETE_QUOTES_3=
        ";
        var parser = new EnvParser();
        
        // Act
        parser
            .DisableTrimValues()
            .Parse(env);

        // Asserts
        GetEnvironmentVariable("DOUBLE_QUOTES_1").Should().Be("VAL");
        GetEnvironmentVariable("DOUBLE_QUOTES_2").Should().Be("  VAL  ");
        GetEnvironmentVariable("DOUBLE_QUOTES_3").Should().Be(" ");
        GetEnvironmentVariable("DOUBLE_QUOTES_4").Should().Be("   ");
        GetEnvironmentVariable("DOUBLE_QUOTES_5").Should().Be(" VAL");

        GetEnvironmentVariable("SINGLE_QUOTES_1").Should().Be("VAL");
        GetEnvironmentVariable("SINGLE_QUOTES_2").Should().Be("  VAL  ");
        GetEnvironmentVariable("SINGLE_QUOTES_3").Should().Be(" ");
        GetEnvironmentVariable("SINGLE_QUOTES_4").Should().Be("   ");
        GetEnvironmentVariable("SINGLE_QUOTES_5").Should().Be(" VAL");

        GetEnvironmentVariable("INCOMPLETE_QUOTES_1").Should().Be("VAL'");
        GetEnvironmentVariable("INCOMPLETE_QUOTES_2").Should().Be("VAL\"");
        GetEnvironmentVariable("INCOMPLETE_QUOTES_3").Should().Be(" ");
    }

    [TestMethod]
    public void Parse_WhenExportPrefixIsFoundBeforeTheKey_ShouldRemoveExportPrefix()
    {
        // Arrange
        var env = @"
            export EXPORT_PREFIX_1=1
            EXPORT_PREFIX_2=1
            export   EXPORT_PREFIX_3=1
            export.EXPORT_PREFIX_4=1
            export_EXPORT_PREFIX_5=1
            exportEXPORT_PREFIX_6=1
                export EXPORT_PREFIX_7=1
            export exportEXPORT_PREFIX_8=1
            EXPORT EXPORT_PREFIX_9=1
            EXPORT export EXPORT_PREFIX_10=1
        ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("EXPORT_PREFIX_1").Should().NotBeNull();
        GetEnvironmentVariable("EXPORT_PREFIX_2").Should().NotBeNull();
        GetEnvironmentVariable("EXPORT_PREFIX_3").Should().NotBeNull();
        GetEnvironmentVariable("export.EXPORT_PREFIX_4").Should().NotBeNull();
        GetEnvironmentVariable("export_EXPORT_PREFIX_5").Should().NotBeNull();
        GetEnvironmentVariable("exportEXPORT_PREFIX_6").Should().NotBeNull();
        GetEnvironmentVariable("EXPORT_PREFIX_7").Should().NotBeNull();
        GetEnvironmentVariable("exportEXPORT_PREFIX_8").Should().NotBeNull();
        GetEnvironmentVariable("EXPORT EXPORT_PREFIX_9").Should().NotBeNull();
        GetEnvironmentVariable("EXPORT export EXPORT_PREFIX_10").Should().NotBeNull();
    }

    [TestMethod]
    public void Parse_WhenReadLineWithKeyValuePair_ShouldExtractKey()
    {
        // Arrange
        var env = @"
            READLINE_WITH_KEY_1=VAL1
            READLINE_WITH_KEY_2=VAL2
            READLINE_WITH_KEY_3=VAL3
        ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("READLINE_WITH_KEY_1").Should().NotBeNull();
        GetEnvironmentVariable("READLINE_WITH_KEY_1").Should().NotBeNull();
        GetEnvironmentVariable("READLINE_WITH_KEY_3").Should().NotBeNull();
    }

    [TestMethod]
    public void Parse_WhenReadLineWithKeyValuePair_ShouldExtractValue()
    {
        // Arrange
        var env = @"
            READLINE_WITH_VALUE_1=VAL1
            READLINE_WITH_VALUE_2=VAL2
            READLINE_WITH_VALUE_3=VAL3
            READLINE_WITH_VALUE_4=server=localhost;user=root;password=1234;
        ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("READLINE_WITH_VALUE_1").Should().Be("VAL1");
        GetEnvironmentVariable("READLINE_WITH_VALUE_2").Should().Be("VAL2");
        GetEnvironmentVariable("READLINE_WITH_VALUE_3").Should().Be("VAL3");
        GetEnvironmentVariable("READLINE_WITH_VALUE_4")
            .Should()
            .Be("server=localhost;user=root;password=1234;");
    }

    [TestMethod]
    public void Parse_WhenValueIsEmptyString_ShouldConvertItToWhiteSpace()
    {
        // Arrange
        var env = @"
            VALUE_EMPTY_STRING_1=
            VALUE_EMPTY_STRING_2=
            VALUE_EMPTY_STRING_3=
        ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("VALUE_EMPTY_STRING_1").Should().Be(" ");
        GetEnvironmentVariable("VALUE_EMPTY_STRING_2").Should().Be(" ");
        GetEnvironmentVariable("VALUE_EMPTY_STRING_3").Should().Be(" ");
    }

    [TestMethod]
    public void Parse_WhenKeyHasWhitespacesAtBothEnds_ShouldIgnoreWhitespaces()
    {
        // Arrange
        var env = @"  
            KEY_HAS_WHITESPACES_1    =     VAL1               
            KEY_HAS_WHITESPACES_2    =     VAL2             
         ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("KEY_HAS_WHITESPACES_1").Should().NotBeNull();
        GetEnvironmentVariable("KEY_HAS_WHITESPACES_2").Should().NotBeNull();
    }

    [TestMethod]
    public void Parse_WhenValueHasWhitespacesAtBothEnds_ShouldIgnoreWhitespaces()
    {
        // Arrange
        var env = @"  
            VALUE_HAS_WHITESPACES_1    =     VAL1               
            VALUE_HAS_WHITESPACES_2    =     VAL2             
        ";
        var parser = new EnvParser();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("VALUE_HAS_WHITESPACES_1").Should().Be("VAL1");
        GetEnvironmentVariable("VALUE_HAS_WHITESPACES_2").Should().Be("VAL2");
    }

    [TestMethod]
    public void Parse_WhenKeyExistsInTheCurrentProcess_ShouldNotOverwriteTheValue()
    {
        // Arrange
        var env = @"  
            KEY_EXISTS_1  =     VAL1               
            KEY_EXISTS_2  =     VAL2             
        ";
        var parser = new EnvParser();
        SetEnvironmentVariable("KEY_EXISTS_1", "1");
        SetEnvironmentVariable("KEY_EXISTS_2", "2");

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("KEY_EXISTS_1").Should().Be("1");
        GetEnvironmentVariable("KEY_EXISTS_2").Should().Be("2");
    }

    [TestMethod]
    public void Parse_WhenVariablesAreInterpolated_ShouldExpandVariables()
    {
        // Arrange
        var env = @"
            MYSQL_USER_EXPAND = root
            EXPAND_1 = 1
            MYSQL_HOST_EXPAND = localhost
            MYSQL_PASSWORD_EXPAND = 1234
            CS_MYSQL_EXPAND = server=${MYSQL_HOST_EXPAND};user=${MYSQL_USER_EXPAND};password=${MYSQL_PASSWORD_EXPAND};
            CS_SQL_EXPAND = ${CS_MYSQL_EXPAND}
            EXPAND_2 = ${TEST asdasd
            EXPAND_3 = {TEST}$ $TEST {}

            A_EXPAND = MYSQL_HOST_EXPAND
            ${A_EXPAND} = 127.0.0.1
        ";
        var parser = new EnvParser().AllowOverwriteExistingVars();

        // Act
        parser.Parse(env);

        // Asserts
        GetEnvironmentVariable("CS_MYSQL_EXPAND")
            .Should()
            .Be("server=localhost;user=root;password=1234;");

        GetEnvironmentVariable("CS_SQL_EXPAND")
            .Should()
            .Be("server=localhost;user=root;password=1234;");

        GetEnvironmentVariable("EXPAND_2")
            .Should()
            .Be("${TEST asdasd");

        GetEnvironmentVariable("EXPAND_3")
            .Should()
            .Be("{TEST}$ $TEST {}");

        GetEnvironmentVariable("MYSQL_HOST_EXPAND")
            .Should()
            .Be("127.0.0.1");

        GetEnvironmentVariable("${A_EXPAND}").Should().BeNull();
    }

    [TestMethod]
    public void Parse_WhenInterpolatedVariableDoesNotExist_ShouldThrowParserException()
    {
        // Arrange
        var env = "EMBEDDED_VAR_1 = asdasd ${VARIABLE_NOT_FOUND}";
        var parser = new EnvParser();
        var expectedMessage = string.Format(VariableNotSetMessage, "VARIABLE_NOT_FOUND");
        var expectedSubstring = $"*{expectedMessage}*";

        // Act
        Action act = () => parser.Parse(env);

        // Assert
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);
    }

    [TestMethod]
    [DataRow("EMBEDDED_VAR_3 = asdasd ${}", "${}")]
    [DataRow("EMBEDDED_VAR_4 = ${    } asdasd ", "${    }")]
    public void Parse_WhenInterpolatedVariableIsEmptyString_ShouldThrowParserException(string source, string line)
    {
        // Arrange
        var parser = new EnvParser();
        var expectedMessage = string.Format(VariableIsAnEmptyStringMessage, line);
        var expectedSubstring = $"*{expectedMessage}*";

        // Act
        Action act = () => parser.Parse(source);

        // Assert
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);
    }

    [TestMethod]
    public void Parse_WhenEnvironmentCanBeModified_ShouldStoreTheKeysInCurrentEnvironment()
    {
        // Arrange
        var parser = new EnvParser();
        var env = "ENV_MODIFIED = 1";

        // Act
        var envVars = parser.Parse(env);

        // Asserts
        envVars["ENV_MODIFIED"].Should().Be("1");
        GetEnvironmentVariable("ENV_MODIFIED").Should().Be("1");
    }
}
