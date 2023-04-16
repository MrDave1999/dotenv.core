namespace DotEnv.Core.Tests.Parser;

public partial class EnvParserTests
{
    [TestMethod]
    public void Parse_WhenDisabledTrimStartValues_ShouldNotIgnoreLeadingWhitepaces()
    {
        // Arrange
        var env = "TRIM_START_VALUES=   1";
        var parser = new EnvParser();

        // Act
        parser
            .DisableTrimStartValues()
            .Parse(env);

        // Assert
        GetEnvironmentVariable("TRIM_START_VALUES").Should().Be("   1");
    }

    [TestMethod]
    public void Parse_WhenDisabledTrimEndValues_ShouldNotIgnoreTrailingWhitepaces()
    {
        // Arrange
        var env = "TRIM_END_VALUES=1   ";
        var parser = new EnvParser();

        // Act
        parser
            .DisableTrimEndValues()
            .Parse(env);

        // Assert
        GetEnvironmentVariable("TRIM_END_VALUES").Should().Be("1   ");
    }

    [TestMethod]
    public void Parse_WhenDisabledTrimValues_ShouldNotIgnoreWhitepaces()
    {
        // Arrange
        var env = "TRIM_VALUES=   1   ";
        var parser = new EnvParser();

        // Act
        parser
            .DisableTrimValues()
            .Parse(env);

        // Assert
        GetEnvironmentVariable("TRIM_VALUES").Should().Be("   1   ");
    }

    [TestMethod]
    public void Parse_WhenDisabledTrimStartKeys_ShouldNotIgnoreLeadingWhitepaces()
    {
        // Arrange
        var env = "   TRIM_START_KEYS=1";
        var parser = new EnvParser();

        // Act
        parser
            .DisableTrimStartKeys()
            .Parse(env);

        // Assert
        GetEnvironmentVariable("   TRIM_START_KEYS").Should().Be("1");
    }

    [TestMethod]
    public void Parse_WhenDisabledTrimEndKeys_ShouldNotIgnoreTrailingWhitepaces()
    {
        // Arrange
        var env = "TRIM_END_KEYS   =1";
        var parser = new EnvParser();

        // Act
        parser
            .DisableTrimEndKeys()
            .Parse(env);

        // Assert
        GetEnvironmentVariable("TRIM_END_KEYS   ").Should().Be("1");
    }

    [TestMethod]
    public void Parse_WhenDisabledTrimKeys_ShouldNotIgnoreWhitepaces()
    {
        // Arrange
        var env = "   TRIM_KEYS   =1";
        var parser = new EnvParser();

        // Act
        parser
            .DisableTrimKeys()
            .Parse(env);

        // Assert
        GetEnvironmentVariable("   TRIM_KEYS   ").Should().Be("1");
    }

