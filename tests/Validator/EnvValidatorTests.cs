namespace DotEnv.Core.Tests.Validator;

[TestClass]
public class EnvValidatorTests
{
    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotPresent_ShouldThrowRequiredKeysNotPresentException()
    {
        // Arrange
        var validator = new EnvValidator()
                            .SetRequiredKeys(
                                "SAMC_KEY", 
                                "API_KEY", 
                                "JWT_TOKEN", 
                                "JWT_TOKEN_ID", 
                                "SERVICE_ID"
                            );

        // Act
        Action act = () => validator.Validate();

        // Assert
        act.Should().Throw<RequiredKeysNotPresentException>();
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysArePresent_ShouldNotThrowRequiredKeysNotPresentException()
    {
        // Arrange
        SetEnvironmentVariable("JWT_TOKEN", "123");
        SetEnvironmentVariable("API_KEY", "123");
        var validator = new EnvValidator()
                            .SetRequiredKeys("JWT_TOKEN", "API_KEY")
                            .IgnoreException();          

        // Act
        validator.Validate(out var result);

        // Asserts
        result.HasError().Should().BeFalse();
        result.Should().HaveCount(0);
        SetEnvironmentVariable("JWT_TOKEN", null);
        SetEnvironmentVariable("API_KEY", null);
    }

    [TestMethod]
    public void Validate_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        // Arrange
        var validator = new EnvValidator()
                            .SetRequiredKeys(
                                "SAMC_KEY", 
                                "API_KEY", 
                                "JWT_TOKEN", 
                                "JWT_TOKEN_ID", 
                                "SERVICE_ID"
                              )
                            .IgnoreException();
        var expectedErrors = new List<string>
        {
            string.Format(RequiredKeysNotPresentMessage, "SAMC_KEY"),
            string.Format(RequiredKeysNotPresentMessage, "API_KEY"),
            string.Format(RequiredKeysNotPresentMessage, "JWT_TOKEN"),
            string.Format(RequiredKeysNotPresentMessage, "JWT_TOKEN_ID"),
            string.Format(RequiredKeysNotPresentMessage, "SERVICE_ID")
        };

        // Act
        validator.Validate(out var result);
        var errors = result.ToList();

        // Asserts
        result.HasError().Should().BeTrue();
        errors.Should().BeEquivalentTo(expectedErrors);
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotPresentAndAreSpecifiedByMeansOfClass_ShouldThrowRequiredKeysNotPresentException()
    {
        // Arrange
        var validator = new EnvValidator().SetRequiredKeys<RequiredKeys>();

        // Act
        Action act = () => validator.Validate();

        // Assert
        act.Should().Throw<RequiredKeysNotPresentException>();
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotPresentInCustomProvider_ShouldThrowRequiredKeysNotPresentException()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var validator = new EnvValidator(customProvider).SetRequiredKeys<RequiredKeys>();

        // Act
        Action act = () => validator.Validate();

        // Assert
        act.Should().Throw<RequiredKeysNotPresentException>();
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysArePresentInCustomProvider_ShouldNotThrowRequiredKeysNotPresentException()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var validator = new EnvValidator(customProvider).SetRequiredKeys<RequiredKeys>();
        customProvider["SAMC_KEY"]     = "";
        customProvider["API_KEY"]      = "";
        customProvider["JWT_TOKEN"]    = "";
        customProvider["JWT_TOKEN_ID"] = "";
        customProvider["SERVICE_ID"]   = "";

        // Act
        validator.Validate(out var result);

        // Asserts
        result.HasError().Should().BeFalse();
        result.Should().HaveCount(0);
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotSpecifiedWithSetRequiredKeysMethod_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var validator = new EnvValidator();

        // Act
        Action act = () => validator.Validate();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void SetRequiredKeys_WhenLengthOfTheKeysListIsZero_ShouldThrowArgumentException()
    {
        // Arrange
        var validator = new EnvValidator();

        // Act
        Action act = () => validator.SetRequiredKeys();

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
