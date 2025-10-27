namespace CommonLibrary.Dtos
{
	public class AssignRoleRequestDto
	{
		public string UserName { get; set; } = string.Empty;

		public int RoleId { get; set; }
	}

	public class AssignRoleResponseDto
	{
		public string UserName { get; set; } = string.Empty;

		public int RoleId { get; set; }
	}

	public class NewRoleRequestDto
	{
		public string Name { get; set; } = string.Empty;
	}

	public class NewRoleResponseDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}

	public class RoleResponseDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}
}