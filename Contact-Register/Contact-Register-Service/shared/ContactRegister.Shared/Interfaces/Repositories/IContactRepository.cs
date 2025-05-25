using ContactRegister.Domain.Entities;

namespace ContactRegister.Shared.Interfaces.Repositories;

public interface IContactRepository
{
    public Task AddContactAsync(Contact contact);
    public Task<Contact?> GetContactByIdAsync(int id);
    public Task<IEnumerable<Contact>> GetContactsAsync();
    public Task UpdateContactAsync(Contact contact);
    public Task DeleteContactAsync(Contact contact);
    public Task<List<Contact>> GetContactsByDdd(int[] codes);
}