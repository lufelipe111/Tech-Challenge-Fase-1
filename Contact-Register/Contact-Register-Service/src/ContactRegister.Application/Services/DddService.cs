using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Domain.Entities;
using ContactRegister.Shared.Interfaces.Repositories;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace ContactRegister.Application.Services;

public class DddService : IDddService
{
	private readonly ILogger<DddService> _logger;
	private readonly IDddRepository _dddRepository;
    private readonly IDddApiService _dddApiService;

	public DddService(ILogger<DddService> logger, IDddRepository dddRepository, IDddApiService dddApiService)
	{
		_logger = logger;
		_dddRepository = dddRepository;
		_dddApiService = dddApiService;
	}

	public async Task<ErrorOr<List<DddDto>>> GetDdd()
	{
		try
		{
			var ddds = await _dddRepository.GetDdds();

			return ddds.Select(x => DddDto.FromEntity(x)).ToList();
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

				if (dddApiResponseDto.Result != null)
				{
					ddd = new Ddd(code, dddApiResponseDto.Result.State, string.Join(", ", dddApiResponseDto.Result.Cities));

					if (!ddd.Validate(out var errors))
					{
						return errors
							.Select(e => Error.Failure("Ddd.Validation", e))
							.ToList();
					}

					_ = await _dddRepository.AddDdd(ddd);
				}

				if (dddApiResponseDto.Error != null)
					return new List<Error> { Error.Failure("Ddd.ExternalApi", dddApiResponseDto.Error.Message) };
			}

			return DddDto.FromEntity(ddd);
		}
		catch (Exception e)
		{
			_logger.LogError(e, e.Message);
			return Error.Failure("Ddd.Get.Exception", e.Message);
		}
    }
}