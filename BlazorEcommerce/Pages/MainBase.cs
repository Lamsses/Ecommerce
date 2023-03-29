using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics.Metrics;

namespace BlazorEcommerce.Pages;

public class MainBase : ComponentBase
{
    [Inject] public NavigationManager? NavigationManager { get; set; }
    [Inject] protected ILocalStorageService? LocalStorage { get; set; }
    [Inject] protected AuthenticationStateProvider? AuthStateProvider { get; set; }
    [Inject] public IHttpClientFactory? factory { get; set; }
    [Inject] public HttpClient? client { get; set; }
    protected AuthenticationModel Authenticat = new();


    public List<ProductsModel>? cartItems = new();

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


    }

    protected async Task Logout()
    {
        NavigationManager!.NavigateTo("/", true);
        await LocalStorage!.RemoveItemAsync("token");
        await AuthStateProvider!.GetAuthenticationStateAsync();
    }

}
