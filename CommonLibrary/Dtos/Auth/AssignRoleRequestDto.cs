namespace CommonLibrary.Dtos.Auth
{
	public class AssignRoleRequestDto
	{
		public string Email { get; set; } = string.Empty;

		public string Role { get; set; } = string.Empty;
	}
}