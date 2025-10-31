using EssentialLayers.Request;
using EssentialLayers.Request.Helpers.Extension;
using Mango.WebApp.Components;
using Mango.WebApp.Providers;
using Mango.WebApp.Service;
using Mango.WebApp.Service.Session;
using Microsoft.AspNetCore.Components.Authorization;

internal class Program
{
	private static void Main(string[] args)
	{
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

		builder.Services.AddRazorComponents().AddInteractiveServerComponents();

		builder.Services.AddCascadingAuthenticationState();
		//builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
		builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthStateProvider>();
		builder.Services.AddHttpContextAccessor();

		builder.Services.AddHttpClients(builder.Configuration);
		builder.Services.ConfigureFactory();

		builder.Services.AddScoped<CookieSessionService>();
		builder.Services.AddScoped<CategoryService>();
		builder.Services.AddScoped<CouponService>();
		builder.Services.AddScoped<ProductService>();
		builder.Services.AddScoped<RoleService>();
		builder.Services.AddScoped<UserService>();
		//builder.Services.AddScoped<LocalStorageSessionService>();

		WebApplication app = builder.Build();

		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error", createScopeForErrors: true);
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseAntiforgery();

		app.MapStaticAssets();
		app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

		app.Run();
	}
}