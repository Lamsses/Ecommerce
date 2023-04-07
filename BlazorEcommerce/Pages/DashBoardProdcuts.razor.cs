using BlazorEcommerce.Services;
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
    [Inject] IOrderProductsService orderProductsService { get; set; }
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
    private ProductsModel addProduct = new();
    private CategoriesModel category=new();
    private CouponModel coupon = new();
    public int productId;


    private async Task<IEnumerable<ProductsModel>> SearchProducts(string searchText)
    {

        var response = await client.GetFromJsonAsync<IEnumerable<ProductsModel>>($"Products/Search/{searchText}");

        return response;
    }
    private async Task AddProduct()
    {
        var response = await productService.AddProduct(addProduct);
        var result = await response.Content.ReadFromJsonAsync<ProductsModel>();
        if (response.IsSuccessStatusCode)
        {
          adminLog.AddLog(result);

        }

        products = await productService.GetProducts();


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
        var result =  response.Content.ReadFromJsonAsync<ProductsModel>();
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

    
  

    private async void GetProductId( int id)
    {
        productId = id;
        client = factory.CreateClient("api");
        editProduct = await productService.GetProductById(id);
        
    }
}
