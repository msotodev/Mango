using Mango.Services.CustomerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CustomerApi.Extensions
{
	public static class IServiceProviderExtension
	{
		public static void ApplyMigration(this IServiceProvider serviceProvider)
		{
			using IServiceScope scope = serviceProvider.CreateScope();

			AppDbContext dataBase = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			if (dataBase.Database.GetPendingMigrations().Any())
			{
				dataBase.Database.Migrate();
			}
		}
	}
}