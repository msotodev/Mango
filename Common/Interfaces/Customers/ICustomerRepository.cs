using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces.Customers
{
	public interface ICustomerRepository
	{
		Task<Customer?> GetAsync(int id);

		Task<Customer> NewAsync(Customer customer);
	}
}