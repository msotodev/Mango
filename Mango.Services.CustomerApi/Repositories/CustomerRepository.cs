using Domain.Entities;
using Domain.Interfaces.Customers;
using Mango.Services.CustomerApi.Data;

namespace Mango.Services.CustomerApi.Repositories
{
	public class CustomerRepository(
		AppDbContext appContext
	) : BaseRepository<Customer>(appContext), ICustomerRepository
	{
		public Task<Customer?> GetAsync(int id) => GetByIdAsync(id);
	}
}