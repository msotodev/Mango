using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.AuthApi.Models
{
	public class AppUser : IdentityUser
	{
		[MaxLength(45)]
		public string Name { get; set; } = string.Empty;
	}
}