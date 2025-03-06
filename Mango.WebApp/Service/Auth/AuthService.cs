using CommonLibrary.Dtos.Auth;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Models;
using EssentialLayers.Request.Services.Http;
using Mango.WebApp.Service.Session;
using Mango.WebApp.Service.Token;
using Microsoft.AspNetCore.Mvc;

namespace Mango.WebApp.Service.Auth
{
	public class AuthService : IAuthService
	{
		private const string CONTROLLER_NAME = "Auth";
		/**/

		private readonly IHttpService _httpService;

		private readonly ISessionService _sessionService;

		private readonly ITokenService _tokenService;

		/**/

		private readonly string _token = string.Empty;

		/**/

		public AuthService(
			IConfiguration configuration,
			IHttpService httpService,
			ISessionService sessionService,
			ITokenService tokenService
		)
		{
			_httpService = httpService;
			_sessionService = sessionService;
			_tokenService = tokenService;

			httpService.SetOptions(
				new HttpOption
				{
					BaseUri = configuration?.GetSection("ServicesUrls").GetValue<string>("AuthApi")!,
					CastResultAsResultHelper = true
				}
			);
		}

		public string Token => _token;

		/**/

		public async Task<ResultHelper<bool>> AssignRoleAsync(AssignRoleRequestDto request)
		{
			HttpResponse<bool> result = await _httpService.PostAsync<bool, AssignRoleRequestDto>(
				new AssignRoleRequestDto
				{
					Email = request.Email,
					Role = request.Role,
				}, $"{CONTROLLER_NAME}/AssignRole"
			);

			return result;
		}

		public async Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request)
		{
			HttpResponse<LoginResponseDto> result = await _httpService.PostAsync<LoginResponseDto, LoginRequestDto>(
				new LoginRequestDto
				{
					Password = request.Password,
					UserName = request.UserName
				}, $"{CONTROLLER_NAME}/Login"
			);

			if (result.Ok && result.Data.Token.NotEmpty())
			{
				_tokenService.SetToken(result.Data.Token);

				ResultHelper<bool> signInResult = await _sessionService.SignInAsync(result.Data.Token);

				if (signInResult.Ok.False()) return ResultHelper<LoginResponseDto>.Fail(signInResult.Message);
			}

			return result;
		}

		public async Task<ResultHelper<UserDto>> RegisterAsync([FromBody] RegisterRequestDto request)
		{
			HttpResponse<UserDto> result = await _httpService.PostAsync<UserDto, RegisterRequestDto>(
				new RegisterRequestDto
				{
					Email = request.Email,
					Name = request.Name,
					Password = request.Password,
					PhoneNumber = request.PhoneNumber,
					Role = request.Role,
				}, $"{CONTROLLER_NAME}/Register"
			);

			return result;
		}
	}
}