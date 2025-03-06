using CommonLibrary.Dtos.Coupon;
using EssentialLayers.Dapper.Services.Procedure;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponApi.Controllers
{
	[Authorize]
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
		[Authorize(Roles = "ADMIN")]
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
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryCouponResultDto>> Put(UpdateCouponRequestDto request)
		{
			try
			{
				ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, UpdateCouponRequestDto>(
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
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryCouponResultDto>> Delete(DeleteCouponRequestDto request)
		{
			try
			{
				ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, DeleteCouponRequestDto>(
					request, "spDeleteCoupon"
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