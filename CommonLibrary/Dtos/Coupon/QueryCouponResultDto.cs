namespace CommonLibrary.Dtos.Coupon
{
	public class QueryCouponResultDto : ResultDto
	{
		public int Id { get; set; }

		public string Code { get; set; } = string.Empty;

		public double DisccountAmount { get; set; }

		public int MinAmount { get; set; }
	}
}