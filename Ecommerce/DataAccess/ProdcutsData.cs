
using Ecommerce.DataAccess;
using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;

public class ProdcutsData : IProdcutsData
{
    private readonly ISqlDataAccess _sql;

    public ProdcutsData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<ProductsModel>> GetAllAssigned()
    {
        return _sql.Loaddata<ProductsModel>("dbo.spTodos_GetAllAssigned", "Default");
    }
    public async Task<ProductsModel?> GetOne(int productId)
    {
        var result = await _sql.Loaddata<ProductsModel, dynamic>("dbo.spProducts_GetOne", new { Id = productId }, "Default");

        return result.FirstOrDefault();
    }

    public async Task<ProductsModel?> Create(string name, string price, int quantity, string img_url, string description, int catagoryId)
    {
        var result = await _sql.Loaddata<ProductsModel, dynamic>("dbo.spProducts_Create", new { name, price, quantity, img_url, description, catagoryId }, "Default");

        return result.FirstOrDefault();
    }

    public Task EditProducts(int product_id, string name, string price, int quantity, string img_url, string description, int catagoryId)
    {
        return _sql.SaveData<dynamic>("dbo.spProducts_EditProducts", new { product_id, name, price, quantity, img_url, description, catagoryId }, "Default");
    }
    public Task DeleteProducts(int product_id, string name, string price, int quantity, string img_url, string description, int catagoryId)
    {
        return _sql.SaveData<dynamic>("dbo.spProducts_DeleteProducts", new { product_id, name, price, quantity, img_url, description, catagoryId }, "Default");
    }
}
