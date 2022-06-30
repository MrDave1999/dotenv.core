namespace DotEnv.Core.Tests.Parser;

[TestClass]
public partial class EnvParserTests
{
    [TestMethod]
    public void Parse_WhenErrorsAreFound_ShouldThrowParserException()
    {
        var parser = new EnvParser();
        string env = @"This is an error
                =VAL1
                ERRORS_ARE_FOUND_1 = ${VARIABLE_NOT_FOUND} World! ${VARIABLE_NOT_FOUND_2}
                ERRORS_ARE_FOUND_3 = VAL
                ERRORS_ARE_FOUND_2 = ${} World! Hello ${   }
            ";

        void action() => parser.Parse(env);

        Assert.ThrowsException<ParserException>(action);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("       ")]
    public void Parse_WhenDataSourceIsEmptyOrWhitespace_ShouldThrowParserException(string input)
    {
        var parser = new EnvParser();

        void action() => parser.Parse(input);

        var ex = Assert.ThrowsException<ParserException>(action);
        StringAssert.Contains(ex.Message, DataSourceIsEmptyOrWhitespaceMessage);
    }

    [TestMethod]
    public void Parse_WhenKeyIsAnEmptyString_ShouldThrowParserException()
    {
        string env = "KEY_EMPTY_STRING=VAL1\n=VAL2";
        var parser = new EnvParser();

        void action() => parser.Parse(env);

        var ex = Assert.ThrowsException<ParserException>(action);
        StringAssert.Contains(ex.Message, string.Format(LineHasNoKeyValuePairMessage, "=VAL2"));
    }

    [TestMethod]
    [DataRow("LINE_HAS_NO_KEY_2;VAL2")]
    [DataRow("LINE_HAS_NO_KEY_3 VAL1")]
    [DataRow("This is a line.")]
    public void Parse_WhenLineHasNoKeyValuePair_ShouldThrowParserException(string input)
    {
        var parser = new EnvParser();

        void action() => parser.Parse(input);

        var ex = Assert.ThrowsException<ParserException>(action);
        StringAssert.Contains(ex.Message, string.Format(LineHasNoKeyValuePairMessage, input));
    }

    [TestMethod]
    public void Parse_WhenLineIsComment_ShouldIgnoreComment()
    {
        string env = @"
                #KEY1=VAL1
                   #KEY2=VAL2
                        #KEY3=VAL3
                #KEY4=VAL4
            ";
        var parser = new EnvParser();

        parser.Parse(env);

        Assert.IsNull(GetEnvironmentVariable("#KEY1"));
        Assert.IsNull(GetEnvironmentVariable("#KEY2"));
        Assert.IsNull(GetEnvironmentVariable("#KEY3"));
        Assert.IsNull(GetEnvironmentVariable("#KEY4"));
    }

