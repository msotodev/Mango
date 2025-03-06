using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Mango.Services.ProductApi.Helpers.Extensions
{
	public static class WebApplicationBuilderExtensions
	{
		public static void AddCustomSwaggerGen(this WebApplicationBuilder self)
		{
			self.Services.AddSwaggerGen(
				option =>
				{
					option.SwaggerDoc("v1", new OpenApiInfo { Title = "Mango.Services.CouponApi", Version = "v1" });
					option.AddSecurityDefinition("Bearer",
						new OpenApiSecurityScheme
						{
							Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
							In = ParameterLocation.Header,
							Type = SecuritySchemeType.ApiKey,
							Name = "Authorization",
							Scheme = JwtBearerDefaults.AuthenticationScheme
						}
					);
					option.AddSecurityRequirement(
						new OpenApiSecurityRequirement
						{
							{
								new OpenApiSecurityScheme
								{
									Reference = new OpenApiReference
									{
										Type = ReferenceType.SecurityScheme,
										Id = JwtBearerDefaults.AuthenticationScheme
									}
								},
								[]
							}
						}
					);
				}
			);
		}

		public static void AddCustomAuthentication(this WebApplicationBuilder self)
		{
			IConfigurationSection jwtOptions = self.Configuration.GetSection("JwtOptions");

			string audience = jwtOptions?.GetValue<string>("Audience")!;
			string issuer = jwtOptions?.GetValue<string>("Issuer")!;
			string secret = jwtOptions?.GetValue<string>("Secret")!;

			self.Services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}
			).AddJwtBearer(
				x =>
				{
					x.RequireHttpsMetadata = false;
					x.SaveToken = true;
					x.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
						ValidateIssuer = true,
						ValidIssuer = issuer,
						ValidateAudience = true,
						ValidAudience = audience,
						ValidateLifetime = true
					};
				}
			);
		}
	}
}