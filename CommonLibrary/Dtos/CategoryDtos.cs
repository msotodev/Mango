namespace CommonLibrary.Dtos
{
	public class DeleteCategoryRequestDto
	{
		public int Id { get; set; }

		public int UserId { get; set; }
	}

	public class NewCategoryRequestDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int UserId { get; set; }
	}

	public class QueryCategoryRequestDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}

	public class QueryCategoryResultDto : ResultDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;
	}

	public class UpdateCategoryRequestDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public int UserId { get; set; }
	}
}