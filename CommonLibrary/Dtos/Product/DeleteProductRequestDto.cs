namespace CommonLibrary.Dtos.Product
{
	public class DeleteProductRequestDto
	{
		public int Id { get; set; }

		public string UserId { get; set; } = string.Empty;
	}
}