using ContactRegister.Domain.ValueObjects;
using Xunit;

namespace ContactRegister.Tests.UnitTests.DomainTests;

public class ContactTests
{
    [Fact]
    public void Validate_ShouldReturnTrue_WhenAllDataIsValid()
    {
        // Arrange
        var address = new Address(
            "Rua teste, 123", 
            "Predio A, Apartamento 42", 
            "São Paulo", 
            "SP", 
            "012345-678");
        var homePhone = new Phone("11111111");
        var mobilePhone = new Phone("922222222");
        IEnumerable<Phone> additionalPhones = [];

        var contact = new Domain.Entities.Contact(
            firstName: "John",
            lastName: "Doe",
            email: "john.doe@example.com",
            address: address,
            homeNumber: homePhone,
            mobileNumber: mobilePhone
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.True(result, "Expected validation to pass");
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_ShouldPass_WhenOnlyHomeNumberIsNull()
    {
        // Arrange
        var address = new Address(
            "Rua teste, 123", 
            "Predio A, Apartamento 42", 
            "São Paulo", 
            "SP", 
            "012345-678");
        
        var homePhone = new Phone("11111111");
        
        var contact = new Domain.Entities.Contact(
            firstName: "Jane",
            lastName: "Doe",
            email: "jane.doe@example.com",
            address: address,
            homeNumber: homePhone,
            mobileNumber: null
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.True(result, "Expected validation to fail due to missing HomeNumber");
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_ShouldPass_WhenOnlyMobileNumberIsNull()
    {
        // Arrange
        var address = new Address(
            "Rua teste, 123", 
            "Predio A, Apartamento 42", 
            "São Paulo", 
            "SP", 
            "012345-678");
        
        var mobilePhone = new Phone("911111111");
        
        var contact = new Domain.Entities.Contact(
            firstName: "Jane",
            lastName: "Doe",
            email: "jane.doe@example.com",
            address: address,
            homeNumber: mobilePhone,
            mobileNumber: null
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.True(result, "Expected validation to fail due to missing MobileNumber");
        Assert.Empty(errors);
    }
    
    [Fact]
    public void Validate_ShouldFail_WhenBothNumbersAreNull()
    {
        // Arrange
        var address = new Address(
            "Rua teste, 123",
            "Predio A, Apartamento 42",
            "São Paulo",
            "SP",
            "012345-678");
        
        var contact = new Domain.Entities.Contact(
            firstName: "Jane",
            lastName: "Doe",
            email: "jane.doe@example.com",
            address: address,
            homeNumber: null,
            mobileNumber: null
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.False(result, "Expected validation to fail due to missing MobileNumber");
        Assert.Contains($"{nameof(contact.HomeNumber)} and {nameof(contact.MobileNumber)} can't not both be null", errors);
    }

    [Fact]
    public void Validate_ShouldFail_WhenEmailHasNoAtSymbol()
    {
        // Arrange
        var address = new Address(
            "Rua teste, 123", 
            "Predio A, Apartamento 42", 
            "São Paulo", 
            "SP", 
            "012345-678");
        
        var homePhone = new Phone("11111111");
        var mobilePhone = new Phone("911111111");
        
        var contact = new Domain.Entities.Contact(
            firstName: "John",
            lastName: "Doe",
            email: "johndoeexample.com", // No '@'
            address: address,
            homeNumber: homePhone,
            mobileNumber: mobilePhone
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.False(result, "Expected validation to fail due to invalid email");
        Assert.Contains("Invalid email format", errors);
    }

    [Fact]
    public void Validate_ShouldFail_WhenEmailBeginsWithNumber()
    {
        // Arrange
        var address = new Address(
            "Rua teste, 123", 
            "Predio A, Apartamento 42", 
            "São Paulo", 
            "SP", 
            "012345-678");
        
        var homePhone = new Phone("11111111");
        var mobilePhone = new Phone("911111111");
        
        var contact = new Domain.Entities.Contact(
            firstName: "John",
            lastName: "Doe",
            email: "1john@example.com",
            address: address,
            homeNumber: homePhone,
            mobileNumber: mobilePhone
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.False(result, "Expected validation to fail due to email starting with a number");
        Assert.Contains("Email can't begin with number", errors);
    }

    [Fact]
    public void Validate_ShouldFail_WhenEmailDomainIsNumeric()
    {
        // Arrange
        var address = new Address(
            "Rua teste, 123", 
            "Predio A, Apartamento 42", 
            "São Paulo", 
            "SP", 
            "012345-678");
        
        var homePhone = new Phone("11111111");
        var mobilePhone = new Phone("911111111");
        
        var contact = new Domain.Entities.Contact(
            firstName: "John",
            lastName: "Doe",
            email: "john.doe@123456",
            address: address,
            homeNumber: homePhone,
            mobileNumber: mobilePhone
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.False(result, "Expected validation to fail due to numeric domain");
        Assert.Contains("Email host can't be numeric", errors);
    }

    [Fact]
    public void Validate_ShouldAccumulateMultipleErrors()
    {
        // Arrange
        // Missing MobileNumber and invalid email
        var address = new Address(
            "Rua teste, 123", 
            "Predio A, Apartamento 42", 
            "São Paulo", 
            "SP", 
            "012345-678");
        
        var contact = new Domain.Entities.Contact(
            firstName: "John",
            lastName: "Doe",
            email: "2john@12345",
            address: address,
            homeNumber: null,
            mobileNumber: null
        );

        // Act
        bool result = contact.Validate(out IList<string> errors);

        // Assert
        Assert.False(result, "Expected validation to fail");
        Assert.Contains($"{nameof(contact.HomeNumber)} and {nameof(contact.MobileNumber)} can't not both be null", errors);
        Assert.Contains("Email can't begin with number", errors);
        Assert.Contains("Email host can't be numeric", errors);
    }
}
