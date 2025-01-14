using ContactRegister.Application.DTOs;
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
		var dddService = new DddService(_loggerMock.Object, _dddRepositoryMock.Object, _dddApiServiceMock.Object);

		//Act
		var actualResult = (await dddService.GetDdd()).Value;

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
		var dddService = new DddService(_loggerMock.Object, _dddRepositoryMock.Object, _dddApiServiceMock.Object);

		//Act
		var actualResult = (await dddService.GetDdd()).Value;

		//Assert
		Assert.Equal(actualResult.Count, expectedResult.Count);
		Assert.Empty(actualResult);
	}

	[Fact]
	public async Task GetDdd_ShouldReturnError_WhenExceptionThrown()
	{
		//Arrange
		var expectedError = "Exception throw when calling repository.";
		_dddRepositoryMock.Setup(x => x.GetDdds()).ThrowsAsync(new Exception(expectedError));
		var dddService = new DddService(_loggerMock.Object, _dddRepositoryMock.Object, _dddApiServiceMock.Object);

		//Act
		var result = await dddService.GetDdd();

		//Assert
		Assert.True(result.IsError, "Expected error not returned when repository throws");
		Assert.Single(result.Errors);
		Assert.Equal(expectedError, result.FirstError.Description);
	}
}
