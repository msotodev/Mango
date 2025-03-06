namespace CommonLibrary.Dtos.Coupon
{
	public class SaveCouponRequestDto
	{
		public int Id { get; set; }

		public string Code { get; set; } = string.Empty;

		public double DisccountAmount { get; set; }

		public int MinAmount { get; set; }

		public string UserId { get; set; } = string.Empty;
	}
}