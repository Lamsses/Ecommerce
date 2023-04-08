using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;

public interface IProductService
{
    Task<List<ProductsModel>> GetProducts();
    Task<ProductsModel> GetProductById(int productId);
    Task<HttpResponseMessage> UpdateProduct(ProductsModel product);
    Task<HttpResponseMessage> DeleteProduct(int productId);
    Task<HttpResponseMessage> AddProduct(ProductsModel product);
    // Task IEnumerable<ProductsModel> SearchProduct(string productName)
}
