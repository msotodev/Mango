using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;
using Microsoft.AspNetCore.Mvc;

namespace Mango.WebApp.Service
{
	public class UserService(IHttpFactory http)
	{
		private const string CONTROLLER_NAME = "User";

		private const string CLIENT_NAME = "AuthApi";

		public async Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request)
		{
			HttpResponse<LoginResponseDto> result = await http.PostAsync<LoginResponseDto, LoginRequestDto>(
				CLIENT_NAME, $"{CONTROLLER_NAME}/Login", new LoginRequestDto
				{
					Password = request.Password,
					UserName = request.UserName
				}
			);

			return result;
		}

		public async Task<ResultHelper<UserResponseDto>> RegisterAsync([FromBody] NewUserRequestDto request)
		{
			HttpResponse<UserResponseDto> result = await http.PostAsync<UserResponseDto, NewUserRequestDto>(
				CLIENT_NAME, $"{CONTROLLER_NAME}/New", new NewUserRequestDto
				{
					Email = request.Email,
					UserName = request.UserName,
					Password = request.Password,
					PhoneNumber = request.PhoneNumber
				}
			);

			return result;
		}
	}
}