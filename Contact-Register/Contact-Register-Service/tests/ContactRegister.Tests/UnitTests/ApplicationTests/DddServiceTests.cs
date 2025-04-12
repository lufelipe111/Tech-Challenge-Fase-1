using ContactRegister.Application.DTOs;
using ContactRegister.Application.DTOs.BrasilApiDTOs;
using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Application.Services;
using ContactRegister.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ContactRegister.Tests.UnitTests.ApplicationTests;

public class DddServiceTests
{
	private readonly Mock<ILogger<DddService>> _loggerMock = new();
	private readonly Mock<IDddRepository> _dddRepositoryMock = new();
	private readonly Mock<IDddApiService> _dddApiServiceMock = new();
	private readonly DddService _dddService;

	public DddServiceTests()
	{
		_dddService = new(_loggerMock.Object, _dddRepositoryMock.Object, _dddApiServiceMock.Object);
	}

	[Fact]
	public async Task GetDdd_ShouldReturnList_WhenSuccessfulRunWithResults()
	{
		//Arrange
		var baseResult = new List<Ddd>
		{
			new Ddd
			{
				Id = 1,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Code = 11,
				State = "SP",
				Region = "EMBU, VÁRZEA PAULISTA, VARGEM GRANDE PAULISTA, VARGEM, TUIUTI, TABOÃO DA SERRA, SUZANO, SÃO ROQUE, SÃO PAULO, SÃO LOURENÇO DA SERRA, SÃO CAETANO DO SUL, SÃO BERNARDO DO CAMPO, SANTO ANDRÉ, SANTANA DE PARNAÍBA, SANTA ISABEL, SALTO, SALESÓPOLIS, RIO GRANDE DA SERRA, RIBEIRÃO PIRES, POÁ, PIRAPORA DO BOM JESUS, PIRACAIA, PINHALZINHO, PEDRA BELA, OSASCO, NAZARÉ PAULISTA, MORUNGABA, MOGI DAS CRUZES, MAUÁ, MAIRIPORÃ, MAIRINQUE, JUQUITIBA, JUNDIAÍ, JOANÓPOLIS, JARINU, JANDIRA, ITUPEVA, ITU, ITATIBA, ITAQUAQUECETUBA, ITAPEVI, ITAPECERICA DA SERRA, IGARATÁ, GUARULHOS, GUARAREMA, FRANCO DA ROCHA, FRANCISCO MORATO, FERRAZ DE VASCONCELOS, EMBU-GUAÇU, DIADEMA, COTIA, CARAPICUÍBA, CAMPO LIMPO PAULISTA, CAJAMAR, CAIEIRAS, CABREÚVA, BRAGANÇA PAULISTA, BOM JESUS DOS PERDÕES, BIRITIBA-MIRIM, BARUERI, ATIBAIA, ARUJÁ, ARAÇARIGUAMA, ALUMÍNIO"
			},
			new Ddd
			{
				Id = 2,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				Code = 21,
				State = "RJ",
				Region = "TERESÓPOLIS, TANGUÁ,SEROPÉDICA, SÃO JOÃO DE MERITI, SÃO GONÇALO, RIO DE JANEIRO, RIO BONITO, QUEIMADOS, PARACAMBI, NOVA IGUAÇU, NITERÓI, NILÓPOLIS, MESQUITA, MARICÁ, MANGARATIBA, MAGÉ, JAPERI, ITAGUAÍ, ITABORAÍ, GUAPIMIRIM, DUQUE DE CAXIAS, CACHOEIRAS DE MACACU, BELFORD ROXO"
			}
		};
		_dddRepositoryMock.Setup(x => x.GetDdds()).ReturnsAsync(baseResult);
		var expectedResult = baseResult.Select(x => DddDto.FromEntity(x)).ToList();

		//Act
		var actualResult = (await _dddService.GetDdd()).Value;

		//Assert
		Assert.Equal(actualResult.Count, expectedResult.Count);
		Assert.Contains(actualResult, x => x.Code == expectedResult[0].Code);
		Assert.Contains(actualResult, x => x.Code == expectedResult[1].Code);
	}

