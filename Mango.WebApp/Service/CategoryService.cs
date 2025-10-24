using CommonLibrary.Dtos;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;

namespace Mango.WebApp.Service
{
	public class CategoryService(IHttpFactory http)
	{
		private const string CONTROLLER_NAME = "Category";

		private const string CLIENT_NAME = "ProductApi";

		public async Task<IList<QueryCategoryResultDto>> GetAllAsync()
		{
			HttpResponse<IList<QueryCategoryResultDto>> result = await http.GetAsync<IList<QueryCategoryResultDto>>(
				CLIENT_NAME, CONTROLLER_NAME
			);

			return result.Ok ? result.Data : [];
		}
	}
}