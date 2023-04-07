using BlazorEcommerce.Pages;
using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;

public class AdminLogService : IAdminLogService
{
    private readonly ILocalStorageService _localStorage;
    private IHttpClientFactory? _factory;
    private readonly ICustomerService _customerService;
    private HttpClient? _client;
    private AdminLogsModel adminLogs = new();

    public AdminLogService
        (ILocalStorageService localStorage, HttpClient? client, IHttpClientFactory? factory, ICustomerService customerService)
    {
        _localStorage = localStorage;
        _client = client;
        _factory = factory;
        _customerService = customerService;
    }
    public async Task AddLog(ProductsModel product)
    {
        _client = _factory.CreateClient("api");
        var token = await _localStorage.GetItemAsync<string>("token");
        var userId = _customerService.GetUserIdFromToken(token);
        var getUserName = await _client.GetFromJsonAsync<CustomersModel>($"Customers/{userId}");
        var userResult = getUserName.first_name;
        adminLogs = new AdminLogsModel
        {
            customer_id = userId,
            log_msg = $"product {product.name} was Created By {userResult}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);
    }
    public async Task UpdateLog(int id)
    {
        _client = _factory.CreateClient("api");
        var token = await _localStorage.GetItemAsync<string>("token");
        var userId = _customerService.GetUserIdFromToken(token);
        var productName = await _client.GetFromJsonAsync<ProductsModel>($"Products/{id}");
        var getUserName = await _client.GetFromJsonAsync<CustomersModel>($"Customers/{userId}");
        var userResult = getUserName.first_name;

        adminLogs = new AdminLogsModel
        {
            customer_id = userId,
            log_msg = $"product ({productName.name})was edited By {userResult}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);

    }

    public async Task DeleteLog(ProductsModel product)
    {
        _client = _factory.CreateClient("api");
        var token = await _localStorage.GetItemAsync<string>("token");
        var userId = _customerService.GetUserIdFromToken(token);
        var getUserName = await _client.GetFromJsonAsync<CustomersModel>($"Customers/{userId}");
        var userResult = getUserName.first_name;

        adminLogs = new AdminLogsModel
        {
            customer_id = _customerService.GetUserIdFromToken(token),
            log_msg = $"product ({product.name})was Deleted By {userResult}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);
    }


}