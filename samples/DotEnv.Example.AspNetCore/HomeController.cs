using DotEnv.Core;
using Microsoft.AspNetCore.Mvc;

namespace DotEnv.Example.AspNetCore;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IEnvReader _reader;

    public HomeController(IEnvReader reader)
    {
        _reader = reader;
    }

    [HttpGet]
    public string Get()
    {
        return _reader["APP_BASE_URL"];
    }
}
