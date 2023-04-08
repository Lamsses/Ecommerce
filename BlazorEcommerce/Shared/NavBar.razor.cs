using BlazorEcommerce.Pages;
using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlazorEcommerce.Shared;

partial class NavBar : CartBase
{
    [Inject]
    public ICartService? CartService { get; set; }
    [Inject]
    public NavigationManager? NavigationManager { get; set; }
    [Inject] protected ILocalStorageService? LocalStorage { get; set; }


    [Inject] protected AuthenticationStateProvider? AuthStateProvider { get; set; }
    [Inject]
    private HttpClient? client { get; set; }
    [Inject]
    private IHttpClientFactory? factory {get; set; }
    private ProductsModel? selectedProduct;
    protected List<CategoriesModel> Categories = new();
    private int _cartItemsCount = 0;




    protected async override Task OnInitializedAsync()
    {
        
        client = factory.CreateClient("api");
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
        

        await ShowCart();

    }
    

    public async Task Logout()
    {
        NavigationManager!.NavigateTo("/", true);
        await LocalStorage!.RemoveItemAsync("token");
        await AuthStateProvider!.GetAuthenticationStateAsync();
    }
    async Task ShowCart()
    {
        CartItems = await CartService.GetCartItems();
    }

    private async Task<IEnumerable<ProductsModel>> SearchProducts(string searchText)
    {
        client = factory.CreateClient("api");
  
        var response = await client.GetFromJsonAsync<IEnumerable<ProductsModel>>($"Products/Search/{searchText}");
        return response;
    }

    private decimal  CalculateTotal()
    {
        decimal total=0;
        if (CartItems is not null)
        {
            foreach (var item in CartItems)
            {
                total += (Convert.ToDecimal(item.price) * Convert.ToDecimal(item.ProductAmount));

            } 
        }
        return total;
    }
    private int CartCount()
    {
        if(CartItems is not null) 
        {
            return CartItems.Count();
        }
        return 0;
    }
    private void HandleSearch(ProductsModel product)
    {
        if (CartItems is null) return;
        selectedProduct= product;
        NavigationManager.NavigateTo($"p/{selectedProduct.product_id}",true);
    }

}
