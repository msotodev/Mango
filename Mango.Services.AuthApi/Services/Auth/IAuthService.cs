using CommonLibrary.Dtos.Auth;
using EssentialLayers.Helpers.Result;

namespace Mango.Services.AuthApi.Services.Auth
{
	public interface IAuthService
	{
		Task<ResultHelper<bool>> AssignRoleAsync(AssignRoleRequestDto request);

		Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request);

		Task<ResultHelper<UserDto>> RegisterAsync(RegisterRequestDto request);
	}
}