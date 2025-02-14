using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Models;
using EssentialLayers.Request.Services.Http;
using Mango.WebApp.Models;
using System.Net;
using static Mango.WebApp.Types.ApiTypes;

namespace Mango.WebApp.Service
{
	public class ServiceBase(IHttpService httpService) : IServiceBase
	{
		private readonly IHttpService _httpService = httpService;

		/**/

		public async Task<ResultHelper<TResult>> Send<TResult, TRequest>(
			RequestDto<TRequest> request
		)
		{
			HttpResponse<TResult> response = HttpResponse<TResult>.Fail(
				string.Empty, HttpStatusCode.InternalServerError
			);

			response = request.ApiType switch
			{
				ApiType.GET => await _httpService.GetAsync<TResult>(
					request.Url, new RequestOptions(request.Token)
				),
				ApiType.POST => await _httpService.PostAsync<TResult, TRequest>(
					request.Data, request.Url, new RequestOptions(request.Token)
				),
				ApiType.PUT => await _httpService.PutAsync<TResult, TRequest>(
					request.Data, request.Url, new RequestOptions(request.Token)
				),
				ApiType.DELETE => await _httpService.DeleteAsync<TResult, TRequest>(
					request.Data, request.Url, new RequestOptions(request.Token)
				),
				_ => HttpResponse<TResult>.Fail(
					$"The method {request.ApiType} doesn't exist", HttpStatusCode.BadRequest
				),
			};

			return new ResultHelper<TResult>(response.Ok, response.Message, response.Data);
		}
	}
}