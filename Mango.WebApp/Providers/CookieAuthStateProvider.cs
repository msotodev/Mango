using CommonLibrary.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.WebApp.Providers
{
	public class CookieAuthStateProvider(
		IHttpContextAccessor httpContextAccessor
	) : AuthenticationStateProvider
	{
		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			try
			{
				if (httpContextAccessor.HttpContext!.Request.Cookies.ContainsKey(Defaults.TokenCookie))
				{
					string? token = httpContextAccessor.HttpContext.Request.Cookies[Defaults.TokenCookie];

					JwtSecurityTokenHandler handler = new();
					JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken;
					List<Claim> claims = [.. jsonToken!.Claims];
					ClaimsIdentity claimsIdentity = new(claims, "jwt");

					return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(claimsIdentity)));
				}
				return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
			}
			catch (Exception)
			{
				return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
			}
		}
	}
}