using EcommerceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.DataAccess;


public class OrdersProductsData : IOrdersProductsData
{
    private readonly ISqlDataAccess _sql;

    public OrdersProductsData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<List<OrdersProductsModel>> GetAll()
    {
        return _sql.Loaddata<OrdersProductsModel>("dbo.spOrdersProducts_GetAll", "Default");
    }
    public async Task<OrdersProductsModel?> GetOne(int order_id)
    {
        var result = await _sql.Loaddata<OrdersProductsModel, dynamic>("dbo.spOrdersProducts_GetOne", new { order_id }, "Default");

        return result.FirstOrDefault();
    }

    public async Task<OrdersProductsModel?> Create(int order_id, int product_id, int amount, decimal price)
    {
        var result = await _sql.Loaddata<OrdersProductsModel, dynamic>("dbo.spOrdersProducts_Create", new { order_id, product_id, amount, price }, "Default");

        return result.FirstOrDefault();
    }

    public Task Update(int order_id, int product_id, int amount, decimal price)
    {
        return _sql.SaveData<dynamic>("dbo.spOrdersProducts_Update", new {  order_id,  product_id,  amount,  price }, "Default");
    }
    public Task Delete(int product_id)
    {
        return _sql.SaveData<dynamic>("dbo.spOrdersProducts_Delete", new { product_id }, "Default");
    }
}
