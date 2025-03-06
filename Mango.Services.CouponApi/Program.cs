using AutoMapper;
using EssentialLayers.Dapper;
using Mango.Services.CouponApi.Helpers;
using Mango.Services.CouponApi.Helpers.Extensions;

namespace Mango.Services.CouponApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();

			builder.AddCustomSwaggerGen();
			builder.AddCustomAuthentication();

			builder.Services.AddAuthorization();
			builder.Services.UseDapper();

			IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
			builder.Services.AddSingleton(mapper);
			//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			WebApplication app = builder.Build();

			IConfiguration? configuration = app.Services.GetService<IConfiguration>();

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