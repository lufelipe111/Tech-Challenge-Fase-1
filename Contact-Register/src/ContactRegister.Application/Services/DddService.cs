using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace ContactRegister.Application.Services;

public class DddService : IDddService
{
    private readonly ILogger<DddService> _logger;
    private readonly IDddRepository _dddRepository;
    
    public DddService(ILogger<DddService> logger, IDddRepository dddRepository)
    {
        _logger = logger;
        _dddRepository = dddRepository;
    }
    
    public async Task AddDdd(DddDto dddDto)
    {
        var ddd = dddDto.ToDdd();

        await _dddRepository.AddDdd(ddd);
    }

    public async Task<DddDto?> GetDddById(int id)
    {
        var ddd = await _dddRepository.GetDddsById(id);

        return ddd == null ? null : DddDto.FromDdd(ddd);
    }
}