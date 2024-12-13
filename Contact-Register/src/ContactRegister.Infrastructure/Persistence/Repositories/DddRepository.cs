using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactRegister.Infrastructure.Persistence.Repositories;

public class DddRepository : IDddRepository
{
    private readonly DbSet<Ddd> _ddds;
    private readonly AppDbContext _context;

    public DddRepository(AppDbContext context)
    {
        _context = context;
        _ddds = context.ddds;
    }
    
    public async Task AddDdd(Ddd ddd)
    {
        try
        {
            _ddds.Add(ddd);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task UpdateDdd(Ddd ddd)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDdd(Ddd ddd)
    {
        throw new NotImplementedException();
    }

    public Task<List<Ddd>> GetDdds()
    {
        throw new NotImplementedException();
    }

    public async Task<Ddd?> GetDddsById(int id)
    {
        var e = await _ddds.FindAsync(id);
        return e;
    }
}