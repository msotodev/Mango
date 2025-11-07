using CommonLibrary.Dtos;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;
using static Mango.ConsoleApp.Constants.Defaults;

namespace Mango.ConsoleApp.Services.Api
{
	public class AuthService(IHttpFactory httpFactory)
	{
		public Task<HttpResponse<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto)
		{
			return httpFactory.PostAsync<LoginResponseDto, LoginRequestDto>(
				AUTH_CLIENT_NAME, "User/Login", requestDto
			);
		}
	}
}