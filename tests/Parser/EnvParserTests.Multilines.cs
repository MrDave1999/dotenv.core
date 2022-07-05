namespace DotEnv.Core.Tests.Parser;

public partial class EnvParserTests
{
    [TestMethod]
    public void Parse_WhenValuesAreOnMultiLines_ShouldGetValuesSeparatedByNewLine()
    {
        var parser = new EnvParser();

        parser.Parse(File.ReadAllText(".env.multi-lines"));

        Assert.AreEqual(
            expected: "first line #a \nsecond line #b\nthird line #c\nfour line #d", 
            actual: GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_1")
        );
        Assert.AreEqual(
            expected: "\nfirst line\nsecond line\nthird line\n", 
            actual: GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_2")
        );
        Assert.AreEqual(expected: "first line\n", actual: GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_3"));
        Assert.AreEqual(expected: "\n", actual: GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_4"));
        Assert.AreEqual(expected: "  \n", actual: GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_5"));
        Assert.AreEqual(
            expected: "first line #a \nsecond line #b\nthird line #c\nfour line #d", 
            actual: GetEnvironmentVariable("MULTI_SINGLE_QUOTED_1")
        );
        Assert.AreEqual(
            expected: "\nfirst line\nsecond line\nthird line\n", 
            actual: GetEnvironmentVariable("MULTI_SINGLE_QUOTED_2")
        );
        Assert.AreEqual(expected: "first line\n", actual: GetEnvironmentVariable("MULTI_SINGLE_QUOTED_3"));
        Assert.AreEqual(expected: "\n", actual: GetEnvironmentVariable("MULTI_SINGLE_QUOTED_4"));
        Assert.AreEqual(expected: "  \n", actual: GetEnvironmentVariable("MULTI_SINGLE_QUOTED_5"));
    }

    [TestMethod]
    [DataRow(@"
        MULTI_UNENDED='THIS
        LINE HAS
        NO END QUOTE 
    ")]
    [DataRow(@"MULTI_UNENDED='
        THIS
        LINE HAS
        NO END QUOTE 
    ")]
    [DataRow(@"
        MULTI_UNENDED='
        THIS
        LINE HAS
        NO END QUOTE""
    ")]
    public void Parse_WhenLineHasNoEndSingleQuote_ShouldThrowParserException(string input)
    {
        var parser = new EnvParser();

        void action() => parser.Parse(input);

        var ex = Assert.ThrowsException<ParserException>(action);
        StringAssert.Contains(ex.Message, LineHasNoEndSingleQuoteMessage);
        Assert.IsNull(GetEnvironmentVariable("MULTI_UNENDED"));
    }

    [TestMethod]
    [DataRow(@"
        MULTI_UNENDED=""THIS
        LINE HAS
        NO END QUOTE 
    ")]
    [DataRow(@"MULTI_UNENDED=""
        THIS
        LINE HAS
        NO END QUOTE 
    ")]
    [DataRow(@"
        MULTI_UNENDED=""
        THIS
        LINE HAS
        NO END QUOTE'
    ")]
    public void Parse_WhenLineHasNoEndDoubleQuote_ShouldThrowParserException(string input)
    {
        var parser = new EnvParser();

        void action() => parser.Parse(input);

        var ex = Assert.ThrowsException<ParserException>(action);
        StringAssert.Contains(ex.Message, LineHasNoEndDoubleQuoteMessage);
        Assert.IsNull(GetEnvironmentVariable("MULTI_UNENDED"));
    }
}