﻿using BlazorEcommerce.Services;
using BlazorEcommerce.Services.Interface;
using Blazored.Toast;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;

namespace BlazorEcommerce.Pages;

partial class Checkout
{
    [Inject] public IProductService productService { get; set; }
    List<ProductsModel> products = new();
    List<CouponModel> Coupons = new();
    public string couponName;

    public decimal productTotal;
    // protected async override Task OnAfterRenderAsync(bool firstRender)
    // {
    //     if (firstRender)
    //     {
    //         products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
    //
    //         StateHasChanged();
    //     }
    // }

    protected async override Task OnInitializedAsync()
    {
        client = factory.CreateClient("api");

        var token = await LocalStorage.GetItemAsync<string>("token");
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));       
        products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

    }


    public async Task<decimal> ApplyCoupon()
    {
        client = factory.CreateClient("api");
        var token = await LocalStorage.GetItemAsync<string>("token");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

        var userId = await customerService.GetUserIdFromToken();

        if (!couponName.IsNullOrEmpty())
        {
            var result = await client.GetAsync($"Coupon/{couponName}");
            if (result.StatusCode.Equals(200))
            {
                var coupon = await result.Content.ReadFromJsonAsync<CouponModel>();

            
            if (coupon != null)
            {
                var customerCoupon = await client.GetAsync
                    ($"CustomerCoupon/{userId}/{coupon.coupon_id}");
                if (!customerCoupon.IsSuccessStatusCode)
                {
                    if (coupon.coupon_use > 0 && coupon.coupon_expire > DateTime.Today)
                    {
                        var product = products.Where(p => p.coupon_id == coupon.coupon_id).FirstOrDefault();
                        if (product != null)
                        {
                            product.discounted_price = ((Convert.ToDecimal(coupon.coupon_discount) / 100) * (Convert.ToDecimal(product.price)
                                * Convert.ToDecimal(product.ProductAmount)));
                            await LocalStorage.SetItemAsync("cart", products);
                            var response = await client.PutAsJsonAsync($"Products/{product.product_id}", product);
                            coupon.coupon_use -= 1;
                            await client.PutAsJsonAsync<CouponModel>($"Coupon/{coupon.coupon_id}", coupon);
                            await client.PostAsJsonAsync<CustomerCouponModel>($"CustomerCoupon",
                                new CustomerCouponModel { coupon_id = coupon.coupon_id, customer_id = userId });
                                ToastService.ShowSuccess("Coupon Apllied successfully");
                            return product.discounted_price;
                        }

                    }
                }
            }
            }
            else
            {
                ToastService.ShowError("Worng code");
            }
        }

        products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        return 0;



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
                    var newPrice = (Convert.ToDecimal(item.discounted_price) * Convert.ToDecimal(item.ProductAmount)).ToString();
                    total += Decimal.Parse(newPrice);

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
