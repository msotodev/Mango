using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Result;

namespace Mango.Services.AuthApi.Services.Auth
{
	public interface IAuthService
	{
		Task<ResultHelper<AssignRoleResponseDto>> AssignRoleAsync(AssignRoleRequestDto request);

		Task<ResultHelper<NewRoleResponseDto>> NewRole(NewRoleRequestDto request);

		Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request);

		Task<ResultHelper<UserResponseDto>> RegisterAsync(NewUserRequestDto request);
	}
}