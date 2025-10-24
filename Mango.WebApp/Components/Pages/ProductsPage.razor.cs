using CommonLibrary.Dtos;
using Mango.WebApp.Service;
using Microsoft.AspNetCore.Components;

namespace Mango.WebApp.Components.Pages
{
	public partial class ProductsPage
	{
		[Inject] private ProductService ProductService { get; set; } = default!;

		public IList<QueryProductResultDto> Products { get; set; } = [];

		protected override async Task OnInitializedAsync()
		{
			Products = await ProductService.GetAllAsync();
		}
	}
}