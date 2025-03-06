namespace Mango.WebApp.Service.Token
{
	public interface ITokenService
	{
		string GetToken();

		void SetToken(string token);

		void RemoveToken();
	}
}