    [TestMethod]
    public void Parse_WhenLineHasInlineComment_ShouldRemoveInlineComment()
    {
        string env = @"
                INLINE_COMMENT_1=VAL # This is an inline comment.
                INLINE_COMMENT_2=VAL  # This is an inline comment # Other comment.
                INLINE_COMMENT_3=VAL ### This is an inline comment.
                INLINE_COMMENT_4=VAL# This isn't an inline comment.
                INLINE_COMMENT_5=#This isn't an inline comment.
                INLINE_COMMENT_6= #This is an inline comment.
                INLINE_COMMENT_TOKEN=6U+c'UDH""l`ZFDD5%/|'t{Ojt.5hzu+#wUBH#:9w*l_I2z{^m/7h-U&!qcLlXZ
            ";

        new EnvParser().Parse(env);

        Assert.AreEqual(expected: "VAL", actual: GetEnvironmentVariable("INLINE_COMMENT_1"));
        Assert.AreEqual(expected: "VAL", actual: GetEnvironmentVariable("INLINE_COMMENT_2"));
        Assert.AreEqual(expected: "VAL", actual: GetEnvironmentVariable("INLINE_COMMENT_3"));
        Assert.AreEqual(expected: "VAL# This isn't an inline comment.", actual: GetEnvironmentVariable("INLINE_COMMENT_4"));
        Assert.AreEqual(expected: "#This isn't an inline comment.", actual: GetEnvironmentVariable("INLINE_COMMENT_5"));
        Assert.AreEqual(expected: " ", actual: GetEnvironmentVariable("INLINE_COMMENT_6"));
        var token = "6U+c'UDH\"l`ZFDD5%/|'t{Ojt.5hzu+#wUBH#:9w*l_I2z{^m/7h-U&!qcLlXZ";
        Assert.AreEqual(expected: token, actual: GetEnvironmentVariable("INLINE_COMMENT_TOKEN"));
    }

    [TestMethod]
    public void Parse_WhenValueIsQuoted_ShouldRemoveSingleOrDoubleQuotes()
    {
        string env = @"
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
        
        new EnvParser()
            .DisableTrimValues()
            .Parse(env);

        Assert.AreEqual(expected: "VAL", actual: GetEnvironmentVariable("DOUBLE_QUOTES_1"));
        Assert.AreEqual(expected: "  VAL  ", actual: GetEnvironmentVariable("DOUBLE_QUOTES_2"));
        Assert.AreEqual(expected: " ", actual: GetEnvironmentVariable("DOUBLE_QUOTES_3"));
        Assert.AreEqual(expected: "   ", actual: GetEnvironmentVariable("DOUBLE_QUOTES_4"));
        Assert.AreEqual(expected: " VAL", actual: GetEnvironmentVariable("DOUBLE_QUOTES_5"));
        
        Assert.AreEqual(expected: "VAL", actual: GetEnvironmentVariable("SINGLE_QUOTES_1"));
        Assert.AreEqual(expected: "  VAL  ", actual: GetEnvironmentVariable("SINGLE_QUOTES_2"));
        Assert.AreEqual(expected: " ", actual: GetEnvironmentVariable("SINGLE_QUOTES_3"));
        Assert.AreEqual(expected: "   ", actual: GetEnvironmentVariable("SINGLE_QUOTES_4"));
        Assert.AreEqual(expected: " VAL", actual: GetEnvironmentVariable("SINGLE_QUOTES_5"));

        Assert.AreEqual(expected: "VAL'", actual: GetEnvironmentVariable("INCOMPLETE_QUOTES_1"));
        Assert.AreEqual(expected: "VAL\"", actual: GetEnvironmentVariable("INCOMPLETE_QUOTES_2"));
        Assert.AreEqual(expected: " ", actual: GetEnvironmentVariable("INCOMPLETE_QUOTES_3"));
    }

    [TestMethod]
    public void Parse_WhenExportPrefixIsFoundBeforeTheKey_ShouldRemoveExportPrefix()
    {
        string env = @"
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

        new EnvParser().Parse(env);

        Assert.IsNotNull(GetEnvironmentVariable("EXPORT_PREFIX_1"));
        Assert.IsNotNull(GetEnvironmentVariable("EXPORT_PREFIX_2"));
        Assert.IsNotNull(GetEnvironmentVariable("EXPORT_PREFIX_3"));
        Assert.IsNotNull(GetEnvironmentVariable("export.EXPORT_PREFIX_4"));
        Assert.IsNotNull(GetEnvironmentVariable("export_EXPORT_PREFIX_5"));
        Assert.IsNotNull(GetEnvironmentVariable("exportEXPORT_PREFIX_6"));
        Assert.IsNotNull(GetEnvironmentVariable("EXPORT_PREFIX_7"));
        Assert.IsNotNull(GetEnvironmentVariable("exportEXPORT_PREFIX_8"));
        Assert.IsNotNull(GetEnvironmentVariable("EXPORT EXPORT_PREFIX_9"));
        Assert.IsNotNull(GetEnvironmentVariable("EXPORT export EXPORT_PREFIX_10"));
    }

    [TestMethod]
    public void Parse_WhenReadLineWithKeyValuePair_ShouldExtractKey()
    {
        string env = @"
                READLINE_WITH_KEY_1=VAL1
                READLINE_WITH_KEY_2=VAL2
                READLINE_WITH_KEY_3=VAL3
            ";
        var parser = new EnvParser();

        parser.Parse(env);

        Assert.IsNotNull(GetEnvironmentVariable("READLINE_WITH_KEY_1"));
        Assert.IsNotNull(GetEnvironmentVariable("READLINE_WITH_KEY_2"));
        Assert.IsNotNull(GetEnvironmentVariable("READLINE_WITH_KEY_3"));
    }

    [TestMethod]
    public void Parse_WhenReadLineWithKeyValuePair_ShouldExtractValue()
    {
        string env = @"
                READLINE_WITH_VALUE_1=VAL1
                READLINE_WITH_VALUE_2=VAL2
                READLINE_WITH_VALUE_3=VAL3
                READLINE_WITH_VALUE_4=server=localhost;user=root;password=1234;
            ";
        var parser = new EnvParser();

        parser.Parse(env);

        Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("READLINE_WITH_VALUE_1"));
        Assert.AreEqual(expected: "VAL2", actual: GetEnvironmentVariable("READLINE_WITH_VALUE_2"));
        Assert.AreEqual(expected: "VAL3", actual: GetEnvironmentVariable("READLINE_WITH_VALUE_3"));
        Assert.AreEqual(expected: "server=localhost;user=root;password=1234;", actual: GetEnvironmentVariable("READLINE_WITH_VALUE_4"));
    }

