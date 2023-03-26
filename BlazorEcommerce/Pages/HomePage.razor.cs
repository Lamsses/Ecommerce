using EcommerceLibrary.Models;
using Microsoft.JSInterop;

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

        await InvokeAsync(StateHasChanged);
    }

    async Task SelectTap(int id)
    {
        categoriesProduct = products.Where(opts => opts.category_id == id);
    }
    private async Task AddToCart(ProductsModel products)
    {
        if (cartItems is null)
        {

            cartItems = new List<ProductsModel>();
        }
        var find = cartItems.Find(p => p.product_id == products.product_id);
        if (find is null)
        {
            cartItems.Add(products);
            await LocalStorage.SetItemAsync("cart", cartItems);

        }
        else
        {
            find.ProductAmount += 1;
            products.ProductAmount = find.ProductAmount;
            await LocalStorage.SetItemAsync("cart", cartItems);
            StateHasChanged();

        }
    }
}
