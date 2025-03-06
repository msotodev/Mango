using EssentialLayers.Helpers.Result;

namespace Mango.WebApp.Service.Session
{
	public interface ISessionService
	{
		Task<ResultHelper<bool>> SignInAsync(string token);

		Task<ResultHelper<bool>> SignOutAsync();

		ResultHelper<string> GetEmail();

		ResultHelper<string> GetName();

		ResultHelper<string> GetSub();
	}
}