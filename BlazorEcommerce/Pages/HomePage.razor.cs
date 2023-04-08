using EcommerceLibrary.Models;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using BlazorEcommerce.Services.Interface;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages;

partial class HomePage : CartBase
{

    protected List<CategoriesModel> Categories = new();
    private HttpClient? client;
    private IEnumerable<ProductsModel>? products;
    private IEnumerable<ProductsModel>? categoriesProduct;
    [Inject] public IProductService ProductService { get; set; }




    protected override async Task OnInitializedAsync()
    {
        
        products = (await ProductService.GetProducts()).Take(4);
        // cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");

        
    }



  


}
