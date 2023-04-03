using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorEcommerce.Pages;

public class MainBase : ComponentBase
{
    [Inject] public NavigationManager? NavigationManager { get; set; }
    [Inject] protected ILocalStorageService? LocalStorage { get; set; }
    [Inject] protected AuthenticationStateProvider? AuthStateProvider { get; set; }
    [Inject] public IHttpClientFactory? factory { get; set; }
    [Inject] public HttpClient? client { get; set; }
    protected AuthenticationModel Authenticat = new();
    private OrdersModel orders;

    



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

    public  async Task Logout()
    {
        NavigationManager!.NavigateTo("/", true);
        await LocalStorage!.RemoveItemAsync("token");
        await AuthStateProvider!.GetAuthenticationStateAsync();
    }
    public async Task OrdersCheckout()
    {
        var token = await LocalStorage.GetItemAsync<string>("token");

        var order = new OrdersModel
        {
            order_date = DateTime.Now,
            customer_id = GetUserIdFromToken(token)
        };


        client = factory.CreateClient("api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsJsonAsync("Orders", order);
        var result = await response.Content.ReadFromJsonAsync<OrdersModel>();


        if (response.IsSuccessStatusCode) 
        {
            var cart =  await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");

            foreach (var item in cart)
            {
                await client.PostAsJsonAsync("OrdersProducts", 
                    new OrdersProductsModel
                    { order_id = result.order_id ,product_id = item.product_id ,amount = item.ProductAmount , price = decimal.Parse( item.price)});
                
            }

        }


    }
    public static int GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "sub");

        return int.Parse( userIdClaim?.Value);
    }

}

