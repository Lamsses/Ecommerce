using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;

public interface IOrderService
{
    Task<List<OrdersModel>> GetOrders();
    Task<HttpResponseMessage> AddOrder(OrdersModel order);
    Task<HttpResponseMessage> UpdateOrder();
    Task<HttpResponseMessage> DeleteOrder(int id);
}
