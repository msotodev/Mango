using Microsoft.AspNetCore.Components;

namespace Mango.WebApp.Components.Pages
{
	public partial class MainPage
	{
		[Inject] private NavigationManager NavigationManager { get; set; } = default!;

		private void HandleLogin() => NavigationManager.NavigateTo("/login");
	}
}