using ContactRegister.Domain.ValueObjects;

namespace ContactRegister.Application.DTOs;

public class PhoneDto
{
    public int Ddd { get; set; } = 0;
    public int Number { get; set; } = 0;

    public Phone ToPhone()
    {
        return new Phone(Number);
    }
}