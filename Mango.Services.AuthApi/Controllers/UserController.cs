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
	public class UserController(
		UserService userService,
		RoleService roleService,
		TokenService tokenService
	) : ControllerBase
	{
		[HttpPost("Login")]
		[AllowAnonymous]
		public async Task<IActionResult> LoginAsync(LoginRequestDto request)
		{
			ResultHelper<LoginResponseDto> result = await userService.LoginAsync(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			UserResponseDto? user = result.Data.User;

			if (user == null) return BadRequest("The user is null");

			string userId = $"{user.Id}";

			IList<RoleResponseDto> roles = await roleService.GetByUserIdAsync(userId);

			ResultHelper<string> tokenResult = tokenService.Generate(
				userId, user.Email, user.Name, roles.Select(r => r.Name)
			);

			if (tokenResult.Ok.False()) return BadRequest("Token was empty");

			return Ok(
				new LoginResponseDto
				{
					Token = tokenResult.Data,
					User = user
				}
			);
		}

		[HttpPost("New")]
		public async Task<IActionResult> NewAsync([FromBody] NewUserRequestDto request)
		{
			ResultHelper<UserResponseDto> result = await userService.NewAsync(request);

			if (result.Ok.False()) return BadRequest(result.Message);

			return Ok(result.Data);
		}
	}
}