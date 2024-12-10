using ContactRegister.Domain.ValueObjects;

namespace ContactRegister.Application.DTOs;

public class AddressDto
{
    public string AddressLine1 { get; set; } = string.Empty;
    public string AddressLine2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public Address ToAddress()
    {
        return new Address(AddressLine1, AddressLine2, City, State, PostalCode);
    }
}