using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public interface ICouponData
{
    Task<List<CouponModel>> GetAll();
    Task<CouponModel> GetCouponByName(string coupon_name);
    Task<CouponModel> Create(string coupon_name,int coupon_use,int coupon_discount,DateTime coupon_expire);
    Task Update(int coupon_id,string coupon_name,int coupon_use,int coupon_discount,DateTime coupon_expire);
    Task Delete(int coupon_id);
    Task<List<ProductsModel>> ApplyCoupon(string couponName, List<ProductsModel> CartItems,int customerId);
    Task<CouponModel> GetCouponById(int coupon_id);
}
