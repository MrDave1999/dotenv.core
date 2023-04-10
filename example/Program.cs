global using DotEnv.Core;

var envVars = new EnvLoader().Load();

var cs = envVars[AppSettings.ConnectionString];
Console.WriteLine("Connection String=" + cs);

var reader = new EnvReader();
var userName = reader[AppSettings.DbUserName];
Console.WriteLine("UserName=" + userName);

var dbHost = EnvReader.Instance[AppSettings.DbHost];
Console.WriteLine("Host=" + dbHost);