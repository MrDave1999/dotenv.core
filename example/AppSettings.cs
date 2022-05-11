public class AppSettings
{
    [EnvKey("MYSQL_HOST")]
    public string? MySqlHost { get; set; }

    [EnvKey("MYSQL_DB")]
    public string? MySqlDb { get; set; }
}