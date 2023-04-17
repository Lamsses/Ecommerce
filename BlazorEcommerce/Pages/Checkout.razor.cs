using System.Net;
using BlazorEcommerce.Services;
using BlazorEcommerce.Services.Interface;
using Blazored.Toast;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System;

namespace BlazorEcommerce.Pages;

partial class Checkout
{
    [Inject] public IProductService productService { get; set; }
    List<ProductsModel> products = new();
    List<CouponModel> Coupons = new();
    public string couponName;

    public CouponModel Coupon { get; set; }

    protected override async Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");

        var token = await LocalStorage.GetItemAsync<string>("token");
        if (token is not null)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

        }
        products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

    }


    public async Task ApplyCoupon()
    {
        try
        {
            var token = await LocalStorage.GetItemAsync<string>("token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

            var userId = await customerService.GetUserIdFromToken();
         

            if (string.IsNullOrEmpty(couponName))
            {
                ToastService.ShowError("Invalid coupon name.");
                return;
            }

            var result = await client.GetAsync($"Coupon/{couponName}");
            if (!result.IsSuccessStatusCode)
            {
                ToastService.ShowError("Coupon not found.");
                return;
            }

            var coupon = await result.Content.ReadFromJsonAsync<CouponModel>();
            if (coupon == null)
            {
                ToastService.ShowError("Invalid coupon data.");
                return;
            }

            var customerCoupon = await client.GetAsync($"CustomerCoupon/{userId}/{coupon.coupon_id}");
            if (!customerCoupon.IsSuccessStatusCode)
            {
                ToastService.ShowError("Coupon already used.");
                return;
            }

            if (coupon.coupon_use == 0 || coupon.coupon_expire <= DateTime.Today)
            {
                ToastService.ShowError("Coupon expired or limit reached.");
                return;
            }

            var cart = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
            if (cart == null || !cart.Any())
            {
                ToastService.ShowError("Cart is empty.");
                return;
            }

            var productsWithCoupon = cart.Where(p => p.coupon_id == coupon.coupon_id).ToList();
            if (!productsWithCoupon.Any())
            {
                ToastService.ShowError("No products with this coupon.");
                return;
            }

            foreach (var product in productsWithCoupon)
            {
                var discountedPrice = (Convert.ToDecimal(product.price) * product.ProductAmount) - ((Convert.ToDecimal(coupon.coupon_discount) / 100) * (Convert.ToDecimal(product.price) * product.ProductAmount));
                discountedPrice = Math.Round(discountedPrice, 2);
                product.discounted_price = discountedPrice;

                await client.PutAsJsonAsync($"Products/{product.product_id}", product);
            }

            coupon.coupon_use -= 1;
            await client.PutAsJsonAsync($"Coupon/{coupon.coupon_id}", coupon);
            await client.PostAsJsonAsync($"CustomerCoupon", new CustomerCouponModel { coupon_id = coupon.coupon_id, customer_id = userId });
            ToastService.ShowSuccess("Coupon applied successfully.");
        }
        catch (Exception ex)
        {
            // Handle exceptions
            ToastService.ShowError("Error applying coupon.");
        }
        finally
        {
            products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        }
    }






    //public  decimal ProductTotal(ProductsModel product)
    //{


    //    var coupon = Coupons.Where(c => c.coupon_name == couponName).FirstOrDefault();
    //    if (coupon is not null)
    //    {
    //        if (product.coupon_id == coupon.coupon_id)
    //        {
    //            return Decimal.Parse(product.price);

    //        }
    //    }

    //    var price = Convert.ToDecimal(product.price) * Convert.ToDecimal(product.ProductAmount);
    //    return price;

    //}
    private decimal CalculateTotal()
    {
        decimal total = 0;
        if (products is not null)
        {
            foreach (var item in products)
            {

                if (item.discounted_price > 0)
                {
                    // var newPrice = (Convert.ToDecimal(item.discounted_price) * Convert.ToDecimal(item.ProductAmount));
                    total += item.discounted_price;

                }
     
              
                else
                    {
                        total += (Convert.ToDecimal(item.price) * Convert.ToDecimal(item.ProductAmount));
                    }



                }
            }
            return total;
        }
    }
