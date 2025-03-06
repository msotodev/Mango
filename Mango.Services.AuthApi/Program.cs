using Mango.Services.AuthApi.Data;
using Mango.Services.AuthApi.Models;
using Mango.Services.AuthApi.Services.Auth;
using Mango.Services.AuthApi.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<AppDbContext>(
				option =>
				{
					option.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
				}
			);

			builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

			builder.Services.AddIdentity<AppUser, IdentityRole>().
				AddEntityFrameworkStores<AppDbContext>().
				AddDefaultTokenProviders();

			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<ITokenService, TokenService>();

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			WebApplication app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			ApplyMigration(app.Services);

			app.Run();
		}

		private static void ApplyMigration(IServiceProvider serviceProvider)
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