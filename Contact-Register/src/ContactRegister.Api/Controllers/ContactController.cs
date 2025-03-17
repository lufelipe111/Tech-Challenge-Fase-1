using ContactRegister.Application.Inputs;
using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
	}

	/// <summary>
	/// Busca as informa��es de um Contato a partir do seu ID �nico informado.
	/// </summary>
	/// <param name="id">Identificador �nico (ID) do contato a ser pesquisado.</param>
	/// <returns>As informa��es do Contato, ou uma lista de erros.</returns>
	/// <response code="200">
	/// Busca realizada com sucesso. Exemplo de retorno:
	/// 
	///     GET /Contact/GetContact/{id}
	///     {
	///       "id": 1,
	///       "firstName": "Sebastiana",
	///       "lastName": "Agatha Gon�alves",
	///       "email": "sebastianaagathagoncalves@sfreitasadvogados.com.br",
	///       "address": {
	///         "addressLine1": "Rua Peroba 367 Eldorado",
	///         "addressLine2": "",
	///         "city": "Porto Velho",
	///         "state": "RO",
	///         "postalCode": "76811-696"
	///       },
	///       "homeNumber": {
	///         "number": "(69) 2775-0368"
	///       },
	///       "mobileNumber": {
	///         "number": "(69) 99835-1786"
	///       },
	///       "ddd": {
	///         "code": 69,
	///         "state": "RO",
	///         "region": "ALTO ALEGRE DO PARECIS, VALE DO PARA�SO, VALE DO ANARI, URUP�, THEOBROMA, TEIXEIR�POLIS, SERINGUEIRAS, S�O FRANCISCO DO GUAPOR�, S�O FELIPE D'OESTE, PRIMAVERA DE ROND�NIA, PIMENTEIRAS DO OESTE, PARECIS, NOVA UNI�O, MONTE NEGRO, MIRANTE DA SERRA, MINISTRO ANDREAZZA, ITAPU� DO OESTE, GOVERNADOR JORGE TEIXEIRA, CUJUBIM, CHUPINGUAIA, CASTANHEIRAS, CANDEIAS DO JAMARI, CAMPO NOVO DE ROND�NIA, CACAUL�NDIA, NOVO HORIZONTE DO OESTE, BURITIS, ALTO PARA�SO, ALVORADA D'OESTE, NOVA MAMOR�, S�O MIGUEL DO GUAPOR�, VILHENA, SANTA LUZIA D'OESTE, ROLIM DE MOURA, RIO CRESPO, PRESIDENTE M�DICI, PORTO VELHO, PIMENTA BUENO, OURO PRETO DO OESTE, NOVA BRASIL�NDIA D'OESTE, MACHADINHO D'OESTE, JI-PARAN�, JARU, GUAJAR�-MIRIM, ESPIG�O D'OESTE, COSTA MARQUES, CORUMBIARA, COLORADO DO OESTE, CEREJEIRAS, CACOAL, CABIXI, ARIQUEMES, ALTA FLORESTA D'OESTE"
	///       }
	///     }
	/// </response>
	/// <response code="404">Contato n�o encontrado</response>
	[HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetContact([FromRoute] int id)
    {

        var contact = await _contactService.GetContactByIdAsync(id);

        if(contact.Value == null)
        {
            return NotFound(null);
        }

        return Ok(contact.Value);
    }

	/// <summary>
	/// Busca as informa��es de Contatos a partir de uma lista de DDDs.
	/// </summary>
	/// <param name="dddCodes">List de DDDs a serem pesquisados.</param>
	/// <remarks>
	/// Exemplo de requisi��o:
	///
	///     POST /Contact/GetContactsByDddCodes
	///     [
	///        11,
	///        17,
	///        21
	///     ]
	///
	/// </remarks>
	/// <returns>A lista com as informa��es dos Contatos, ou uma lista de erros.</returns>
	/// <response code="200">
	/// Busca realizada com sucesso. Exemplo de retorno:
	/// 
	///     POST /Contact/GetContact
	///		[
	///		  {
	///			"id": 1,
	///			"firstName": "Sebastiana",
	///			"lastName": "Agatha Gon�alves",
	///			"email": "sebastianaagathagoncalves@sfreitasadvogados.com.br",
	///			"address": {
	///				"addressLine1": "Rua Peroba 367 Eldorado",
	///				"addressLine2": "",
	///				"city": "Porto Velho",
	///				"state": "RO",
	///				"postalCode": "76811-696"
	///			},
	///			"homeNumber": {
	///				"number": "(69) 2775-0368"
	///			},
	///			"mobileNumber": {
	///			"number": "(69) 99835-1786"
	///			},
	///			"ddd": {
	///				"code": 69,
	///				"state": "RO",
	///				"region": "ALTO ALEGRE DO PARECIS, VALE DO PARA�SO, VALE DO ANARI, URUP�, THEOBROMA, TEIXEIR�POLIS, SERINGUEIRAS, S�O FRANCISCO DO GUAPOR�, S�O FELIPE D'OESTE, PRIMAVERA DE ROND�NIA, PIMENTEIRAS DO OESTE, PARECIS, NOVA UNI�O, MONTE NEGRO, MIRANTE DA SERRA, MINISTRO ANDREAZZA, ITAPU� DO OESTE, GOVERNADOR JORGE TEIXEIRA, CUJUBIM, CHUPINGUAIA, CASTANHEIRAS, CANDEIAS DO JAMARI, CAMPO NOVO DE ROND�NIA, CACAUL�NDIA, NOVO HORIZONTE DO OESTE, BURITIS, ALTO PARA�SO, ALVORADA D'OESTE, NOVA MAMOR�, S�O MIGUEL DO GUAPOR�, VILHENA, SANTA LUZIA D'OESTE, ROLIM DE MOURA, RIO CRESPO, PRESIDENTE M�DICI, PORTO VELHO, PIMENTA BUENO, OURO PRETO DO OESTE, NOVA BRASIL�NDIA D'OESTE, MACHADINHO D'OESTE, JI-PARAN�, JARU, GUAJAR�-MIRIM, ESPIG�O D'OESTE, COSTA MARQUES, CORUMBIARA, COLORADO DO OESTE, CEREJEIRAS, CACOAL, CABIXI, ARIQUEMES, ALTA FLORESTA D'OESTE"
	///			}
	///		},
	///		{
	///			"id": 2,
	///			"firstName": "Rebeca",
	///			"lastName": "Carolina Jesus",
	///			"email": "rebeca_jesus@truran.com.br",
	///			"address": {
	///				"addressLine1": "Travessa S�o Francisco 558 Belo Jardim II",
	///				"addressLine2": "",
	///				"city": "Rio Branco",
	///				"state": "AC",
	///				"postalCode": "69908-012"
	///			},
	///			"homeNumber": {
	///				"number": "(68) 3964-5765"
	///			},
	///			"mobileNumber": {
	///				"number": "(68) 98483-4750"
	///			},
	///			"ddd": {
	///				"code": 68,
	///				"state": "AC",
	///				"region": "PORTO ACRE, XAPURI, TARAUAC�, SENA MADUREIRA, SENADOR GUIOMARD, SANTA ROSA DO PURUS, RODRIGUES ALVES, RIO BRANCO, PORTO WALTER, PL�CIDO DE CASTRO, MARECHAL THAUMATURGO, MANOEL URBANO, M�NCIO LIMA, JORD�O, FEIJ�, EPITACIOL�NDIA, CRUZEIRO DO SUL, CAPIXABA, BUJARI, BRASIL�IA, ASSIS BRASIL, ACREL�NDIA"
	///			}
	///		}
	///	]
	/// </response>
	/// <response code="404">Contato n�o encontrado</response>
	/// <response code="400">Erro ao realizar a busca</response>
	[HttpPost("[action]")]
    public async Task<IActionResult> GetContactsByDddCodes([FromBody] int[] dddCodes)
    {
        var contacts = await _contactService.GetContactsByDdd(dddCodes);
        
        return contacts.Match<IActionResult>(
            c => c.Any() 
                ? Ok(c) 
                : NotFound(null)
            , BadRequest);
    }

	/// <summary>
	/// Busca as informa��es de Contatos a partir de um filtro informado.
	/// </summary>
	/// <param name="firstName">Primeiro nome do Contato.</param>
	/// <param name="lastName">Sobrenome do Contato.</param>
	/// <param name="email">E-mail do Contato. Precisa estar no padr�o xx@xx.xx</param>
	/// <param name="city">A cidade do Contato.</param>
	/// <param name="state">O Estado do Contato.</param>
	/// <param name="postalCode">O CEP do Contato.</param>
	/// <param name="addressLine1">Primeiro linha do endere�o do Contato.</param>
	/// <param name="addressLine2">Segunda linha do endere�o do Contato, se aplic�vel.</param>
	/// <param name="homeNumber">N�mero de telefone fixo do Contato.</param>
	/// <param name="mobileNumber">N�mero de celular do Contato.</param>
	/// <param name="dddCode">DDD do Contato.</param>
	/// <param name="skip">Para busca paginada. P�gina a ser pesquisada. O padr�o � 0 (primeira p�gina).</param>
	/// <param name="take">Quantidade de contatos a serem retornados. O padr�o � 50.</param>
	/// <returns>A lista com as informa��es dos Contatos, ou vazio.</returns>
	/// <response code="200">
	/// Busca realizada com sucesso. Exemplo de retorno:
	/// 
	///     POST /Contact/GetContact
	///		[
	///		  {
	///			"id": 1,
	///			"firstName": "Sebastiana",
	///			"lastName": "Agatha Gon�alves",
	///			"email": "sebastianaagathagoncalves@sfreitasadvogados.com.br",
	///			"address": {
	///				"addressLine1": "Rua Peroba 367 Eldorado",
	///				"addressLine2": "",
	///				"city": "Porto Velho",
	///				"state": "RO",
	///				"postalCode": "76811-696"
	///			},
	///			"homeNumber": {
	///				"number": "(69) 2775-0368"
	///			},
	///			"mobileNumber": {
	///			"number": "(69) 99835-1786"
	///			},
	///			"ddd": {
	///				"code": 69,
	///				"state": "RO",
	///				"region": "ALTO ALEGRE DO PARECIS, VALE DO PARA�SO, VALE DO ANARI, URUP�, THEOBROMA, TEIXEIR�POLIS, SERINGUEIRAS, S�O FRANCISCO DO GUAPOR�, S�O FELIPE D'OESTE, PRIMAVERA DE ROND�NIA, PIMENTEIRAS DO OESTE, PARECIS, NOVA UNI�O, MONTE NEGRO, MIRANTE DA SERRA, MINISTRO ANDREAZZA, ITAPU� DO OESTE, GOVERNADOR JORGE TEIXEIRA, CUJUBIM, CHUPINGUAIA, CASTANHEIRAS, CANDEIAS DO JAMARI, CAMPO NOVO DE ROND�NIA, CACAUL�NDIA, NOVO HORIZONTE DO OESTE, BURITIS, ALTO PARA�SO, ALVORADA D'OESTE, NOVA MAMOR�, S�O MIGUEL DO GUAPOR�, VILHENA, SANTA LUZIA D'OESTE, ROLIM DE MOURA, RIO CRESPO, PRESIDENTE M�DICI, PORTO VELHO, PIMENTA BUENO, OURO PRETO DO OESTE, NOVA BRASIL�NDIA D'OESTE, MACHADINHO D'OESTE, JI-PARAN�, JARU, GUAJAR�-MIRIM, ESPIG�O D'OESTE, COSTA MARQUES, CORUMBIARA, COLORADO DO OESTE, CEREJEIRAS, CACOAL, CABIXI, ARIQUEMES, ALTA FLORESTA D'OESTE"
	///			}
	///		},
	///		{
	///			"id": 2,
	///			"firstName": "Rebeca",
	///			"lastName": "Carolina Jesus",
	///			"email": "rebeca_jesus@truran.com.br",
	///			"address": {
	///				"addressLine1": "Travessa S�o Francisco 558 Belo Jardim II",
	///				"addressLine2": "",
	///				"city": "Rio Branco",
	///				"state": "AC",
	///				"postalCode": "69908-012"
	///			},
	///			"homeNumber": {
	///				"number": "(68) 3964-5765"
	///			},
	///			"mobileNumber": {
	///				"number": "(68) 98483-4750"
	///			},
	///			"ddd": {
	///				"code": 68,
	///				"state": "AC",
	///				"region": "PORTO ACRE, XAPURI, TARAUAC�, SENA MADUREIRA, SENADOR GUIOMARD, SANTA ROSA DO PURUS, RODRIGUES ALVES, RIO BRANCO, PORTO WALTER, PL�CIDO DE CASTRO, MARECHAL THAUMATURGO, MANOEL URBANO, M�NCIO LIMA, JORD�O, FEIJ�, EPITACIOL�NDIA, CRUZEIRO DO SUL, CAPIXABA, BUJARI, BRASIL�IA, ASSIS BRASIL, ACREL�NDIA"
	///			}
	///		}
	///	]
	/// </response>
	/// <response code="404">Contato n�o encontrado</response>
	[HttpGet("[action]")]
    public async Task<IActionResult> GetContacts(
        [FromQuery] string? firstName,
        [FromQuery] string? lastName,
        [FromQuery] string? email,
        [FromQuery] string? city,
        [FromQuery] string? state,
        [FromQuery] string? postalCode,
        [FromQuery] string? addressLine1,
        [FromQuery] string? addressLine2,
        [FromQuery] string? homeNumber,
        [FromQuery] string? mobileNumber,
        [FromQuery] int dddCode = 0,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {

        var contact = await _contactService.GetContactsAsync(
			dddCode,
			firstName,
            lastName,
            email,
            city,
            state,
            postalCode,
            addressLine1,
            addressLine2,
            homeNumber,
            mobileNumber);
        
        if (contact.Value == null)
        {
            return NotFound(null);
        }
        return Ok(contact.Value.Skip(skip).Take(take));
    }

    /// <summary>
    /// Cria um novo contato.
    /// </summary>
    /// <response code="201">Criado com sucesso</response>
    /// <response code="400">
    /// Exemplo de erro ao criar um contato:
    /// 
    ///     POST /Contact/UpdateContact
    ///     [
    ///       {
    ///         "code": "Ddd.ExternalApi",
    ///         "description": "DDD n�o encontrado",
    ///         "type": 0,
    ///         "numericType": 0,
    ///         "metadata": null
    ///       }
    ///     ]
    /// </response>
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateContact([FromBody] ContactInput contact)
    {
        var result = await _contactService.AddContactAsync(contact);

        if (result.IsError)
            return BadRequest(null);

		return Created(HttpContext.Request.GetDisplayUrl(), new { Id = Guid.NewGuid() });
    }

    /// <summary>
    /// Atualiza um Contato a partir do seu ID �nico informado.
    /// </summary>
    /// <param name="id">Identificador �nico (ID) do contato a ser exclu�do.</param>
    /// <response code="204">Atualiza��o realizada com sucesso</response>
    /// <response code="400">
    /// Exemplo de erro ao atualizar o contato:
    /// 
    ///     PUT /Contact/UpdateContact/{id}
    ///     [
    ///       {
    ///         "code": "Ddd.ExternalApi",
    ///         "description": "DDD n�o encontrado",
    ///         "type": 0,
    ///         "numericType": 0,
    ///         "metadata": null
    ///       }
    ///     ]
    /// </response>
    [HttpPut("[action]/{id:int}")]
	public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] ContactInput contact)
	{
		var result = await _contactService.UpdateContactAsync(id, contact);

		if (result.IsError)
			return BadRequest(null);

		return NoContent();
	}

	/// <summary>
	/// Exclui um Contato a partir do seu ID �nico informado.
	/// </summary>
	/// <param name="id">Identificador �nico (ID) do contato a ser exclu�do.</param>
	/// <returns>A confirma��o de exclus�o do Contato, ou uma lista de erros.</returns>
	/// <response code="204">Exclus�o realizada com sucesso</response>
	/// <response code="400">
	/// Erro ao excluir contato. Exemplo de retorno:
	/// 
	///		DELETE /Contact/DeleteContact/{id}
	///		[
	///			{
	///				"code": "Contact.NotFound",
	///				"description": "Contact 3 not found",
	///				"type": 4,
	///				"numericType": 4,
	///				"metadata": null
	///			}
	///		]
	/// </response>
	[HttpDelete("[action]/{id:int}")]
    public async Task<IActionResult> DeleteContact([FromRoute] int id)
    {
        var result = await _contactService.DeleteContactAsync(id);

        if (result.IsError)
            return BadRequest(null);

        return NoContent();
    }
}