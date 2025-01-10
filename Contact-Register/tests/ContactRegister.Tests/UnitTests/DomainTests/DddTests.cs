using ContactRegister.Domain.Entities;
using Xunit;

namespace ContactRegister.Tests.UnitTests.DomainTests;

public class DddTests
{
	[Fact]
	public void Validate_ShouldReturnTrue_WhenAllDataIsValid()
	{
		// Arrange
		var ddd = new Ddd(
			11,
			"SP",
			"EMBU, VÁRZEA PAULISTA, VARGEM GRANDE PAULISTA, VARGEM, TUIUTI, TABOÃO DA SERRA, SUZANO, SÃO ROQUE, SÃO PAULO, SÃO LOURENÇO DA SERRA, SÃO CAETANO DO SUL, SÃO BERNARDO DO CAMPO, SANTO ANDRÉ, SANTANA DE PARNAÍBA, SANTA ISABEL, SALTO, SALESÓPOLIS, RIO GRANDE DA SERRA, RIBEIRÃO PIRES, POÁ, PIRAPORA DO BOM JESUS, PIRACAIA, PINHALZINHO, PEDRA BELA, OSASCO, NAZARÉ PAULISTA, MORUNGABA, MOGI DAS CRUZES, MAUÁ, MAIRIPORÃ, MAIRINQUE, JUQUITIBA, JUNDIAÍ, JOANÓPOLIS, JARINU, JANDIRA, ITUPEVA, ITU, ITATIBA, ITAQUAQUECETUBA, ITAPEVI, ITAPECERICA DA SERRA, IGARATÁ, GUARULHOS, GUARAREMA, FRANCO DA ROCHA, FRANCISCO MORATO, FERRAZ DE VASCONCELOS, EMBU-GUAÇU, DIADEMA, COTIA, CARAPICUÍBA, CAMPO LIMPO PAULISTA, CAJAMAR, CAIEIRAS, CABREÚVA, BRAGANÇA PAULISTA, BOM JESUS DOS PERDÕES, BIRITIBA-MIRIM, BARUERI, ATIBAIA, ARUJÁ, ARAÇARIGUAMA, ALUMÍNIO"
		);

		// Act
		bool result = ddd.Validate(out IList<string> errors);

		// Assert
		Assert.True(result, "Expected validation to pass");
		Assert.Empty(errors);
	}

	[Fact]
	public void Validate_ShouldReturnFalse_WhenCodeIsLesserThanAllowed()
	{
		// Arrange
		var ddd = new Ddd(
			10,
			"SP",
			"EMBU, VÁRZEA PAULISTA, VARGEM GRANDE PAULISTA, VARGEM, TUIUTI, TABOÃO DA SERRA, SUZANO, SÃO ROQUE, SÃO PAULO, SÃO LOURENÇO DA SERRA, SÃO CAETANO DO SUL, SÃO BERNARDO DO CAMPO, SANTO ANDRÉ, SANTANA DE PARNAÍBA, SANTA ISABEL, SALTO, SALESÓPOLIS, RIO GRANDE DA SERRA, RIBEIRÃO PIRES, POÁ, PIRAPORA DO BOM JESUS, PIRACAIA, PINHALZINHO, PEDRA BELA, OSASCO, NAZARÉ PAULISTA, MORUNGABA, MOGI DAS CRUZES, MAUÁ, MAIRIPORÃ, MAIRINQUE, JUQUITIBA, JUNDIAÍ, JOANÓPOLIS, JARINU, JANDIRA, ITUPEVA, ITU, ITATIBA, ITAQUAQUECETUBA, ITAPEVI, ITAPECERICA DA SERRA, IGARATÁ, GUARULHOS, GUARAREMA, FRANCO DA ROCHA, FRANCISCO MORATO, FERRAZ DE VASCONCELOS, EMBU-GUAÇU, DIADEMA, COTIA, CARAPICUÍBA, CAMPO LIMPO PAULISTA, CAJAMAR, CAIEIRAS, CABREÚVA, BRAGANÇA PAULISTA, BOM JESUS DOS PERDÕES, BIRITIBA-MIRIM, BARUERI, ATIBAIA, ARUJÁ, ARAÇARIGUAMA, ALUMÍNIO"
		);

		// Act
		bool result = ddd.Validate(out IList<string> errors);

		// Assert
		Assert.False(result, $"Expected validation to fail due to {nameof(ddd.Code)} lesser than allowed");
		Assert.Contains($"{nameof(ddd.Code)} must be greater than 10 and lesser than 100", errors);
	}