	[Fact]
	public async Task GetDdd_ShouldReturnEmptyList_WhenSuccessfulRunWithoutResults()
	{
		//Arrange
		var baseResult = new List<Ddd>();
		_dddRepositoryMock.Setup(x => x.GetDdds()).ReturnsAsync(baseResult);
		var expectedResult = baseResult.Select(x => DddDto.FromEntity(x)).ToList();

		//Act
		var actualResult = (await _dddService.GetDdd()).Value;

		//Assert
		Assert.Equal(actualResult.Count, expectedResult.Count);
		Assert.Empty(actualResult);
	}

	[Fact]
	public async Task GetDdd_ShouldReturnError_WhenExceptionThrows()
	{
		//Arrange
		var expectedError = "Exception throws when calling repository.";
		_dddRepositoryMock.Setup(x => x.GetDdds()).ThrowsAsync(new Exception(expectedError));

		//Act
		var result = await _dddService.GetDdd();

		//Assert
		Assert.True(result.IsError, "Expected error not returned when repository throws");
		Assert.Single(result.Errors);
		Assert.Equal(expectedError, result.FirstError.Description);
	}

	[Fact]
	public async Task GetDddByCode_ShouldReturnDto_WhenFoundInDatabase()
	{
		//Arrange
		var baseResult = new Ddd
		{
			Id = 2,
			CreatedAt = DateTime.UtcNow,
			UpdatedAt = DateTime.UtcNow,
			Code = 21,
			State = "RJ",
			Region = "TERESÓPOLIS, TANGUÁ,SEROPÉDICA, SÃO JOÃO DE MERITI, SÃO GONÇALO, RIO DE JANEIRO, RIO BONITO, QUEIMADOS, PARACAMBI, NOVA IGUAÇU, NITERÓI, NILÓPOLIS, MESQUITA, MARICÁ, MANGARATIBA, MAGÉ, JAPERI, ITAGUAÍ, ITABORAÍ, GUAPIMIRIM, DUQUE DE CAXIAS, CACHOEIRAS DE MACACU, BELFORD ROXO"
		};
		var expectedResult = DddDto.FromEntity(baseResult);
		_dddRepositoryMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(baseResult);

		//Act
		var actualResult = (await _dddService.GetDddByCode(It.IsAny<int>())).Value;

		//Assert
		Assert.NotNull(actualResult);
		Assert.Equal(expectedResult.Code, actualResult.Code);
		Assert.Equal(expectedResult.State, actualResult.State);
		Assert.Equal(expectedResult.Region, actualResult.Region);
	}

	[Fact]
	public async Task GetDddByCode_ShouldAddInDatabaseAndReturnDto_WhenNotInDatabaseButFoundInApi()
	{
		//Arrange
		var code = 21;
		Ddd? dbResult = null;
		var apiResult = new DddApiResponseDto(new DddApiSuccessResponseDto
		{
			State = "RJ",
			Cities = new List<string>
			{
				"TERESÓPOLIS","TANGUÁ","SEROPÉDICA","SÃO JOÃO DE MERITI","SÃO GONÇALO","RIO DE JANEIRO","RIO BONITO","QUEIMADOS","PARACAMBI","NOVA IGUAÇU","NITERÓI","NILÓPOLIS","MESQUITA","MARICÁ","MANGARATIBA","MAGÉ","JAPERI","ITAGUAÍ","ITABORAÍ","GUAPIMIRIM","DUQUE DE CAXIAS","CACHOEIRAS DE MACACU","BELFORD ROXO"
			}
		}, null);
		var expectedResult = new DddDto
		{
			Code = code,
			State = apiResult.Result!.State,
			Region = string.Join(", ", apiResult.Result.Cities)
		};
		_dddRepositoryMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(dbResult);
		_dddApiServiceMock.Setup(x => x.GetByCode(It.IsAny<int>())).ReturnsAsync(apiResult);
		_dddRepositoryMock.Setup(x => x.AddDdd(It.IsAny<Ddd>())).ReturnsAsync(1).Verifiable();

		//Act
		var actualResult = (await _dddService.GetDddByCode(code)).Value;

		//Assert
		Assert.NotNull(actualResult);
		_dddRepositoryMock.Verify(x => x.AddDdd(It.IsAny<Ddd>()), Times.Once());
		Assert.Equal(expectedResult.Code, actualResult.Code);
		Assert.Equal(expectedResult.State, actualResult.State);
		Assert.Equal(expectedResult.Region, actualResult.Region);
	}

