using DotEnv.Core;
using DotEnv.Example.Binding;

new EnvLoader().Load();
var appSettings = new EnvBinder().Bind<AppSettings>();
Console.WriteLine("DB_CONNECTION_STRING=" + appSettings.DbConnectionString);

var emailSettings = new EnvBinder()
                        .IgnoreException()
                        .Bind<EmailSettings>(out var validationResult);

if(validationResult.HasError())
{
    Console.WriteLine($"'{nameof(EmailSettings.ApiKey)}' property does not correspond to any configuration key.");
    Console.WriteLine("IsNull: " + string.IsNullOrEmpty(emailSettings.ApiKey));
}