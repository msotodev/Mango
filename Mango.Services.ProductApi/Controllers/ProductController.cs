using CommonLibrary.Dtos.Product;
using EssentialLayers.Dapper.Services.Procedure;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductController(
		IProcedureService procedureService
	) : ControllerBase
	{
		private readonly IProcedureService _procedureService = procedureService;

		/**/

		[HttpGet]
		public async Task<ResultHelper<IEnumerable<QueryProductResultDto>>> GetAsync()
		{
			try
			{
				ResultHelper<IEnumerable<QueryProductResultDto>> result = await _procedureService.ExecuteAllAsync<QueryProductResultDto, object>(
					new { }, "spQueryProducts"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<IEnumerable<QueryProductResultDto>>.Fail(e);
			}
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultHelper<QueryProductResultDto>> GetAsync(int id)
		{
			try
			{
				ResultHelper<QueryProductResultDto> result = await _procedureService.ExecuteAsync<QueryProductResultDto, QueryProductRequestDto>(
					new QueryProductRequestDto { Id = id }, "spQueryProducts"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryProductResultDto>.Fail(e);
			}
		}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryProductResultDto>> Post(SaveProductRequestDto request)
		{
			try
			{
				ResultHelper<QueryProductResultDto> result = await _procedureService.ExecuteAsync<QueryProductResultDto, SaveProductRequestDto>(
					request, "spSaveProduct"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryProductResultDto>.Fail(e);
			}
		}

		[HttpPut]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryProductResultDto>> Put(SaveProductRequestDto request)
		{
			try
			{
				ResultHelper<QueryProductResultDto> result = await _procedureService.ExecuteAsync<QueryProductResultDto, SaveProductRequestDto>(
					request, "spSaveProduct"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryProductResultDto>.Fail(e);
			}
		}

		[HttpDelete]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryProductResultDto>> Delete(DeleteProductRequestDto request)
		{
			try
			{
				ResultHelper<QueryProductResultDto> result = await _procedureService.ExecuteAsync<QueryProductResultDto, DeleteProductRequestDto>(
					request, "spDeleteProduct"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryProductResultDto>.Fail(e);
			}
		}
	}
}