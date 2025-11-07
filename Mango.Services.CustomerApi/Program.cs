using CommonLibrary.Options;
using Domain.Interfaces.Customers;
using Mango.Services.CustomerApi.Data;
using Mango.Services.CustomerApi.Extensions;
using Mango.Services.CustomerApi.Repositories;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(
	option =>
	{
		option.UseSqlServer(configuration.GetConnectionString("MangoCustomer"));
	}
);
builder.Services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();
builder.AddCustomAuthentication();
builder.AddCustomSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Services.ApplyMigration();

app.Run();