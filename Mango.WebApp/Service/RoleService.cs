using CommonLibrary.Dtos;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;

namespace Mango.WebApp.Service
{
	public class RoleService(IHttpFactory http)
	{
		private const string CONTROLLER_NAME = "Role";

		private const string CLIENT_NAME = "AuthApi";

		public async Task<HttpResponse<AssignRoleResponseDto>> AssignRoleAsync(AssignRoleRequestDto request)
		{
			HttpResponse<AssignRoleResponseDto> result = await http.PostAsync<AssignRoleResponseDto, AssignRoleRequestDto>(
				CLIENT_NAME, $"{CONTROLLER_NAME}/AssignRole", new AssignRoleRequestDto
				{
					UserName = request.UserName,
					RoleId = request.RoleId,
				}
			);

			return result;
		}
	}
}