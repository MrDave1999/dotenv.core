using DotEnv.Core;
using DotEnv.Example.OptionsPattern;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Loads the .env file.
new EnvLoader().Load();
// Adds environment variables in the configuration collection.
builder.Configuration.AddEnvironmentVariables();
// Registers a configuration instance which AppSettings will bind against.
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
