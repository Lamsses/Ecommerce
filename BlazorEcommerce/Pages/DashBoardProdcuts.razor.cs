using EcommerceLibrary.Models;

namespace BlazorEcommerce.Pages;

partial class DashBoardProdcuts : MainBase
{
    protected List<ProductsModel> products= new();
    protected override async Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");

        products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");


    }
    private ProductsModel selectedProduct = new();
    private AdminLogsModel adminLogs = new ();
    private ProductsModel editProduct = new();


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

            adminLogs = new AdminLogsModel
            {
                customer_id = GetUserIdFromToken(token),
                log_msg = $"product {result.name} was Created"
            };
            var log = await client.PostAsJsonAsync<AdminLogsModel>("AdminLogs", adminLogs);

        }

    }
    private async Task Delete(int id )
    {
        client = factory.CreateClient("api");

        var respons =await client.DeleteAsync($"OrdersProducts/{id}");
        var response = await client.DeleteAsync($"Products/{id}");

    }
    public bool OkayDisabled =false;

    private void Enable(int id)
    {

         OkayDisabled = true;

      
    }
    private async Task EditProduct(int id)
    {
        client = factory.CreateClient("api");
        var response = await client.PutAsJsonAsync<ProductsModel>($"Products/{id}", editProduct);
    }
}
