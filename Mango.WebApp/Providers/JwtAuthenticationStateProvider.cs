using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.WebApp.Providers
{
	public class JwtAuthenticationStateProvider(IJSRuntime jsRuntime) : AuthenticationStateProvider
	{
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			string token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

			if (string.IsNullOrWhiteSpace(token)) return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

			ClaimsPrincipal identity = GetClaimsPrincipalFromJwt(token);

			return new AuthenticationState(identity);
		}

		public void NotifyUserAuthentication(string token)
		{
			ClaimsPrincipal identity = GetClaimsPrincipalFromJwt(token);
			Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(identity));

			NotifyAuthenticationStateChanged(authState);
		}

		public void NotifyUserLogout()
		{
			ClaimsPrincipal anonymous = new(new ClaimsIdentity());

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
		}

		private static ClaimsPrincipal GetClaimsPrincipalFromJwt(string jwt)
		{
			JwtSecurityTokenHandler handler = new();
			JwtSecurityToken token = handler.ReadJwtToken(jwt);

			ClaimsIdentity identity = new(token.Claims, "jwt");

			return new ClaimsPrincipal(identity);
		}
	}
}