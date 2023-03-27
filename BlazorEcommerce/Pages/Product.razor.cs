using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages;

partial class Product : MainBase
{
    [Parameter]
    public int Id { get; set; }
    public ProductsModel product = new();
    protected async override Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");
        product = await client.GetFromJsonAsync<ProductsModel>($"Products/{Id}");

    }
}
