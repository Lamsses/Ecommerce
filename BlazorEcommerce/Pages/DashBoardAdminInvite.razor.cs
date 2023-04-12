using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

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
            var token = await LocalStorage.GetItemAsync<string>("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
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
            var roleId = CustomerFound.role_id = 2;
            var a = await client.PatchAsJsonAsync($"Customers/{CustomerFound.email}", roleId);
            if (a.IsSuccessStatusCode)
            {
                ToastService.ShowSuccess("User Promoted Successfully");
            }
            else
            {
                ToastService.ShowError("Something Went Wrong");
            }
            //var json = JsonSerializer.Serialize(CustomerFound);
            //var content = new StringContent(roleId.ToString(), Encoding.UTF8, "application/json");
            //var a = await client.PatchAsync($"Customers/{CustomerFound.email}", content);
        }

    }


}
