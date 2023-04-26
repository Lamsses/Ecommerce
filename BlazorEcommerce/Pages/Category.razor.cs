using BlazorEcommerce.Services;
using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages
{
    partial class Category : CartBase
    {
        [Parameter]
        public int Id { get; set; }
        [Inject] public IProductService ProductService { get; set; }
        [Inject] public IProductCategoryService ProductCategoryService { get; set; }

        private IEnumerable<ProductCategoryModel>? productCategory;
        private IEnumerable<ProductsModel>? products = new List<ProductsModel>();
        private List<ProductsModel>? categoriesProduct =  new List<ProductsModel>();
        protected override async Task OnInitializedAsync()
        {

            productCategory = await ProductCategoryService.GetProductCategory();
            productCategory = productCategory.Where(c => c.category_id == Id);
            products = await ProductService.GetProducts();
            foreach (var item in productCategory)
            {

               categoriesProduct.Add(products.Where(c => c.product_id == item.product_id).First());
             
            }

        }
    }
}
