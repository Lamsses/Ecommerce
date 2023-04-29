using BlazorEcommerce.Services;
using BlazorEcommerce.Services.Interface;
using Blazored.Toast.Services;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using Blazored.Typeahead;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorEcommerce.Pages;


partial class DashBoardProdcuts : MainBase
{
    [Inject] IAdminLogService adminLog { get; set; }
    [Inject] ICustomerService customerService { get; set; }
    [Inject] IProductService productService { get; set; }
    [Inject] IOrderProductsService orderProductsService { get; set; }
    [Inject] IToastService toastService { get; set; }
    [Inject] IConfiguration config { get; set; }
    [Inject] IProductCategoryService productCategoryService { get; set; }
    [Inject] IJSRuntime jsRuntime { get; set; }
    protected List<ProductsModel> products = new();
    protected List<CategoriesModel> Categories = new();
    protected List<CategoriesModel> selectedCategories = new();
    protected List<CouponModel> Coupons = new ();
    [CascadingParameter]
    private Task<AuthenticationState>? authenticationState { get; set; }

    List<string> options = new List<string> { "Option 1", "Option 2", "Option 3" };
    List<string> selectedOptions = new List<string>();

    protected override async Task OnInitializedAsync()
    {
        if (authenticationState is not null)
        {

            var authState = await authenticationState;
            var user = authState?.User;
            selectedOptions.Add(options[0]);


            client = factory.CreateClient("api");
            var token = await LocalStorage.GetItemAsync<string>("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");
            Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");
            Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

        }
    }
    private ProductsModel selectedProduct   ;
    private AdminLogsModel adminLogs = new();
    private ProductsModel editProduct = new();
    private List<ProductsModel> searchItems= new();

    private ProductsModel addProduct = new();
    private List<ProductCategoryModel>productCategory = new();
    private CategoriesModel category = new();
    private CouponModel coupon = new();
    public int productId;

    private string searchQuery;


    private async Task<IEnumerable<ProductsModel>> SearchProducts(string searchText)
    {

        var response = await client.GetFromJsonAsync<IEnumerable<ProductsModel>>($"Products/Search/{searchText}");
        if (response == null) return new List<ProductsModel>();
        searchItems = response.ToList();
        return response;
    }

    private async Task<HttpResponseMessage> AddProduct()
    {

        try
        {
            errors.Clear();
            client = factory.CreateClient("api");
            var product = products.Where(p => p.name == addProduct.name).FirstOrDefault();
            if (product == null)
            {
                if (Decimal.Parse(addProduct.price) < addProduct.original_price)
                {
                    toastService.ShowError("Price cannot be less the Original price");
                    return new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("Price cannot be less the Original price")
                    };
                }
                string relativePath = await CaptureFile();
                addProduct.img_url = relativePath;

                var response = await productService.AddProduct(addProduct);
                var result = await response.Content.ReadFromJsonAsync<ProductsModel>();
                if (response.IsSuccessStatusCode)
                {
                foreach (var item in Categories)
                {
                    if (item.isSelected)
                    {
                        var s = await productCategoryService.AddProductCategory(new ProductCategoryModel
                        {
        category_id = item.category_id,
        product_id = result.product_id
                        });
                    }
                }
                    toastService.ShowSuccess("Product Added Successfully");
                    adminLog.AddLog(result);
                    products = await productService.GetProducts();

                }
                else
                {
                    toastService.ShowError("An error occurred Please try again");
                }

            }
            else
            {
                toastService.ShowError("Product Already Exists");
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Product Already Exists")
                };
            }
            else
            {
                    toastService.ShowError("Item with this name already exist");

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
        client = factory.CreateClient("api");
        var token = await LocalStorage.GetItemAsync<string>("token");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        var response = await client.PostAsJsonAsync("Categories", category.Name);

        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");


        // if (response.IsSuccessStatusCode)
        // {
        //      adminLog.AddCategeoryLog(category.Name);
        // }

    }
    private async Task AddProductCategory()
    {
        client = factory.CreateClient("api");
        var token = await LocalStorage.GetItemAsync<string>("token");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
         await client.PostAsJsonAsync("Categories", category.Name);

        var response = await client.PostAsJsonAsync
            ("ProductCategory", new ProductCategoryModel {category_id = category.category_id, product_id = addProduct.product_id  });
        productCategory = await client.GetFromJsonAsync<List<ProductCategoryModel>>("ProductCategory");
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

    }
    private async Task EditCoupon()
    {
        var response = await client.PutAsJsonAsync($"Coupon/{coupon.coupon_id}", coupon);
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

    }
    private async Task DeleteCoupon()
    {
        client = factory.CreateClient("api");
        var token = await LocalStorage.GetItemAsync<string>("token");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
        var response = await client.DeleteAsync($"Coupon/{coupon.coupon_id}");
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

    }



    private async void GetProductId(int id)
    {
        productId = id;
        client = factory.CreateClient("api");
        editProduct = await productService.GetProductById(id);

    }

    public async void LoadProductData()
    {
        editProduct = await productService.GetProductById(productId);

    }

    public  void ClearProductData()
    {
        editProduct = new ProductsModel();
    }
    private string CreateWebPath(string path)
    {
        if (!string.IsNullOrEmpty(path))
        {

            return Path.Combine(config.GetValue<string>("WebStorageRoot")!, path);
        }
        return "";
    }
    private void HandleSearch(ProductsModel product)
    {
        if (product == null) return;
        selectedProduct = product;

    }
    private long maxFileSize = 1024 * 1024 * 10; // represents 3MB
    private int maxAllowedFiles = 3;
    private List<string> errors = new();
    private IBrowserFile? file;

    private void LoadFiles(InputFileChangeEventArgs e)
    {
        try
        {
            file = e.File;
        }
        catch (Exception ex)
        {

            errors.Add(ex.Message);
        }
    }
    private async Task<string> CaptureFile()
    {
        if (file is null)
        {
            return null;
        }

        try
        {
            if (Path.GetExtension(file.Name) != ".jpg" && Path.GetExtension(file.Name) != ".png" && Path.GetExtension(file.Name) != ".jpeg")
            {
                errors.Add("File must be an image file (.jpg, .png, .jpeg)");
            }
            string newFileName = Path.ChangeExtension(
                Path.GetRandomFileName(),
                Path.GetExtension(file.Name));
            var userName = await customerService.GetUserNameFromToken();
            string path = Path.Combine(
                config.GetValue<string>("FileStorage")!,
                userName,
                newFileName);

            string relativePath = Path.Combine(
                userName,
                newFileName);

            Directory.CreateDirectory(Path.Combine(
                config.GetValue<string>("FileStorage")!,
                userName));

            await using FileStream fs = new(path, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
            return relativePath;
        }
        catch (Exception ex)
        {
            errors.Add($"File: {file.Name} Error: {ex.Message}");
            throw;
        }
    }
    //private async Task getCouponById(int id)
    //{
    //    var couponName = coupon.coupon_name.Where(x => x.coupon_id == id);

    //}
}