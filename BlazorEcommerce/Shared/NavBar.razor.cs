using EcommerceLibrary.Models;

namespace BlazorEcommerce.Shared;

partial class NavBar
{
    private ProductsModel selectedProduct;
    private HttpClient? client;
    async Task ShowCart()
    {
        cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
    }


    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            cartItems = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");

            StateHasChanged();
        }
    }
    private async Task Delete(ProductsModel item)
    {

        cartItems.Remove(item);
        await LocalStorage.SetItemAsync("cart", cartItems);
        StateHasChanged();

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
    private void HandleSearch()
    {
        if (cartItems is null) return;
        selectedProduct = cartItems;
        NavigationManager.NavigateTo($"p/{selectedProduct.product_id}");
    }
}
