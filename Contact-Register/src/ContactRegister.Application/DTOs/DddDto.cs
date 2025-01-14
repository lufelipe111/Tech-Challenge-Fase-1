using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.DTOs;

public class DddDto
{
    public int Code { get; set; }
	public string State { get; set; } = string.Empty;
	public string Region { get; set; } = string.Empty;

    public Ddd ToDdd()
    {
        return new Ddd(Code, State, Region);
    }

    public static DddDto FromEntity(Ddd entity)
    {
        return new DddDto
        {
            Code = entity.Code,
            State = entity.State,
            Region = entity.Region
        };
    }
}