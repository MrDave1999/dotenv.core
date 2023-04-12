namespace DotEnv.Core.Tests.Binder;

[TestClass]
public class EnvBinderTests
{
    [TestMethod]
    public void Bind_WhenPropertyNameDoesNotMatchRealKey_ShouldReturnsSettingsInstance()
    {
        // Arrange
        var binder = new EnvBinder();
        SetEnvironmentVariable("REAL_KEY", "1234D");

        // Act
        var settings = binder.Bind<SettingsExample0>();

        // Assert
        settings.RealKey.Should().Be("1234D");
    }

    [TestMethod]
    public void Bind_WhenPropertiesAreLinkedToTheDefaultProviderInstance_ShouldReturnsSettingsInstance()
    {
        // Arrange
        var binder = new EnvBinder();
        SetEnvironmentVariable("BIND_JWT_SECRET", "12example");
        SetEnvironmentVariable("BIND_TOKEN_ID",   "e32d");
        SetEnvironmentVariable("BIND_RACE_TIME",  "23");
        SetEnvironmentVariable("BindSecretKey",   "12example");
        SetEnvironmentVariable("BindJwtSecret",   "secret123");
        SetEnvironmentVariable("COLOR_NAME",      "RED");

        // Act
        var settings = binder.Bind<AppSettings>();

        // Asserts
        settings.JwtSecret.Should().Be("12example");
        settings.TokenId.Should().Be("e32d");
        settings.RaceTime.Should().Be(23);
        settings.BindSecretKey.Should().Be("12example");
        settings.BindJwtSecret.Should().Be("secret123");
        settings.ColorName.Should().Be(Colors.Red);
    }

