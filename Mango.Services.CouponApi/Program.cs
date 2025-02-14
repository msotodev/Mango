using AutoMapper;
using EssentialLayers.Dapper;
using Mango.Services.CouponApi.Helpers;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.UseDapper();

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

WebApplication app = builder.Build();

IConfiguration? configuration = app.Services.GetService<IConfiguration>();
string? connectionString = configuration?.GetConnectionString("MangoCoupon");

app.Services.ConfigureDapper(connectionString!);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();