using EcommerceLibrary.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace BlazorEcommerce.Pages;

partial class Checkout
{

    List<ProductsModel> products = new();
    List<CouponModel> Coupons =new ();
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
        products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");
        Coupons = await client.GetFromJsonAsync<List<CouponModel>>("Coupon");

    }


    public async void ApplyCoupon()
    {
        client =  factory.CreateClient("api");
        if (!couponName.IsNullOrEmpty())
        {
            var coupon = await client.GetFromJsonAsync<CouponModel>($"Coupon/{couponName}");
            if (coupon != null)
            {
                if (coupon.coupon_use > 0 && coupon.coupon_expire > DateTime.Today)
                {

                    var product = products.Where(p => p.coupon_id == coupon.coupon_id).FirstOrDefault();
                    if (product != null)
                    {
                        product.price = ((Convert.ToDecimal(coupon.coupon_discount) / 100) * (Convert.ToDecimal(product.price) * Convert.ToDecimal(product.ProductAmount))).ToString();
                        await LocalStorage.SetItemAsync("cart", products);

                    coupon.coupon_use -= 1;
                     await client.PutAsJsonAsync<CouponModel>($"Coupon/{coupon.coupon_id}",coupon);
                    }

                }
            }

        }
        products = await LocalStorage.GetItemAsync<List<ProductsModel>>("cart");


    }

    public  decimal ProductTotal(ProductsModel product)
    {

        if (!couponName.IsNullOrEmpty())
        {
            var coupon =  Coupons.Where(c=> c.coupon_name == couponName).FirstOrDefault();
            if (coupon is not null)
            {
                return Decimal.Parse(product.price);

            }

        }

        var price = Convert.ToDecimal(product.price) * Convert.ToDecimal(product.ProductAmount);
        return price;
    }
    private decimal CalculateTotal()
    {
        decimal total = 0;
        if (products is not null)
        {
            foreach (var item in products)
            {
                if (!couponName.IsNullOrEmpty())
                {
                    var coupon = Coupons.Where(c => c.coupon_name == couponName).FirstOrDefault();

                    if (item.coupon_id == coupon.coupon_id)
                    {
                        total +=Decimal.Parse(item.price);

                    }
                    else
                    {
                        total += (Convert.ToDecimal(item.price) * Convert.ToDecimal(item.ProductAmount));
                    }

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
