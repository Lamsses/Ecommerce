using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace BlazorEcommerce.Pages;

partial class CustomerLog
{
    [Inject] private IProductService productService { get; set; }
    private List<CustomerLogsModel> customerLog = new();
    private List<OrdersModel> orders = new();
    private List<CustomerLogsModel> orderProducts;
    public List<CustomersModel> Customers { get; set; }
    public List<ProductsModel> Products { get; set; }
    public IEnumerable<dynamic> CustomersOrders { get; set; } 
    public IEnumerable<dynamic> ProductsOrders { get; set; } 
    public int orderId;

    


    public void GetOrderId(int id)
    {
        orderId = id;
        orderProducts = customerLog.Where(x => x.order_id == orderId).ToList();

    }

    public async Task<string> GetProductName(int id)
    {
        var product = await ProductService.GetProductById(id);
        return product.name;
    }

    public async Task<string> GetUserName(int userId)
    {
    client = factory.CreateClient("api");
        var user = await  client.GetAsync($"Customers/{userId}");
        return await user.Content.ReadAsStringAsync();
    }
    protected override async Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");
        client = factory.CreateClient("api");
        var token = await LocalStorage.GetItemAsync<string>("token");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        customerLog = await client.GetFromJsonAsync<List<CustomerLogsModel>>("CustomerLogs");
        orders = await client.GetFromJsonAsync<List<OrdersModel>>("Orders");
        Customers = await client.GetFromJsonAsync<List<CustomersModel>>("Customers");
        Products = await productService.GetProducts();
        CustomersOrders = from o in orders
            join c in Customers
                on o.customer_id equals c.customer_id
            select new
            {
                 o.order_id, c.first_name,o.receipt
            };
             ProductsOrders = from cl in customerLog
            join p in Products
                on cl.product_id equals p.product_id
            select new
            {
             p.name,cl.amount,cl.total,cl.order_id
            };

    }
}
