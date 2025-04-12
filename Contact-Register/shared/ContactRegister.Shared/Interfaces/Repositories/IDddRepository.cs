using ContactRegister.Domain.Entities;

namespace ContactRegister.Shared.Interfaces.Repositories;

public interface IDddRepository
{
    Task<int> AddDdd(Ddd ddd);
    Task<List<Ddd>> GetDdds();
    Task<Ddd?> GetDddByCode(int code);
}
