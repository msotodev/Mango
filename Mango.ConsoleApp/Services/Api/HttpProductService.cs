using CommonLibrary.Dtos;
using EssentialLayers.Request.Helpers;
using System.Net;
using System.Net.Http.Json;
using static Mango.ConsoleApp.Constants.Defaults;

namespace Mango.ConsoleApp.Services.Api
{
	public class HttpProductService(IHttpClientFactory httpClientFactory)
	{
		private const string CONTROLLER_NAME = "Product";

		private readonly HttpClient _httpClient = httpClientFactory.CreateClient(PRODUCT_CLIENT_NAME);

		public async Task<HttpResponse<QueryProductResultDto>> NewAsync(NewProductRequestDto requestDto)
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
				CONTROLLER_NAME, requestDto
			);

			if (response.IsSuccessStatusCode)
			{
				QueryProductResultDto? data = await response.Content.ReadFromJsonAsync<QueryProductResultDto>();

				if(data != null)
				{
					return HttpResponse<QueryProductResultDto>.Success(data, HttpStatusCode.OK);
				}
			}

			return HttpResponse<QueryProductResultDto>.Fail("Valio", HttpStatusCode.InternalServerError);
		}
	}
}