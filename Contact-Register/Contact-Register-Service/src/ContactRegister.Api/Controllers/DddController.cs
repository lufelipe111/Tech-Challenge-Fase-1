using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DddController : ControllerBase
{
    private readonly IDddService _dddService;
    private readonly IDddApiService _dddApiService;

    public DddController(IDddService dddService, IDddApiService dddApiService)
    {
	    _dddService = dddService;
	    _dddApiService = dddApiService;
    }

    /// <summary>
    /// Busca a lista com todos os DDDs cadastrados na base de dados.
    /// </summary>
    /// <returns>A lista com todos os DDDs, ou uma lista de erros.</returns>
    /// <response code="200">
    ///	Busca realizada com sucesso. Exemplo de retorno:
    ///		
    ///		GET /Ddd/GetDdd
    ///		[
    ///			{
    ///				"code": 69,
    ///				"state": "RO",
    ///				"region": "ALTO ALEGRE DO PARECIS, VALE DO PARA�SO, ..."
    ///			},
    ///			{
    ///				"code": 68,
    ///				"state": "AC",
    ///				"region": "PORTO ACRE, XAPURI, TARAUAC�, SENA MADUREIRA, ..."
    ///			}
    ///		]
    /// </response>
    /// <response code="400">
	/// Erro ao efetuar a busca:
	/// 
    ///		GET /Ddd/GetDdd/
    ///		[
    ///			{
    ///				"code": "Ddd.Get.Exception",
    ///				"description": "Invalid object",
    ///				"type": 0,
    ///				"numericType": 0,
    ///				"metadata": null
    ///			}
    ///		]
    /// </response>
    [HttpGet("[action]")]
	public async Task<IActionResult> GetDdd()
	{
		var result = await _dddService.GetDdd();

		if (result.IsError)
			return BadRequest(result.Errors);

		return Ok(result.Value);
	}

	/// <summary>
	/// Busca as informa��es regionais (estado e lista de cidades) a partir de um DDD informado.
	/// </summary>
	/// <param name="code">C�digo DDD a ser pesquisado.</param>
	/// <returns>A informa��o sobre o DDD, ou uma lista de erros.</returns>
	/// <response code="200">
	///	Busca realizada com sucesso. Exemplo de retorno:
	///		
	///		GET /Ddd/GetDdd/{code}
	///		{
	///			"code": 68,
	///			"state": "AC",
	///			"region": "PORTO ACRE, XAPURI, TARAUAC�, ..."
	///		}
	/// </response>
	/// <response code="400">
	/// Erro ao efetuar a busca. Exemplo de retorno:
	/// 
	///		GET /Ddd/GetDdd/{code}
	///		[
	///			{
	///				"code": "Ddd.ExternalApi",
	///				"description": "DDD n�o encontrado",
	///				"type": 0,
	///				"numericType": 0,
	///				"metadata": null
	///			}
	///		]
	/// </response>
	[HttpGet("[action]/{code:int}")]
	public async Task<IActionResult> GetDdd(int code)
    {
        var result = await _dddService.GetDddByCode(code);

		if (result.IsError)
			return BadRequest(result.Errors);

		return Ok(result.Value);
	}

	[HttpGet("[action]/{code:int}")]
	public async Task<IActionResult> GetDddByCode(int code)
	{
		var result = await _dddApiService.GetByCode(code);
		return Ok(result);
	}
}