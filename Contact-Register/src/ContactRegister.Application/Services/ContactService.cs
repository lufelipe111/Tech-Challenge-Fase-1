using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace ContactRegister.Application.Services;

public class ContactService : IContactService
{
    private readonly ILogger<ContactService> _logger;
    private readonly IContactRepository _contactRepository;
    private readonly IDddService _dddService;
    private readonly ICacheService _cacheService;

	public ContactService(
        ILogger<ContactService> logger,
        IContactRepository contactRepository,
        IDddService dddService,
        ICacheService cacheService)
	{
		_logger = logger;
		_contactRepository = contactRepository;
		_dddService = dddService;
        _cacheService = cacheService;
    }

	public async Task<ErrorOr<Success>> AddContactAsync(ContactDto contact)
    {
        try
        {
            var ddd = await _dddService.GetDddByCode(contact.Ddd?.Code ?? 0);

            if (ddd.IsError) return ddd.Errors;

            contact.Ddd = ddd.Value;

			var contactEntity = contact.ToContact();

            if (!contactEntity.Validate(out var errors))
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
		int dddCode,
		string? firstName = null,
        string? lastName = null,
        string? email = null,
        string? city = null,
        string? state = null,
        string? postalCode = null,
        string? addressLine1 = null,
        string? addressLine2 = null,
        string? homeNumber = null,
        string? mobileNumber = null)
    {
        try
        {
            var contactsQuery = await _contactRepository.GetContactsAsync();
            if (!string.IsNullOrEmpty(firstName)) { contactsQuery = contactsQuery.Where(c => c.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(lastName)) { contactsQuery = contactsQuery.Where(c => c.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(email)) { contactsQuery = contactsQuery.Where(c => c.Email.Contains(email, StringComparison.OrdinalIgnoreCase)); }
            if (dddCode > 0) { contactsQuery = contactsQuery.Where(c => c.Ddd.Code == dddCode); }
            if (!string.IsNullOrEmpty(city)) { contactsQuery = contactsQuery.Where(c => c.Address.City.Contains(city, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(state)) { contactsQuery = contactsQuery.Where(c => c.Address.State.Contains(state, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(postalCode)) { contactsQuery = contactsQuery.Where(c => c.Address.PostalCode.Contains(postalCode, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(addressLine1)) { contactsQuery = contactsQuery.Where(c => c.Address.AddressLine1.Contains(addressLine1, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(addressLine2)) { contactsQuery = contactsQuery.Where(c => c.Address.AddressLine2.Contains(addressLine2, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(homeNumber)) { contactsQuery = contactsQuery.Where(c => c.HomeNumber.Number.Contains(homeNumber, StringComparison.OrdinalIgnoreCase)); }
            if (!string.IsNullOrEmpty(mobileNumber)) { contactsQuery = contactsQuery.Where(c => c.MobileNumber.Number.Contains(mobileNumber, StringComparison.OrdinalIgnoreCase)); }

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

			var ddd = await _dddService.GetDddByCode(contact.Ddd?.Code ?? 0);

			if (ddd.IsError) return ddd.Errors;
			
			targetContact.Ddd = ddd.Value!.ToDdd();

			if (!targetContact.Validate(out var errors))
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
            return Error.Failure("Contact.Update.Exception", e.Message);
		}
	}

    public async Task<ErrorOr<Success>> DeleteContactAsync(int id)
    {
        try
        {
            var targetContact = await _contactRepository.GetContactByIdAsync(id);

			if (targetContact == null)
                return Error.NotFound("Contact.NotFound", $"Contact {id} not found");

            await _contactRepository.DeleteContactAsync(targetContact);
			return Result.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return Error.Failure("Contact.Delete.Exception", e.Message);
		}
	}

	public async Task<ErrorOr<IEnumerable<ContactDto>>> GetContactsByDdd(int[] dddCodes)
    {
        var contacts = await _cacheService.GetOrCreateAsync(
            $"{nameof(GetContactByIdAsync)}:{string.Join(",", dddCodes)}", 
            async () => await _contactRepository.GetContactsByDdd(dddCodes));
        return contacts.Select(ContactDto.FromEntity).ToList();
    }
}