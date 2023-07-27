using DotEnv.Core;

var requiredKeys = new[]
{
    "DB_PORT",
    "DB_USERNAME",
    "DB_PASSWORD",
    "DB_DATABASE"
};
var envVars = new EnvLoader().Load();
Console.WriteLine("DB_HOST=" + envVars["DB_HOST"]);

// Checks if the required keys are present in the application.
// In this case, the application will fail because the following keys are not present in the .env file:
// DB_PORT
// DB_USERNAME
// DB_PASSWORD
// DB_DATABASE
new EnvValidator()
    .SetRequiredKeys(requiredKeys)
    .Validate();
