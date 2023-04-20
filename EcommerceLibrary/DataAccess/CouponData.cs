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

    public CouponData(ISqlDataAccess sql)
    {
        _sql = sql;
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

    public async Task<CouponModel> Create(string coupon_name, int coupon_use, int coupon_discount, DateTime coupon_expire)
    {
        var result = await _sql.Loaddata<CouponModel, dynamic>("dbo.spCoupon_Create", new { coupon_name, coupon_use, coupon_discount, coupon_expire }, "Default");
        return result.FirstOrDefault();
    }

    public Task Update(int coupon_id, string coupon_name, int coupon_use, int coupon_discount, DateTime coupon_expire)
    {
        return _sql.SaveData<dynamic>("dbo.spCoupon_Update", new { coupon_id,coupon_name,coupon_use,coupon_discount,coupon_expire }, "Default");

    }

    public async Task<HttpResponseMessage> ApplyCoupon(string couponName, List<ProductsModel> CartItems)
    {
        var coupon = await GetCouponByName(couponName);
        if (coupon is  null)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        if (CartItems.Count <= 0)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest);

        }
        if (coupon.coupon_use > 0 && coupon.coupon_expire > DateTime.Today)
        {
            foreach (var item in CartItems)
            {
                if (item.coupon_id == coupon.coupon_id)
                {
                    item.discounted_price = ((Convert.ToDecimal(coupon.coupon_discount) / 100) *
                                             (Convert.ToDecimal(item.price)
                                              * Convert.ToDecimal(item.ProductAmount)));

                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);

        }
        return new HttpResponseMessage(HttpStatusCode.BadRequest);


        // return _sql.SaveData<dynamic>("dbo.spCoupon_Update", coupon, "Default");

    }
    public Task Delete(int coupon_id)
    {
        return _sql.SaveData<dynamic>("dbo.spCoupon_Delete", new { coupon_id }, "Default");

    }
}
