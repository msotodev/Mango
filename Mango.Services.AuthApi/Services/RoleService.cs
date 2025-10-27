using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Helpers.Result;
using Mango.Services.AuthApi.Data;
using Mango.Services.AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthApi.Services
{
	public class RoleService(
		AppDbContext dbContext,
		RoleManager<ApplicationRole> roleManager,
		UserManager<ApplicationUser> userManager
	)
	{
		public async Task<ResultHelper<AssignRoleResponseDto>> AssignAsync(AssignRoleRequestDto request)
		{
			ApplicationUser? user = dbContext.Users.FirstOrDefault(x => x.UserName == request.UserName);

			if (user == null) return ResultHelper<AssignRoleResponseDto>.Fail($"The user '{request.UserName}' doesn't exists");

			ApplicationRole? role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId);

			if (role == null) return ResultHelper<AssignRoleResponseDto>.Fail("The role doesn't exists");

			if (role.Name == null) return ResultHelper<AssignRoleResponseDto>.Fail("Role name is empty");

			await userManager.AddToRoleAsync(user, role.Name);

			return ResultHelper<AssignRoleResponseDto>.Success(
				new AssignRoleResponseDto
				{
					RoleId = role.Id,
					UserName = user.UserName ?? string.Empty
				}
			);
		}

		public async Task<ResultHelper<NewRoleResponseDto>> NewAsync(NewRoleRequestDto request)
		{
			bool exists = await roleManager.RoleExistsAsync(request.Name);

			if (exists.False())
			{
				await roleManager.CreateAsync(new ApplicationRole { Name = request.Name });
			}

			ApplicationRole role = roleManager.Roles.First(x => x.Name == request.Name);

			return ResultHelper<NewRoleResponseDto>.Success(
				new NewRoleResponseDto
				{
					Id = role.Id,
					Name = role.Name ?? string.Empty
				}
			);
		}

		public async Task<IList<RoleResponseDto>> GetByUserIdAsync(string userId)
		{
			ApplicationUser? user = await userManager.FindByIdAsync(userId);

			if (user == null) return [];

			IList<string> roles = await userManager.GetRolesAsync(user);

			return [.. roles.Select(roleName => new RoleResponseDto { Name = roleName })];
		}
	}
}