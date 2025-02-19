using CommonLibrary.Dtos.Coupon;
using EssentialLayers.Request.Helpers;

namespace Mango.WebApp.Service.Coupon
{
	public interface ICouponService
	{
		Task<HttpResponse<QueryCouponResultDto>> Delete(int id);

		Task<HttpResponse<IEnumerable<QueryCouponResultDto>>> GetAsync();

		Task<HttpResponse<QueryCouponResultDto>> GetAsync(int id);

		Task<HttpResponse<QueryCouponResultDto>> GetAsync(string code);

		Task<HttpResponse<QueryCouponResultDto>> Post(NewCouponRequestDto request);

		Task<HttpResponse<QueryCouponResultDto>> Put(NewCouponRequestDto request);
	}
}