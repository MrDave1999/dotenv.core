namespace DotEnv.Core.Tests.Parser;

public partial class EnvParserTests
{
    [TestMethod]
    public void Parse_WhenValuesAreOnMultiLines_ShouldGetValuesSeparatedByNewLine()
    {
        // Arrage
        var parser = new EnvParser();
        var source = File.ReadAllText(".env.multi-lines");

        // Act
        parser.Parse(source);

        // Asserts
        GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_1")
            .Should()
            .Be("first line #a \nsecond line #b\nthird line #c\nfour line #d");

        GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_2")
            .Should()
            .Be("\nfirst line\nsecond line\nthird line\n");

        GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_3")
            .Should()
            .Be("first line\n");

        GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_4").Should().Be("\n");
        GetEnvironmentVariable("MULTI_DOUBLE_QUOTED_5").Should().Be("  \n");

        GetEnvironmentVariable("MULTI_SINGLE_QUOTED_1")
            .Should()
            .Be("first line #a \nsecond line #b\nthird line #c\nfour line #d");

        GetEnvironmentVariable("MULTI_SINGLE_QUOTED_2")
            .Should()
            .Be("\nfirst line\nsecond line\nthird line\n");

        GetEnvironmentVariable("MULTI_SINGLE_QUOTED_3")
            .Should()
            .Be("first line\n");

        GetEnvironmentVariable("MULTI_SINGLE_QUOTED_4").Should().Be("\n");
        GetEnvironmentVariable("MULTI_SINGLE_QUOTED_5").Should().Be("  \n");
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
    public void Parse_WhenLineHasNoEndSingleQuote_ShouldThrowParserException(string source)
    {
        // Arrange
        var parser = new EnvParser();
        var expectedSubstring = $"*{LineHasNoEndSingleQuoteMessage}*";

        // Act
        Action act = () => parser.Parse(source);

        // Asserts
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);

        GetEnvironmentVariable("MULTI_UNENDED").Should().BeNull();
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
    public void Parse_WhenLineHasNoEndDoubleQuote_ShouldThrowParserException(string source)
    {
        // Arrange
        var parser = new EnvParser();
        var expectedSubstring = $"*{LineHasNoEndDoubleQuoteMessage}*";

        // Act
        Action act = () => parser.Parse(source);

        // Asserts
        act.Should()
           .Throw<ParserException>()
           .WithMessage(expectedSubstring);

        GetEnvironmentVariable("MULTI_UNENDED").Should().BeNull();
    }
}