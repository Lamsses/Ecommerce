using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages;

public class ProductBase: ComponentBase
{
    [Inject] public IProductService ProductService { get; set; }
    [Inject] public ICartService CartService { get; set; }
    public List<ProductsModel> Products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Products = await ProductService.GetProducts();
        
    }
}