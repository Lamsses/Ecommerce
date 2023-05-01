using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public class CouponData: ICouponData
{
    private readonly ISqlDataAccess _sql;
    private readonly IProductsData _products;
    private readonly ICustomerCouponData _customerCoupon;

    public CouponData(ISqlDataAccess sql ,IProductsData products,ICustomerCouponData customerCoupon)
    {
        _sql = sql;
        _products = products;
        _customerCoupon = customerCoupon;
    }
    public Task<List<CouponModel>> GetAll()
    {
        return _sql.Loaddata<CouponModel>("dbo.spCoupon_GetAll", "Default");
    }

    public async Task<CouponModel> GetCouponByName(string coupon_name)
    {
        var result = await _sql.Loaddata<CouponModel, dynamic>("dbo.spCoupon_GetCouponByName", new { coupon_name }, "Default");

        return result.FirstOrDefault();
    }
    public async Task<CouponModel> GetCouponById(int coupon_id)
    {
        var result = await _sql.Loaddata<CouponModel, dynamic>("dbo.spCoupon_GetCouponById", new { coupon_id }, "Default");

        return result.FirstOrDefault();
    }

    public async Task<CouponModel> Create(string coupon_name, int coupon_use, int coupon_discount, DateTime coupon_expire)
    {
        var result = await _sql.Loaddata<CouponModel, dynamic>("dbo.spCoupon_Create", new { coupon_name, coupon_use, coupon_discount, coupon_expire }, "Default");
        return result.FirstOrDefault();
    }
    public async Task<List<ProductsModel>> ApplyCoupon(string couponName, List<ProductsModel> CartItems,int customerId)
    {
        var coupon = await GetCouponByName(couponName);
        if (coupon is null)
        {
            throw new ArgumentException("Coupon not found.");
        }

        if (CartItems.Count <= 0)
        {
            throw new ArgumentException("Cart is empty.");
        }

        var customerCoupons = (await _customerCoupon.GetAll(customerId, coupon.coupon_id)).FirstOrDefault();
        if (customerCoupons is  null || customerCoupons.IsUsed != true)
        {

        if (coupon.coupon_use > 0 && coupon.coupon_expire > DateTime.Today)
        {
            foreach (var item in CartItems)
            {
                if (item.coupon_id == coupon.coupon_id)
                {
                    item.discounted_price = (Convert.ToDecimal(item.price) * item.ProductAmount) - ((Convert.ToDecimal(coupon.coupon_discount) / 100) *
                                                                (Convert.ToDecimal(item.price)
                                                                 * item.ProductAmount));
                    item.discounted_price = Math.Round(item.discounted_price, 2);
                    await _products.Update(item.product_id, item.name, Convert.ToDecimal(item.price), item.quantity, item.img_url,
                        item.description, item.coupon_id, item.discounted_price, item.original_price);

                    //return CartItems;

                }

            }
            
         if (customerCoupons is null)
        
        {
            var r = await _customerCoupon.Create(customerId, coupon.coupon_id);
        }
        }
        else
        {
            throw new ArgumentException("Coupon is expired or has already been used up.");
        }
        }


        return  CartItems;
    }

    public Task Update(int coupon_id, string coupon_name, int coupon_use, int coupon_discount, DateTime coupon_expire)
    {
        return _sql.SaveData<dynamic>("dbo.spCoupon_Update", new { coupon_id,coupon_name,coupon_use,coupon_discount,coupon_expire }, "Default");

    }

    //public async Task<List<ProductsModel>> ApplyCoupon(string couponName, List<ProductsModel> CartItems)
    //{
    //    var coupon = await GetCouponByName(couponName);
    //    if (coupon is null)
    //    {
    //        throw new ArgumentException("Coupon not found.");
    //    }

    //    if (CartItems.Count <= 0)
    //    {
    //        throw new ArgumentException("Cart is empty.");
    //    }

    //    if (coupon.coupon_use > 0 && coupon.coupon_expire > DateTime.Today)
    //    {
    //        foreach (var item in CartItems)
    //        {
    //            if (item.coupon_id == coupon.coupon_id)
    //            {
    //                item.discounted_price = ((Convert.ToDecimal(coupon.coupon_discount) / 100) *
    //                                             (Convert.ToDecimal(item.price)
    //                                              * Convert.ToDecimal(item.ProductAmount)));
    //               await _products.Update(item.product_id, item.name, Convert.ToDecimal(item.price), item.quantity, item.img_url,
    //                    item.description, item.coupon_id, item.discounted_price);

    //            }
            
    //        }
    //    }
        
    //    else
    //    {
    //        throw new ArgumentException("Coupon is expired or has already been used up.");
    //    }

    //    return CartItems;
    //}



    // return _sql.SaveData<dynamic>("dbo.spCoupon_Update", coupon, "Default");


public Task Delete(int coupon_id)
    {
        return _sql.SaveData<dynamic>("dbo.spCoupon_Delete", new { coupon_id }, "Default");

    }
}
