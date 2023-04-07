using BlazorEcommerce.Services.Interface;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Pages;

public class CartBase: ComponentBase
{
    [Inject] 
    public ICartService CartService { get; set; }
    public List<ProductsModel> CartItems = new();
    public event Action CartChanged;

    protected override async Task OnInitializedAsync()
    {
        CartItems = await CartService.GetCartItems();

    }

    public async Task AddToCart(ProductsModel product)
    {
        var cart = await CartService.GetCartItems();
        if (cart is null)
        {
            cart = new List<ProductsModel>();
        }
        var find = cart.Find(p => p.product_id == product.product_id);
        if (find is null)
        {
            cart.Add(product);
        }
        else
        {
            find.ProductAmount += 1;
            product.ProductAmount = find.ProductAmount;

        }

        await CartService.SetCartItems(cart);
        

    }
    public async Task DeleteFromCart(ProductsModel product)
    {
        var cart = await CartService.GetCartItems();

        var find = cart.Find(p => p.product_id == product.product_id);
        cart.Remove(find);
        await CartService.SetCartItems(cart);
        CartItems = await CartService.GetCartItems();
        // await InvokeAsync(StateHasChanged);



    }

    
}
