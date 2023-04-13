﻿using BlazorEcommerce.Services.Interface;
using Blazored.LocalStorage;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using Blazored.Toast.Services;

namespace BlazorEcommerce.Pages;

public class MainBase : ComponentBase
{
    [Inject] public NavigationManager? NavigationManager { get; set; }
    [Inject] protected ILocalStorageService? LocalStorage { get; set; }
    [Inject] protected AuthenticationStateProvider? AuthStateProvider { get; set; }
    [Inject] public IHttpClientFactory? factory { get; set; }
    [Inject] public HttpClient? client { get; set; }
    [Inject] public ICustomerService customerService { get; set; }
    [Inject] public IOrderService orderService { get; set; }
    [Inject] public IOrderProductsService orderProductsService { get; set; }
    [Inject] public IProductService ProductService { get; set; }
    [Inject] public IToastService ToastService { get; set; }
    protected AuthenticationModel Authenticat = new();
    private OrdersModel orders;

    



    public async Task AddToCart(ProductsModel products)
    {
        var cart = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        if (cart is null)
        {
    
            cart = new List<ProductsModel>();
        }
        var find = cart.Find(p => p.product_id == products.product_id);
        if (find is null)
        {
            cart.Add(products);
    
        }
        else
        {
            find.ProductAmount += 1;
            products.ProductAmount = find.ProductAmount;
    
        }
        await LocalStorage.SetItemAsync("cart", cart);
    
    
    }

    public  async Task Logout()
    {
        NavigationManager!.NavigateTo("/", true);
        await LocalStorage!.RemoveItemAsync("token");
        await AuthStateProvider!.GetAuthenticationStateAsync();
    }
    public string ReceiptGenrator()
    {
        Random random = new Random();
        int uniqueNumber;

        do
        {
            uniqueNumber = random.Next(10000, 99999);
        } while (IsDuplicate(uniqueNumber));

        return uniqueNumber.ToString();


    }
    private static List<int> generatedNumbers = new();

    private static bool IsDuplicate(int number)
    {
        if (generatedNumbers.Contains(number))
        {
            return true;
        }
        else
        {
            generatedNumbers.Add(number);
            return false;
        }
    }
    public async Task OrdersCheckout()
    {

        var cart =  await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        var token = await LocalStorage.GetItemAsync<string>("token");



        var order = new OrdersModel
        {
            order_date = DateTime.Now,
            customer_id =await customerService.GetUserIdFromToken(),
            receipt = ReceiptGenrator()
        };

        client = factory.CreateClient("api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        if (cart is not null)
        {

            var response = await orderService.AddOrder(order);
            var result = await response.Content.ReadFromJsonAsync<OrdersModel>();

            if (response.IsSuccessStatusCode)
            {




                foreach (var item in cart)
                {
                    if (item.discounted_price <= 0)
                    {

                        await orderProductsService.Add(new OrdersProductsModel
                        {
                            order_id = result.order_id, product_id = item.product_id, amount = item.ProductAmount,
                            price = decimal.Parse(item.price)
                        });
                        item.quantity -= item.ProductAmount;
                        var a = await ProductService.UpdateProduct(item);

                    }
                    else
                    {
                        await orderProductsService.Add(new OrdersProductsModel
                        {
                            order_id = result.order_id, product_id = item.product_id, amount = item.ProductAmount,
                            price = item.discounted_price
                        });
                        item.quantity -= item.ProductAmount;
                        //await ProductService.UpdateProduct(item);

                        item.discounted_price = 0;
                        var a1 = await client.PutAsJsonAsync<ProductsModel>($"Products/{item.product_id}", item);

                    }

                }


                ToastService.ShowSuccess("Order Successfuly Completed");
                await LocalStorage.RemoveItemAsync("cart");
                Thread.Sleep(1500);
                NavigationManager.NavigateTo("/", true);
            }


        }
        else
        {
            ToastService.ShowError("Add Some Items to your cart first!!");

        }



    }


    


}

