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
    public Ddd Ddd { get; set; } = default!;
    public Phone HomeNumber { get; set; }
    public Phone MobileNumber { get; set; }

    public Contact() { }

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
        HomeNumber = homeNumber;
        MobileNumber = mobileNumber;
        Ddd = ddd;
    }
    public bool Validate(out IList<string> errors)
    {
        bool result = true;
        errors = [];    

        if (!ValidateDdd(Ddd))
        {
			errors.Add($"Invalid {nameof(Ddd)}");
			result = false;
		}

        if (!ValidatePhones(HomeNumber, MobileNumber))
        {
            errors.Add($"{nameof(HomeNumber)} and {nameof(MobileNumber)} can't not both be null");
            result = false;
        }
        
        if (!ValidateEmail(errors))
            result = false;
        
        return result;
    }

	private bool ValidateDdd(Ddd? ddd)
	{
		return ddd != null;
	}

	private bool ValidatePhones(Phone? home, Phone? mobile)
    {
        return home != null || mobile != null;
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