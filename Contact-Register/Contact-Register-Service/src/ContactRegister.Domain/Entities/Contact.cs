using ContactRegister.Domain.Entities.Abstractions;
using ContactRegister.Domain.ValueObjects;

namespace ContactRegister.Domain.Entities;

public class Contact : AbstractEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public int DddId { get; set; }
    public Ddd Ddd { get; set; }
    public Phone HomeNumber { get; set; }
    public Phone MobileNumber { get; set; }

    public Contact()
    {
        
    }
    
    public Contact(string firstName,
        string lastName,
        string email,
        Address address,
        Phone homeNumber,
        Phone mobileNumber,
        Ddd ddd)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Address = address;
        HomeNumber = homeNumber;
        MobileNumber = mobileNumber;
        Ddd = ddd;
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
        HomeNumber = homeNumber ?? new Phone("");
        MobileNumber = mobileNumber ?? new Phone("");
        Ddd = ddd;
    }
    public bool Validate(out IList<string> errors)
    {
        var result = true;
        errors = [];    

        if (!ValidateDdd())
        {
			errors.Add($"Invalid {nameof(Ddd)}");
			result = false;
		}

        if (!ValidatePhones())
        {
            errors.Add($"{nameof(HomeNumber)} and {nameof(MobileNumber)} can't not both be null");
            result = false;
        }
        
        if (!ValidateEmail(errors))
            result = false;
        
        return result;
    }

	private bool ValidateDdd()
	{
		return Ddd != null;
	}

	private bool ValidatePhones()
    {
        return !string.IsNullOrEmpty(HomeNumber?.Number?.Trim()) || !string.IsNullOrEmpty(MobileNumber?.Number?.Trim());
    }

    private bool ValidateEmail(IList<string> errors)
    {
        var result = true;
        var mailParts = Email.Split('@');

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