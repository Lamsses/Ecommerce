using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess
{
    public interface IOrdersProductsData
    {
        Task<OrdersProductsModel?> Create(int order_id, int product_id, int amount, decimal price);
        Task Delete(int product_id);
        Task<List<OrdersProductsModel>> GetAll();
        Task<OrdersProductsModel?> GetOne(int order_id);
        Task Update(int order_id, int product_id, int amount, decimal price);
    }
}