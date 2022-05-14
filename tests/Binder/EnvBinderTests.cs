namespace DotEnv.Core.Tests.Binder;

[TestClass]
public class EnvBinderTests
{
    [TestMethod]
    public void Bind_WhenPropertiesAreLinkedToTheDefaultProviderInstance_ShouldReturnsSettingsInstance()
    {
        SetEnvironmentVariable("BIND_JWT_SECRET", "12example");
        SetEnvironmentVariable("BIND_TOKEN_ID",   "e32d");
        SetEnvironmentVariable("BIND_RACE_TIME",  "23");
        SetEnvironmentVariable("BindSecretKey",   "12example");
        SetEnvironmentVariable("BindJwtSecret",   "secret123");

        var settings = new EnvBinder().Bind<AppSettings>();

        Assert.AreEqual(expected: "12example",  actual: settings.JwtSecret);
        Assert.AreEqual(expected: "e32d",       actual: settings.TokenId);
        Assert.AreEqual(expected: 23,           actual: settings.RaceTime);
        Assert.AreEqual(expected: "12example",  actual: settings.BindSecretKey);
        Assert.AreEqual(expected: "secret123",  actual: settings.BindJwtSecret);
    }

    [TestMethod]
    public void Bind_WhenPropertiesAreLinkedToTheCustomProviderInstance_ShouldReturnsSettingsInstance()
    {
        var customProvider = new CustomEnvironmentVariablesProvider();
        customProvider["BIND_JWT_SECRET"] = "13example";
        customProvider["BIND_TOKEN_ID"]   = "e31d";
        customProvider["BIND_RACE_TIME"]  = "24";
        customProvider["BindSecretKey"]   = "13example";
        customProvider["BindJwtSecret"]   = "secret124";

        var settings = new EnvBinder(customProvider).Bind<AppSettings>();

        Assert.AreEqual(expected: "13example",  actual: settings.JwtSecret);
        Assert.AreEqual(expected: "e31d",       actual: settings.TokenId);
        Assert.AreEqual(expected: 24,           actual: settings.RaceTime);
        Assert.AreEqual(expected: "13example",  actual: settings.BindSecretKey);
        Assert.AreEqual(expected: "secret124",  actual: settings.BindJwtSecret);
    }

    [TestMethod]
    public void Bind_WhenPropertyDoesNotMatchConfigurationKey_ShouldThrowBinderException()
    {
        var binder = new EnvBinder();

        void action() => binder.Bind<SettingsExample1>();

        var ex = Assert.ThrowsException<BinderException>(action);
        StringAssert.Contains(ex.Message, string.Format(PropertyDoesNotMatchConfigKeyMessage, "SecretKey"));
    }

    [TestMethod]
    public void Bind_WhenKeyAssignedToThePropertyIsNotSet_ShouldThrowBinderException()
    {
        var binder = new EnvBinder();

        void action() => binder.Bind<SettingsExample2>();

        var ex = Assert.ThrowsException<BinderException>(action);
        StringAssert.Contains(ex.Message, string.Format(
            KeyAssignedToPropertyIsNotSetMessage, 
            "SettingsExample2", 
            "SecretKey", 
            "SECRET_KEY"
        ));
    }

    [TestMethod]
    public void Bind_WhenConfigurationValueCannotBeConvertedToAnotherDataType_ShouldThrowBinderException()
    {
        var binder = new EnvBinder();
        SetEnvironmentVariable("BIND_WEATHER_ID", "This is not an int");

        void action() => binder.Bind<SettingsExample3>();

        var ex = Assert.ThrowsException<BinderException>(action);
        StringAssert.Contains(ex.Message, string.Format(
            FailedConvertConfigurationValueMessage, 
            "BIND_WEATHER_ID", 
            "Int32", 
            "This is not an int", 
            "Int32"
        ));
    }

    [TestMethod]
    public void Bind_WhenErrorsAreFound_ShouldReadTheErrors()
    {
        string msg;
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider).IgnoreException();
        customProvider["BIND_RACE_TIME"]  = "This is not an int";
        
        binder.Bind<AppSettings>(out var result);

        Assert.AreEqual(expected: true, actual: result.HasError());
        Assert.AreEqual(expected: 5, actual: result.Count);

        msg = result.ErrorMessages;
        StringAssert.Contains(msg, string.Format(
            KeyAssignedToPropertyIsNotSetMessage, 
            "AppSettings", 
            "JwtSecret", 
            "BIND_JWT_SECRET"
        ));
        StringAssert.Contains(msg, string.Format(
            KeyAssignedToPropertyIsNotSetMessage, 
            "AppSettings", 
            "TokenId", 
            "BIND_TOKEN_ID"
        ));
        StringAssert.Contains(msg, string.Format(
            FailedConvertConfigurationValueMessage, 
            "BIND_RACE_TIME", 
            "Int32", 
            "This is not an int", 
            "Int32"
        ));
        StringAssert.Contains(msg, string.Format(PropertyDoesNotMatchConfigKeyMessage, "BindSecretKey"));
        StringAssert.Contains(msg, string.Format(PropertyDoesNotMatchConfigKeyMessage, "BindJwtSecret"));
    }

    [TestMethod]
    public void Bind_WhenPropertyIsReadOnly_ShouldIgnoreTheReadOnlyProperty()
    {
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider);
        customProvider["SECRET_KEY"]  = "12345ex";
        customProvider["ApiKey"]      = "example12345";

        var settings = binder.Bind<ReadOnlyProperties>();

        Assert.AreNotEqual(notExpected: "12345ex",      actual: settings.SecretKey);
        Assert.AreNotEqual(notExpected: "example12345", actual: settings.ApiKey);
    }

    [TestMethod]
    public void Bind_WhenPropertyIsWriteOnly_ShouldIgnoreTheWriteOnlyProperty()
    {
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider);
        customProvider["WEATHER_ID"]  = "10";
        customProvider["ApiKey"]      = "123456";

        var settings = binder.Bind<WriteOnlyProperties>();

        Assert.AreNotEqual(notExpected: 10,       actual: settings.weatherId);
        Assert.AreNotEqual(notExpected: "123456", actual: settings.apiKey);
    }

    [TestMethod]
    public void Bind_WhenAllowedBindNonPublicProperties_ShouldSetNonPublicProperties()
    {
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider).AllowBindNonPublicProperties();
        customProvider["TokenId"]    = "e45d";
        customProvider["ApiKey"]     = "example123";
        customProvider["WeatherId"]  = "3";
        customProvider["SecretKey"]  = "23example";
        customProvider["TimeId"]     = "34";
        customProvider["Url"]        = "example.com";

        var settings = binder.Bind<NonPublicProperties>();

        Assert.AreEqual(expected: "e45d",        actual: settings.TokenId);
        Assert.AreEqual(expected: "example123",  actual: settings.apiKey);
        Assert.AreEqual(expected: 3,             actual: settings.weatherId);
        Assert.AreEqual(expected: "23example",   actual: settings.SecretKey);
        Assert.AreEqual(expected: 34,            actual: settings.TimeId);
        Assert.AreEqual(expected: "example.com", actual: settings.url);
    }
}