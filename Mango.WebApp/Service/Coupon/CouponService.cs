using CommonLibrary.Dtos.Coupon;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Models;
using EssentialLayers.Request.Services.Http;
using Mango.WebApp.Service.Auth;
using static EssentialLayers.Request.Helpers.Types.HttpTypes;

namespace Mango.WebApp.Service.Coupon
{
	public class CouponService : ICouponService
	{
		private const string CONTROLLER_NAME = "Coupon";

		private readonly IHttpService _httpService;

		/**/

		public CouponService(
			IAuthService authService,
			IConfiguration configuration,
			IHttpService httpService
		)
		{
			_httpService = httpService;

			_httpService.SetOptions(
				new HttpOption
				{
					BaseUri = configuration?.GetSection("ServicesUrls").GetValue<string>("CouponApi")!,
					ResultType = ResultType.ResultHelper,
					BearerToken = authService.Token
				}
			);
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Delete(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.DeleteAsync<QueryCouponResultDto, object>(
				new { id }, CONTROLLER_NAME
			);

			return result;
		}

		public async Task<HttpResponse<IEnumerable<QueryCouponResultDto>>> GetAsync()
		{
			HttpResponse<IEnumerable<QueryCouponResultDto>> result = await _httpService.GetAsync<IEnumerable<QueryCouponResultDto>>(
				CONTROLLER_NAME
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.GetAsync<QueryCouponResultDto>(
				$"{CONTROLLER_NAME}/{id}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(string code)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.GetAsync<QueryCouponResultDto>(
				$"{CONTROLLER_NAME}/ByCode/{code}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Post(NewCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.PostAsync<QueryCouponResultDto, NewCouponRequestDto>(
				request, CONTROLLER_NAME
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Put(UpdateCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.PutAsync<QueryCouponResultDto, UpdateCouponRequestDto>(
				request, CONTROLLER_NAME
			);

			return result;
		}
	}
}