using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;

public interface ICartService
{
    Task<List<ProductsModel>> GetCartItems();
    Task SetCartItems(List<ProductsModel> cartItems);
    
}
