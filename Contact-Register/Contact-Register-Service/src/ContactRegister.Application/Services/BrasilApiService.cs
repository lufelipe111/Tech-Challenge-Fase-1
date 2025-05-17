using ContactRegister.Application.DTOs.BrasilApiDTOs;
using ContactRegister.Application.Interfaces.Services;
using System.Text.Json;
using Polly.CircuitBreaker;

namespace ContactRegister.Application.Services;

public class BrasilApiService(IHttpClientFactory clientFactory) : IDddApiService
{
	private string _route = "/api/ddd/v1/{0}";
	public async Task<DddApiResponseDto?> GetByCode(int code)
	{
		try
		{
			var client = clientFactory.CreateClient("brasil-api");
			HttpResponseMessage response = await client.GetAsync(string.Format(_route, code));
			var responseContent = await response.Content.ReadAsStringAsync();

			return response.IsSuccessStatusCode
				? new DddApiResponseDto(JsonSerializer.Deserialize<DddApiSuccessResponseDto>(responseContent), null)
				: new DddApiResponseDto(null, JsonSerializer.Deserialize<DddApiErrorResponseDto>(responseContent));
		}
		catch (BrokenCircuitException)
		{
			throw;
		}
		catch (Exception e)
		{
			return null;
		}
	}

	public void SetRoute(string route)
	{
		_route = route;
	}
}
