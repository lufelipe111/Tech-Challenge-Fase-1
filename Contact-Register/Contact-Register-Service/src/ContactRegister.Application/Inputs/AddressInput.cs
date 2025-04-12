using ContactRegister.Application.DTOs;

namespace ContactRegister.Application.Inputs;

public class AddressInput
{
	public string AddressLine1 { get; set; } = string.Empty;
	public string AddressLine2 { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string State { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;

	public AddressDto ToDto()
	{
		return new AddressDto
		{
			AddressLine1 = AddressLine1,
			AddressLine2 = AddressLine2,
			City = City,
			State = State,
			PostalCode = PostalCode
		};
	}
}
