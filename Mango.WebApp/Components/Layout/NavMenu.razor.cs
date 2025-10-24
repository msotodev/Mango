using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Services.Factory;
using Mango.WebApp.Providers;
using Mango.WebApp.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Mango.WebApp.Components.Layout
{
	public partial class NavMenu
	{
		[Inject] private IFactoryTokenProvider TokenProvider { get; set; } = default!;

		[Inject] private IJSRuntime JSRuntime { get; set; } = default!;

		[Inject] private AuthenticationStateProvider AuthenticationState { get; set; } = default!;

		[Inject] private UserService UserService { get; set; } = default!;

		private async Task LoginAsync()
		{
			ResultHelper<LoginResponseDto> logged = await UserService.LoginAsync(
				new LoginRequestDto
				{
					UserName = "MSoto",
					Password = "Admin123_"
				}
			);

			if (logged.Ok)
			{
				TokenProvider.SetToken(logged.Data.Token);

				await JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", logged.Data.Token);

				JwtAuthenticationStateProvider authProvider = (JwtAuthenticationStateProvider)AuthenticationState;

				authProvider.NotifyUserAuthentication(logged.Data.Token);
			}
		}

		private async Task LogoutAsync()
		{
			await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

			JwtAuthenticationStateProvider authProvider = (JwtAuthenticationStateProvider)AuthenticationState;

			authProvider.NotifyUserLogout();
		}
	}
}