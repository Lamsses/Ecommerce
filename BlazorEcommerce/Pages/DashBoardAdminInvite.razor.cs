using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;

namespace BlazorEcommerce.Pages;

partial class DashBoardAdminInvite : MainBase
{
    protected CustomersModel Customers = new();
    private async Task<IEnumerable<CustomersModel>> MakeAdmin(string CustomerEmail)
    {
        client = factory.CreateClient("api");

        var response = await client.GetFromJsonAsync<IEnumerable<CustomersModel>>($"Customers/Search/{CustomerEmail}");
        return response;

    }

}
