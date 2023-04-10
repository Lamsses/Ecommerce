using EcommerceLibrary.Dto;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace BlazorEcommerce.Pages;

partial class Login : MainBase
{
    // private AuthenticationModel Authenticat = new();
    public LoginInput user = new();


    private async void HandleValidLogIn()
    {

        var client = factory.CreateClient("api");

        var info = await client.PostAsJsonAsync("Customers/token", user);

        var Token = await info.Content.ReadAsStringAsync();
        if (info.IsSuccessStatusCode)
        {
            await LocalStorage!.SetItemAsync("token", Token);
            await AuthStateProvider!.GetAuthenticationStateAsync();
            
            await InvokeAsync(StateHasChanged);
            NavigationManager.NavigateTo("/", true);

        }
        else
        {
            ToastService.ShowError(Token);

        }
    }
    
}
