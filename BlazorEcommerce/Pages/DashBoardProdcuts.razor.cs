using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages;

partial class DashBoardProdcuts : MainBase
{
    [Inject] IAdminLogService adminLog { get; set; }
    [Inject] ICustomerService customerService { get; set; }
    [Inject] IProductService productService { get; set; }
    protected List<ProductsModel> products= new();
    protected List<CategoriesModel> Categories= new();
    protected List<CouponModel> Coupons= new();
    protected override async Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");

        products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");


    }
    private ProductsModel selectedProduct = new();
    private AdminLogsModel adminLogs = new();
    private ProductsModel editProduct = new();
    private CategoriesModel category=new();
    private CouponModel coupon = new();


    private async Task<IEnumerable<ProductsModel>> SearchProducts(string searchText)
    {

        var response = await client.GetFromJsonAsync<IEnumerable<ProductsModel>>($"Products/Search/{searchText}");

        return response;
    }
    private async Task AddProduct()
    {

        var response = await client.PostAsJsonAsync<ProductsModel>("Products", selectedProduct);
        var result = await response.Content.ReadFromJsonAsync<ProductsModel>();
        if (response.IsSuccessStatusCode)
        {
          adminLog.AddLog(result);

        }
        products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");

    }
    private async Task Delete(int id)
    {

        var product = await client.GetFromJsonAsync<ProductsModel>($"Products/{id}");
        await client.DeleteAsync($"OrdersProducts/{id}");
        var response = await client.DeleteAsync($"Products/{id}");
        if (response.IsSuccessStatusCode)
        {
            adminLog.DeleteLog(product);
        }
        products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");
    }
    private async Task EditProduct(int id)
    {
        
        var response = await client.PutAsJsonAsync($"Products/{id}", editProduct);
        var result =  response.Content.ReadFromJsonAsync<ProductsModel>();
        if (response.IsSuccessStatusCode)
        {
            adminLog.UpdateLog(id);
        }
    }
    private async Task AddCategory()
    {
        var response = await client.PostAsJsonAsync("Categories", category.Name);
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");

    }
    private async Task AddCoupon()
    {
        var response = await client.PostAsJsonAsync<CouponModel>("Coupon", coupon);
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

        }
    
    public bool OkayDisabled = false;

    private void Enable(int id)
    {

        OkayDisabled = true;


    }
}
