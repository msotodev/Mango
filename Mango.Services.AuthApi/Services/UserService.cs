using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Mango.Services.AuthApi.Data;
using Mango.Services.AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthApi.Services
{
	public class UserService(
		AppDbContext dbContext,
		UserManager<ApplicationUser> userManager
	)
	{
		private readonly AppDbContext _dbContext = dbContext;

		private readonly UserManager<ApplicationUser> _userManager = userManager;

		public async Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request)
		{
			ApplicationUser? user = await _dbContext.AppUser.FirstOrDefaultAsync(u => u.UserName == request.UserName);

			if (user != null)
			{
				bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);

				if (isValid.False()) return ResultHelper<LoginResponseDto>.Fail(
					"Username or password is incorrect"
				);

				IList<string> roles = await _userManager.GetRolesAsync(user);

				return ResultHelper<LoginResponseDto>.Success(
					new LoginResponseDto
					{
						User = new UserResponseDto
						{
							Email = user.Email ?? string.Empty,
							Id = user.Id,
							Name = user.UserName ?? string.Empty,
							PhoneNumber = user.PhoneNumber!
						},
						Token = string.Empty
					}
				);
			}

			return ResultHelper<LoginResponseDto>.Fail("The user doesn't exists");
		}

		public async Task<ResultHelper<UserResponseDto>> NewAsync(NewUserRequestDto request)
		{
			ApplicationUser appUser = new()
			{
				UserName = request.UserName,
				PasswordHash = request.Password,
				Email = request.Email,
				NormalizedEmail = request.Email,
				PhoneNumber = request.PhoneNumber
			};

			try
			{
				IdentityResult created = await _userManager.CreateAsync(appUser, request.Password);

				if (created.Succeeded)
				{
					ApplicationUser? createdUser = _dbContext.AppUser.FirstOrDefault(u => u.Email == request.Email);

					if (createdUser == null) return ResultHelper<UserResponseDto>.Fail("The user created is null");

					return ResultHelper<UserResponseDto>.Success(
						new UserResponseDto
						{
							Id = createdUser.Id,
							Email = createdUser.Email ?? string.Empty,
							Name = createdUser.UserName ?? string.Empty,
							PhoneNumber = createdUser.PhoneNumber!,
						}
					);
				}

				return ResultHelper<UserResponseDto>.Fail(created.Errors.First().Description);
			}
			catch (Exception e)
			{
				return ResultHelper<UserResponseDto>.Fail(e);
			}
		}
	}
}