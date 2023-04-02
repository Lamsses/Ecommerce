using EcommerceLibrary.Models;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorEcommerce.Pages;

partial class HomePage : MainBase
{

    protected List<CategoriesModel> Categories = new();
    private HttpClient? client;
    private IEnumerable<ProductsModel>? products;
    private IEnumerable<ProductsModel>? categoriesProduct;




    protected override async Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
        products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");
        cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");

        await InvokeAsync(StateHasChanged);
    }

    async Task SelectTap(int id)
    {
        categoriesProduct = products.Where(opts => opts.category_id == id);
    }

  


}