    [TestMethod]
    public void Parse_WhenValueIsEmptyString_ShouldConvertItToWhiteSpace()
    {
        string env = @"
                VALUE_EMPTY_STRING_1=
                VALUE_EMPTY_STRING_2=
                VALUE_EMPTY_STRING_3=
            ";
        var parser = new EnvParser();

        parser.Parse(env);

        Assert.AreEqual(expected: " ", actual: GetEnvironmentVariable("VALUE_EMPTY_STRING_1"));
        Assert.AreEqual(expected: " ", actual: GetEnvironmentVariable("VALUE_EMPTY_STRING_2"));
        Assert.AreEqual(expected: " ", actual: GetEnvironmentVariable("VALUE_EMPTY_STRING_3"));
    }

    [TestMethod]
    public void Parse_WhenKeyHasWhitespacesAtBothEnds_ShouldIgnoreWhitespaces()
    {
        string env = @"  
                       KEY_HAS_WHITESPACES_1    =     VAL1               
                       KEY_HAS_WHITESPACES_2    =     VAL2             
            ";
        var parser = new EnvParser();

        parser.Parse(env);

        Assert.IsNotNull(GetEnvironmentVariable("KEY_HAS_WHITESPACES_1"));
        Assert.IsNotNull(GetEnvironmentVariable("KEY_HAS_WHITESPACES_2"));
    }

    [TestMethod]
    public void Parse_WhenValueHasWhitespacesAtBothEnds_ShouldIgnoreWhitespaces()
    {
        string env = @"  
                       VALUE_HAS_WHITESPACES_1    =     VAL1               
                       VALUE_HAS_WHITESPACES_2    =     VAL2             
            ";
        var parser = new EnvParser();

        parser.Parse(env);

        Assert.AreEqual(expected: "VAL1", actual: GetEnvironmentVariable("VALUE_HAS_WHITESPACES_1"));
        Assert.AreEqual(expected: "VAL2", actual: GetEnvironmentVariable("VALUE_HAS_WHITESPACES_2"));
    }

    [TestMethod]
    public void Parse_WhenKeyExistsInTheCurrentProcess_ShouldNotOverwriteTheValue()
    {
        string env = @"  
                       KEY_EXISTS_1  =     VAL1               
                       KEY_EXISTS_2  =     VAL2             
            ";
        var parser = new EnvParser();
        SetEnvironmentVariable("KEY_EXISTS_1", "1");
        SetEnvironmentVariable("KEY_EXISTS_2", "2");

        parser.Parse(env);

        Assert.AreEqual(expected: "1", actual: GetEnvironmentVariable("KEY_EXISTS_1"));
        Assert.AreEqual(expected: "2", actual: GetEnvironmentVariable("KEY_EXISTS_2"));
    }

    [TestMethod]
    public void Parse_WhenVariablesAreInterpolated_ShouldExpandVariables()
    {
        string env = @"
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

        parser.Parse(env);

        Assert.AreEqual(expected: "server=localhost;user=root;password=1234;", actual: GetEnvironmentVariable("CS_MYSQL_EXPAND"));
        Assert.AreEqual(expected: "server=localhost;user=root;password=1234;", actual: GetEnvironmentVariable("CS_SQL_EXPAND"));
        Assert.AreEqual(expected: "${TEST asdasd", actual: GetEnvironmentVariable("EXPAND_2"));
        Assert.AreEqual(expected: "{TEST}$ $TEST {}", actual: GetEnvironmentVariable("EXPAND_3"));
        Assert.AreEqual(expected: "127.0.0.1", actual: GetEnvironmentVariable("MYSQL_HOST_EXPAND"));
        Assert.IsNull(GetEnvironmentVariable("${EXPAND_1}"));
    }

    [TestMethod]
    public void Parse_WhenInterpolatedVariableDoesNotExist_ShouldThrowParserException()
    {
        string env = "EMBEDDED_VAR_1 = asdasd ${VARIABLE_NOT_FOUND}";
        var parser = new EnvParser();

        void action() => parser.Parse(env);

        var ex = Assert.ThrowsException<ParserException>(action);
        StringAssert.Contains(ex.Message, string.Format(VariableNotSetMessage, "VARIABLE_NOT_FOUND"));
    }

    [TestMethod]
    [DataRow("EMBEDDED_VAR_3 = asdasd ${}", "${}")]
    [DataRow("EMBEDDED_VAR_4 = ${    } asdasd ", "${    }")]
    public void Parse_WhenInterpolatedVariableIsEmptyString_ShouldThrowParserException(string env, string value)
    {
        var parser = new EnvParser();

        void action() => parser.Parse(env);

        var ex = Assert.ThrowsException<ParserException>(action);
        StringAssert.Contains(ex.Message, string.Format(VariableIsAnEmptyStringMessage, value));
    }

    [TestMethod]
    public void Parse_WhenEnvironmentCanBeModified_ShouldStoreTheKeysInCurrentEnvironment()
    {
        var parser = new EnvParser();
        string env = "ENV_MODIFIED = 1";

        var envVars = parser.Parse(env);

        Assert.AreEqual(expected: "1", actual: envVars["ENV_MODIFIED"]);
        Assert.AreEqual(expected: "1", actual: GetEnvironmentVariable("ENV_MODIFIED"));
    }
}
