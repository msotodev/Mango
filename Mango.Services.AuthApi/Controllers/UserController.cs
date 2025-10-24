using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Mango.Services.AuthApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(
		IAuthService authService
	) : ControllerBase
	{
		[HttpPost("Login")]
		public async Task<IActionResult> LoginAsync(LoginRequestDto request)
		{
			ResultHelper<LoginResponseDto> result = await authService.LoginAsync(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}

		[HttpPost("New")]
		public async Task<IActionResult> RegisterAsync([FromBody] NewUserRequestDto request)
		{
			ResultHelper<UserResponseDto> result = await authService.RegisterAsync(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}
	}
}