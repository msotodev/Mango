using Mango.Services.AuthApi.Models;

namespace Mango.Services.AuthApi.Services.Token
{
	public interface ITokenService
	{
		string Generate(AppUser appUser, IEnumerable<string> roles);
	}
}