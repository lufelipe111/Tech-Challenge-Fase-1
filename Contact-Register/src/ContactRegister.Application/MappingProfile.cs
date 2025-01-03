using AutoMapper;
using ContactRegister.Application.DTOs;
using ContactRegister.Domain.Entities;

namespace ContactRegister.Application;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Ddd, DddDto>().ReverseMap();
	}
}
