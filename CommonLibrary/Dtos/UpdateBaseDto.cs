using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Dtos
{
	public class UpdateBaseDto
	{
		[Key]
		public int Id { get; set; }
	}
}