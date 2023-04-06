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
    public  async Task DeleteLog(int productId)
    {
        _client = _factory.CreateClient("api");
         var productName = await _client.GetFromJsonAsync<ProductsModel>($"Products/{productId}");
        var token = await _localStorage.GetItemAsync<string>("token");
        var userId = _customerService.GetUserIdFromToken(token);
        var getUserName = await _client.GetFromJsonAsync<CustomersModel>($"Customers/{userId}");
        var userResult = getUserName.first_name;

        adminLogs = new AdminLogsModel
        {
            customer_id = _customerService.GetUserIdFromToken(token),
            log_msg = $"product ({productName})was Deleted By {userResult}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);
    }
}
