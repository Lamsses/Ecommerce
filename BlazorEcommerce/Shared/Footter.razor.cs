using BlazorEcommerce.Pages;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Shared
{
    partial class Footter : MainBase
    {
        protected List<CategoriesModel> Categories = new();

        protected override async Task OnInitializedAsync()
        {
            client = factory.CreateClient("api");
            Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
        }
    }
}
