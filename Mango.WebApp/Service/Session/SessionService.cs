using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.WebApp.Service.Session
{
	public class SessionService(
		IHttpContextAccessor httpContextAccessor
	) : ISessionService
	{
		private ClaimsPrincipal? _claimsPrincipal;

		/**/

		public ResultHelper<string> GetEmail()
		{
			Claim sub = _claimsPrincipal!.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)!;

			if (sub.NotNull() && sub.Value.NotEmpty()) return ResultHelper<string>.Success(sub.Value);

			return ResultHelper<string>.Fail("The value of Email is empty, check your token generation");
		}

		public ResultHelper<string> GetName()
		{
			Claim sub = _claimsPrincipal!.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)!;

			if (sub.NotNull() && sub.Value.NotEmpty()) return ResultHelper<string>.Success(sub.Value);

			return ResultHelper<string>.Fail("The value of Name is empty, check your token generation");
		}

		public ResultHelper<string> GetSub()
		{
			Claim sub = _claimsPrincipal!.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)!;

			if (sub.NotNull() && sub.Value.NotEmpty()) return ResultHelper<string>.Success(sub.Value);

			return ResultHelper<string>.Fail("The value of Sub is empty, check your token generation");
		}

		public async Task<ResultHelper<bool>> SignInAsync(string token)
		{
			try
			{
				JwtSecurityTokenHandler jwtSecurityToken = new();
				JwtSecurityToken jwtToken = jwtSecurityToken.ReadJwtToken(token);
				ClaimsIdentity identity = new(CookieAuthenticationDefaults.AuthenticationScheme);

				Claim? email = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email);

				if (email.NotNull()) identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, email!.Value));

				Claim? sub = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

				if (sub.NotNull()) identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, sub!.Value));

				Claim? name = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name);

				if (name.NotNull()) identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, name!.Value));

				identity.AddClaim(
					new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)!.Value)
				);

				_claimsPrincipal = new(identity);

				await httpContextAccessor.HttpContext!.SignInAsync(_claimsPrincipal);

				return ResultHelper<bool>.Success(true);
			}
			catch (Exception e)
			{
				return ResultHelper<bool>.Fail(e);
			}
		}

		public async Task<ResultHelper<bool>> SignOutAsync()
		{
			try
			{
				_claimsPrincipal = null;

				await httpContextAccessor.HttpContext!.SignOutAsync();

				return ResultHelper<bool>.Success(true);
			}
			catch (Exception e)
			{
				return ResultHelper<bool>.Fail(e);
			}
		}
	}
}