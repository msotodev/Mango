using EssentialLayers.Request;
using EssentialLayers.Request.Helpers.Extension;
using Mango.ConsoleApp.App;
using Mango.ConsoleApp.Services.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mango.ConsoleApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(
				(context, services) =>
				{
					string settingsJson = "appsettings.json";

#if DEBUG
					settingsJson = "appsettings.development.json";
#endif

					IConfigurationRoot configuration = new ConfigurationBuilder()
						.SetBasePath(AppContext.BaseDirectory)
						.AddJsonFile(settingsJson, optional: false, reloadOnChange: true)
						.Build();

					services.ConfigureFactory();
					services.AddHttpClients(configuration);

					services.AddScoped<AuthService>();
					services.AddScoped<CategoryService>();
					services.AddScoped<ProductService>();
					services.AddScoped<HttpProductService>();

					services.AddSingleton<MainProduct>();
				}
			).Build();

			MainProduct? mainProduct = host.Services.GetService<MainProduct>();

			if (mainProduct == null) return;

			await mainProduct.InitAsync();
		}
	}
}