using Mango.Services.AuthApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(option =>
	{
		option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
	}
);
builder.Services.AddIdentity<IdentityUser, IdentityRole>().
	AddEntityFrameworkStores<AppDbContext>().
	AddDefaultTokenProviders();

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

ApplyMigration();

app.Run();
void ApplyMigration()
{
	using IServiceScope scope = app.Services.CreateScope();
	AppDbContext dataBase = scope.ServiceProvider.GetRequiredService<AppDbContext>();

	if (dataBase.Database.GetPendingMigrations().Any())
	{
		dataBase.Database.Migrate();
	}
}