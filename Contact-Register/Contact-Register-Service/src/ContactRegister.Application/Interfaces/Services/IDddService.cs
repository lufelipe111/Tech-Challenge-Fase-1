using ContactRegister.Application.DTOs;
using ErrorOr;

namespace ContactRegister.Application.Interfaces.Services;

public interface IDddService
{
	Task<ErrorOr<List<DddDto>>> GetDdd();
	Task<ErrorOr<DddDto?>> GetDddByCode(int code);
}