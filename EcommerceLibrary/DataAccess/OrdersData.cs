
using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;


public class OrdersData : IOrdersData
{

    private readonly ISqlDataAccess _sql;

    public OrdersData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<CategoriesModel>> GetAll()
    {
        return _sql.Loaddata<CategoriesModel>("dbo.spOrders_GetAll", "Default");
    }
    public async Task<CategoriesModel?> GetOne(int order_id)
    {
        var result = await _sql.Loaddata<CategoriesModel, dynamic>("dbo.spOrders_GetOne", new { order_id }, "Default");

        return result.FirstOrDefault();
    }

    public async Task<CategoriesModel?> Create(DateTime order_date, int customer_id)
    {
        var result = await _sql.Loaddata<CategoriesModel, dynamic>("dbo.spOrders_Create", new { order_date, customer_id }, "Default");

        return result.FirstOrDefault();
    }

    public Task Update(int order_id, DateTime order_date, int customer_id)
    {
        return _sql.SaveData<dynamic>("dbo.spOrders_Update", new { order_id, order_date, customer_id }, "Default");
    }
    public Task Delete(int order_id)
    {
        return _sql.SaveData<dynamic>("dbo.spOrders_Delete", new { order_id }, "Default");
    }
}
