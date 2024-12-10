namespace ContactRegister.Domain.ValueObjects;

public record Address(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string PostalCode);