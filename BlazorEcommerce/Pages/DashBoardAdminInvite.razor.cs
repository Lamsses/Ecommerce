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
        client = factory.CreateClient("api");

        var response = await client.GetFromJsonAsync<IEnumerable<CustomersModel>>($"Customers/Search/{CustomerEmail}");
        CustomerFound = response.FirstOrDefault();
       
        return response;

    }
    private async Task GiveAdmin()
    {
        if (CustomerFound is not null)
        {
            var roleId = CustomerFound.role_id = 1;
            //var json = JsonSerializer.Serialize(CustomerFound);
            //var content = new StringContent(roleId.ToString(), Encoding.UTF8, "application/json");
            //var a = await client.PatchAsync($"Customers/{CustomerFound.email}", content);
            var a = await client.PatchAsJsonAsync($"Customers/{CustomerFound.email}", roleId);
        }

    }


}
