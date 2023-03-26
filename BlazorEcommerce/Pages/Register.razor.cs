using EcommerceLibrary.Models;
using System.Net.Http.Json;

namespace BlazorEcommerce.Pages
{
    partial class Register : MainBase
    {
        private async Task HandleRegisteration()
        {
            var client = factory.CreateClient("api");

            var info = await client.PostAsJsonAsync<AuthenticationModel>("Customers", Authenticat);

            await InvokeAsync(StateHasChanged);
        }
    }
}
