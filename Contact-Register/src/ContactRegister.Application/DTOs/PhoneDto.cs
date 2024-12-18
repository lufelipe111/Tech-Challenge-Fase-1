using ContactRegister.Domain.ValueObjects;

namespace ContactRegister.Application.DTOs;

public class PhoneDto
{
    public int Ddd { get; set; } = 0;
    public string Number { get; set; } = string.Empty;

    public Phone ToPhone()
    {
        return new Phone(Number);
    }
}