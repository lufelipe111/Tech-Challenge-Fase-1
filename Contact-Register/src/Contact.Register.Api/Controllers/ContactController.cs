using Microsoft.AspNetCore.Mvc;

namespace Contact.Register.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly ILogger<ContactController> _logger;

    public ContactController(ILogger<ContactController> logger)
    {
        _logger = logger;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Teste");
        return Ok("OK");
    }
}