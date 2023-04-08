using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Services
{
    public class ProductService : IProductService
    {
        private  IHttpClientFactory? _factory;
        private  HttpClient? _client;

        public ProductService(IHttpClientFactory factory,HttpClient client)
        {
            _factory = factory;
            _client = client;
        }
        
        public async Task<List<ProductsModel>> GetProducts()
        {
            _client = _factory.CreateClient("api");
            var response = await _client.GetFromJsonAsync<List<ProductsModel>>("Products");
             return response;
        }
        public async Task<ProductsModel> GetProductById(int productId)
        {
            _client = _factory.CreateClient("api");
            var response = await _client.GetFromJsonAsync<ProductsModel>($"Products/{productId}");
            return response;
        }

        public async Task<HttpResponseMessage> AddProduct(ProductsModel product)
        {
            _client = _factory.CreateClient("api");
            var response = await _client.PostAsJsonAsync("Products", product);
            return response;
        }


        public async Task<HttpResponseMessage> UpdateProduct(ProductsModel product)
        {
            _client = _factory.CreateClient("api");
             var response = await _client.PutAsJsonAsync($"Products/{product.product_id}",product);
            return  response;


        }
        public async Task<HttpResponseMessage> DeleteProduct(int productId)
        {
            _client = _factory.CreateClient("api");
            var response = await _client.DeleteAsync($"Products/{productId}");
            return response;
        }

    }
}
