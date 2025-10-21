using EssentialLayers.Request;
using Mango.WebApp.Components;
using Mango.WebApp.Service;
using Mango.WebApp.Service.Auth;
using Mango.WebApp.Service.Coupon;
using Mango.WebApp.Service.Session;
using Mango.WebApp.Service.Token;
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddHttpClient("GitHub", c => {
	c.BaseAddress = new Uri("https://api.github.com/");
	c.DefaultRequestHeaders.UserAgent.ParseAdd("MyApp/1.0");
});

builder.Services.UseRequest();
builder.Services.AddMudServices();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IServiceBase, ServiceBase>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICouponService, CouponService>();

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