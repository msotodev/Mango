using CommonLibrary.Dtos.Auth;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Mvc;

namespace Mango.WebApp.Service.Auth
{
	public interface IAuthService
	{
		string Token { get; }

		/**/

		Task<ResultHelper<bool>> AssignRoleAsync(AssignRoleRequestDto request);

		Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request);

		Task<ResultHelper<UserDto>> RegisterAsync([FromBody] RegisterRequestDto request);
	}
}