using CommonLibrary.Dtos.Auth;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Mango.Services.AuthApi.Data;
using Mango.Services.AuthApi.Models;
using Mango.Services.AuthApi.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthApi.Services.Auth
{
	public class AuthService(
		AppDbContext dbContext,
		RoleManager<IdentityRole> roleManager,
		UserManager<AppUser> userManager,
		ITokenService tokenService
	) : IAuthService
	{
		private readonly ITokenService _tokenService = tokenService;

		/**/

		private readonly AppDbContext _dbContext = dbContext;

		private readonly RoleManager<IdentityRole> _roleManager = roleManager;

		private readonly UserManager<AppUser> _userManager = userManager;

		/**/

		public async Task<ResultHelper<bool>> AssignRoleAsync(AssignRoleRequestDto request)
		{
			AppUser? user = _dbContext.AppUser.FirstOrDefault(x => x.Email == request.Email);

			if (user.IsNull()) return ResultHelper<bool>.Fail($"The user '{request.Email}' doesn't exists");

			string role = request.Role.ToUpper();

			bool exists = await _roleManager.RoleExistsAsync(role);

			if (exists.False())
			{
				await _roleManager.CreateAsync(new IdentityRole(role));
			}

			await _userManager.AddToRoleAsync(user!, role);

			return ResultHelper<bool>.Success(true);
		}

		public async Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request)
		{
			AppUser? user = await _dbContext.AppUser.FirstOrDefaultAsync(u => u.UserName == request.UserName);

			if (user.NotNull())
			{
				bool isValid = await _userManager.CheckPasswordAsync(user!, request.Password);

				if (isValid.False()) return ResultHelper<LoginResponseDto>.Fail(
					"Username or password is incorrect"
				);

				IList<string> roles = await _userManager.GetRolesAsync(user!);
				string token = _tokenService.Generate(user!, roles);

				return ResultHelper<LoginResponseDto>.Success(
					new LoginResponseDto
					{
						User = new UserDto
						{
							Email = user!.Email!,
							Id = user.Id,
							Name = user.Name,
							PhoneNumber = user.PhoneNumber!
						},
						Token = token
					}
				);
			}

			return ResultHelper<LoginResponseDto>.Fail("The user doesn't exists");
		}

		public async Task<ResultHelper<UserDto>> RegisterAsync(RegisterRequestDto request)
		{
			AppUser appUser = new()
			{
				UserName = request.Email,
				PasswordHash = request.Password,
				Email = request.Email,
				NormalizedEmail = request.Email,
				PhoneNumber = request.PhoneNumber,
				Name = request.Name
			};

			try
			{
				IdentityResult created = await _userManager.CreateAsync(appUser, request.Password);

				if (created.Succeeded)
				{
					AppUser createdUser = _dbContext.AppUser.FirstOrDefault(u => u.Email == request.Email)!;

					return ResultHelper<UserDto>.Success(
						new UserDto
						{
							Id = createdUser.Id,
							Email = createdUser.Email!,
							Name = createdUser.Name,
							PhoneNumber = createdUser.PhoneNumber!,
						}
					);
				}

				return ResultHelper<UserDto>.Fail(created.Errors.First().Description);
			}
			catch (Exception e)
			{
				return ResultHelper<UserDto>.Fail(e);
			}
		}
	}
}