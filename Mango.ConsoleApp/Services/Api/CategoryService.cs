using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;
using static Mango.ConsoleApp.Constants.Defaults;

namespace Mango.ConsoleApp.Services.Api
{
	public class CategoryService(IHttpFactory httpFactory)
	{
		private const string CONTROLLER_NAME = "Category";

		public Task<HttpResponse<IList<QueryCategoryResultDto>>> GetAsync(string name = "")
		{
			string endPoint = CONTROLLER_NAME;

			if (name.NotEmpty()) endPoint = $"{CONTROLLER_NAME}/ByName/{name}";

			return httpFactory.GetAsync<IList<QueryCategoryResultDto>>(PRODUCT_CLIENT_NAME, endPoint);
		}

		public Task<HttpResponse<QueryCategoryResultDto>> NewAsync(NewCategoryRequestDto requestDto)
		{
			return httpFactory.PostAsync<QueryCategoryResultDto, NewCategoryRequestDto>(
				PRODUCT_CLIENT_NAME, CONTROLLER_NAME, requestDto
			);
		}
	}
}