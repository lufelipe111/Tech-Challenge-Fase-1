using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DddController : ControllerBase
{
    private readonly IDddService _dddService;

    public DddController(IDddService dddService)
    {
        _dddService = dddService;
    }

	[HttpGet("[action]")]
	public async Task<IActionResult> GetDdd()
	{
		return Ok(await _dddService.GetDdd());
	}

	[HttpGet("[action]/{code:int}")]
    public async Task<IActionResult> GetDdd(int code)
    {
        return Ok(await _dddService.GetDddByCode(code));
    }
}