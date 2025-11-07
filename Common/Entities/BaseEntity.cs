using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public int CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public int UpdatedBy { get; set; }

		public bool Deleted { get; set; }

		public DateTime DeletedAt { get; set; }
	}
}