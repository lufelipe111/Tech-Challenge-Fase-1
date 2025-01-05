using ContactRegister.Application.DTOs;
using ErrorOr;

namespace ContactRegister.Application.Interfaces.Services;

public interface IContactService
{
    public Task<ErrorOr<Success>> AddContactAsync(ContactDto contact);
    public Task<ErrorOr<ContactDto?>> GetContactByIdAsync(int id);
    public Task<ErrorOr<IEnumerable<ContactDto>>> GetContactsAsync(
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
        string mobileNumber);
    public Task<ErrorOr<Success>> UpdateContactAsync(int id, ContactDto contact);
    public Task<ErrorOr<Success>> DeleteContactAsync(int id);
}