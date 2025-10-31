using EssentialLayers.Helpers.Result;
using Mango.WebApp.Service.Session;
using Microsoft.AspNetCore.Components;

namespace Mango.WebApp.Components.Pages.Identity
{
	public partial class LoginPostPage
	{
		[Inject] private NavigationManager NavigationManager { get; set; } = default!;

		[Inject] private CookieSessionService CookieSessionService { get; set; } = default!;

		[SupplyParameterFromQuery] private string? Token { get; set; }

		protected override void OnInitialized()
		{
			if (string.IsNullOrEmpty(Token))
			{
				NavigationManager.NavigateTo("/login");
			}
			else
			{
				Response result = CookieSessionService.SignInAsync(Token).Result;

				if (result.Ok)
				{
					NavigationManager.NavigateTo("/home");
				}
			}
		}
	}
}