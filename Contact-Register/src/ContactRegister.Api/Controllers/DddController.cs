using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DddController : ControllerBase
{
    private readonly ILogger<DddController> _logger;
    private readonly IDddService _dddService;

    public DddController(ILogger<DddController> logger, IDddService dddService)
    {
        _logger = logger;
        _dddService = dddService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateDdd([FromBody] DddDto dddDto)
    {
        await _dddService.AddDdd(dddDto);
        return Created();
    }

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetDdd(int id)
    {
        return Ok(await _dddService.GetDddById(id));
    }
}