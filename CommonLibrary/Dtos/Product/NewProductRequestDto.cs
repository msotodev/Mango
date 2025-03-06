namespace CommonLibrary.Dtos.Product
{
	public class NewProductRequestDto
	{
		public string Name { get; set; } = string.Empty;

		public decimal Price { get; set; }

		public int CategoryId { get; set; }

		public string ImageUrl { get; set; } = string.Empty;

		public string UserId { get; set; } = string.Empty;
	}
}