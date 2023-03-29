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

    public async Task ShowCart()
    {
        cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
    }
    protected async Task Logout()
    {
        NavigationManager!.NavigateTo("/", true);
        await LocalStorage!.RemoveItemAsync("token");
        await AuthStateProvider!.GetAuthenticationStateAsync();
    }
    public int counter =0;
    public int CartCount()
    {
        if (cartItems is not null)
        {
            counter= cartItems.Count();
            StateHasChanged();
            return counter;

        }
        StateHasChanged();
        return counter;
    }
    

}
