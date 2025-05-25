using ContactRegister.Domain.Entities;
using ContactRegister.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ContactRegister.Infrastructure.Persistence.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly DbSet<Contact> _contacts;
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
        _contacts = context.Contacts;
    }
    
    public async Task AddContactAsync(Contact contact)
    {
        contact.Ddd = await _context.Ddds.FirstAsync(ddd => ddd.Code == contact.Ddd.Code);
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
		contact.Ddd = await _context.Ddds.FirstAsync(ddd => ddd.Code == contact.Ddd.Code);
        contact.DddId = contact.Ddd.Id;
		_contacts.Update(contact);
        _ = await _context.SaveChangesAsync();
    }

    public async Task DeleteContactAsync(Contact contact)
    {
        var contactToDelete = await _contacts.SingleAsync(c => c.Id == contact.Id);
        _contacts.Remove(contactToDelete); 
        await _context.SaveChangesAsync();
    }

    public async Task<List<Contact>> GetContactsByDdd(int[] codes) =>
        await _contacts
            .Include(c => c.Ddd)
            .Where(c => codes.Contains(c.Ddd.Code))
            .ToListAsync();
}