using ContactRegister.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactRegister.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly ILogger<ContactController> _logger;
    private readonly IContactService _contactService;

    public ContactController(ILogger<ContactController> logger, IContactService contactService)
    {
        _logger = logger;
        _contactService = contactService;
    }

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> GetContact(int id)
    {
        var contact = await _contactService.GetContactByIdAsync(id);
        if(contact.Value == null)
        {
            return NotFound();
        }
        return Ok();
    }
}