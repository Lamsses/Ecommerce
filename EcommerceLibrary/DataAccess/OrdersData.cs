
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace EcommerceLibrary.DataAccess;


public class OrdersData : IOrdersData
{


    private readonly ISqlDataAccess _sql;
    private readonly IProductsData _products;
    private readonly ICustomerCouponData _customerCoupon;
    private readonly ICouponData _coupon;
    private IConfiguration config;


    public OrdersData(ISqlDataAccess sql, IProductsData products,ICustomerCouponData customerCoupon,ICouponData coupon)
    {
        _sql = sql;
        _products = products;
        _customerCoupon = customerCoupon;
        _coupon = coupon;
    }

    public Task<List<OrdersModel>> GetAll()
    {
        return _sql.Loaddata<OrdersModel>("dbo.spOrders_GetAll", "Default");
    }
    public async Task<OrdersModel?> GetOne(int order_id)
    {
        var result = await _sql.Loaddata<OrdersModel, dynamic>("dbo.spOrders_GetOne", new { order_id }, "Default");

        return result.FirstOrDefault();
    }

    public async Task<OrdersModel?> Create(DateTime order_date, int customer_id, string receipt, List<ProductsModel>? Products)
    {


            try
            {
                _sql.StartTransaction("Default");
                var result = (await _sql.LoaddataInTransaction<OrdersModel, dynamic>("dbo.spOrders_Create", new { order_date, customer_id, receipt })).FirstOrDefault();
                foreach (var product in Products)
                {
                    //item.order_id = result.order_id;
                    //var product = await _products.GetOne(item.product_id);
                    if (product.discounted_price <= 0)
                    {
                        await _sql.LoaddataInTransaction<OrdersProductsModel, dynamic>("dbo.spOrdersProducts_Create", new { result.order_id, product.product_id, amount = product.ProductAmount  , product.price });
                        product.quantity -= 1;
                        await _sql.SaveDataInTransaction<dynamic>("dbo.spProducts_Update", new { product.product_id, product.name, product.price, product.quantity, product.img_url, product.description, product.coupon_id, product.discounted_price,product.original_price });
                    }
                    else
                    {
                        //product.price =  product.discounted_price.ToString();
                        await _sql.LoaddataInTransaction<OrdersProductsModel, dynamic>("dbo.spOrdersProducts_Create", new { result.order_id, product.product_id, amount = product.ProductAmount, price = product.discounted_price });
                        product.quantity -= 1;
                        product.discounted_price = 0;
                        await _sql.SaveDataInTransaction<dynamic>("dbo.spProducts_Update", new { product.product_id, product.name, product.price, product.quantity, product.img_url, product.description, product.coupon_id, product.discounted_price,product.original_price });

                    }
                }

                var customerCoupons = await _customerCoupon.GetByCustomerId(customer_id);
                if (customerCoupons is not null)
                {
                    foreach (var customerCouponModel in customerCoupons)
                    {
                        if (customerCouponModel.IsUsed == false)
                        {
                             var r = await _customerCoupon.Update(customerCouponModel.customer_id, customerCouponModel.coupon_id,
                                true);
                             var coupon = await _coupon.GetCouponById(customerCouponModel.coupon_id);
                             coupon.coupon_use -= 1;
                              await _coupon.Update(coupon.coupon_id, coupon.coupon_name, coupon.coupon_use,
                                 coupon.coupon_discount, coupon.coupon_expire);

                        }
                    }
                }
                _sql.CommitTransaction();
                return result;
            }
            catch(Exception e) 
            {
                _sql.RollbackTransaction();
                throw;
            }
        
            
        
    }

    public Task Update(int order_id, DateTime order_date, int customer_id, string receipt)
    {
        return _sql.SaveData<dynamic>("dbo.spOrders_Update", new { order_id, order_date, customer_id , receipt }, "Default");
    }
    public Task Delete(int order_id)
    {
        return _sql.SaveData<dynamic>("dbo.spOrders_Delete", new { order_id }, "Default");
    }

   
}
