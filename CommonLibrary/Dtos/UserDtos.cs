namespace CommonLibrary.Dtos
{
	public class LoginRequestDto
	{
		public string UserName { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;
	}

	public class LoginResponseDto
	{
		public UserResponseDto? User { get; set; }

		public string Token { get; set; } = string.Empty;
	}

	public class NewUserRequestDto
	{
		public string Email { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		public string PhoneNumber { get; set; } = string.Empty;
	}

	public class UserResponseDto
	{
		public int Id { get; set; }

		public string Email { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;

		public string PhoneNumber { get; set; } = string.Empty;
	}
}