namespace CommonLibrary.Dtos.Product
{
	public class QueryProductResultDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public double Price { get; set; }

		public string CategoryName { get; set; } = string.Empty;

		public string ImageUrl { get; set; } = string.Empty;
	}
}