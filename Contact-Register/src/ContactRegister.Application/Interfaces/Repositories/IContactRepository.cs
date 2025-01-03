using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.Interfaces.Repositories;

public interface IContactRepository
{
    public Task AddContactAsync(Contact contact);
    public Task<Contact?> GetContactByIdAsync(int id);
    public Task<IEnumerable<Contact>> GetContactsAsync();
    public Task UpdateContactAsync(Contact contact);
    public Task DeleteContactAsync(int id);
}