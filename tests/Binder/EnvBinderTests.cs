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
        StringAssert.Contains(ex.Message, string.Format(KeyAssignedToPropertyIsNotSet, "SettingsExample2", "SecretKey", "SECRET_KEY"));
    }

    [TestMethod]
    public void Bind_WhenConfigurationValueCannotBeConvertedToAnotherDataType_ShouldThrowBinderException()
    {
        var binder = new EnvBinder();
        SetEnvironmentVariable("BIND_WEATHER_ID", "This is not an int");

        void action() => binder.Bind<SettingsExample3>();

        var ex = Assert.ThrowsException<BinderException>(action);
        StringAssert.Contains(ex.Message, string.Format(FailedConvertConfigurationValueMessage, "BIND_WEATHER_ID", "Int32", "This is not an int", "Int32"));
    }
}