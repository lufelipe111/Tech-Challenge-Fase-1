using ContactRegister.Application.DTOs;

namespace ContactRegister.Application.Interfaces.Services;

public interface IDddService
{
	Task<List<DddDto>> GetDdd();
	Task<DddDto?> GetDddByCode(int code);
}