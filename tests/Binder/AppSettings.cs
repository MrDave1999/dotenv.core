namespace DotEnv.Core.Tests.Binder;

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
}
