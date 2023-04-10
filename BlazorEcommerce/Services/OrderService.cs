using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlazorEcommerce.Services;

public class OrderService : IOrderService
{
private IHttpClientFactory? _factory;
    private readonly ILocalStorageService localStorage;
    private HttpClient? _client;

public OrderService(HttpClient client, IHttpClientFactory factory, ILocalStorageService localStorage)
{
    _factory = factory;
        this.localStorage = localStorage;
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
        var token = await localStorage.GetItemAsync<string>("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
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
