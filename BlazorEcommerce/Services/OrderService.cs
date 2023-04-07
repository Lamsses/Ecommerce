using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using System.Net.Http.Json;

namespace BlazorEcommerce.Services;

public class OrderService : IOrderService
{
private IHttpClientFactory? _factory;
private HttpClient? _client;

public OrderService(HttpClient client, IHttpClientFactory factory)
{
    _factory = factory;
    _client = client;

}
    public  async Task<List<OrdersModel>> GetOrders()
    {
        _client = _factory.CreateClient("api");
        var response = await _client.GetFromJsonAsync<List<OrdersModel>>("Orders");
        return response;

    }

    public async Task<HttpResponseMessage> AddOrder(OrdersModel order)
    {
        _client = _factory.CreateClient("api");
        var response = await _client.PostAsJsonAsync("Orders",order);
        return response;

    }

    public async Task<HttpResponseMessage> UpdateOrder()
    {
        throw new NotImplementedException();

    }

    public async Task<HttpResponseMessage> DeleteOrder(int id)
    {
        throw new NotImplementedException();
    }
}
