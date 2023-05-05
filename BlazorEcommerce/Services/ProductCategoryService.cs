using System.Net.Http.Headers;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;

public class ProductCategoryService : IProductCategoryService
{

    private IHttpClientFactory? _factory;
    private HttpClient? _client;
    private readonly ILocalStorageService localStorage;

    public ProductCategoryService(IHttpClientFactory factory, HttpClient client, ILocalStorageService localStorage)
    {
        _factory = factory;
        _client = client;
        this.localStorage = localStorage;
    }

    public async Task<List<ProductCategoryModel>> GetProductCategory()
    {
        _client = _factory.CreateClient("api");
        var token = await localStorage.GetItemAsync<string>("token");
        // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        var response = await _client.GetFromJsonAsync<List<ProductCategoryModel>>("ProductCategory");
        return response;
    }

    public async Task<HttpResponseMessage> AddProductCategory(ProductCategoryModel model)
    {
        _client = _factory.CreateClient("api");
        var token = await localStorage.GetItemAsync<string>("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        var response = await _client.PostAsJsonAsync("ProductCategory",model);
        return response;
    }
    public async Task<HttpResponseMessage> DeleteProductCategory(ProductCategoryModel model)
    {
        _client = _factory.CreateClient("api");
        var token = await localStorage.GetItemAsync<string>("token");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        var response = await _client.DeleteAsync($"ProductCategory/{model.category_id}/{model.product_id}");
        return response;
    }
}
