using CommonLibrary.Dtos.Product;
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
	public class ProductController(
		IProcedureService procedureService
	) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			ResultHelper<IEnumerable<QueryProductResultDto>> result = await procedureService.ExecuteAllAsync<QueryProductResultDto, object>(
				new { }, "spQueryProducts"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IActionResult> GetAsync(int id)
		{
			ResultHelper<QueryProductResultDto> result = await procedureService.ExecuteAsync<QueryProductResultDto, QueryProductRequestDto>(
				new QueryProductRequestDto { Id = id }, "spQueryProducts"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPost]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Post(NewProductRequestDto request)
		{
			ResultHelper<QueryProductResultDto> result = await procedureService.ExecuteAsync<QueryProductResultDto, NewProductRequestDto>(
				request, "spSaveProduct"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPut]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Put(UpdateProductRequestDto request)
		{
			ResultHelper<QueryProductResultDto> result = await procedureService.ExecuteAsync<QueryProductResultDto, UpdateProductRequestDto>(
				request, "spSaveProduct"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpDelete]
		[Authorize(Roles = ADMIN)]
		public async Task<IActionResult> Delete(DeleteProductRequestDto request)
		{
			ResultHelper<QueryProductResultDto> result = await procedureService.ExecuteAsync<QueryProductResultDto, DeleteProductRequestDto>(
				request, "spDeleteProduct"
			);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}
	}
}