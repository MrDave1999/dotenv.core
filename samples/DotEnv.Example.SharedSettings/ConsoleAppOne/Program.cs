using DotEnv.Core;

// Loads the config.env file.
new EnvLoader()
    .EnableFileNotFoundException()
    .SetBasePath("Shared")
    .AddEnvFile("config.env")
    .Load();

var reader = new EnvReader();
Console.WriteLine("APP_BASE_URL=" + reader["APP_BASE_URL"]);

// Loads the .env.dev.local and .env.local files.
new EnvLoader()
    .SetBasePath("Shared")
    .EnableFileNotFoundException()
    .LoadEnv();

Console.WriteLine("DB_USERNAME=" + reader["DB_USERNAME"]);
Console.WriteLine("DB_PASSWORD=" + reader["DB_PASSWORD"]);