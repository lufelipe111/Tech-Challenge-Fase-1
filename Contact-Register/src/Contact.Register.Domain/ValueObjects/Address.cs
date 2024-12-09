namespace Contact.Register.Domain.ValueObjects;

public class Address(
    string addressLine1,
    string addressLine2,
    string city,
    string state,
    string postalCode)
{
    public string AddressLine1 { get; set; } = addressLine1;
    public string AddressLine2 { get; set; } = addressLine2;
    public string City { get; set; } = city;
    public string State { get; set; } = state;
    public string PostalCode { get; set; } = postalCode;
}