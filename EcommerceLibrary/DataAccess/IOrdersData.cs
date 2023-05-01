using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess
{
    public interface IOrdersData
    {
        Task<OrdersModel?> Create(DateTime order_date, int customer_id, string receipt, List<ProductsModel>? Products);
        Task Delete(int order_id);
        Task<List<OrdersModel>> GetAll();
        Task<OrdersModel?> GetOne(int order_id);
        Task Update(int order_id, DateTime order_date, int customer_id,string receipt);


    }
}