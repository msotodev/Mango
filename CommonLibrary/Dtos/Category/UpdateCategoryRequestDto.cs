namespace CommonLibrary.Dtos.Category
{
	public class UpdateCategoryRequestDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public string UserId { get; set; } = string.Empty;
	}
}