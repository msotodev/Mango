using EssentialLayers.Request;
using EssentialLayers.Request.Helpers.Extension;
using Mango.WebApp.Components;
using Mango.WebApp.Providers;
using Mango.WebApp.Service;
using Mango.WebApp.Service.Session;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

internal class Program
{
	private static void Main(string[] args)
	{
		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

		builder.Services.AddRazorComponents().AddInteractiveServerComponents();

		builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
		builder.Services.AddAuthorizationCore();

		builder.Services.AddHttpContextAccessor();
		builder.Services.AddHttpClient();

		builder.Services.AddHttpClient("GitHub", c =>
		{
			c.BaseAddress = new Uri("https://api.github.com/");
			c.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0");
		});

		builder.Services.AddMudServices();

		builder.Services.AddHttpClients(builder.Configuration);
		builder.Services.ConfigureFactory();

		builder.Services.AddScoped<ISessionService, SessionService>();
		builder.Services.AddScoped<CategoryService>();
		builder.Services.AddScoped<CouponService>();
		builder.Services.AddScoped<ProductService>();
		builder.Services.AddScoped<RoleService>();
		builder.Services.AddScoped<UserService>();

		builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
			option =>
			{
				option.ExpireTimeSpan = TimeSpan.FromHours(10);
				option.LoginPath = "/Auth/Login";
				option.AccessDeniedPath = "/Auth/AccessDenied";
			}
		);

		WebApplication app = builder.Build();

		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error", createScopeForErrors: true);
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseStaticFiles();
		app.UseAntiforgery();

		app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

		app.Run();
	}
}