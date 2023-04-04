using EcommerceLibrary.Models;

namespace BlazorEcommerce.Pages;

partial class DashBoardProdcuts : MainBase
{
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
        client = factory.CreateClient("api");

        var response = await client.GetFromJsonAsync<IEnumerable<ProductsModel>>($"Products/Search/{searchText}");

        return response;
    }
    private async Task AddProduct()
    {
        client = factory.CreateClient("api");
        var response = await client.PostAsJsonAsync<ProductsModel>("Products", selectedProduct);
        var result = await response.Content.ReadFromJsonAsync<ProductsModel>();

        if (response.IsSuccessStatusCode)
        {
            var token = await LocalStorage.GetItemAsync<string>("token");
            var userId = GetUserIdFromToken(token);

        }

    }
    private async Task Delete(int id)
    {
        client = factory.CreateClient("api");

            var productName = await client.GetFromJsonAsync<ProductsModel>($"Products/{id}");
        var respons = await client.DeleteAsync($"OrdersProducts/{id}");
        var response = await client.DeleteAsync($"Products/{id}");
 

        if (response.IsSuccessStatusCode)
        {
            var token = await LocalStorage.GetItemAsync<string>("token");
            var userId = GetUserIdFromToken(token);
            var getUserName = await client.GetFromJsonAsync<CustomersModel>($"Customers/{userId}");
            var userResult = getUserName.first_name;

            adminLogs = new AdminLogsModel
            {
                customer_id = GetUserIdFromToken(token),
                log_msg = $"product ({productName.name})was Deleted By {userResult}"
            };
            var log = await client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);

        }

        products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");
    }
    private async Task AddCategory()
    {
        client = factory.CreateClient("api");
        var response = await client.PostAsJsonAsync("Categories", category.Name);
        Categories = await client.GetFromJsonAsync<List<CategoriesModel>>("Categories");

    }
    private async Task AddCoupon()
    {
        client = factory.CreateClient("api");
        var response = await client.PostAsJsonAsync<CouponModel>("Coupon", coupon);
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

        }
    }
    public bool OkayDisabled = false;

    private void Enable(int id)
    {

        OkayDisabled = true;


    }
    private async Task EditProduct(int id)
    {
        client = factory.CreateClient("api");
        var response = await client.PutAsJsonAsync($"Products/{id}", editProduct);
        var result =  response.Content.ReadFromJsonAsync<ProductsModel>();
        if (response.IsSuccessStatusCode)
        {
            var token = await LocalStorage.GetItemAsync<string>("token");
            var userId = GetUserIdFromToken(token);
            var productName = await client.GetFromJsonAsync<ProductsModel>($"Products/{id}");
            var getUserName = await client.GetFromJsonAsync<CustomersModel>($"Customers/{userId}");
            var userResult = getUserName.first_name;

            adminLogs = new AdminLogsModel
            {
                customer_id = userId,
                log_msg = $"product ({productName.name})was edited By {userResult}"
            };
            var log = await client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);
        }
    }
}
