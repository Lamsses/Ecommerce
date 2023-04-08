using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;

public interface IOrderProductsService
{
    Task<List<OrdersProductsModel>> GetAll();
    Task<OrdersProductsModel> GetById(int id);
    Task<HttpResponseMessage> Add(OrdersProductsModel model);
    Task<HttpResponseMessage> Delete(int id);
    Task<HttpResponseMessage> Update(OrdersProductsModel model);
}
