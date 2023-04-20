
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace EcommerceLibrary.DataAccess;


public class OrdersData : IOrdersData
{


    private readonly ISqlDataAccess _sql;
    private readonly IProductsData _products;
    private IConfiguration config;


    public OrdersData(ISqlDataAccess sql, IProductsData products)
    {
        _sql = sql;
        _products = products;
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

    public async Task<OrdersModel?> Create(DateTime order_date, int customer_id, string receipt, List<OrdersProductsModel>? OrderProducts)
    {


            try
            {
                _sql.StartTransaction("Default");
                var result = (await _sql.LoaddataInTransaction<OrdersModel, dynamic>("dbo.spOrders_Create", new { order_date, customer_id, receipt })).FirstOrDefault();
                foreach (var item in OrderProducts)
                {
                    item.order_id = result.order_id;
                    var product = await _products.GetOne(item.product_id);
                    if (product.discounted_price <= 0)
                    {
                        await _sql.LoaddataInTransaction<OrdersProductsModel, dynamic>("dbo.spOrdersProducts_Create", new { item.order_id, item.product_id, item.amount, item.price });
                        product.quantity -= 1;
                        await _sql.SaveDataInTransaction<dynamic>("dbo.spProducts_Update", new { product.product_id, product.name, product.price, product.quantity, product.img_url, product.description, product.category_id, product.coupon_id, product.discounted_price });
                    }
                    else
                    {
                        item.price = product.discounted_price;
                        await _sql.LoaddataInTransaction<OrdersProductsModel, dynamic>("dbo.spOrdersProducts_Create", new { item.order_id, item.product_id, item.amount, item.price });
                        product.quantity -= 1;
                        product.discounted_price = 0;
                        await _sql.SaveDataInTransaction<dynamic>("dbo.spProducts_Update", new { product.product_id, product.name, product.price, product.quantity, product.img_url, product.description, product.category_id, product.coupon_id, product.discounted_price });

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
