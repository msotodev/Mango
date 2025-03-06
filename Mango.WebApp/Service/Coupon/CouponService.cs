using CommonLibrary.Dtos.Coupon;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Models;
using EssentialLayers.Request.Services.Http;
using Mango.WebApp.Service.Auth;

namespace Mango.WebApp.Service.Coupon
{
	public class CouponService : ICouponService
	{
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
					CastResultAsResultHelper = true,
					BearerToken = authService.Token
				}
			);
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Delete(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.DeleteAsync<QueryCouponResultDto, object>(
				new { id }, "Coupon"
			);

			return result;
		}

		public async Task<HttpResponse<IEnumerable<QueryCouponResultDto>>> GetAsync()
		{
			HttpResponse<IEnumerable<QueryCouponResultDto>> result = await _httpService.GetAsync<IEnumerable<QueryCouponResultDto>>(
				"Coupon"
			);
		
			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.GetAsync<QueryCouponResultDto>(
				$"Coupon/{id}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(string code)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.GetAsync<QueryCouponResultDto>(
				$"Coupon/ByCode/{code}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Post(NewCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.PostAsync<QueryCouponResultDto, NewCouponRequestDto>(
				request, "Coupon"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Put(UpdateCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await _httpService.PutAsync<QueryCouponResultDto, UpdateCouponRequestDto>(
				request, "Coupon"
			);

			return result;
		}
	}
}