using Mango.Services.AuthApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthApi.Services.Token
{
	public class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
	{
		private readonly JwtOptions _jwtOptions = jwtOptions.Value;

		/**/

		public string Generate(AppUser appUser, IEnumerable<string> roles)
		{
			JwtSecurityTokenHandler tokenHandler = new();

			byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

			List<Claim> claims =
			[
				new Claim(JwtRegisteredClaimNames.Sub, appUser.Id!),
				new Claim(JwtRegisteredClaimNames.Email, appUser.Email!),
				new Claim(JwtRegisteredClaimNames.Name, appUser.UserName!)
			];

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Audience = _jwtOptions.Audience,
				Issuer = _jwtOptions.Issuer,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
				)
			};

			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}