	[Fact]
	public void Validate_ShouldReturnFalse_WhenCodeIsGreatherThanAllowed()
	{
		// Arrange
		var ddd = new Ddd(
			100,
			"SP",
			"EMBU, VÁRZEA PAULISTA, VARGEM GRANDE PAULISTA, VARGEM, TUIUTI, TABOÃO DA SERRA, SUZANO, SÃO ROQUE, SÃO PAULO, SÃO LOURENÇO DA SERRA, SÃO CAETANO DO SUL, SÃO BERNARDO DO CAMPO, SANTO ANDRÉ, SANTANA DE PARNAÍBA, SANTA ISABEL, SALTO, SALESÓPOLIS, RIO GRANDE DA SERRA, RIBEIRÃO PIRES, POÁ, PIRAPORA DO BOM JESUS, PIRACAIA, PINHALZINHO, PEDRA BELA, OSASCO, NAZARÉ PAULISTA, MORUNGABA, MOGI DAS CRUZES, MAUÁ, MAIRIPORÃ, MAIRINQUE, JUQUITIBA, JUNDIAÍ, JOANÓPOLIS, JARINU, JANDIRA, ITUPEVA, ITU, ITATIBA, ITAQUAQUECETUBA, ITAPEVI, ITAPECERICA DA SERRA, IGARATÁ, GUARULHOS, GUARAREMA, FRANCO DA ROCHA, FRANCISCO MORATO, FERRAZ DE VASCONCELOS, EMBU-GUAÇU, DIADEMA, COTIA, CARAPICUÍBA, CAMPO LIMPO PAULISTA, CAJAMAR, CAIEIRAS, CABREÚVA, BRAGANÇA PAULISTA, BOM JESUS DOS PERDÕES, BIRITIBA-MIRIM, BARUERI, ATIBAIA, ARUJÁ, ARAÇARIGUAMA, ALUMÍNIO"
		);

		// Act
		bool result = ddd.Validate(out IList<string> errors);

		// Assert
		Assert.False(result, $"Expected validation to fail due to {nameof(ddd.Code)} greather than allowed");
		Assert.Contains($"{nameof(ddd.Code)} must be greater than 10 and lesser than 100", errors);
	}

	[Fact]
	public void Validate_ShouldReturnFalse_WhenStateIsNotInAllowedList()
	{
		// Arrange
		var ddd = new Ddd(
			11,
			"PS",
			"EMBU, VÁRZEA PAULISTA, VARGEM GRANDE PAULISTA, VARGEM, TUIUTI, TABOÃO DA SERRA, SUZANO, SÃO ROQUE, SÃO PAULO, SÃO LOURENÇO DA SERRA, SÃO CAETANO DO SUL, SÃO BERNARDO DO CAMPO, SANTO ANDRÉ, SANTANA DE PARNAÍBA, SANTA ISABEL, SALTO, SALESÓPOLIS, RIO GRANDE DA SERRA, RIBEIRÃO PIRES, POÁ, PIRAPORA DO BOM JESUS, PIRACAIA, PINHALZINHO, PEDRA BELA, OSASCO, NAZARÉ PAULISTA, MORUNGABA, MOGI DAS CRUZES, MAUÁ, MAIRIPORÃ, MAIRINQUE, JUQUITIBA, JUNDIAÍ, JOANÓPOLIS, JARINU, JANDIRA, ITUPEVA, ITU, ITATIBA, ITAQUAQUECETUBA, ITAPEVI, ITAPECERICA DA SERRA, IGARATÁ, GUARULHOS, GUARAREMA, FRANCO DA ROCHA, FRANCISCO MORATO, FERRAZ DE VASCONCELOS, EMBU-GUAÇU, DIADEMA, COTIA, CARAPICUÍBA, CAMPO LIMPO PAULISTA, CAJAMAR, CAIEIRAS, CABREÚVA, BRAGANÇA PAULISTA, BOM JESUS DOS PERDÕES, BIRITIBA-MIRIM, BARUERI, ATIBAIA, ARUJÁ, ARAÇARIGUAMA, ALUMÍNIO"
		);

		// Act
		bool result = ddd.Validate(out IList<string> errors);

		// Assert
		Assert.False(result, $"Expected validation to fail due to {nameof(ddd.State)} not in allowed list");
		Assert.Contains($"{nameof(ddd.State)} is invalid", errors);
	}

	[Fact]
	public void Validate_ShouldReturnFalse_WhenRegionIsNullOrWhiteSpace()
	{
		// Arrange
		var ddd = new Ddd(
			11,
			"SP",
			null
		);

		// Act
		bool result = ddd.Validate(out IList<string> errors);

		// Assert
		Assert.False(result, $"Expected validation to fail due to {nameof(ddd.Region)} null or white space");
		Assert.Contains($"{nameof(ddd.Region)} can't be null", errors);
	}

	[Fact]
	public void Validate_ShouldAccumulateMultipleErrors()
	{
		// Arrange
		var ddd = new Ddd(
			10,
			"PS",
			null
		);

		// Act
		bool result = ddd.Validate(out IList<string> errors);

		// Assert
		Assert.False(result, "Expected validation to fail");
		Assert.Contains($"{nameof(ddd.Code)} must be greater than 10 and lesser than 100", errors);
		Assert.Contains($"{nameof(ddd.State)} is invalid", errors);
		Assert.Contains($"{nameof(ddd.Region)} can't be null", errors);
	}
}
