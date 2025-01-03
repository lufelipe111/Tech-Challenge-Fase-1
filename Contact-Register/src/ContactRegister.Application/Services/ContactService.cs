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
    

    public async Task<ErrorOr<ContactDto?>> GetContactByIdAsync(int id)
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

    public async Task<ErrorOr<IEnumerable<ContactDto>>> GetContactsAsync(
        string firstName,
        string lastName,
        string email,
        int dddCode,
        string city,
        string state,
        string postalCode,
        string addressLine1,
        string addressLine2,
        string homeNumber,
        string mobileNumber)
    {
        try
        {
            var contactsQuery = await _contactRepository.GetContactsAsync();
            if (!string.IsNullOrEmpty(firstName)) { contactsQuery = contactsQuery.Where(c => c.FirstName.Contains(firstName)); }
            if (!string.IsNullOrEmpty(lastName)) { contactsQuery = contactsQuery.Where(c => c.LastName.Contains(lastName)); }
            if (!string.IsNullOrEmpty(email)) { contactsQuery = contactsQuery.Where(c => c.Email.Contains(email)); }
            if (dddCode > 0) { contactsQuery = contactsQuery.Where(c => c.DddId == dddCode); }
            if (!string.IsNullOrEmpty(city)) { contactsQuery = contactsQuery.Where(c => c.Address.City.Contains(city)); }
            if (!string.IsNullOrEmpty(state)) { contactsQuery = contactsQuery.Where(c => c.Address.State.Contains(state)); }
            if (!string.IsNullOrEmpty(postalCode)) { contactsQuery = contactsQuery.Where(c => c.Address.PostalCode.Contains(postalCode)); }
            if (!string.IsNullOrEmpty(addressLine1)) { contactsQuery = contactsQuery.Where(c => c.Address.AddressLine1.Contains(addressLine1)); }
            if (!string.IsNullOrEmpty(addressLine2)) { contactsQuery = contactsQuery.Where(c => c.Address.AddressLine2.Contains(addressLine2)); }
            if (!string.IsNullOrEmpty(homeNumber)) { contactsQuery = contactsQuery.Where(c => c.HomeNumber.Number.Contains(homeNumber)); }
            if (!string.IsNullOrEmpty(mobileNumber)) { contactsQuery = contactsQuery.Where(c => c.MobileNumber.Number.Contains(mobileNumber)); }

            var dtos = contactsQuery.Select(ContactDto.FromEntity).ToList();
            return dtos;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Error.Failure("Contact.Get.Exception", e.Message);
        }
    }

    public async Task<ErrorOr<Success>> UpdateContactAsync(int id, ContactDto contact)
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

    public async Task<ErrorOr<Success>> DeleteContactAsync(int id)
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