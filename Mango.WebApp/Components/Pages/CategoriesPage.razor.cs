using CommonLibrary.Dtos;
using Mango.WebApp.Service;
using Microsoft.AspNetCore.Components;

namespace Mango.WebApp.Components.Pages
{
	public partial class CategoriesPage
	{
		[Inject] private CategoryService CategoryService { get; set; } = default!;

		public IList<QueryCategoryResultDto> Categories { get; set; } = [];

		protected override async Task OnInitializedAsync()
		{
			Categories = await CategoryService.GetAllAsync();
		}
	}
}