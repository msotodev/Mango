using Mango.Common.Domain.Entities;

namespace Mango.Common.Domain.Interfaces.Customers
{
	public interface ICustomerRepository
	{
		Task<Customer?> GetAsync(int id);

		Task<Customer> NewAsync(Customer customer);
	}
}