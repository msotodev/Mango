using System;

namespace CommonLibrary.Dtos
{
	public class NewCustomerRequestDto
	{
		public string Name { get; set; } = string.Empty;

		public DateTime Birthday { get; set; }

		public int UserId { get; set; }
	}

	public class QueryCustomerResultDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = string.Empty;

		public DateTime Birthday { get; set; }
	}
}