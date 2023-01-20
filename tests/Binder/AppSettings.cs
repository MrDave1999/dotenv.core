namespace DotEnv.Core.Tests.Binder;

public enum Colors { Red };

public class AppSettings
{
    [EnvKey("BIND_JWT_SECRET")]
    public string JwtSecret { get; set; }

    [EnvKey("BIND_TOKEN_ID")]
    public string TokenId { get; set; }

    [EnvKey("BIND_RACE_TIME")]
    public int RaceTime { get; set; }

    public string BindSecretKey { get; set; }
    public string BindJwtSecret { get; set; }
    public Colors ColorName { get; set; }
    private string IgnoredProperty { get; set; }
}

public class SettingsExample0
{
    public string RealKey { get; set; }
}

public class SettingsExample1
{
    public string SecretKey { get; set; }
}

public class SettingsExample2
{
    [EnvKey("SECRET_KEY")]
    public string SecretKey { get; set; }
}

public class SettingsExample3
{
    [EnvKey("BIND_WEATHER_ID")]
    public int WeatherId { get; set; }
    public Colors ColorName { get; set; }
}

public class ReadOnlyProperties
{
    [EnvKey("SECRET_KEY")]
    public string SecretKey { get; }
    public string ApiKey { get; }
}

public class WriteOnlyProperties
{
    public int weatherId;
    public string apiKey;

    [EnvKey("WEATHER_ID")]
    public int WeatherId { set => weatherId = value; }
    public string ApiKey { set => apiKey = value; }
}

public class NonPublicProperties
{
    public string apiKey;
    public int weatherId;
    public string url;
    public string TokenId { get; private set; }
    private string ApiKey { get => apiKey; set => apiKey = value; }
    protected int WeatherId { get => weatherId; set => weatherId = value; }
    internal string SecretKey { get; set; }
    protected internal int TimeId { get; set; }
    private protected string Url { get => url; set => url = value; }
}
