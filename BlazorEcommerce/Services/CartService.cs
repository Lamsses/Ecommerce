using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;

public class CartService: ICartService
{
    private readonly ILocalStorageService _localStorage;

    public CartService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }
    public async Task<List<ProductsModel>> GetCartItems()
    {
        return await _localStorage.GetItemAsync<List<ProductsModel>>("cart");
    }

    public async Task SetCartItems(List<ProductsModel> cartItems)
    {
         await _localStorage.SetItemAsync("cart", cartItems);
    }


}
