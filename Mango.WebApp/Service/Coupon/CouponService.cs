using CommonLibrary.Dtos.Coupon;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Http;

namespace Mango.WebApp.Service.Coupon
{
	public class CouponService(IHttpService httpService) : ICouponService
	{
		public async Task<HttpResponse<QueryCouponResultDto>> Delete(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await httpService.DeleteAsync<QueryCouponResultDto, object>(
				new { id }, "Coupon"
			);

			return result;
		}

		public async Task<HttpResponse<IEnumerable<QueryCouponResultDto>>> GetAsync()
		{
			HttpResponse<IEnumerable<QueryCouponResultDto>> result = await httpService.GetAsync<IEnumerable<QueryCouponResultDto>>(
				"Coupon"
			);
		
			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await httpService.GetAsync<QueryCouponResultDto>(
				$"Coupon/{id}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(string code)
		{
			HttpResponse<QueryCouponResultDto> result = await httpService.GetAsync<QueryCouponResultDto>(
				$"Coupon/ByCode/{code}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Post(NewCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await httpService.PostAsync<QueryCouponResultDto, NewCouponRequestDto>(
				request, "Coupon"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Put(NewCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await httpService.PutAsync<QueryCouponResultDto, NewCouponRequestDto>(
				request, "Coupon"
			);

			return result;
		}
	}
}