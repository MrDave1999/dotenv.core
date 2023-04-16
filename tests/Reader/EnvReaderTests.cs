namespace DotEnv.Core.Tests.Reader;

[TestClass]
public partial class EnvReaderTests
{
    private const string VariableNotFound = nameof(VariableNotFound);

    [TestMethod]
    public void HasValue_WhenVariableExistsInCurrentProcess_ShouldReturnsTrue()
    {
        // Arrange
        var reader = new EnvReader();
        SetEnvironmentVariable("VARIABLE_NAME", "1");

        // Act
        bool actual = reader.HasValue("VARIABLE_NAME");

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod]
    public void HasValue_WhenVariableDoesNotExistsInCurrentProcess_ShouldReturnsFalse()
    {
        // Arrange
        var reader = new EnvReader();
        SetEnvironmentVariable("VARIABLE_NAME", default);

        // Act
        bool actual = reader.HasValue("VARIABLE_NAME");

        // Assert
        actual.Should().BeFalse();
    }
}
