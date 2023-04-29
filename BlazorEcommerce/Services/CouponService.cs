using Blazored.LocalStorage;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;

public class CouponService
{
    private IHttpClientFactory? _factory;
    private HttpClient? _client;
    private ILocalStorageService _localStorage;

    public CouponService(HttpClient client, IHttpClientFactory factory, ILocalStorageService localStorage)
    {
        _factory = factory;
        _localStorage = localStorage;
        _client = client;

    }
    
    
}
