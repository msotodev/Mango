using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CustomerApi.Data
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<Customer> Customer { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}
	}
}