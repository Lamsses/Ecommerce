using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages
{
    partial class Category : MainBase
    {
        [Parameter]
        public int Id { get; set; }
        private IEnumerable<ProductsModel>? products;
        private IEnumerable<ProductsModel>? categoriesProduct;
        protected async override Task OnInitializedAsync()
        {
            client = factory.CreateClient("api");
            products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");
            categoriesProduct = products.Where(c => c.category_id == Id);

        }
    }
}
