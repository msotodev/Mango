using CommonLibrary.Dtos.Auth;
using EssentialLayers.Helpers.Result;
using Mango.WebApp.Service.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Mango.WebApp.Components.Pages
{
	public partial class Home
	{
		[Inject] private AuthenticationStateProvider AuthenticationState { get; set; }

		[Inject] private IAuthService AuthService { get; set; }

		/**/

		public string Authorized { get; set; } = string.Empty;

		/**/

		protected override async Task OnInitializedAsync()
		{
			AuthenticationState authenticationState = await AuthenticationState.GetAuthenticationStateAsync();

			ResultHelper<LoginResponseDto> logged = await AuthService.LoginAsync(
				new LoginRequestDto
				{
					UserName = "mariosotomor@gmail.com",
					Password = "0965cf943F."
				}
			);

			Authorized = logged.Ok && authenticationState.User.Identity!.IsAuthenticated ? "Autorizado" : "No Autorizado";
		}
	}
}