using ContactRegister.Application.DTOs;
using ContactRegister.Application.Inputs;
using ErrorOr;

namespace ContactRegister.Application.Interfaces.Services;

public interface IContactService
{
    public Task<ErrorOr<Success>> AddContactAsync(ContactInput contactInput);
    public Task<ErrorOr<ContactDto?>> GetContactByIdAsync(int id);
    public Task<ErrorOr<IEnumerable<ContactDto>>> GetContactsAsync(
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
        string? mobileNumber = null);
    public Task<ErrorOr<Success>> UpdateContactAsync(int id, ContactInput contactInput);
    public Task<ErrorOr<Success>> DeleteContactAsync(int id);
    public Task<ErrorOr<IEnumerable<ContactDto>>> GetContactsByDdd(int[] dddCodes);
}