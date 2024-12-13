using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces;
using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace ContactRegister.Application.Services;

public class ContactService : IContactService
{
    private readonly ILogger<ContactService> _logger;
    private readonly IContactRepository _contactRepository;

    public ContactService(
        ILogger<ContactService> logger, 
        IContactRepository contactRepository)
    {
        _logger = logger;
        _contactRepository = contactRepository;
    }

    public async Task<ErrorOr<Success>> AddContactAsync(ContactDto contact)
    {
        try
        {
            var contactEntity = contact.ToContact();

            if (contactEntity.Validate(out var errors))
            {
                return errors
                    .Select(e => Error.Failure("Contact.Validation", e))
                    .ToList();
            }
            
            await _contactRepository.AddContactAsync(contactEntity);

            return Result.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Error.Failure("Contact.Add.Exception", e.Message);
        }
    }
    

    public async Task<ErrorOr<ContactDto?>> GetContactByIdAsync(long id)
    {
        try
        {
            var contactEntity = await _contactRepository.GetContactByIdAsync(id);

            if (contactEntity == null)
                return ErrorOrFactory.From<ContactDto?>(null);
            
            var dto = ContactDto.FromEntity(contactEntity);

            return dto;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Error.Failure("Contact.Get.Exception", e.Message);
        }
    }

    public async Task<ErrorOr<List<ContactDto>>> GetContactsAsync()
    {
        try
        {
            var contacts = await _contactRepository.GetContactsAsync();
            var dtos = contacts.Select(ContactDto.FromEntity).ToList();
            return dtos;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Error.Failure("Contact.Get.Exception", e.Message);
        }
    }

    public async Task<ErrorOr<Success>> UpdateContactAsync(long id, ContactDto contact)
    {
        try
        {
            var targetContact = await _contactRepository.GetContactByIdAsync(id);
            
            if (targetContact == null)
                return Error.NotFound("Contact.NotFound", $"Contact {id} not found");
            
            targetContact.MobileNumber = contact.MobileNumber?.ToPhone();
            targetContact.HomeNumber = contact.HomeNumber?.ToPhone();
            targetContact.Address = contact.Address.ToAddress();
            targetContact.FirstName = contact.FirstName;
            targetContact.LastName = contact.LastName;
            targetContact.Email = contact.Email;

            if (targetContact.Validate(out var errors))
            {
                return errors
                    .Select(e => Error.Failure("Contact.Validation", e))
                    .ToList();
            }
             
            await _contactRepository.UpdateContactAsync(targetContact);
            
            return Result.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Error.Failure("Contact.Get.Exception", e.Message);
        }
    }

    public async Task<ErrorOr<Success>> DeleteContactAsync(long id)
    {
        try
        {
            var targetContact = await _contactRepository.GetContactByIdAsync(id);
            
            if (targetContact == null)
                return Error.NotFound("Contact.NotFound", $"Contact {id} not found");

            await _contactRepository.DeleteContactAsync(id);
            return Result.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Error.Failure("Contact.Get.Exception", e.Message);
        }
    }
}