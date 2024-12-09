using Contact.Register.Domain.Entities.Abstractions;
using Contact.Register.Domain.ValueObjects;

namespace Contact.Register.Domain.Entities;

public class Contact(
    string firstName,
    string lastName,
    string email,
    Address address,
    Phone? homeNumber,
    Phone? mobileNumber)
    : AbstractEntity<ulong>
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public Address Address { get; set; } = address;
    public Phone? HomeNumber { get; init; } = homeNumber;
    public Phone? MobileNumber { get; init; } = mobileNumber;

    public bool Validate(out IList<string> errors)
    {
        bool result = true;
        errors = [];    

        if (HomeNumber == null && MobileNumber == null)
        {
            errors.Add($"{nameof(HomeNumber)} and {nameof(MobileNumber)} can't not both be null");
            result = false;
        }
        
        if (!ValidateEmail(errors))
            result = false;
        
        return result;
    }

    private bool ValidateEmail(IList<string> errors)
    {
        bool result = true;
        string[] mailParts = Email.Split('@');

        if (mailParts.Length != 2)
        {
            errors.Add($"Invalid email format");
            result = false;
        }

        if (int.TryParse(mailParts[0][0].ToString(), out _))
        {
            errors.Add($"Email can't begin with number");
            result = false;
        }

        if (mailParts.Length >= 2 && long.TryParse(mailParts[1], out _))
        {
            errors.Add($"Email host can't be numeric");
            result = false;
        }
        
        return result;
    }
}