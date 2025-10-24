namespace CommonLibrary.Dtos
{
	public class CouponByCodeRquestDto
	{
		public string Code { get; set; } = string.Empty;
	}

	public class DeleteCouponRequestDto
	{
		public int Id { get; set; }

		public int UserId { get; set; }
	}

	public class NewCouponRequestDto
	{
		public int Id { get; set; }

		public string Code { get; set; } = string.Empty;

		public double DisccountAmount { get; set; }

		public int MinAmount { get; set; }

		public int UserId { get; set; }
	}

	public class QueryCouponRequestDto
	{
		public int Id { get; set; }
	}

	public class QueryCouponResultDto : ResultDto
	{
		public int Id { get; set; }

		public string Code { get; set; } = string.Empty;

		public double DisccountAmount { get; set; }

		public int MinAmount { get; set; }
	}

	public class UpdateCouponRequestDto
	{
		public int Id { get; set; }

		public string Code { get; set; } = string.Empty;

		public double DisccountAmount { get; set; }

		public int MinAmount { get; set; }

		public int UserId { get; set; }
	}
}
