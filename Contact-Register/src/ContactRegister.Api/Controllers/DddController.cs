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
	///				"region": "ALTO ALEGRE DO PARECIS, VALE DO PARAÍSO, VALE DO ANARI, URUPÁ, THEOBROMA, TEIXEIRÓPOLIS, SERINGUEIRAS, SÃO FRANCISCO DO GUAPORÉ, SÃO FELIPE D'OESTE, PRIMAVERA DE RONDÔNIA, PIMENTEIRAS DO OESTE, PARECIS, NOVA UNIÃO, MONTE NEGRO, MIRANTE DA SERRA, MINISTRO ANDREAZZA, ITAPUÃ DO OESTE, GOVERNADOR JORGE TEIXEIRA, CUJUBIM, CHUPINGUAIA, CASTANHEIRAS, CANDEIAS DO JAMARI, CAMPO NOVO DE RONDÔNIA, CACAULÂNDIA, NOVO HORIZONTE DO OESTE, BURITIS, ALTO PARAÍSO, ALVORADA D'OESTE, NOVA MAMORÉ, SÃO MIGUEL DO GUAPORÉ, VILHENA, SANTA LUZIA D'OESTE, ROLIM DE MOURA, RIO CRESPO, PRESIDENTE MÉDICI, PORTO VELHO, PIMENTA BUENO, OURO PRETO DO OESTE, NOVA BRASILÂNDIA D'OESTE, MACHADINHO D'OESTE, JI-PARANÁ, JARU, GUAJARÁ-MIRIM, ESPIGÃO D'OESTE, COSTA MARQUES, CORUMBIARA, COLORADO DO OESTE, CEREJEIRAS, CACOAL, CABIXI, ARIQUEMES, ALTA FLORESTA D'OESTE"
	///			},
	///			{
	///				"code": 68,
	///				"state": "AC",
	///				"region": "PORTO ACRE, XAPURI, TARAUACÁ, SENA MADUREIRA, SENADOR GUIOMARD, SANTA ROSA DO PURUS, RODRIGUES ALVES, RIO BRANCO, PORTO WALTER, PLÁCIDO DE CASTRO, MARECHAL THAUMATURGO, MANOEL URBANO, MÂNCIO LIMA, JORDÃO, FEIJÓ, EPITACIOLÂNDIA, CRUZEIRO DO SUL, CAPIXABA, BUJARI, BRASILÉIA, ASSIS BRASIL, ACRELÂNDIA"
	///			}
	///		]
	/// </response>
	/// <response code="400">Erro ao efetuar a busca</response>
	[HttpGet("[action]")]
	public async Task<IActionResult> GetDdd()
	{
		var result = await _dddService.GetDdd();

		if (result.IsError)
			return BadRequest(result.Errors);

		return Ok(result.Value);
	}

	/// <summary>
	/// Busca as informações regionais (estado e lista de cidades) a partir de um DDD informado.
	/// </summary>
	/// <param name="code">Código DDD a ser pesquisado.</param>
	/// <returns>A informação sobre o DDD, ou uma lista de erros.</returns>
	/// <response code="200">
	///	Busca realizada com sucesso. Exemplo de retorno:
	///		
	///		GET /Ddd/GetDdd/{code}
	///		{
	///			"code": 68,
	///			"state": "AC",
	///			"region": "PORTO ACRE, XAPURI, TARAUACÁ, SENA MADUREIRA, SENADOR GUIOMARD, SANTA ROSA DO PURUS, RODRIGUES ALVES, RIO BRANCO, PORTO WALTER, PLÁCIDO DE CASTRO, MARECHAL THAUMATURGO, MANOEL URBANO, MÂNCIO LIMA, JORDÃO, FEIJÓ, EPITACIOLÂNDIA, CRUZEIRO DO SUL, CAPIXABA, BUJARI, BRASILÉIA, ASSIS BRASIL, ACRELÂNDIA"
	///		}
	/// </response>
	/// <response code="400">
	/// Erro ao efetuar a busca. Exemplo de retorno:
	/// 
	///		GET /Ddd/GetDdd/{code}
	///		[
	///			{
	///				"code": "Ddd.ExternalApi",
	///				"description": "DDD não encontrado",
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
}