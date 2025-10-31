using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Mango.Services.AuthApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = "Bearer")]
	[ApiController]
	public class RoleController(
		RoleService roleService
	) : ControllerBase
	{
		[HttpPost("Assign")]
		public async Task<IActionResult> AssignRoleAsync(AssignRoleRequestDto request)
		{
			ResultHelper<AssignRoleResponseDto> result = await roleService.AssignAsync(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPost("New")]
		public async Task<IActionResult> NewAsync(NewRoleRequestDto request)
		{
			ResultHelper<NewRoleResponseDto> result = await roleService.NewAsync(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}
	}
}