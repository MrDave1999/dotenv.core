using DotEnv.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Loads the .env file.
var envVars = new EnvLoader().Load();
// Registers IEnvReader to access environment variables.
builder.Services.AddSingleton<IEnvReader, EnvReader>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Access the environment variable.
var baseUrl = envVars["APP_BASE_URL"];
// Prints the value of the variable.
app.Logger.LogInformation("APP_BASE_URL: {BaseUrl}", baseUrl);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
