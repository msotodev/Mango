using CommonLibrary.Constants;
using EssentialLayers.Helpers.Result;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.WebApp.Service.Session
{
	public class CookieSessionService(
		IHttpContextAccessor httpContextAccessor,
		ILogger<CookieSessionService> logger
	) : ISessionService
	{
		public Task<Response> SignInAsync(string token)
		{
			try
			{
				if (httpContextAccessor.HttpContext == null) return Task.FromResult(
					Response.Fail($"{nameof(httpContextAccessor)} is null")
				);

				httpContextAccessor.HttpContext.Response.Cookies.Append(
					Defaults.TokenCookie, token,
					new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.Strict,
						Expires = DateTime.UtcNow.AddHours(1)
					}
				);

				return Task.FromResult(Response.Success());
			}
			catch (Exception e)
			{
				logger.LogError(e, "Sign-in failed");

				return Task.FromResult(Response.Fail(e.Message));
			}
		}

		public Task<Response> SignOutAsync()
		{
			try
			{
				httpContextAccessor.HttpContext!.Response.Cookies.Delete(Defaults.TokenCookie);

				return Task.FromResult(Response.Success());
			}
			catch (Exception e)
			{
				logger.LogError(e, "Sign-out failed");

				return Task.FromResult(Response.Fail(e.Message));
			}
		}

		private ClaimsPrincipal GetClaimsPrincipal(string token)
		{
			if (token == null) return new ClaimsPrincipal(new ClaimsIdentity());

			JwtSecurityTokenHandler handler = new();

			if (handler.ReadToken(token) is not JwtSecurityToken jsonToken) return new ClaimsPrincipal(new ClaimsIdentity());

			List<Claim> claims = [.. jsonToken.Claims];
			ClaimsIdentity claimsIdentity = new(claims, "jwt");

			return new(new ClaimsIdentity(claimsIdentity));
		}
	}
}