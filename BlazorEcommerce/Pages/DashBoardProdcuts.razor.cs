using BlazorEcommerce.Services;
using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace BlazorEcommerce.Pages;


partial class DashBoardProdcuts : MainBase
{
    [Inject] IAdminLogService adminLog { get; set; }
    [Inject] ICustomerService customerService { get; set; }
    [Inject] IProductService productService { get; set; }
    [Inject] IOrderProductsService orderProductsService { get; set; }
    protected List<ProductsModel> products = new();
    protected List<CategoriesModel> Categories = new();
    protected List<CouponModel> Coupons = new();
    [CascadingParameter]
    private Task<AuthenticationState>? authenticationState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (authenticationState is not null)
        {

            var authState = await authenticationState;
            var user = authState?.User;

    
                client = factory.CreateClient("api");
                var token = await LocalStorage.GetItemAsync<string>("token");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

                products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");
                Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
                Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");
            
        }
    }
    private ProductsModel selectedProduct = new();
    private AdminLogsModel adminLogs = new();
    private ProductsModel editProduct = new();
    private ProductsModel addProduct = new();
    private CategoriesModel category = new();
    private CouponModel coupon = new();
    public int productId;


    private async Task<IEnumerable<ProductsModel>> SearchProducts(string searchText)
    {

        var response = await client.GetFromJsonAsync<IEnumerable<ProductsModel>>($"Products/Search/{searchText}");

        return response;
    }
    private async Task AddProduct()
    {
        client = factory.CreateClient("api");
        var product = products.Where(p => p.name == addProduct.name).FirstOrDefault();
        if (product == null)
        {
            var response = await productService.AddProduct(addProduct);
            var result = await response.Content.ReadFromJsonAsync<ProductsModel>();
            if (response.IsSuccessStatusCode)
            {
                adminLog.AddLog(result);
                products = await productService.GetProducts();

            }
            else
            {
                Console.WriteLine("");
            }
        }
    }
    private async Task Delete(int id)
    {

        var product = await productService.GetProductById(id);
        await orderProductsService.Delete(id);
        var response = await productService.DeleteProduct(id);
        if (response.IsSuccessStatusCode)
        {
            adminLog.DeleteLog(product);
        }
        products = await productService.GetProducts();
    }
    private async Task EditProduct()
    {

        var response = await productService.UpdateProduct(editProduct);
        var result = response.Content.ReadFromJsonAsync<ProductsModel>();
        if (response.IsSuccessStatusCode)
        {
            adminLog.UpdateLog(productId);
        }
        products = await productService.GetProducts();

    }
    private async Task AddCategory()
    {
        var response = await client.PostAsJsonAsync("Categories", category.Name);
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
        if (response.IsSuccessStatusCode)
        {
            adminLog.AddCategeoryLog(category.Name);
        }

    }

    private async Task EditCategory()
    {
        var response = await client.PutAsJsonAsync($"Categories/{category.category_id}", category);
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");



    }
    private async Task DeleteCategory()
    {
        var response = await client.DeleteAsync($"Categories/{category.category_id}");
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");



    }
    private async Task AddCoupon()
    {
        await client.PostAsJsonAsync<CouponModel>("Coupon", coupon);
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

        }private async Task EditCoupon()
    {
        var response = await client.PutAsJsonAsync($"Coupon/{coupon.coupon_id}", coupon);
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

        }private async Task DeleteCoupon()
    {
        var response = await client.DeleteAsync($"Coupon/{coupon.coupon_id}");
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

        }
    
    public bool OkayDisabled = false;

    private void Enable(int id)
    {

        OkayDisabled = true;


    }


    private async void GetProductId(int id)
    {
        productId = id;
        client = factory.CreateClient("api");
        editProduct = await productService.GetProductById(id);

    }
  
    private void HandleSearch(ProductsModel product)
    {
        if (product == null) return;
        selectedProduct = product;

    }



}
