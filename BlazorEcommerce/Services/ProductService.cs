using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Services
{
    public class ProductService : IProductService
    {
        private  IHttpClientFactory? _factory;
        private  HttpClient? _client;
        private readonly ILocalStorageService localStorage;

        public ProductService(IHttpClientFactory factory,HttpClient client,ILocalStorageService localStorage)
        {
            _factory = factory;
            _client = client;
            this.localStorage = localStorage;
        }
        
        public async Task<List<ProductsModel>> GetProducts()
        {
            _client = _factory.CreateClient("api");
            var token = await localStorage.GetItemAsync<string>("token");
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            var response = await _client.GetFromJsonAsync<List<ProductsModel>>("Products");
             return response;
        }
        public async Task<ProductsModel> GetProductById(int productId)
        {
            _client = _factory.CreateClient("api");
            // var token = await localStorage.GetItemAsync<string>("token");
            // _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            var response = await _client.GetFromJsonAsync<ProductsModel>($"Products/{productId}");
            return response;
        }

        public async Task<HttpResponseMessage> AddProduct(ProductsModel product)
        {

                _client = _factory.CreateClient("api");

            var token = await localStorage.GetItemAsync<string>("token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            var response = await _client.PostAsJsonAsync("Products", product);
                return response;

        }


        public async Task<HttpResponseMessage> UpdateProduct(ProductsModel product)
        {
            _client = _factory.CreateClient("api");
            var token = await localStorage.GetItemAsync<string>("token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            var response = await _client.PutAsJsonAsync($"Products/{product.product_id}",product);
            return  response;
    

        }
        public async Task<HttpResponseMessage> DeleteProduct(int productId)
        {
            _client = _factory.CreateClient("api");
            var token = await localStorage.GetItemAsync<string>("token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            var response = await _client.DeleteAsync($"Products/{productId}");
            return response;
        }

    }
}
