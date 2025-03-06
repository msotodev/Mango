using CommonLibrary.Dtos.Category;
using EssentialLayers.Dapper.Services.Procedure;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoryController(
		IProcedureService procedureService
	) : ControllerBase
	{
		private readonly IProcedureService _procedureService = procedureService;

		/**/

		[HttpGet]
		public async Task<ResultHelper<IEnumerable<QueryCategoryResultDto>>> GetAsync()
		{
			try
			{
				ResultHelper<IEnumerable<QueryCategoryResultDto>> result = await _procedureService.ExecuteAllAsync<QueryCategoryResultDto, object>(
					new { }, "spQueryCategories"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<IEnumerable<QueryCategoryResultDto>>.Fail(e);
			}
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultHelper<QueryCategoryResultDto>> GetAsync(int id)
		{
			try
			{
				ResultHelper<QueryCategoryResultDto> result = await _procedureService.ExecuteAsync<QueryCategoryResultDto, QueryCategoryRequestDto>(
					new QueryCategoryRequestDto { Id = id }, "spQueryCategories"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCategoryResultDto>.Fail(e);
			}
		}

		[HttpPost]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryCategoryResultDto>> Post(NewCategoryRequestDto request)
		{
			try
			{
				ResultHelper<QueryCategoryResultDto> result = await _procedureService.ExecuteAsync<QueryCategoryResultDto, NewCategoryRequestDto>(
					request, "spSaveCategory"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCategoryResultDto>.Fail(e);
			}
		}

		[HttpPut]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryCategoryResultDto>> Put(UpdateCategoryRequestDto request)
		{
			try
			{
				ResultHelper<QueryCategoryResultDto> result = await _procedureService.ExecuteAsync<QueryCategoryResultDto, UpdateCategoryRequestDto>(
					request, "spSaveCategory"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCategoryResultDto>.Fail(e);
			}
		}

		[HttpDelete]
		[Authorize(Roles = "ADMIN")]
		public async Task<ResultHelper<QueryCategoryResultDto>> Delete(DeleteCategoryRequestDto request)
		{
			try
			{
				ResultHelper<QueryCategoryResultDto> result = await _procedureService.ExecuteAsync<QueryCategoryResultDto, DeleteCategoryRequestDto>(
					request, "spDeleteCategory"
				);

				return result;
			}
			catch (Exception e)
			{
				return ResultHelper<QueryCategoryResultDto>.Fail(e);
			}
		}
	}
}