using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.Interfaces.Repositories;

public interface IDddRepository
{
    Task AddDdd(Ddd ddd);
    Task UpdateDdd(Ddd ddd);
    Task DeleteDdd(Ddd ddd);
    Task<List<Ddd>> GetDdds();
    Task<Ddd?> GetDddsById(int id);
}