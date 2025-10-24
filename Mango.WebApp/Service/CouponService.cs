using CommonLibrary.Dtos;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;

namespace Mango.WebApp.Service
{
	public class CouponService(IHttpFactory http)
	{
		private const string CONTROLLER_NAME = "Coupon";

		private const string CLIENT_NAME = "CouponApi";

		public async Task<HttpResponse<QueryCouponResultDto>> Delete(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await http.DeleteAsync<QueryCouponResultDto>(
				CLIENT_NAME, $"{CONTROLLER_NAME}/{id}"
			);

			return result;
		}

		public async Task<HttpResponse<IEnumerable<QueryCouponResultDto>>> GetAsync()
		{
			HttpResponse<IEnumerable<QueryCouponResultDto>> result = await http.GetAsync<IEnumerable<QueryCouponResultDto>>(
				CLIENT_NAME, CONTROLLER_NAME
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(int id)
		{
			HttpResponse<QueryCouponResultDto> result = await http.GetAsync<QueryCouponResultDto>(
				CLIENT_NAME, $"{CONTROLLER_NAME}/{id}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> GetAsync(string code)
		{
			HttpResponse<QueryCouponResultDto> result = await http.GetAsync<QueryCouponResultDto>(
				CLIENT_NAME, $"{CONTROLLER_NAME}/ByCode/{code}"
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Post(NewCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await http.PostAsync<QueryCouponResultDto, NewCouponRequestDto>(
				CLIENT_NAME, CONTROLLER_NAME, request
			);

			return result;
		}

		public async Task<HttpResponse<QueryCouponResultDto>> Put(UpdateCouponRequestDto request)
		{
			HttpResponse<QueryCouponResultDto> result = await http.PutAsync<QueryCouponResultDto, UpdateCouponRequestDto>(
				CLIENT_NAME, CONTROLLER_NAME, request
			);

			return result;
		}
	}
}