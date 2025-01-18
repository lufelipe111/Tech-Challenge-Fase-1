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

	public async Task<int> AddDdd(Ddd ddd)
	{
		_ = await _ddds.AddAsync(ddd);
		return await _context.SaveChangesAsync();
	}

	public async Task<List<Ddd>> GetDdds()
    {
        return await _ddds.ToListAsync();
    }

    public async Task<Ddd?> GetDddByCode(int code)
    {
        var ddd = await _ddds.FirstOrDefaultAsync(ddd => ddd.Code == code);
        return ddd;
    }
}