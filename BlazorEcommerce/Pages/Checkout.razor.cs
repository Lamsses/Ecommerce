using EcommerceLibrary.Models;

namespace BlazorEcommerce.Pages;

partial class Checkout
{

    List<ProductsModel> products = new();
    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");

            StateHasChanged();
        }
    }
    private decimal CalculateTotal()
    {
        decimal total = 0;
        if (products is not null)
        {
            foreach (var item in products)
            {
                total += (Convert.ToDecimal(item.price) * Convert.ToDecimal(item.ProductAmount));

            }
        }
        return total;
    }
 
}
