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
        var userId = await _customerService.GetUserIdFromToken();
        var getUserName = await _customerService.GetUserNameFromToken();

        adminLogs = new AdminLogsModel
        {
            customer_id = userId,
            log_msg = $"product {product.name} was Created By {getUserName}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);

    }
    public async Task UpdateLog(int id)
    {
        _client = _factory.CreateClient("api");
        var userId = await _customerService.GetUserIdFromToken();
        var getUserName = await _customerService.GetUserNameFromToken();
        var productName = await _client.GetFromJsonAsync<ProductsModel>($"Products/{id}");


        adminLogs = new AdminLogsModel
        {
            customer_id = userId,
            log_msg = $"product ({productName.name})was edited By {getUserName}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);

    }

    public async Task DeleteLog(ProductsModel product)
    {
        _client = _factory.CreateClient("api");
        var userId = await _customerService.GetUserIdFromToken();
        var getUserName = await _customerService.GetUserNameFromToken();

        adminLogs = new AdminLogsModel
        {
            customer_id = await _customerService.GetUserIdFromToken(),
            log_msg = $"product ({product.name})was Deleted By {getUserName}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);
    }
    public async Task AddCategeoryLog(string name)
    {
        _client = _factory.CreateClient("api");
        var getUserName = await _customerService.GetUserNameFromToken();
        adminLogs = new AdminLogsModel
        {
            customer_id = await _customerService.GetUserIdFromToken(),
            log_msg = $"product ({name})was Deleted By {getUserName}"
        };
        var log = await _client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);
    }

}