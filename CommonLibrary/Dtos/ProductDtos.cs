namespace CommonLibrary.Dtos
{
	public class DeleteProductRequestDto
	{
		public int Id { get; set; }

		public int UserId { get; set; }
	}

	public class NewProductRequestDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public int CategoryId { get; set; }

		public string ImageUrl { get; set; } = string.Empty;

		public int UserId { get; set; }
	}

	public class QueryProductRequestDto
	{
		public int Id { get; set; }
	}

	public class QueryProductResultDto : ResultDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public double Price { get; set; }

		public string CategoryName { get; set; } = string.Empty;

		public string ImageUrl { get; set; } = string.Empty;
	}

	public class UpdateProductRequestDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public int CategoryId { get; set; }

		public string ImageUrl { get; set; } = string.Empty;

		public int UserId { get; set; }
	}
}