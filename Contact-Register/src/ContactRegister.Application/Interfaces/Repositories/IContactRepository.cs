using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.Interfaces.Repositories;

public interface IContactRepository
{
    public Task AddContactAsync(Contact contact);
    public Task<Contact?> GetContactByIdAsync(long id);
    public Task<List<Contact>> GetContactsAsync();
    public Task UpdateContactAsync(Contact contact);
    public Task DeleteContactAsync(long id);
}