	[Fact]
	public async Task GetDddByCode_ShouldReturnError_WhenNotInDatabaseNorInApi()
	{
		//Arrange
		Ddd? dbResult = null;
		var apiResult = new DddApiResponseDto(null, new DddApiErrorResponseDto
		{
			Message = "DDD não encontrado",
			Name = "DDD_NOT_FOUND",
			Type = "ddd_error"
		});
		_dddRepositoryMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(dbResult);
		_dddApiServiceMock.Setup(x => x.GetByCode(It.IsAny<int>())).ReturnsAsync(apiResult);

		//Act
		var result = await _dddService.GetDddByCode(It.IsAny<int>());

		//Assert
		Assert.True(result.IsError, "Expected error not returned when ddd was not found in db or API");
		Assert.Single(result.Errors);
		Assert.Equal("Ddd.ExternalApi", result.FirstError.Code);
		Assert.Equal("DDD não encontrado", result.FirstError.Description);
	}

	[Fact]
	public async Task GetDddByCode_ShouldReturnError_WhenApiIsInError()
	{
		//Arrange
		Ddd? dbResult = null;
		var apiResult = new DddApiResponseDto(null, new DddApiErrorResponseDto
		{
			Message = "Todos os serviços de DDD retornaram erro.",
			Name = "ddd_error",
			Type = "service_error"
		});
		_dddRepositoryMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(dbResult);
		_dddApiServiceMock.Setup(x => x.GetByCode(It.IsAny<int>())).ReturnsAsync(apiResult);

		//Act
		var result = await _dddService.GetDddByCode(It.IsAny<int>());

		//Assert
		Assert.True(result.IsError, "Expected error not returned when API is in error");
		Assert.Single(result.Errors);
		Assert.Equal("Ddd.ExternalApi", result.FirstError.Code);
		Assert.Equal("Todos os serviços de DDD retornaram erro.", result.FirstError.Description);
	}

	[Fact]
	public async Task GetDddByCode_ShouldReturnError_WhenValidationFails()
	{
		//Arrange
		Ddd? dbResult = null;
		var apiResult = new DddApiResponseDto(new DddApiSuccessResponseDto
		{
			State = "JR",
			Cities = new List<string>()
		}, null);
		_dddRepositoryMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ReturnsAsync(dbResult);
		_dddApiServiceMock.Setup(x => x.GetByCode(It.IsAny<int>())).ReturnsAsync(apiResult);

		//Act
		var result = await _dddService.GetDddByCode(0);

		//Assert
		Assert.True(result.IsError, "Expected errors not returned when validation fails");
		Assert.Contains(result.Errors, x => x.Code == "Ddd.Validation" && x.Description == $"{nameof(Ddd.Code)} must be greater than 10 and lesser than 100");
		Assert.Contains(result.Errors, x => x.Code == "Ddd.Validation" && x.Description == $"{nameof(Ddd.State)} is invalid");
		Assert.Contains(result.Errors, x => x.Code == "Ddd.Validation" && x.Description == $"{nameof(Ddd.Region)} can't be null");
	}

	[Fact]
	public async Task GetDddByCode_ShouldReturnError_WhenExceptionThrows()
	{
		//Arrange
		var expectedError = "Exception throws when calling repository.";
		_dddRepositoryMock.Setup(x => x.GetDddByCode(It.IsAny<int>())).ThrowsAsync(new Exception(expectedError));

		//Act
		var result = await _dddService.GetDddByCode(It.IsAny<int>());

		//Assert
		Assert.True(result.IsError, "Expected error not returned when exception throws");
		Assert.Single(result.Errors);
		Assert.Equal("Ddd.Get.Exception", result.FirstError.Code);
		Assert.Equal(expectedError, result.FirstError.Description);
	}
}
