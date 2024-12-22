using AutoMapper;
using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.Services;

public class DddService : IDddService
{
    private readonly IDddRepository _dddRepository;
    private readonly IMapper _mapper;
    private readonly IDddApiService _dddApiService;

	public DddService(IDddRepository dddRepository, IMapper mapper, IDddApiService dddApiService)
	{
		_dddRepository = dddRepository;
		_mapper = mapper;
		_dddApiService = dddApiService;
	}

	public async Task<List<DddDto>> GetDdd()
	{
		var ddds = await _dddRepository.GetDdds();

		return _mapper.Map<List<DddDto>>(ddds);
	}

	public async Task<DddDto?> GetDddByCode(int code)
    {
        var ddd = await _dddRepository.GetDddByCode(code);

		if (ddd == null)
		{
			var dddApiResponseDto = await _dddApiService.GetByCode(code);

			if (dddApiResponseDto.Result != null) ddd = await AddDdd(code, dddApiResponseDto.Result.State, string.Join(", ", dddApiResponseDto.Result.Cities));
		}

        return _mapper.Map<DddDto?>(ddd);
    }

	private async Task<Ddd> AddDdd(int code, string state, string region)
	{
		var ddd = new Ddd(code, state, region);

		_ = await _dddRepository.AddDdd(ddd);

		return ddd;
	}
}