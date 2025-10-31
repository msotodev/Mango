using CommonLibrary.Options;
using EssentialLayers.Dapper;
using Mango.Services.CouponApi.Extensions;

namespace Mango.Services.CouponApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			ConfigurationManager configuration = builder.Configuration;

			builder.Services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddAuthorization();
			builder.AddCustomAuthentication();
			builder.AddCustomSwaggerGen();

			builder.Services.UseDapper();

			WebApplication app = builder.Build();

			string? connectionString = configuration?.GetConnectionString("Local");

			app.Services.ConfigureDapper(connectionString!);

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}