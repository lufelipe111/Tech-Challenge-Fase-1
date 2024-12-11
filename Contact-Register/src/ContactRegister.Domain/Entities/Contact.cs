using ContactRegister.Domain.Entities.Abstractions;
using ContactRegister.Domain.ValueObjects;

namespace ContactRegister.Domain.Entities;

public class Contact : AbstractEntity<ulong>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public Ddd DddCode { get; set; } = default!;
    public Phone HomeNumber { get; set; }
    public Phone MobileNumber { get; set; }

    public Contact() { }

    public Contact(string firstName,
        string lastName,
        string email,
        Address address,
        Phone homeNumber,
        Phone mobileNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
        HomeNumber = homeNumber;
        MobileNumber = mobileNumber;
    }

    public void Update(
        string firstName,
        string lastName,
        string email,
        Ddd ddd,
        Address address,
        Phone? homeNumber,
        Phone? mobileNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
        HomeNumber = homeNumber;
        MobileNumber = mobileNumber;
        DddCode = ddd;
    }
    public bool Validate(out IList<string> errors)
    {
        bool result = true;
        errors = [];    

        if (ValidatePhones(HomeNumber, MobileNumber))
        {
            errors.Add($"{nameof(HomeNumber)} and {nameof(MobileNumber)} can't not both be null");
            result = false;
        }
        
        if (!ValidateEmail(errors))
            result = false;
        
        return result;
    }

    private bool ValidatePhones(Phone? home, Phone? mobile)
    {
        return home == null && mobile == null;
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