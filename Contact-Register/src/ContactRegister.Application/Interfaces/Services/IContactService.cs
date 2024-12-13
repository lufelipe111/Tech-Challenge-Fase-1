using ContactRegister.Application.DTOs;
using ErrorOr;

namespace ContactRegister.Application.Interfaces.Services;

public interface IContactService
{
    public Task<ErrorOr<Success>> AddContactAsync(ContactDto contact);
    public Task<ErrorOr<ContactDto?>> GetContactByIdAsync(long id);
    public Task<ErrorOr<List<ContactDto>>> GetContactsAsync();
    public Task<ErrorOr<Success>> UpdateContactAsync(long id, ContactDto contact);
    public Task<ErrorOr<Success>> DeleteContactAsync(long id);
}