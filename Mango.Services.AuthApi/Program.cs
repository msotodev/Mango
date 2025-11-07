using CommonLibrary.Options;
using Mango.Services.AuthApi.Data;
using Mango.Services.AuthApi.Extensions;
using Mango.Services.AuthApi.Models;
using Mango.Services.AuthApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			ConfigurationManager configuration = builder.Configuration;

			builder.Services.AddDbContext<AppDbContext>(
				option =>
				{
					option.UseSqlServer(builder.Configuration.GetConnectionString("MangoAuth"));
				}
			);

			builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

			builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().
				AddEntityFrameworkStores<AppDbContext>().
				AddDefaultTokenProviders();

			builder.Services.AddScoped<RoleService>();
			builder.Services.AddScoped<TokenService>();
			builder.Services.AddScoped<UserService>();

			builder.Services.AddAuthorization();
			builder.AddCustomAuthentication();
			builder.AddCustomSwaggerGen();

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

			app.Services.ApplyMigration();

			app.Run();
		}
	}
}