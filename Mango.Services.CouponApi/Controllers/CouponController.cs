using EssentialLayers.Dapper.Services.Procedure;
using EssentialLayers.Helpers.Result;
using Mango.Services.CouponApi.Dtos.Coupon;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CouponController(IProcedureService procedureService) : ControllerBase
	{
		private readonly IProcedureService _procedureService = procedureService;

		/**/

		[HttpGet]
		public async Task<ResultHelper<IEnumerable<QueryCouponResultDto>>> GetAsync()
		{
			try
			{
				ResultHelper<IEnumerable<QueryCouponResultDto>> result = await _procedureService.ExecuteAllAsync<QueryCouponResultDto, object>(
					new { }, "spQueryCoupons"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<IEnumerable<QueryCouponResultDto>>.Fail(e);
			}
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultHelper<QueryCouponResultDto>> GetAsync(int id)
		{
			try
			{
				ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, QueryCouponRequestDto>(
					new QueryCouponRequestDto { Id = id }, "spQueryCoupons"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCouponResultDto>.Fail(e);
			}
		}

		[HttpGet]
		[Route("ByCode/{code}")]
		public async Task<ResultHelper<QueryCouponResultDto>> GetAsync(string code)
		{
			try
			{
				ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, CouponByCodeRquestDto>(
					new CouponByCodeRquestDto { Code = code }, "spQueryCouponsByCode"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCouponResultDto>.Fail(e);
			}
		}

		[HttpPost]
		public async Task<ResultHelper<QueryCouponResultDto>> Post(NewCouponRequestDto request)
		{
			try
			{
				ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, NewCouponRequestDto>(
					request, "spSaveCoupon"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCouponResultDto>.Fail(e);
			}
		}

		[HttpPut]
		public async Task<ResultHelper<QueryCouponResultDto>> Put(NewCouponRequestDto request)
		{
			try
			{
				ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, NewCouponRequestDto>(
					request, "spSaveCoupon"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCouponResultDto>.Fail(e);
			}
		}

		[HttpDelete]
		public async Task<ResultHelper<QueryCouponResultDto>> Delete(int id)
		{
			try
			{
				ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, DeleteCouponRequestDto>(
					new DeleteCouponRequestDto { Id = id }, "spDeleteCoupon"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCouponResultDto>.Fail(e);
			}
		}
	}
}