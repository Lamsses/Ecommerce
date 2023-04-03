using BlazorEcommerce.Pages;
using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Shared
{
    partial class Footter
    {
        protected List<CategoriesModel> Categories = new();
        [Inject] public ICategoryService CategoryService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
            Categories = await CategoryService.GetCategories();
        }
    }
}
