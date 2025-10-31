using CommonLibrary.Options;
using EssentialLayers.Helpers.Result;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthApi.Services
{
	public class TokenService(IOptions<JwtOptions> jwtOptions)
	{
		private readonly JwtOptions _jwtOptions = jwtOptions.Value;

		public ResultHelper<string> Generate(
			string id, string email, string userName, IEnumerable<string> roles
		)
		{
			JwtSecurityTokenHandler tokenHandler = new();

			byte[] key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

			List<Claim> claims =
			[
				new Claim(JwtRegisteredClaimNames.Sub, id),
				new Claim(JwtRegisteredClaimNames.Email, email),
				new Claim(JwtRegisteredClaimNames.Name, userName)
			];

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Audience = _jwtOptions.Audience,
				Issuer = _jwtOptions.Issuer,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
				)
			};

			SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

			string token = tokenHandler.WriteToken(securityToken);

			return ResultHelper<string>.Success(token);
		}
	}
}