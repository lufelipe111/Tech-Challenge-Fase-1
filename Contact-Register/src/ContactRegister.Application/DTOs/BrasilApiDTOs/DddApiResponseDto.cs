namespace ContactRegister.Application.DTOs.BrasilApiDTOs;

public class DddApiResponseDto
{
	public DddApiSuccessResponseDto? Result { get; set; }
	public DddApiErrorResponseDto? Error { get; set; }

	public DddApiResponseDto(DddApiSuccessResponseDto? result, DddApiErrorResponseDto? error)
	{
		if (result == null && error == null) throw new InvalidOperationException("A API deve retornar o resultado ou erro. Nenhum dos dois foi informado.");

		if (result != null && error != null) throw new InvalidOperationException("A API deve retornar o resultado ou erro, mas não ambos.");

		Result = result;
		Error = error;
	}
}
