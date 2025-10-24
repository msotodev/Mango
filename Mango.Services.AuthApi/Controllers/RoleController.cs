using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Mango.Services.AuthApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController(
		IAuthService authService
	) : ControllerBase
	{
		[HttpPost("Assign")]
		public async Task<IActionResult> AssignRoleAsync(AssignRoleRequestDto request)
		{
			ResultHelper<AssignRoleResponseDto> result = await authService.AssignRoleAsync(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPost("New")]
		public async Task<IActionResult> AssignRoleAsync(NewRoleRequestDto request)
		{
			ResultHelper<NewRoleResponseDto> result = await authService.NewRole(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}
	}
}