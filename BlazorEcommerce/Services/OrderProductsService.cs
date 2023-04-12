using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace BlazorEcommerce.Services;

public class OrderProductsService : IOrderProductsService
{
    private IHttpClientFactory? _factory;
    private HttpClient? _client;
    private ILocalStorageService _localStorage;

    public OrderProductsService(HttpClient client, IHttpClientFactory factory, ILocalStorageService localStorage)
    {
        _factory = factory;
        _localStorage = localStorage;
        _client = client;
        
    }
    public async Task<List<OrdersProductsModel>> GetAll()
    {
        _client = _factory.CreateClient("api");
        var response = await _client.GetFromJsonAsync<List<OrdersProductsModel>>("OrdersProducts");
        return response;
    }

    public async Task<OrdersProductsModel> GetById(int id)
    {
        _client = _factory.CreateClient("api");
        var response = await _client.GetFromJsonAsync<OrdersProductsModel>($"OrdersProducts/{id}");
        return response;
    }

    public async Task<HttpResponseMessage> Add(OrdersProductsModel model)
    {
        _client = _factory.CreateClient("api");
        var token = await _localStorage.GetItemAsync<string>("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        var response = await _client.PostAsJsonAsync("OrdersProducts",model);
        return response;

    }

    public async Task<HttpResponseMessage> Delete(int id)
    {
        _client = _factory.CreateClient("api");

        var token = await _localStorage.GetItemAsync<string>("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        var response = await _client.DeleteAsync($"api/OrdersProducts/{id}");
        return response;

    }

    public async Task<HttpResponseMessage> Update(OrdersProductsModel model)
    {
        throw new NotImplementedException();
    }
}
