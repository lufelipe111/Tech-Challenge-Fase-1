using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ICacheService _cacheService;

    public ContactController(IContactService contactService, ICacheService cacheService)
    {
        _contactService = contactService;
        _cacheService = cacheService;
    }

	/// <summary>
	/// Busca as informações de um Contato a partir do seu ID único informado.
	/// </summary>
	/// <param name="id">Identificador único (ID) do contato a ser pesquisado.</param>
	/// <returns>As informações do Contato, ou uma lista de erros.</returns>
	/// <response code="200">Busca realizada com sucesso</response>
	/// <response code="404">Contato não encontrado</response>
	[HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetContact([FromRoute] int id)
    {
        var key = $"GetContact-{id}";
        var cachedContacts = _cacheService.Get(key);

        if (cachedContacts != null)
        {
            return Ok(cachedContacts);
        }

        var contact = await _contactService.GetContactByIdAsync(id);

        if(contact.Value == null)
        {
            return NotFound();
        }

        _cacheService.Set(key, contact.Value);

        return Ok(contact.Value);
    }

	/// <summary>
	/// Busca as informações de Contatos a partir de uma lista de DDDs.
	/// </summary>
	/// <param name="dddCodes">List de DDDs a serem pesquisados.</param>
	/// <remarks>
	/// Exemplo de requisição:
	///
	///     POST /Contact/GetContactsByDddCodes
	///     [
	///        11,
	///        17,
	///        21
	///     ]
	///
	/// </remarks>
	/// <returns>A lista com as informações dos Contatos, ou uma lista de erros.</returns>
	/// <response code="200">Busca realizada com sucesso</response>
	/// <response code="404">Contato não encontrado</response>
	/// <response code="400">Erro ao realizar a busca</response>
	[HttpPost("[action]")]
    public async Task<IActionResult> GetContactsByDddCodes([FromBody] int[] dddCodes)
    {
        var contacts = await _contactService.GetContactsByDdd(dddCodes);
        
        return contacts.Match<IActionResult>(
            c => c.Any() 
                ? Ok(c) 
                : NotFound()
            , BadRequest);
    }

	/// <summary>
	/// Busca as informações de Contatos a partir de um filtro informado.
	/// </summary>
	/// <param name="firstName">Primeiro nome do Contato.</param>
	/// <param name="lastName">Sobrenome do Contato.</param>
	/// <param name="email">E-mail do Contato. Precisa estar no padrão xx@xx.xx</param>
	/// <param name="city">A cidade do Contato.</param>
	/// <param name="state">O Estado do Contato.</param>
	/// <param name="postalCode">O CEP do Contato.</param>
	/// <param name="addressLine1">Primeiro linha do endereço do Contato.</param>
	/// <param name="addressLine2">Segunda linha do endereço do Contato, se aplicável.</param>
	/// <param name="homeNumber">Número de telefone fixo do Contato.</param>
	/// <param name="mobileNumber">Número de celular do Contato.</param>
	/// <param name="dddCode">DDD do Contato.</param>
	/// <param name="skip">Para busca paginada. Página a ser pesquisada. O padrão é 0 (primeira página).</param>
	/// <param name="take">Quantidade de contatos a serem retornados. O padrão é 50.</param>
	/// <returns>A lista com as informações dos Contatos, ou vazio.</returns>
	/// <response code="200">Busca realizada com sucesso</response>
	/// <response code="404">Contato não encontrado</response>
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
            return NotFound();
        }
        return Ok(contact.Value.Skip(skip).Take(take));
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> CreateContact([FromBody] ContactDto contact)
    {
        var result = await _contactService.AddContactAsync(contact);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok();
    }

	[HttpPut("[action]/{id:int}")]
	public async Task<IActionResult> UpdateContact([FromRoute] int id, [FromBody] ContactDto contact)
	{
		var result = await _contactService.UpdateContactAsync(id, contact);

		if (result.IsError)
			return BadRequest(result.Errors);

		return Ok();
	}

	/// <summary>
	/// Exclui um Contato a partir do seu ID único informado.
	/// </summary>
	/// <param name="id">Identificador único (ID) do contato a ser excluído.</param>
	/// <returns>A confirmação de exclusão do Contato, ou uma lista de erros.</returns>
	/// <response code="200">Exclusão realizada com sucesso</response>
	/// <response code="400">Erro ao excluir contato</response>
	[HttpDelete("[action]/{id:int}")]
    public async Task<IActionResult> DeleteContact([FromRoute] int id)
    {
        var result = await _contactService.DeleteContactAsync(id);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok();
    }
}