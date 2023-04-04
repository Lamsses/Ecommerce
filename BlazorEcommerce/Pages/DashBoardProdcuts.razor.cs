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
    private async Task Delete(int id )
    {
        client = factory.CreateClient("api");

        var respons =await client.DeleteAsync($"OrdersProducts/{id}");
        var response = await client.DeleteAsync($"Products/{id}");
        products = await client.GetFromJsonAsync<List<ProductsModel>>("Products");


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
