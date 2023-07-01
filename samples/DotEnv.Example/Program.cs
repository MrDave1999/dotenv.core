using DotEnv.Core;

var envVars = new EnvLoader().Load();

var cs = envVars[AppSettings.ConnectionString];
Console.WriteLine("Connection String=" + cs);

var reader = new EnvReader();
var userName = reader[AppSettings.DbUserName];
Console.WriteLine("UserName=" + userName);

var dbHost = EnvReader.Instance[AppSettings.DbHost];
Console.WriteLine("Host=" + dbHost);

var dbPassword = AppSettings.DbPassword.GetEnv();
Console.WriteLine("Password=" + dbPassword);

int dbPort = AppSettings.DbPort.GetEnv<int>();
Console.WriteLine("Port=" + dbPort);

public class AppSettings
{
    public const string DbHost           = "DB_HOST";
    public const string DbPort           = "DB_PORT";
    public const string DbUserName       = "DB_USERNAME";
    public const string DbPassword       = "DB_PASSWORD";
    public const string ConnectionString = "DB_CONNECTION_STRING";
}