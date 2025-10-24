using CommonLibrary.Dtos;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;

namespace Mango.WebApp.Service
{
	public class ProductService(IHttpFactory http)
	{
		private const string CONTROLLER_NAME = "Product";

		private const string CLIENT_NAME = "ProductApi";

		public async Task<IList<QueryProductResultDto>> GetAllAsync()
		{
			HttpResponse<IList<QueryProductResultDto>> result = await http.GetAsync<IList<QueryProductResultDto>>(
				CLIENT_NAME, CONTROLLER_NAME
			);

			return result.Ok ? result.Data : [];
		}
	}
}