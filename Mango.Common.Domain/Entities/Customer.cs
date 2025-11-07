using System.ComponentModel.DataAnnotations;

namespace Mango.Common.Domain.Entities
{
	public class Customer : BaseEntity
	{
		[Required]
		[MaxLength(45)]
		public string Name { get; set; } = string.Empty;

		[Required]
		public DateTime Birthday { get; set; }
	}
}