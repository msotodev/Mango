namespace CommonLibrary.Dtos.Coupon
{
	public class NewCouponRequestDto
	{
		public string Code { get; set; } = string.Empty;

		public double DisccountAmount { get; set; }

		public int MinAmount { get; set; }

		public string UserId { get; set; } = string.Empty;
	}
}