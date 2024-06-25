using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DotEnv.Example.OptionsPattern;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly AppSettings _appSettings;

    public HomeController(IOptions<AppSettings> options)
    {
        _appSettings = options.Value;
    }

    [HttpGet]
    public AppSettings Get()
    {
        return _appSettings;
    }
}
