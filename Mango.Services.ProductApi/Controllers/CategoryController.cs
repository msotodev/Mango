using CommonLibrary.Dtos;
using EssentialLayers.Dapper.Services.Procedure;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static CommonLibrary.Constants.RoleConstant;

namespace Mango.Services.ProductApi.Controllers
{
	[ApiController]
	[Authorize(AuthenticationSchemes = "Bearer")]
	[Route("api/[controller]")]
	public class CategoryController(
		IProcedureService procedureService
	) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			try
			{
				ResultHelper<IEnumerable<QueryCategoryResultDto>> result = await procedureService.ExecuteAllAsync<QueryCategoryResultDto, object>(
					new { }, "spQueryCategories"
				);

				if (result.Ok.False()) return BadRequest(result.Message);

				Debug.WriteLine(result.Data);

				return Ok(result.Data);
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);

				return BadRequest(e);
			}
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

		[HttpGet("ByName/{name}")]
		public async Task<IActionResult> GetAsync(string name)
		{
			ResultHelper<IEnumerable<QueryCategoryResultDto>> result = await procedureService.ExecuteAllAsync<QueryCategoryResultDto, QueryCategoryRequestDto>(
				new QueryCategoryRequestDto { Id = -1, Name = name }, "spQueryCategories"
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