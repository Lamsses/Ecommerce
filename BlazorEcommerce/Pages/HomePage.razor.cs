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
        cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");

        await InvokeAsync(StateHasChanged);
    }

    async Task SelectTap(int id)
    {
        categoriesProduct = products.Where(opts => opts.category_id == id);
    }
    public async Task AddToCart(ProductsModel products)
    {
        var cart = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        if (cart is null)
        {

            cart = new List<ProductsModel>();
        }
        var find = cart.Find(p => p.product_id == products.product_id);
        if (find is null)
        {
            cart.Add(products);

        }
        else
        {
            find.ProductAmount += 1;
            products.ProductAmount = find.ProductAmount;

        }
        await LocalStorage.SetItemAsync("cart", cart);
        CartCount();
        ShowCart();

    }


}
