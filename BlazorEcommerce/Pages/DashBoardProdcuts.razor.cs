using BlazorEcommerce.Services;
using BlazorEcommerce.Services.Interface;
using Blazored.Toast.Services;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text.Json;

namespace BlazorEcommerce.Pages;

partial class DashBoardProdcuts : MainBase
{
    [Inject] IAdminLogService adminLog { get; set; }
    [Inject] ICustomerService customerService { get; set; }
    [Inject] IProductService productService { get; set; }
    [Inject] IOrderProductsService orderProductsService { get; set; }
    [Inject] IToastService toastService { get; set; }
    protected List<ProductsModel> products = new();
    protected List<CategoriesModel> Categories = new();
    protected List<CouponModel> Coupons = new();
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
    private CategoriesModel category = new();
    private CouponModel coupon = new();
    public int productId;


    private async Task<IEnumerable<ProductsModel>> SearchProducts(string searchText)
    {

        var response = await client.GetFromJsonAsync<IEnumerable<ProductsModel>>($"Products/Search/{searchText}");

        return response;
    }
    private async Task<HttpResponseMessage> AddProduct()
    {

        try
        {
            client = factory.CreateClient("api");
            var product = products.Where(p => p.name == addProduct.name).FirstOrDefault();
            if (product == null)
            {
                var response = await productService.AddProduct(addProduct);
                var result = await response.Content.ReadFromJsonAsync<ProductsModel>();
                if (response.IsSuccessStatusCode)
                {
                    toastService.ShowSuccess("Product Added Successfully");
                    adminLog.AddLog(result);
                    products = await productService.GetProducts();

                }
                else
                {
                    toastService.ShowError("An error occurred Please try again");
                }
            }
        }
        catch (HttpRequestException ex)
        {
            toastService.ShowError("An error occurred Please try again");

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("An error occurred while making an HTTP request to add a product. Please try again later.")
            };
        }
        catch (JsonException ex)
        {
            toastService.ShowError("There are Missing Inputs");


            // Return an informative error response
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("The JSON data for adding a product is invalid. Please check the input data and try again.")
            };
        }

        return new HttpResponseMessage(HttpStatusCode.OK);

    }
    private async Task<HttpResponseMessage> Delete()
    {

        try
        {
            var product = await productService.GetProductById(productId);
            var d = await orderProductsService.Delete(productId);
            var response = await productService.DeleteProduct(productId);
            if (response.IsSuccessStatusCode)
            {
                adminLog.DeleteLog(product);
                ToastService.ShowSuccess("Item Deleted Successfully");
            }
            else
            {
                ToastService.ShowError("Something Went Wrong Please try again");
            }
            products = await productService.GetProducts();
        }
        catch (HttpRequestException ex)
        {
            toastService.ShowError("An error occurred Please try again");

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("An error occurred while making an HTTP request to add a product. Please try again later.")
            };
        }
        catch (JsonException ex)
        {
            toastService.ShowError("An error occurred Please try again");


            // Return an informative error response
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("The JSON data for adding a product is invalid. Please check the input data and try again.")
            };
        }

        return new HttpResponseMessage(HttpStatusCode.OK);
    }
    private async Task<HttpResponseMessage> EditProduct()
    {

        var response = await productService.UpdateProduct(editProduct);
        var result = response.Content.ReadFromJsonAsync<ProductsModel>();
        try
        {
            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess("Product Edited Successfully");

                adminLog.UpdateLog(productId);
            }
            else
            {
                toastService.ShowError("Something Wrong Happened Pleas Try again");
            }
            products = await productService.GetProducts();
        }
        catch (HttpRequestException ex)
        {
            toastService.ShowError("An error occurred Please try again");

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("An error occurred while making an HTTP request to add a product. Please try again later.")
            };
        }
        catch (JsonException ex)
        {
            toastService.ShowError("An error occurred Please try again");


            // Return an informative error response
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("The JSON data for adding a product is invalid. Please check the input data and try again.")
            };
        }

        return new HttpResponseMessage(HttpStatusCode.OK);

    }
    private async Task AddCategory()
    {
        var response = await client.PostAsJsonAsync("Categories", category.Name);
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
        // if (response.IsSuccessStatusCode)
        // {
        //      adminLog.AddCategeoryLog(category.Name);
        // }

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



    private async void GetProductId(int id)
    {
        productId = id;
        client = factory.CreateClient("api");
        editProduct = await productService.GetProductById(id);

    }

}
