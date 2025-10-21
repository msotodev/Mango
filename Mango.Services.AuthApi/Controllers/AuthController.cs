using CommonLibrary.Dtos.Auth;
using EssentialLayers.Helpers.Result;
using Mango.Services.AuthApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController(
		IAuthService authService
	) : ControllerBase
	{
		private readonly IAuthService _authService = authService;

		/**/

		[HttpPost("AssignRole")]
		public async Task<IActionResult> AssignRoleAsync(AssignRoleRequestDto request)
		{
			ResultHelper<bool> result = await _authService.AssignRoleAsync(request);

			return Ok(result);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> LoginAsync(LoginRequestDto request)
		{
			ResultHelper<LoginResponseDto> result = await _authService.LoginAsync(request);

			return Ok(result.Data);
		}

		[HttpPost("Register")]
		public async Task <IActionResult> RegisterAsync([FromBody] RegisterRequestDto request)
		{
			ResultHelper<UserDto> result = await _authService.RegisterAsync(request);

			return Ok(result);
		}
	}
}