using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;

public interface IProductService
{
    Task<List<ProductsModel>> GetProducts();
    Task<ProductsModel> GetProductById(int productId);
    Task<ProductsModel> UpdateProduct(ProductsModel product);
    Task<ProductsModel> DeleteProduct(ProductsModel product);
    // Task IEnumerable<ProductsModel> SearchProduct(string productName)
}
