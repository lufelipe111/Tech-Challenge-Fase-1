using ContactRegister.Application.DTOs;
using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Application.Services;
using ContactRegister.Domain.Entities;
using ContactRegister.Domain.ValueObjects;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ContactRegister.Tests.UnitTests.ApplicationTests;

public class ContactServiceTests
{
	private readonly Mock<ILogger<ContactService>> _loggerMock = new();
	private readonly Mock<IContactRepository> _contactRepositoryMock = new();
	private readonly Mock<IDddService> _dddServiceMock = new();
	private readonly Mock<ICacheService> _cacheServiceMock = new();
	private readonly ContactService _contactService;
	private readonly ContactDto _contactDto;
	private readonly DddDto _dddDto;
	private readonly Contact _contact;
	private readonly List<Contact> _allContacts;

	public ContactServiceTests()
	{
		_contactService = new(_loggerMock.Object, _contactRepositoryMock.Object, _dddServiceMock.Object, _cacheServiceMock.Object);
		_contactDto = new ContactDto
		{
			Id = 0,
			FirstName = "Silvana",
			LastName = "Andreia Lavínia Souza",
			Email = "silvanaandreiasouza@cbb.com.br",
			Address = new AddressDto
			{
				AddressLine1 = "5ª Travessa da Batalha nº 330, Jordão",
				AddressLine2 = "",
				City = "Recife",
				State = "PE",
				PostalCode = "51260-215"
			},
			HomeNumber = new PhoneDto
			{
				Number = "(81) 2644-3282"
			},
			MobileNumber = new PhoneDto
			{
				Number = "(81) 99682-5038"
			},
			Ddd = new DddDto
			{
				Code = 21
			}
		};
		_dddDto = new DddDto
		{
			Code = 21,
			State = "RJ",
			Region = "TERESÓPOLIS, TANGUÁ,SEROPÉDICA, SÃO JOÃO DE MERITI, SÃO GONÇALO, RIO DE JANEIRO, RIO BONITO, QUEIMADOS, PARACAMBI, NOVA IGUAÇU, NITERÓI, NILÓPOLIS, MESQUITA, MARICÁ, MANGARATIBA, MAGÉ, JAPERI, ITAGUAÍ, ITABORAÍ, GUAPIMIRIM, DUQUE DE CAXIAS, CACHOEIRAS DE MACACU, BELFORD ROXO"
		};
		_contact = new Contact
		{
			Id = 1,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow,
			FirstName = "Silvana",
			LastName = "Andreia Lavínia Souza",
			Email = "silvanaandreiasouza@cbb.com.br",
			Address = new Address("5ª Travessa da Batalha nº 330, Jordão", "", "Recife", "PE", "51260-215"),
			HomeNumber = new Phone("(81) 2644-3282"),
			MobileNumber = new Phone("(81) 99682-5038"),
			DddId = 2,
			Ddd = new Ddd
			{
				Id = 2,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Code = 21,
				State = "RJ",
				Region = "TERESÓPOLIS, TANGUÁ,SEROPÉDICA, SÃO JOÃO DE MERITI, SÃO GONÇALO, RIO DE JANEIRO, RIO BONITO, QUEIMADOS, PARACAMBI, NOVA IGUAÇU, NITERÓI, NILÓPOLIS, MESQUITA, MARICÁ, MANGARATIBA, MAGÉ, JAPERI, ITAGUAÍ, ITABORAÍ, GUAPIMIRIM, DUQUE DE CAXIAS, CACHOEIRAS DE MACACU, BELFORD ROXO"
			}
		};
		_allContacts = new List<Contact>
		{
			_contact,
			new Contact
			{
				Id = 2,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				FirstName = "Luan",
				LastName = "Roberto Souza",
				Email = "luan.roberto.souza@ematelecom.com.br",
				Address = new Address("Rua da Prosa nº 109, Itapuã", "", "Salvador", "BA", "41630-285"),
				HomeNumber = new Phone("(71) 2539-7864"),
				MobileNumber = new Phone("(71) 98538-2740"),
				DddId = 34,
				Ddd = new Ddd
				{
					Id = 34,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow,
					Code = 68,
					State = "AC",
					Region = "PORTO ACRE, XAPURI, TARAUACÁ, SENA MADUREIRA, SENADOR GUIOMARD, SANTA ROSA DO PURUS, RODRIGUES ALVES, RIO BRANCO, PORTO WALTER, PLÁCIDO DE CASTRO, MARECHAL THAUMATURGO, MANOEL URBANO, MÂNCIO LIMA, JORDÃO, FEIJÓ, EPITACIOLÂNDIA, CRUZEIRO DO SUL, CAPIXABA, BUJARI, BRASILÉIA, ASSIS BRASIL, ACRELÂNDIA"
				}
			}
		};
	}

