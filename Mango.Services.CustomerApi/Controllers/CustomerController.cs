using CommonLibrary.Dtos;
using CommonLibrary.Extensions;
using Domain.Entities;
using Domain.Interfaces.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CustomerApi.Controllers
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = "Bearer")]
	[ApiController]
	public class CustomerController(
		ICustomerRepository customerRepository
	) : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetAsync(int id)
		{
			Customer? customer = await customerRepository.GetAsync(id);

			if (customer == null) return BadRequest("The customer is null");

			return Ok(customer);
		}

		[HttpPost]
		public async Task<IActionResult> NewAsync(NewCustomerRequestDto requestDto)
		{
			Customer? customer = await customerRepository.NewAsync(requestDto.ToDto());

			if (customer == null) return BadRequest("The customer is null");

			return Ok(customer.ToDto());
		}
	}
}