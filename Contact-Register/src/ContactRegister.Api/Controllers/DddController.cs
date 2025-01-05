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
		var result = await _dddService.GetDdd();

		if (result.IsError)
			return BadRequest(result.Errors);

		return Ok(result.Value);
	}

	[HttpGet("[action]/{code:int}")]
    public async Task<IActionResult> GetDdd(int code)
    {
        var result = await _dddService.GetDddByCode(code);

		if (result.IsError)
			return BadRequest(result.Errors);

		return Ok(result.Value);
	}
}