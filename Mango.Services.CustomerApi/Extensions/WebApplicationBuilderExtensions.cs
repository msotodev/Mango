using CommonLibrary.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Mango.Services.CustomerApi.Extensions
{
	public static class WebApplicationBuilderExtensions
	{
		public static void AddCustomSwaggerGen(this WebApplicationBuilder self)
		{
			self.Services.AddSwaggerGen(
				option =>
				{
					option.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(AuthApi), Version = "v1" });
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
			JwtOptions? jwtOptions = self.Configuration.GetSection("Jwt").Get<JwtOptions>();

			if (jwtOptions == null) return;

			self.Services.AddAuthentication(
				options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				}
			).AddJwtBearer(
				options =>
				{
					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = jwtOptions.Issuer,
						ValidAudience = jwtOptions.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key))
					};
				}
			);
		}
	}
}