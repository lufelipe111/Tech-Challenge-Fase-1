using ContactRegister.Application.DTOs;

namespace ContactRegister.Application.Inputs;

public class ContactInput
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public AddressInput Address { get; set; } = default!;
	public string? HomeNumber { get; set; }
	public string? MobileNumber { get; set; }
	public int? Ddd { get; set; }

	public ContactDto ToDto()
	{
		return new ContactDto
		{
			FirstName = FirstName,
			LastName = LastName,
			Email = Email,
			Address = Address.ToDto(),
			HomeNumber = HomeNumber != null ? new PhoneDto { Number = HomeNumber ?? string.Empty } : null,
			MobileNumber = MobileNumber != null ? new PhoneDto { Number = MobileNumber ?? string.Empty } : null,
			Ddd = Ddd != null ? new DddDto { Code = Ddd.Value } : null
		};
	}
}