    [TestMethod]
    public void Parse_WhenDisabledTrimStartComments_ShouldNotIgnoreLeadingWhitepaces()
    {
        // Arrange
        var env = "   #comment1";
        var parser = new EnvParser().DisableTrimStartComments();
        var expectedMessage = string.Format(LineHasNoKeyValuePairMessage, "  ");
        var expectedSubstring = $"*{expectedMessage}*";

        // Act
        Action act = () => parser.Parse(env);

        // Assert
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);
    }

    [TestMethod]
    public void Parse_WhenAllowedOverwriteExistingVars_ShouldBeAbleToOverwriteValue()
    {
        // Arrange
        var env = @"
            ALLOW_OVERWRITE_1 = VAL1
            ALLOW_OVERWRITE_2 = VAL2
        ";
        var parser = new EnvParser();
        SetEnvironmentVariable("ALLOW_OVERWRITE_1", "1");
        SetEnvironmentVariable("ALLOW_OVERWRITE_2", "2");

        // Act
        parser
            .AllowOverwriteExistingVars()
            .Parse(env);

        // Asserts
        GetEnvironmentVariable("ALLOW_OVERWRITE_1").Should().Be("VAL1");
        GetEnvironmentVariable("ALLOW_OVERWRITE_2").Should().Be("VAL2");
    }

    [TestMethod]
    public void Parse_WhenSetsCommentChar_ShouldIgnoreComment()
    {
        // Arrange
        var env = @"
            ;KEY1 = VAL1
            ;KEY2 = VAL2
        ";
        var parser = new EnvParser();

        // Act
        parser
            .SetCommentChar(';')
            .Parse(env);

        // Asserts
        GetEnvironmentVariable(";KEY1").Should().BeNull();
        GetEnvironmentVariable(";KEY2").Should().BeNull();

    }

    [TestMethod]
    public void Parse_WhenSetsDelimiterKeyValuePair_ShouldReadKeyValuePair()
    {
        // Arrange
        var env = @"
            DELIMITER_KEYVALUE_1 : VAL1
            DELIMITER_KEYVALUE_2:VAL2
        ";
        var parser = new EnvParser();

        // Act
        parser
            .SetDelimiterKeyValuePair(':')
            .Parse(env);

        // Asserts
        GetEnvironmentVariable("DELIMITER_KEYVALUE_1").Should().Be("VAL1");
        GetEnvironmentVariable("DELIMITER_KEYVALUE_2").Should().Be("VAL2");
    }

    [TestMethod]
    public void Parse_WhenIgnoresParserException_ShouldNotThrowParserException()
    {
        // Arrange
        var env = @"
            asdasdasdasd
            IGNORE_EXCEPTION_1=VAL1 ${IGNORE_EXCEPTION_2} ...
            KEY1:VAL1
            This isn't a comment.
            IGNORE_EXCEPTION_2=VAL2 ${IGNORE_EXCEPTION_2} ...
            =VAL1
            IGNORE_EXCEPTION_3= ASDASD ${} ${   }
            IGNORE_EXCEPTION_4= ASDASD ${   } ASDASD ${}
            IGNORE_EXCEPTION_5 = server=${MYSQL_HOST};user=root${ };
            IGNORE_EXCEPTION_6 = server= ${MYSQL_HOST} ; user=root;
        ";
        var parser = new EnvParser();

        // Act
        parser
            .IgnoreParserException()
            .Parse(env);

        // Asserts
        GetEnvironmentVariable("IGNORE_EXCEPTION_1").Should().Be("VAL1  ...");
        GetEnvironmentVariable("IGNORE_EXCEPTION_2").Should().Be("VAL2  ...");
        GetEnvironmentVariable("IGNORE_EXCEPTION_3").Should().Be("ASDASD");
        GetEnvironmentVariable("IGNORE_EXCEPTION_4").Should().Be("ASDASD  ASDASD");
        GetEnvironmentVariable("IGNORE_EXCEPTION_5").Should().Be("server=;user=root;");
        GetEnvironmentVariable("IGNORE_EXCEPTION_6").Should().Be("server=  ; user=root;");
    }

    [TestMethod]
    public void Parse_WhenAllowedConcatDuplicateKeysAtEnd_ShouldConcatenateDuplicateKeys()
    {
        // Arrange
        var env = @"
            CONCAT_END2 = 1
            CONCAT_END1 = Hello
            CONCAT_END1 = World
            CONCAT_END1 = !
            CONCAT_END1 = !
        ";
        var parser = new EnvParser();
        SetEnvironmentVariable("CONCAT_END2", "2");

        // Act
        parser
            .AllowConcatDuplicateKeys()
            .Parse(env);

        // Asserts
        GetEnvironmentVariable("CONCAT_END1").Should().Be("HelloWorld!!");
        GetEnvironmentVariable("CONCAT_END2").Should().Be("21");
    }


    [TestMethod]
    public void Parse_WhenAllowedConcatDuplicateKeysAtStart_ShouldConcatenateDuplicateKeys()
    {
        // Arrange
        var env = @"
            CONCAT_START2 = 1
            CONCAT_START1 = !
            CONCAT_START1 = !
            CONCAT_START1 = World
            CONCAT_START1 = Hello
        ";
        var parser = new EnvParser();
        SetEnvironmentVariable("CONCAT_START2", "2");

        // Act
        parser
            .AllowConcatDuplicateKeys(ConcatKeysOptions.Start)
            .Parse(env);

        // Asserts
        GetEnvironmentVariable("CONCAT_START1").Should().Be("HelloWorld!!");
        GetEnvironmentVariable("CONCAT_START2").Should().Be("12");
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(3)]
    [DataRow(4)]
    public void AllowConcatDuplicateKeys_WhenOptionIsInvalid_ShouldThrowArgumentException(int option)
    {
        // Arrange
        var parser = new EnvParser();

        // Act
        Action act = () => parser.AllowConcatDuplicateKeys((ConcatKeysOptions)option);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Parse_WhenEnvironmentCannotBeModified_ShouldStoreTheKeysInDictionary()
    {
        // Arrange
        var env = @"
            AVOID_MOD_1 = Hello
            AVOID_MOD_1 = World
            AVOID_MOD_2 = ${AVOID_MOD_1}
        ";
        var parser = new EnvParser();

        // Act
        var keyValuePairs = parser
                                .AvoidModifyEnvironment()
                                .AllowConcatDuplicateKeys()
                                .Parse(env);

        // Asserts
        keyValuePairs["AVOID_MOD_1"].Should().Be("HelloWorld");
        keyValuePairs["AVOID_MOD_2"].Should().Be("HelloWorld");
        GetEnvironmentVariable("AVOID_MOD_1").Should().BeNull();
        GetEnvironmentVariable("AVOID_MOD_2").Should().BeNull();
    }

    [TestMethod]
    public void Parse_WhenKeyDoesNotExistInTheDictionary_ShouldThrowParserException()
    {
        // Arrange
        var parser = new EnvParser().AvoidModifyEnvironment();
        var env = @"
            AVOID_MOD_1 = 1
            AVOID_MOD_1 = ${VARIABLE_NOT_FOUND}
        ";
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
    public void Parse_WhenSetsEnvironmentVariablesProvider_ShouldStoreTheKeysInCustomProvider()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var parser = new EnvParser();
        var source = "KEY1=1\nKEY2=2\nKEY1=2";

        // Act
        parser
            .AllowOverwriteExistingVars()
            .SetEnvironmentVariablesProvider(provider: customProvider)
            .Parse(source);

        // Asserts
        customProvider["KEY1"].Should().Be("2");
        customProvider["KEY2"].Should().Be("2");
        GetEnvironmentVariable("KEY1").Should().BeNull();
        GetEnvironmentVariable("KEY2").Should().BeNull();
    }
}
