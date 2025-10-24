using CommonLibrary.Dtos;
using EssentialLayers.Dapper.Services.Procedure;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CommonLibrary.Constants.RoleConstant;

namespace Mango.Services.CouponApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class CouponController(IProcedureService procedureService) : ControllerBase
	{
		private readonly IProcedureService _procedureService = procedureService;

		/**/

		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			ResultHelper<IEnumerable<QueryCouponResultDto>> result = await _procedureService.ExecuteAllAsync<QueryCouponResultDto, object>(
				new { }, "spQueryCoupons"
			);

			return Ok(result.Data);
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, QueryCouponRequestDto>(
				new QueryCouponRequestDto { Id = id }, "spQueryCoupons"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpGet]
		[Route("ByCode/{code}")]
		public async Task<IActionResult> GetAsync(string code)
		{
			ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, CouponByCodeRquestDto>(
				new CouponByCodeRquestDto { Code = code }, "spQueryCouponsByCode"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPost]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Post(NewCouponRequestDto request)
		{
			ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, NewCouponRequestDto>(
				request, "spSaveCoupon"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPut]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Put(UpdateCouponRequestDto request)
		{
			ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, UpdateCouponRequestDto>(
				request, "spSaveCoupon"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpDelete]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Delete(DeleteCouponRequestDto request)
		{
			ResultHelper<QueryCouponResultDto> result = await _procedureService.ExecuteAsync<QueryCouponResultDto, DeleteCouponRequestDto>(
				request, "spDeleteCoupon"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}
	}
}