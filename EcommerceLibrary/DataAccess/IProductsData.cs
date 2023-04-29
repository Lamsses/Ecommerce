using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;

public interface IProductsData
{
    Task<ProductsModel?> Create(string name, decimal price, int quantity, string? img_url, string description,int? coupon_id, decimal discounted_price, decimal original_price);
    Task Delete(int product_id);
    Task Update(int product_id, string name, decimal price, int quantity, string? img_url, string description,int? coupon_id, decimal discounted_price, decimal original_price);
    Task<List<ProductsModel>> GetAll();
    Task<ProductsModel?> GetOne(int productId);

    Task<IEnumerable<ProductsModel>?> SearchProducts(string searchText);
}