    [TestMethod]
    public void Bind_WhenPropertiesAreLinkedToTheCustomProviderInstance_ShouldReturnsSettingsInstance()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider);
        customProvider["BIND_JWT_SECRET"] = "13example";
        customProvider["BIND_TOKEN_ID"]   = "e31d";
        customProvider["BIND_RACE_TIME"]  = "24";
        customProvider["BindSecretKey"]   = "13example";
        customProvider["BindJwtSecret"]   = "secret124";
        customProvider["COLOR_NAME"]      = "RED";

        // Act
        var settings = binder.Bind<AppSettings>();

        // Asserts
        settings.JwtSecret.Should().Be("13example");
        settings.TokenId.Should().Be("e31d");
        settings.RaceTime.Should().Be(24);
        settings.BindSecretKey.Should().Be("13example");
        settings.BindJwtSecret.Should().Be("secret124");
        settings.ColorName.Should().Be(Colors.Red);
    }

    [TestMethod]
    public void Bind_WhenPropertyDoesNotMatchConfigurationKey_ShouldThrowBinderException()
    {
        // Arrange
        var binder = new EnvBinder();
        var expectedMessage = string.Format(
            PropertyDoesNotMatchConfigKeyMessage,
            nameof(SettingsExample1),
            nameof(SettingsExample1.SecretKey)
        );

        // Act
        Action act = () => binder.Bind<SettingsExample1>();

        // Assert
        act.Should()
           .Throw<BinderException>()
           .WithMessage(expectedMessage);
    }

    [TestMethod]
    public void Bind_WhenKeyAssignedToThePropertyIsNotSet_ShouldThrowBinderException()
    {
        // Arrange
        var binder = new EnvBinder();
        var expectedMessage = string.Format(
            KeyAssignedToPropertyIsNotSetMessage,
            nameof(SettingsExample2),
            nameof(SettingsExample2.SecretKey),
            "SECRET_KEY"
        );

        // Act
        Action act = () => binder.Bind<SettingsExample2>();

        // Assert
        act.Should()
           .Throw<BinderException>()
           .WithMessage(expectedMessage);
    }

    [TestMethod]
    public void Bind_WhenConfigurationValueCannotBeConvertedToAnotherDataType_ShouldThrowBinderException()
    {
        // Arrange
        var binder = new EnvBinder();
        SetEnvironmentVariable("BIND_WEATHER_ID", "This is not an int");
        SetEnvironmentVariable("COLOR_NAME",      "Red");
        var expectedMessage = string.Format(
            FailedConvertConfigurationValueMessage,
            "BIND_WEATHER_ID",
            nameof(Int32),
            "This is not an int",
            nameof(Int32)
        );

        // Act
        Action act = () => binder.Bind<SettingsExample3>();

        // Assert
        act.Should()
           .Throw<BinderException>()
           .WithMessage(expectedMessage);
    }

    [TestMethod]
    public void Bind_WhenKeyCannotBeConvertedToEnum_ShouldThrowBinderException()
    {
        // Arrange
        var binder = new EnvBinder();
        SetEnvironmentVariable("BIND_WEATHER_ID", "1");
        SetEnvironmentVariable("COLOR_NAME",      "Yellow");
        var expectedMessage = string.Format(
            FailedConvertConfigurationValueMessage,
            "COLOR_NAME",
            nameof(Colors),
            "Yellow",
            nameof(Colors)
        );

        // Act
        Action act = () => binder.Bind<SettingsExample3>();

        // Assert
        act.Should()
           .Throw<BinderException>()
           .WithMessage(expectedMessage);
    }

    [TestMethod]
    public void Bind_WhenAnErrorIsFound_ShouldStoreErrorMessageInCollection()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider).IgnoreException();
        customProvider["BIND_RACE_TIME"]  = "This is not an int";
        customProvider["ColorName"]       = "Yellow";
        var expectedErrors = new List<string>
        {
            string.Format(KeyAssignedToPropertyIsNotSetMessage, 
                nameof(AppSettings), nameof(AppSettings.JwtSecret), "BIND_JWT_SECRET"),
            string.Format(KeyAssignedToPropertyIsNotSetMessage, 
                nameof(AppSettings), nameof(AppSettings.TokenId), "BIND_TOKEN_ID"),
            string.Format(FailedConvertConfigurationValueMessage, 
                "BIND_RACE_TIME", nameof(Int32), "This is not an int", nameof(Int32)),
            string.Format(FailedConvertConfigurationValueMessage, 
                "ColorName", nameof(Colors), "Yellow", nameof(Colors)),
            string.Format(PropertyDoesNotMatchConfigKeyMessage, 
                nameof(AppSettings), nameof(AppSettings.BindSecretKey)),
            string.Format(PropertyDoesNotMatchConfigKeyMessage,
                nameof(AppSettings), nameof(AppSettings.BindJwtSecret))
        };

        // Act
        binder.Bind<AppSettings>(out var result);
        var errors = result.ToList();

        // Asserts
        result.HasError().Should().BeTrue();
        errors.Should().BeEquivalentTo(expectedErrors);
    }

    [TestMethod]
    public void Bind_WhenPropertyIsReadOnly_ShouldIgnoreTheReadOnlyProperty()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider);
        customProvider["SECRET_KEY"]  = "12345ex";
        customProvider["ApiKey"]      = "example12345";

        // Act
        var settings = binder.Bind<ReadOnlyProperties>();

        // Asserts
        settings.SecretKey.Should().NotBe("12345ex");
        settings.ApiKey.Should().NotBe("example12345");
    }

    [TestMethod]
    public void Bind_WhenPropertyIsWriteOnly_ShouldIgnoreTheWriteOnlyProperty()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider);
        customProvider["WEATHER_ID"]  = "10";
        customProvider["ApiKey"]      = "123456";

        // Act
        var settings = binder.Bind<WriteOnlyProperties>();

        // Asserts
        settings.weatherId.Should().NotBe(10);
        settings.apiKey.Should().NotBe("123456");
    }

    [TestMethod]
    public void Bind_WhenAllowedBindNonPublicProperties_ShouldSetNonPublicProperties()
    {
        // Arrange
        var customProvider = new CustomEnvironmentVariablesProvider();
        var binder = new EnvBinder(customProvider).AllowBindNonPublicProperties();
        customProvider["TokenId"]    = "e45d";
        customProvider["ApiKey"]     = "example123";
        customProvider["WeatherId"]  = "3";
        customProvider["SecretKey"]  = "23example";
        customProvider["TimeId"]     = "34";
        customProvider["Url"]        = "example.com";

        // Act
        var settings = binder.Bind<NonPublicProperties>();

        // Asserts
        settings.TokenId.Should().Be("e45d");
        settings.apiKey.Should().Be("example123");
        settings.weatherId.Should().Be(3);
        settings.SecretKey.Should().Be("23example");
        settings.TimeId.Should().Be(34);
        settings.url.Should().Be("example.com");
    }
}