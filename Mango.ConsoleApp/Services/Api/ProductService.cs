using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;
using static Mango.ConsoleApp.Constants.Defaults;

namespace Mango.ConsoleApp.Services.Api
{
	public class ProductService(IHttpFactory httpFactory)
	{
		private const string CONTROLLER_NAME = "Product";

		public Task<HttpResponse<IList<QueryProductResultDto>>> GetAsync(string name = "")
		{
			string endPoint = CONTROLLER_NAME;

			if (name.NotEmpty()) endPoint = $"{CONTROLLER_NAME}/ByName/{name}";

			return httpFactory.GetAsync<IList<QueryProductResultDto>>(PRODUCT_CLIENT_NAME, endPoint);
		}

		public Task<HttpResponse<QueryProductResultDto>> NewAsync(NewProductRequestDto requestDto)
		{
			return httpFactory.PostAsync<QueryProductResultDto, NewProductRequestDto>(
				PRODUCT_CLIENT_NAME, CONTROLLER_NAME, requestDto
			);
		}
	}
}