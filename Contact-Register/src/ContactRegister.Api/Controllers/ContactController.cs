using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly ILogger<ContactController> _logger;
    private readonly IContactService _contactService;
    private readonly ICacheService _cacheService;

    public ContactController(ILogger<ContactController> logger, IContactService contactService, ICacheService cacheService)
    {
        _logger = logger;
        _contactService = contactService;
        _cacheService = cacheService;
    }

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
            firstName,
            lastName,
            email,
            dddCode,
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

    [HttpDelete("[action]/{id:int}")]
    public async Task<IActionResult> DeleteContact([FromRoute] int id)
    {
        var result = await _contactService.DeleteContactAsync(id);

        if (result.IsError)
            return BadRequest(result.Errors);

        return Ok();
    }
}