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
    public string Get()
    {
        return _configuration["APP_BASE_URL"];
    }
}
