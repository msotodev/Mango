using EssentialLayers.Request;
using EssentialLayers.Request.Models;
using Mango.WebApp.Components;
using Mango.WebApp.Service;
using Mango.WebApp.Service.Coupon;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.UseRequest();

builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IServiceBase, ServiceBase>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

IConfiguration? configuration = app.Services.GetService<IConfiguration>();

app.Services.ConfigureRequest(
	new HttpOption
	{
		BaseUri	= configuration?.GetSection("ServicesUrls").GetValue<string>("CouponApi")!,
	}
);

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();