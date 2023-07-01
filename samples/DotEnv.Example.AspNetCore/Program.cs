using DotEnv.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Load the .env file.
var envVars = new EnvLoader().Load();
// Register IEnvReader to access environment variables.
builder.Services.AddSingleton<IEnvReader, EnvReader>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var baseUrl = envVars["APP_BASE_URL"];
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
