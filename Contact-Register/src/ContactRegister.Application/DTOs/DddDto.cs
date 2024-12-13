using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.DTOs;

public class DddDto
{
    public int Code { get; set; }
    public string Region { get; set; } = string.Empty;

    public Ddd ToDdd()
    {
        return new Ddd()
        {
            Code = Code,
            Region = Region
        };
    }

    public static DddDto FromDdd(Ddd ddd)
    {
        return new DddDto()
        {
            Code = ddd.Code,
            Region = ddd.Region
        };
    }
}