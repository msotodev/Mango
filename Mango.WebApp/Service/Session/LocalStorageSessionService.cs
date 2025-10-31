using EssentialLayers.Helpers.Result;
using Mango.WebApp.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Mango.WebApp.Service.Session
{
	public class LocalStorageSessionService(
		AuthenticationStateProvider AuthenticationState,
		IJSRuntime JSRuntime,
		ILogger<LocalStorageSessionService> logger
	) : ISessionService
	{
		public async Task<Response> SignInAsync(string token)
		{
			try
			{
				await JSRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);

				JwtAuthStateProvider authProvider = (JwtAuthStateProvider)AuthenticationState;

				authProvider.NotifyUserAuthentication(token);

				return Response.Success();
			}
			catch (Exception e)
			{
				logger.LogError("Error {Message}", e.Message);

				return Response.Fail(e.Message);
			}
		}

		public async Task<Response> SignOutAsync()
		{
			try
			{
				await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");

				JwtAuthStateProvider authProvider = (JwtAuthStateProvider)AuthenticationState;

				authProvider.NotifyUserLogout();

				return Response.Success();
			}
			catch (Exception e)
			{
				logger.LogError("Error {Message}", e.Message);

				return Response.Fail(e.Message);
			}
		}
	}
}