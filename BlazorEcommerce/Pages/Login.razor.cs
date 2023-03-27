using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorEcommerce.Pages;

partial class Login : MainBase
{
    // private AuthenticationModel Authenticat = new();

    private async void HandleValidLogIn()
    {
        var client = factory.CreateClient("api");

        var info = await client.PostAsJsonAsync<AuthenticationModel>("Customers/token", Authenticat);

        var Token = await info.Content.ReadAsStringAsync();
        if (info.IsSuccessStatusCode)
        {
            await LocalStorage!.SetItemAsync("token", Token);
            await AuthStateProvider!.GetAuthenticationStateAsync();
            
            await InvokeAsync(StateHasChanged);
            NavigationManager.NavigateTo("/", true);

        }
    }
    
}
