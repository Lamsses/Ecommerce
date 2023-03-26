using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorEcommerce.Pages;

public class MainBase : ComponentBase
{
    [Inject] public NavigationManager? NavigationManager { get; set; }
    [Inject] protected ILocalStorageService? LocalStorage { get; set; }
    [Inject] protected AuthenticationStateProvider? AuthStateProvider { get; set; }
     protected AuthenticationModel Authenticat = new();


    public List<ProductsModel>? cartItems;

    
    public IHttpClientFactory? factory;
    protected async Task Logout()
    {
         NavigationManager!.NavigateTo("/", true);
        await LocalStorage!.RemoveItemAsync("token");
        await AuthStateProvider!.GetAuthenticationStateAsync();
    }

}
