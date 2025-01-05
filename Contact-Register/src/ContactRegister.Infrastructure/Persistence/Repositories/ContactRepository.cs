using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContactRegister.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private DbSet<Contact> _contacts;
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
        _contacts = context.contacts;
    }
    
    public Task AddContactAsync(Contact contact)
    {
        throw new NotImplementedException();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _contacts.FindAsync(id);
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _contacts.Include(e => e.Ddd).ToListAsync();
    }

    public Task UpdateContactAsync(Contact contact)
    {
        throw new NotImplementedException();
    }

    public Task DeleteContactAsync(int id)
    {
        throw new NotImplementedException();
    }
}