using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess
{
    public interface IProdcutsData
    {
        Task<ProductsModel?> Create(string name, string price, int quantity, string img_url, string description, int catagoryId);
        Task DeleteProducts(int product_id, string name, string price, int quantity, string img_url, string description, int catagoryId);
        Task EditProducts(int product_id, string name, string price, int quantity, string img_url, string description, int catagoryId);
        Task<List<ProductsModel>> GetAllAssigned();
        Task<ProductsModel?> GetOne(int productId);
    }
}