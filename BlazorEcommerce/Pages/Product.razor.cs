using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages;

partial class Product : CartBase
{
    [Parameter]
    public int Id { get; set; }
    [Inject] public IProductService ProductService { get; set; }
    public ProductsModel product = new();
    protected async override Task OnInitializedAsync()
    {
    
        product = await ProductService.GetProductById(Id);
    
    }
}
