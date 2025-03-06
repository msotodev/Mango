using CommonLibrary.Constants;

namespace Mango.WebApp.Service.Token
{
	public class TokenService(IHttpContextAccessor httpContextAccessor) : ITokenService
	{
		public string GetToken()
		{
			if (httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(Defaults.TokenCookie, out string? jwtToken) == true)
			{
				return jwtToken ?? string.Empty;
			}

			return string.Empty;
		}

		public void RemoveToken()
		{
			httpContextAccessor.HttpContext?.Response.Cookies.Delete(Defaults.TokenCookie);
		}

		public void SetToken(string token)
		{
			httpContextAccessor.HttpContext?.Response.Cookies.Append(Defaults.TokenCookie, token);
		}
	}
}