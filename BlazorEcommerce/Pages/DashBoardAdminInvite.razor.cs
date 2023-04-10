using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using System.Text.Json;
using System.Text;

namespace BlazorEcommerce.Pages;

partial class DashBoardAdminInvite : MainBase
{
    protected CustomersModel Customers = new();
    protected CustomersModel CustomerFound = new();
    private async Task<IEnumerable<CustomersModel>> MakeAdmin(string CustomerEmail)
    {
        try
        {
            client = factory.CreateClient("api");
            var response = await client.GetFromJsonAsync<IEnumerable<CustomersModel>>($"Customers/Search/{CustomerEmail}");
            CustomerFound = response.FirstOrDefault();

            return response;
        }
        catch (Exception)
        {

            return Enumerable.Empty<CustomersModel>();
        }

    }
    private async Task GiveAdmin()
    {
        if (CustomerFound is not null)
        {
            var roleId = CustomerFound.role_id = 1;
            var a = await client.PatchAsJsonAsync($"Customers/{CustomerFound.email}", roleId);
            //var json = JsonSerializer.Serialize(CustomerFound);
            //var content = new StringContent(roleId.ToString(), Encoding.UTF8, "application/json");
            //var a = await client.PatchAsync($"Customers/{CustomerFound.email}", content);
        }

    }


}
