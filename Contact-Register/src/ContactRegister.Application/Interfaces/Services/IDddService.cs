using ContactRegister.Application.DTOs;

namespace ContactRegister.Application.Interfaces.Services;

public interface IDddService
{
    Task AddDdd(DddDto dddDto);
    Task<DddDto?> GetDddById(int id);
}