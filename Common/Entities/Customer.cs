using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
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