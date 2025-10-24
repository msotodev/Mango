using CommonLibrary.Dtos;
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
		RoleManager<AppRole> roleManager,
		UserManager<AppUser> userManager,
		ITokenService tokenService
	) : IAuthService
	{
		private readonly ITokenService _tokenService = tokenService;

		/**/

		private readonly AppDbContext _dbContext = dbContext;

		private readonly RoleManager<AppRole> _roleManager = roleManager;

		private readonly UserManager<AppUser> _userManager = userManager;

		/**/

		public async Task<ResultHelper<AssignRoleResponseDto>> AssignRoleAsync(AssignRoleRequestDto request)
		{
			AppUser? user = _dbContext.AppUser.FirstOrDefault(x => x.UserName == request.UserName);

			if (user == null) return ResultHelper<AssignRoleResponseDto>.Fail($"The user '{request.UserName}' doesn't exists");

			AppRole? role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId);

			if (role == null) return ResultHelper<AssignRoleResponseDto>.Fail("The role doesn't exists");

			if (role.Name == null) return ResultHelper<AssignRoleResponseDto>.Fail("Role name is empty");

			await _userManager.AddToRoleAsync(user, role.Name);

			return ResultHelper<AssignRoleResponseDto>.Success(
				new AssignRoleResponseDto
				{
					RoleId = role.Id,
					UserName = user.UserName ?? string.Empty
				}
			);
		}

		public async Task<ResultHelper<NewRoleResponseDto>> NewRole(NewRoleRequestDto request)
		{
			bool exists = await _roleManager.RoleExistsAsync(request.Name);

			if (exists.False())
			{
				await _roleManager.CreateAsync(new AppRole { Name = request.Name });
			}

			AppRole role = _roleManager.Roles.First(x => x.Name == request.Name);

			return ResultHelper<NewRoleResponseDto>.Success(
				new NewRoleResponseDto
				{
					Id = role.Id,
					Name = role.Name ?? string.Empty
				}
			);
		}

		public async Task<ResultHelper<LoginResponseDto>> LoginAsync(LoginRequestDto request)
		{
			AppUser? user = await _dbContext.AppUser.FirstOrDefaultAsync(u => u.UserName == request.UserName);

			if (user != null)
			{
				bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);

				if (isValid.False()) return ResultHelper<LoginResponseDto>.Fail(
					"Username or password is incorrect"
				);

				IList<string> roles = await _userManager.GetRolesAsync(user);

				string token = _tokenService.Generate(user, roles);

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
						Token = token
					}
				);
			}

			return ResultHelper<LoginResponseDto>.Fail("The user doesn't exists");
		}

		public async Task<ResultHelper<UserResponseDto>> RegisterAsync(NewUserRequestDto request)
		{
			AppUser appUser = new()
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
					AppUser? createdUser = _dbContext.AppUser.FirstOrDefault(u => u.Email == request.Email);

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