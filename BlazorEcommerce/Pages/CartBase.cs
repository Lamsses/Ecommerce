using BlazorEcommerce.Services.Interface;
using Blazored.Toast.Services;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace BlazorEcommerce.Pages;

public class CartBase: ComponentBase
{
    [Inject] 
    public ICartService CartService { get; set; }
    [Inject] ICustomerService customerService { get; set; }
    public List<ProductsModel> CartItems = new();
    // public event Action CartChanged;
    [Inject] public IToastService toastService { get; set; }

    [Inject] HttpClient client { get; set; }
    [Inject] IHttpClientFactory factory { get; set; }
    [Inject] ILocalStorageService localStorage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        CartItems = await CartService.GetCartItems();

    }

    public async Task AddToCart(ProductsModel product)
    {
        var cart = await CartService.GetCartItems();
        if (cart is null)
        {
            cart = new List<ProductsModel>();
        }
        var find = cart.Find(p => p.product_id == product.product_id);
        if (find is null)
        {
            cart.Add(product);
        }
        else
        {
            find.ProductAmount += 1;
            product.ProductAmount = find.ProductAmount;

        }
        toastService.ShowSuccess($"{product.name} Added To Cart");

        await CartService.SetCartItems(cart);
        

    }
    public async Task DeleteFromCart(ProductsModel product)
    {
        var cart = await CartService.GetCartItems();

        var find = cart.Find(p => p.product_id == product.product_id);
        if (find.discounted_price > 0)
        {
            var userId = await customerService.GetUserIdFromToken();
            var couponId = find.coupon_id;
            var token = await localStorage.GetItemAsync<string>("token");
            client = factory.CreateClient("api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            var response = await client.DeleteAsync($"CustomerCoupon/{userId}/{couponId}");

        }
        cart.Remove(find);
        await CartService.SetCartItems(cart);
        CartItems = await CartService.GetCartItems();
        // await InvokeAsync(StateHasChanged);



    }

    
}
