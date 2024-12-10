using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Domain.Entities;

namespace ContactRegister.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    public Task AddContactAsync(Contact contact)
    {
        throw new NotImplementedException();
    }

    public Task<Contact?> GetContactByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Contact>> GetContactsAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateContactAsync(Contact contact)
    {
        throw new NotImplementedException();
    }

    public Task DeleteContactAsync(long id)
    {
        throw new NotImplementedException();
    }
}