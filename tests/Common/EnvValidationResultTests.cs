namespace DotEnv.Core.Tests.Common;

[TestClass]
public class EnvValidationResultTests
{
    [TestMethod]
    public void HasError_WhenThereAreErrors_ShouldReturnsTrue()
    {
        // Arrange
        var validationResult = new EnvValidationResult
        {
            "Error!"
        };

        // Act
        bool actual = validationResult.HasError();

        // Assert
        actual.Should().BeTrue();
    }

    [TestMethod]
    public void HasError_WhenThereAreNoErrors_ShouldReturnsFalse()
    {
        // Arrange
        var validationResult = new EnvValidationResult();

        // Act
        bool actual = validationResult.HasError();

        // Assert
        actual.Should().BeFalse();
    }

    [TestMethod]
    public void ErrorMessages_WhenThereAreNoErrors_ShouldReturnsEmptyString()
    {
        // Arrange
        var validationResult = new EnvValidationResult();

        // Act
        string actual = validationResult.ErrorMessages;

        // Assert
        actual.Should().BeEmpty();
    }

    [TestMethod]
    public void ErrorMessages_WhenThereAreErrors_ShouldReturnsSetOfErrors()
    {
        // Arrange
        var validationResult = new EnvValidationResult
        {
            "Error1",
            "Error2",
            "Error3"
        };
        // Add new line at the beginning and end.
        var expectedMessages =
        """

        Error1
        Error2
        Error3

        """;

        // Act
        string actual = validationResult.ErrorMessages;

        // Assert
        actual.Should().Be(expectedMessages);
    }

    [TestMethod]
    public void GetEnumerator_WhenThereAreErrors_ShouldAllowIteratingOverThem()
    {
        // Arrange
        var validationResult = new EnvValidationResult
        {
            "Error1",
            "Error2",
            "Error3"
        };
        var expected = new[]
        {
            "Error1",
            "Error2",
            "Error3"
        };

        // Act
        IEnumerable<string> actual = validationResult.AsEnumerable();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}
