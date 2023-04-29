using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages
{
    partial class Category : CartBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject] public IProductService ProductService { get; set; }
        private IEnumerable<ProductsModel>? products;
        private IEnumerable<ProductsModel>? categoriesProduct;
        protected override async Task OnInitializedAsync()
        {

            products = await ProductService.GetProducts();
            //categoriesProduct = products.Where(c => c.category_id == Id);

        }
    }
}
