using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task AddContactAsync(Contact contact)
    {
        contact.Ddd = await _context.ddds.FirstAsync(ddd => ddd.Code == contact.Ddd.Code);
		_ = await _contacts.AddAsync(contact);
        _ = await _context.SaveChangesAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _contacts.Include(c => c.Ddd).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync()
    {
        return await _contacts.Include(e => e.Ddd).ToListAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
		contact.Ddd = await _context.ddds.FirstAsync(ddd => ddd.Code == contact.Ddd.Code);
        contact.DddId = contact.Ddd.Id;
		_contacts.Update(contact);
        _ = await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(Contact contact)
    {
        _contacts.Remove(contact); 
        await _context.SaveChangesAsync();
    }

    public async Task<List<Contact>> GetContactsByDdd(int[] codes) =>
        await _contacts
            .Include(c => c.Ddd)
            .Where(c => codes.Contains(c.Ddd.Code))
            .ToListAsync();
}