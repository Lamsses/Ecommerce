using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;

public class CustomerService : ICustomerService
{
    private readonly ILocalStorageService _localStorage;
    private  HttpClient _client;
    private  IHttpClientFactory _factory;

    public CustomerService(ILocalStorageService localStorage, HttpClient client, IHttpClientFactory factory)
    {
        _localStorage = localStorage;
        _client = client;
        _factory = factory;
    }

    public async Task<int> GetUserIdFromToken( )
    {
        var token = await _localStorage.GetItemAsync<string>("token");
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "sub");

        return int.Parse(userIdClaim?.Value);
    }
    public async Task<string> GetUserNameFromToken()
    {
        var token = await _localStorage.GetItemAsync<string>("token");
        _client = _factory.CreateClient("api");

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

        var userId = await GetUserIdFromToken();
        var getUserName = await _client.GetFromJsonAsync<CustomersModel>($"Customers/{userId}");
        return  getUserName.first_name;

    }

}
