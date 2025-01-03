using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.DTOs;

public class DddDto
{
    public int Code { get; set; }
	public string State { get; set; } = string.Empty;
	public string Region { get; set; } = string.Empty;
}