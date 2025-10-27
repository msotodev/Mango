using EssentialLayers.Helpers.Result;

namespace Mango.WebApp.Service.Session
{
	public interface ISessionService
	{
		Task<Response> SignInAsync(string token);

		Task<Response> SignOutAsync();
	}
}