	[Fact]
	public async Task AddContact_ShouldReturnSuccess_WhenContactSaved()
	{
		//Arrange
		_dddServiceMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(_dddDto);
		_contactRepositoryMock.Setup(x => x.AddContactAsync(It.IsAny<Contact>())).Verifiable();

		//Act
		var result = await _contactService.AddContactAsync(_contactDto);

		//Assert
		Assert.Equal(Result.Success, result);
		_contactRepositoryMock.Verify(x => x.AddContactAsync(It.IsAny<Contact>()), Times.Once());
	}

	[Fact]
	public async Task AddContact_ShouldReturnError_WhenDddServiceReturnsError()
	{
		//Arrange
		var expectedResult = ErrorOr<DddDto?>.From(new List<Error>
		{
			Error.Failure("Ddd.Get.Exception", "Exception throws when calling service.")
		});
		_dddServiceMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(expectedResult);

		//Act
		var actualResult = await _contactService.AddContactAsync(_contactDto);

		//Assert
		Assert.True(actualResult.IsError, "Expected error not returned when DDD Service returns error");
		Assert.Single(actualResult.Errors);
		Assert.Equal(expectedResult.FirstError.Code, actualResult.FirstError.Code);
		Assert.Equal(expectedResult.FirstError.Description, actualResult.FirstError.Description);
	}

	[Fact]
	public async Task AddContact_ShouldReturnError_WhenValidationFails()
	{
		//Arrange
		DddDto? dddDto = null;
		_dddServiceMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(dddDto);

		//Act
		var result = await _contactService.AddContactAsync(_contactDto);

		//Assert
		Assert.True(result.IsError, "Expected errors not returned when validation fails");
		Assert.DoesNotContain(result.Errors, x => x.Code != "Contact.Validation");
	}

	[Fact]
	public async Task AddContact_ShouldReturnError_WhenExceptionThrows()
	{
		//Arrange
		var expectedError = "Exception throws when calling service.";
		_dddServiceMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ThrowsAsync(new Exception(expectedError));

		//Act
		var result = await _contactService.AddContactAsync(_contactDto);

		//Assert
		Assert.True(result.IsError, "Expected error not returned when exception throws");
		Assert.Single(result.Errors);
		Assert.Equal("Contact.Add.Exception", result.FirstError.Code);
		Assert.Equal(expectedError, result.FirstError.Description);
	}

	[Fact]
	public async Task GetContactById_ShouldReturnDto_WhenFound()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(_contact);

		//Act
		var result = await _contactService.GetContactByIdAsync(It.IsAny<int>());

