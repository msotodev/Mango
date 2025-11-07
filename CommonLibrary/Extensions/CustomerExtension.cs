using CommonLibrary.Dtos;
using Domain.Entities;
using System;

namespace CommonLibrary.Extensions
{
	public static class CustomerExtension
	{
		public static Customer ToDto(this NewCustomerRequestDto dto) => new Customer
		{
			Name = dto.Name,
			Birthday = dto.Birthday,
			CreatedAt = DateTime.UtcNow,
			CreatedBy = dto.UserId
		};

		public static QueryCustomerResultDto ToDto(
			this Customer customer
		) => new QueryCustomerResultDto
		{
			Id = customer.Id,
			Name = customer.Name,
			Birthday = customer.Birthday
		};
	}
}