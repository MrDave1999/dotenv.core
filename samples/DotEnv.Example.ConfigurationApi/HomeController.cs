using Microsoft.AspNetCore.Mvc;

namespace DotEnv.Example.ConfigurationApi;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public SettingsDto Get()
    {
        var section = _configuration.GetSection("Settings");
        return new()
        {
            AppBaseUrl = section["APP_BASE_URL"],
            Server     = section["SERVER"],
            Database   = section["DATABASE"]
        };
    }

    public class SettingsDto
    {
        public string AppBaseUrl { get; init; }
        public string Server { get; init; }
        public string Database { get; init; }
    }
}
