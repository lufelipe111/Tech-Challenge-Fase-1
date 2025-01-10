using AutoMapper;
using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Domain.Entities;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace ContactRegister.Application.Services;

public class DddService : IDddService
{
	private readonly ILogger<DddService> _logger;
	private readonly IDddRepository _dddRepository;
    private readonly IMapper _mapper;
    private readonly IDddApiService _dddApiService;

	public DddService(ILogger<DddService> logger, IDddRepository dddRepository, IMapper mapper, IDddApiService dddApiService)
	{
		_logger = logger;
		_dddRepository = dddRepository;
		_mapper = mapper;
		_dddApiService = dddApiService;
	}

	public async Task<ErrorOr<List<DddDto>>> GetDdd()
	{
		try
		{
			var ddds = await _dddRepository.GetDdds();

			return _mapper.Map<List<DddDto>>(ddds);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);
			return Error.Failure("Ddd.Get.Exception", e.Message);
		}
	}

	public async Task<ErrorOr<DddDto?>> GetDddByCode(int code)
    {
		try
		{
			var ddd = await _dddRepository.GetDddByCode(code);

			if (ddd == null)
			{
				var dddApiResponseDto = await _dddApiService.GetByCode(code);

				if (dddApiResponseDto.Result != null) ddd = await AddDdd(code, dddApiResponseDto.Result.State, string.Join(", ", dddApiResponseDto.Result.Cities));

				if (dddApiResponseDto.Error != null)
					return new List<Error> { Error.Failure("Ddd.Validation", dddApiResponseDto.Error.Message) };
			}

			if (!ddd!.Validate(out var errors))
			{
				return errors
					.Select(e => Error.Failure("Ddd.Validation", e))
					.ToList();
			}

			return _mapper.Map<DddDto?>(ddd);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);
			return Error.Failure("Ddd.Get.Exception", e.Message);
		}
    }

	private async Task<Ddd> AddDdd(int code, string state, string region)
	{
		var ddd = new Ddd(code, state, region);

		_ = await _dddRepository.AddDdd(ddd);

		return ddd;
	}
}