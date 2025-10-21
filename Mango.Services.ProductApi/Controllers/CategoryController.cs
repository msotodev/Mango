using CommonLibrary.Dtos.Category;
using EssentialLayers.Dapper.Services.Procedure;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CommonLibrary.Constants.RoleConstant;

namespace Mango.Services.ProductApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoryController(
		IProcedureService procedureService
	) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			ResultHelper<IEnumerable<QueryCategoryResultDto>> result = await procedureService.ExecuteAllAsync<QueryCategoryResultDto, object>(
				new { }, "spQueryCategories"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			ResultHelper<QueryCategoryResultDto> result = await procedureService.ExecuteAsync<QueryCategoryResultDto, QueryCategoryRequestDto>(
				new QueryCategoryRequestDto { Id = id }, "spQueryCategories"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPost]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Post(NewCategoryRequestDto request)
		{
			ResultHelper<QueryCategoryResultDto> result = await procedureService.ExecuteAsync<QueryCategoryResultDto, NewCategoryRequestDto>(
				request, "spSaveCategory"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPut]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Put(UpdateCategoryRequestDto request)
		{
			ResultHelper<QueryCategoryResultDto> result = await procedureService.ExecuteAsync<QueryCategoryResultDto, UpdateCategoryRequestDto>(
				request, "spSaveCategory"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpDelete]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Delete(DeleteCategoryRequestDto request)
		{
			ResultHelper<QueryCategoryResultDto> result = await procedureService.ExecuteAsync<QueryCategoryResultDto, DeleteCategoryRequestDto>(
				request, "spDeleteCategory"
			);

			return Ok(result);
		}
	}
}