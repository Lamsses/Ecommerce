using BlazorEcommerce.Pages;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlazorEcommerce.Shared;

partial class NavBar : MainBase
{
    protected List<CategoriesModel> Categories = new();

    private ProductsModel selectedProduct;
    private HttpClient? client;
    async Task ShowCart()
    {
        cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
    }


    protected async override Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
        await ShowCart();
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
        if (cartItems is not null)
        {
            foreach (var item in cartItems)
            {
                total += (Convert.ToDecimal(item.price) * Convert.ToDecimal(item.ProductAmount));

            } 
        }
        return total;
    }
    private int CartCount()
    {
        if(cartItems is not null) 
        {
            return cartItems.Count();
        }
        return 0;
    }
    private void HandleSearch(ProductsModel product)
    {
        if (cartItems is null) return;
        selectedProduct= product;
        NavigationManager.NavigateTo($"p/{selectedProduct.product_id}",true);
    }
    public async Task Delete(ProductsModel item)
    {
        var cart = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");

        var find = cart.Find(p => p.product_id == item.product_id);

        cart.Remove(find);
        await LocalStorage.SetItemAsync("cart", cart);
        cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        await InvokeAsync(StateHasChanged);



    }
}
