using ContactRegister.Application.DTOs.BrasilApiDTOs;

namespace ContactRegister.Application.Interfaces.Services;

public interface IDddApiService
{
	Task<DddApiResponseDto?> GetByCode(int code);
	void SetRoute(string route);
}
