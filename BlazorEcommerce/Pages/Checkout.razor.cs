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
        client = factory.CreateClient("api");
        var token = await LocalStorage.GetItemAsync<string>("token");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));

        var customerId = await customerService.GetUserIdFromToken();

        if (!string.IsNullOrEmpty(couponName))
        {
            var request = await client.PostAsJsonAsync($"Coupon/Apply/{couponName}/{customerId}", products);
            
            if (request.IsSuccessStatusCode)
            {
                products = await request.Content.ReadFromJsonAsync<List<ProductsModel>>();
                await LocalStorage.SetItemAsync("cart", products);
                foreach (var item in products)
                {
                    if (item.discounted_price > 0)
                    {
                        item.discounted_price = 0;  
                        await productService.UpdateProduct(item);
                    }

                } 
                ToastService.ShowSuccess("Coupon Applied successfully");

            }
            // var result = await client.GetAsync($"Coupon/{couponName}");
            // if (result.StatusCode == HttpStatusCode.OK)
            // {
            //     var coupon = await result.Content.ReadFromJsonAsync<CouponModel>();
            //     if (coupon != null)
            //     {
            //         var customerCoupon = await client.GetAsync
            //             ($"CustomerCoupon/{userId}/{coupon.coupon_id}");
            //         if (!customerCoupon.IsSuccessStatusCode)
            //         {
            //             if (coupon.coupon_use > 0 && coupon.coupon_expire > DateTime.Today)
            //             {
            // foreach (var item in products)
            // {
            // var product = products.Where(p => p.coupon_id == coupon.coupon_id).FirstOrDefault();
            // if (item.coupon_id == coupon.coupon_id)
            // {
            //     item.discounted_price = ((Convert.ToDecimal(coupon.coupon_discount) / 100) *
            //                              (Convert.ToDecimal(item.price)
            //                               * Convert.ToDecimal(item.ProductAmount)));
            //     await LocalStorage.SetItemAsync("cart", products);
            //var response = await client.PutAsJsonAsync($"Products/{product.product_id}", product);
            // coupon.coupon_use -= 1;
            // await client.PutAsJsonAsync<CouponModel>($"Coupon/{coupon.coupon_id}", coupon);
            // await client.PostAsJsonAsync<CustomerCouponModel>($"CustomerCoupon",
            //     new CustomerCouponModel { coupon_id = coupon.coupon_id, customer_id = userId });
            // }
            // }
            //             }
            //         }
            //     }
            // }
        }
        else
        {
            ToastService.ShowError("Wrong Coupon");
        }

        products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
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
                    total += item.discounted_price * item.ProductAmount;
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