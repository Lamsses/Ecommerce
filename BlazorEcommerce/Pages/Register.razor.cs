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
            var response = await info.Content.ReadAsStringAsync();
            await InvokeAsync(StateHasChanged);
            if(info.IsSuccessStatusCode)

            {
                ToastService.ShowSuccess("Account Created Successfully");
                await Task.Delay(3000);

                NavigationManager.NavigateTo("/login", true);

            }
            else
            {
                ToastService.ShowError(response);
            }

        }
    }
}
