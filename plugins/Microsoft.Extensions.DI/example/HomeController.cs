namespace DotEnv.Extensions.Microsoft.DI.Example;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly AppSettings _settings;

    public HomeController(AppSettings settings)
    {
        _settings = settings;
    }

    [HttpGet]
    public string Get()
        => _settings.Summaries;
}
