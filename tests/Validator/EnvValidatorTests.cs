namespace DotEnv.Core.Tests.Validator;

[TestClass]
public class EnvValidatorTests
{
    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotPresent_ShouldThrowRequiredKeysNotPresentException()
    {
        var validator = new EnvValidator()
                    .SetRequiredKeys(
                        "SAMC_KEY", 
                        "API_KEY", 
                        "JWT_TOKEN", 
                        "JWT_TOKEN_ID", 
                        "SERVICE_ID"
                    );

        void action() => validator.Validate();

        Assert.ThrowsException<RequiredKeysNotPresentException>(action);
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysArePresent_ShouldNotThrowRequiredKeysNotPresentException()
    {
        SetEnvironmentVariable("JWT_TOKEN", "123");
        SetEnvironmentVariable("API_KEY", "123");
        var validator = new EnvValidator()
                    .SetRequiredKeys("JWT_TOKEN", "API_KEY")
                    .IgnoreException();          

        validator.Validate(out var result);

        Assert.AreEqual(expected: false, actual: result.HasError());
        Assert.AreEqual(expected: 0, actual: result.Count);
        SetEnvironmentVariable("JWT_TOKEN", null);
        SetEnvironmentVariable("API_KEY", null);
    }

    [TestMethod]
    public void Validate_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        string msg;
        var validator = new EnvValidator()
                    .SetRequiredKeys(
                        "SAMC_KEY", 
                        "API_KEY", 
                        "JWT_TOKEN", 
                        "JWT_TOKEN_ID", 
                        "SERVICE_ID"
                      )
                    .IgnoreException();

        validator.Validate(out var result);

        Assert.AreEqual(expected: true, actual: result.HasError());
        Assert.AreEqual(expected: 5, actual: result.Count);

        msg = result.ErrorMessages;
        StringAssert.Contains(msg, string.Format(RequiredKeysNotPresentMessage, "SAMC_KEY"));
        StringAssert.Contains(msg, string.Format(RequiredKeysNotPresentMessage, "API_KEY"));
        StringAssert.Contains(msg, string.Format(RequiredKeysNotPresentMessage, "JWT_TOKEN"));
        StringAssert.Contains(msg, string.Format(RequiredKeysNotPresentMessage, "JWT_TOKEN_ID"));
        StringAssert.Contains(msg, string.Format(RequiredKeysNotPresentMessage, "SERVICE_ID"));
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotPresentAndAreSpecifiedByMeansOfClass_ShouldThrowRequiredKeysNotPresentException()
    {
        var validator = new EnvValidator().SetRequiredKeys<RequiredKeys>();

        void action() => validator.Validate();

        Assert.ThrowsException<RequiredKeysNotPresentException>(action);
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotPresentInCustomProvider_ShouldThrowRequiredKeysNotPresentException()
    {
        var customProvider = new CustomEnvironmentVariablesProvider();
        var validator = new EnvValidator(customProvider).SetRequiredKeys<RequiredKeys>();

        void action() => validator.Validate();

        Assert.ThrowsException<RequiredKeysNotPresentException>(action);
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysArePresentInCustomProvider_ShouldNotThrowRequiredKeysNotPresentException()
    {
        var customProvider = new CustomEnvironmentVariablesProvider();
        var validator = new EnvValidator(customProvider).SetRequiredKeys<RequiredKeys>();
        customProvider["SAMC_KEY"]     = "";
        customProvider["API_KEY"]      = "";
        customProvider["JWT_TOKEN"]    = "";
        customProvider["JWT_TOKEN_ID"] = "";
        customProvider["SERVICE_ID"]   = "";

        validator.Validate(out var result);

        Assert.AreEqual(expected: false, actual: result.HasError());
        Assert.AreEqual(expected: 0, actual: result.Count);
    }

    [TestMethod]
    public void Validate_WhenRequiredKeysAreNotSpecifiedWithSetRequiredKeysMethod_ShouldThrowInvalidOperationException()
    {
        var validator = new EnvValidator();

        void action() => validator.Validate();

        Assert.ThrowsException<InvalidOperationException>(action);
    }

    [TestMethod]
    public void AddRequiredKeys_WhenLengthOfTheKeysListIsZero_ShouldThrowArgumentException()
    {
        var validator = new EnvValidator();

        void action() => validator.SetRequiredKeys();

        Assert.ThrowsException<ArgumentException>(action);
    }
}
