using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;

public class CategoryService : ICategoryService
{
    private IHttpClientFactory? _factory;
    private HttpClient? _client;

    public CategoryService(IHttpClientFactory factory, HttpClient client)
    {
        _factory = factory;
        _client = client;
    }
    public async Task<List<CategoriesModel>> GetCategories()
    {
        _client = _factory.CreateClient("api");

            var response = await _client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
            return response;
    }
}
