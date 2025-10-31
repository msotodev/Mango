using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Result;
using EssentialLayers.Request.Services.Factory;
using Mango.WebApp.Service;
using Microsoft.AspNetCore.Components;

namespace Mango.WebApp.Components.Pages.Identity
{
	public partial class LoginPage
	{
		[Inject] private IFactoryTokenProvider TokenProvider { get; set; } = default!;

		//[Inject] private LocalStorageSessionService SessionService { get; set; } = default!;

		[Inject] private UserService UserService { get; set; } = default!;

		[Inject] private NavigationManager NavigationManager { get; set; } = default!;

		[SupplyParameterFromForm] public LoginRequestDto LoginModel { get; set; } = new();

		public string Message { get; set; } = string.Empty;

		private async Task HandleLoginAsync()
		{
			ResultHelper<LoginResponseDto> logged = await UserService.LoginAsync(LoginModel);

			if (logged.Ok)
			{
				string token = logged.Data.Token;

				TokenProvider.SetToken(token);

				NavigationManager.NavigateTo($"/loginpost?Token={token}");
			}
			else
			{
				Message = logged.Message;
			}
		}
	}
}