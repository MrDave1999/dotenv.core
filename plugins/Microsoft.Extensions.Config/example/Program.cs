using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
    .AddEnvFile(path: ".env", optional: true)
    .AddEnvFile("config.env")
    .Build();

Console.WriteLine("DB_HOST="     + config["DB_HOST"]);
Console.WriteLine("DB_PORT="     + config["DB_PORT"]);
Console.WriteLine("DB_USERNAME=" + config["DB_USERNAME"]);
Console.WriteLine("DB_PASSWORD=" + config["DB_PASSWORD"]);
Console.WriteLine();

Console.WriteLine("BASE_URL=" + config["BASE_URL"]);
Console.WriteLine();

IConfigurationSection section = config.GetSection("MYSQL");
Console.WriteLine("MYSQL__HOST="     + section["HOST"]);
Console.WriteLine("MYSQL__PORT="     + section["PORT"]);
Console.WriteLine("MYSQL__USERNAME=" + section["USERNAME"]);
Console.WriteLine("MYSQL__PASSWORD=" + section["PASSWORD"]);