		//Assert
		Assert.NotNull(result.Value);
		Assert.False(result.IsError, "Unexpected error returned");
		Assert.Equal(_contact.Id, result.Value.Id);
	}

	[Fact]
	public async Task GetContactById_ShouldReturnNull_WhenNotFound()
	{
		//Arrange
		Contact? contact = null;
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(contact);

		//Act
		var result = await _contactService.GetContactByIdAsync(It.IsAny<int>());

		//Assert
		Assert.Null(result.Value);
		Assert.False(result.IsError, "Unexpected error returned");
	}

	[Fact]
	public async Task GetContactById_ShouldReturnError_WhenExceptionThrows()
	{
		//Arrange
		var expectedError = "Exception throws when calling repository.";
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception(expectedError));

		//Act
		var result = await _contactService.GetContactByIdAsync(It.IsAny<int>());

		//Assert
		Assert.True(result.IsError, "Expected error not returned when exception throws");
		Assert.Single(result.Errors);
		Assert.Equal("Contact.Get.Exception", result.FirstError.Code);
		Assert.Equal(expectedError, result.FirstError.Description);
	}

	[Fact]
	public async Task GetContacts_ShouldReturnList_WhenOneFieldMatch()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactsAsync()).ReturnsAsync(_allContacts);

		//Act
		var result = await _contactService.GetContactsAsync(0, lastName: "souza");

		//Assert
		Assert.NotNull(result.Value);
		Assert.False(result.IsError, "Unexpected error returned");
		Assert.NotEmpty(result.Value);
		Assert.Equal(_allContacts.Count, result.Value.Count());
		Assert.Contains(result.Value, x => x.Id == _allContacts[0].Id);
		Assert.Contains(result.Value, x => x.Id == _allContacts[1].Id);
	}

	[Fact]
	public async Task GetContacts_ShouldReturnList_WhenTwoFieldsMatch()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactsAsync()).ReturnsAsync(new List<Contact> { _allContacts[0] });

		//Act
		var result = await _contactService.GetContactsAsync(0, firstName: _allContacts[0].FirstName, lastName: _allContacts[0].LastName);

		//Assert
		Assert.NotNull(result.Value);
		Assert.False(result.IsError, "Unexpected error returned");
		Assert.NotEmpty(result.Value);
		Assert.Single(result.Value);
		Assert.Contains(result.Value, x => x.Id == _allContacts[0].Id);
	}

	[Fact]
	public async Task GetContacts_ShouldReturnEmptyList_WhenThereIsNoMatch()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactsAsync()).ReturnsAsync(new List<Contact>());

		//Act
		var result = await _contactService.GetContactsAsync(0, firstName: _allContacts[0].FirstName, lastName: _allContacts[1].LastName);

		//Assert
		Assert.NotNull(result.Value);
		Assert.False(result.IsError, "Unexpected error returned");
		Assert.Empty(result.Value);
	}

	[Fact]
	public async Task GetContacts_ShouldReturnEmptyList_WhenAtLeastOneFieldNotMatch()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactsAsync()).ReturnsAsync(new List<Contact>());
		var contactTest = _allContacts[0];

		//Act
		var result = await _contactService.GetContactsAsync(contactTest.Ddd.Code, contactTest.FirstName, contactTest.LastName, contactTest.Email, contactTest.Address.City, contactTest.Address.State, contactTest.Address.PostalCode, contactTest.Address.AddressLine1, contactTest.Address.AddressLine2, contactTest.HomeNumber.Number, _allContacts[1].MobileNumber.Number);

		//Assert
		Assert.NotNull(result.Value);
		Assert.False(result.IsError, "Unexpected error returned");
		Assert.Empty(result.Value);
	}

	[Fact]
	public async Task GetContacts_ShouldReturnError_WhenExceptionThrows()
	{
		//Arrange
		var expectedError = "Exception throws when calling repository.";
		_contactRepositoryMock.Setup(x => x.GetContactsAsync()).ThrowsAsync(new Exception(expectedError));

		//Act
		var result = await _contactService.GetContactsAsync(0);

		//Assert
		Assert.True(result.IsError, "Expected error not returned when exception throws");
		Assert.Single(result.Errors);
		Assert.Equal("Contact.Get.Exception", result.FirstError.Code);
		Assert.Equal(expectedError, result.FirstError.Description);
	}

	[Fact]
	public async Task UpdateContact_ShouldReturnSuccess_WhenContactUpdated()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(_allContacts[1]);
		_dddServiceMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(_dddDto);
		_contactRepositoryMock.Setup(x => x.UpdateContactAsync(It.IsAny<Contact>())).Verifiable();

		//Act
		var result = await _contactService.UpdateContactAsync(It.IsAny<int>(), _contactDto);

		//Assert
		Assert.Equal(Result.Success, result);
		_contactRepositoryMock.Verify(x => x.UpdateContactAsync(It.IsAny<Contact>()), Times.Once());
	}

	[Fact]
	public async Task UpdateContact_ShouldReturnError_WhenContactNotFound()
	{
		//Arrange
		Contact? contact = null;
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(contact);
		var id = 1;

		//Act
		var result = await _contactService.UpdateContactAsync(id, _contactDto);

		//Assert
		Assert.True(result.IsError, "Expected error not returned when contact not found");
		Assert.Single(result.Errors);
		Assert.Equal("Contact.NotFound", result.FirstError.Code);
		Assert.Equal($"Contact {id} not found", result.FirstError.Description);
	}

	[Fact]
	public async Task UpdateContact_ShouldReturnError_WhenDddServiceReturnsError()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(_contact);
		var expectedResult = ErrorOr<DddDto?>.From(new List<Error>
		{
			Error.Failure("Ddd.Get.Exception", "Exception throws when calling service.")
		});
		_dddServiceMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(expectedResult);

		//Act
		var actualResult = await _contactService.UpdateContactAsync(It.IsAny<int>(), _contactDto);

		//Assert
		Assert.True(actualResult.IsError, "Expected error not returned when DDD Service returns error");
		Assert.Single(actualResult.Errors);
		Assert.Equal(expectedResult.FirstError.Code, actualResult.FirstError.Code);
		Assert.Equal(expectedResult.FirstError.Description, actualResult.FirstError.Description);
	}

	[Fact]
	public async Task UpdateContact_ShouldReturnError_WhenValidationFails()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(_contact);
		_dddServiceMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(_dddDto);
		_contactDto.Email = _contactDto.Email.Replace("@", string.Empty);

		//Act
		var result = await _contactService.UpdateContactAsync(It.IsAny<int>(), _contactDto);

		//Assert
		Assert.True(result.IsError, "Expected errors not returned when validation fails");
		Assert.DoesNotContain(result.Errors, x => x.Code != "Contact.Validation");
	}

	[Fact]
	public async Task UpdateContact_ShouldReturnError_WhenExceptionThrows()
	{
		//Arrange
		var expectedError = "Exception throws when calling repository.";
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception(expectedError));

		//Act
		var result = await _contactService.UpdateContactAsync(It.IsAny<int>(), _contactDto);

		//Assert
		Assert.True(result.IsError, "Expected error not returned when exception throws");
		Assert.Single(result.Errors);
		Assert.Equal("Contact.Update.Exception", result.FirstError.Code);
		Assert.Equal(expectedError, result.FirstError.Description);
	}

	[Fact]
	public async Task DeleteContact_ShouldReturnSuccess_WhenContactDeleted()
	{
		//Arrange
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(_contact);
		_contactRepositoryMock.Setup(x => x.DeleteContactAsync(It.IsAny<Contact>())).Verifiable();

		//Act
		var result = await _contactService.DeleteContactAsync(It.IsAny<int>());

		//Assert
		Assert.Equal(Result.Success, result);
		_contactRepositoryMock.Verify(x => x.DeleteContactAsync(It.IsAny<Contact>()), Times.Once());
	}

	[Fact]
	public async Task DeleteContact_ShouldReturnError_WhenContactNotFound()
	{
		//Arrange
		Contact? contact = null;
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ReturnsAsync(contact);
		var id = 1;

		//Act
		var result = await _contactService.DeleteContactAsync(id);

		//Assert
		Assert.True(result.IsError, "Expected error not returned when contact not found");
		Assert.Single(result.Errors);
		Assert.Equal("Contact.NotFound", result.FirstError.Code);
		Assert.Equal($"Contact {id} not found", result.FirstError.Description);
	}

	[Fact]
	public async Task DeleteContact_ShouldReturnError_WhenExceptionThrows()
	{
		//Arrange
		var expectedError = "Exception throws when calling repository.";
		_contactRepositoryMock.Setup(x => x.GetContactByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception(expectedError));

		//Act
		var result = await _contactService.DeleteContactAsync(It.IsAny<int>());

		//Assert
		Assert.True(result.IsError, "Expected error not returned when exception throws");
		Assert.Single(result.Errors);
		Assert.Equal("Contact.Delete.Exception", result.FirstError.Code);
		Assert.Equal(expectedError, result.FirstError.Description);
	}
}