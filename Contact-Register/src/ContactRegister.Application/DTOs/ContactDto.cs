using ContactRegister.Domain.Entities;

namespace ContactRegister.Application.DTOs;

public class ContactDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = default!;
    public PhoneDto? HomeNumber { get; set; } = null;
    public PhoneDto? MobileNumber { get; set; } = null;
    public DddDto? Ddd { get; set; } = null;

    public Contact ToContact()
    {
        return new Contact(
            FirstName, 
            LastName,
            Email,
            Address.ToAddress(),
            HomeNumber?.ToPhone(),
            MobileNumber?.ToPhone(),
            Ddd?.ToDdd());
    }

    public static ContactDto FromEntity(Contact contactEntity)
    {
        var homeNumber = contactEntity.HomeNumber != null 
            ? new PhoneDto { Number = contactEntity.HomeNumber.Number }
            : null;
            
        var mobileNumber = contactEntity.MobileNumber != null 
            ? new PhoneDto { Number = contactEntity.MobileNumber.Number }
            : null;

        var ddd = contactEntity.Ddd != null
            ? new DddDto { Code = contactEntity.Ddd.Code, State = contactEntity.Ddd.State, Region = contactEntity.Ddd.Region }
            : null;

        var dto = new ContactDto
        {
            Address = new AddressDto
            {
                City = contactEntity.Address.City,
                State = contactEntity.Address.State,
                AddressLine1 = contactEntity.Address.AddressLine1,
                AddressLine2 = contactEntity.Address.AddressLine2,
                PostalCode = contactEntity.Address.PostalCode
            },
            HomeNumber = homeNumber,
            MobileNumber = mobileNumber,
            Email = contactEntity.Email,
            LastName = contactEntity.LastName,
            FirstName = contactEntity.FirstName,
            Ddd = ddd,
        };
        
        return dto;
    }
}