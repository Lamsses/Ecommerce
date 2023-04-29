using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;

public class ProductsData : IProductsData
{
    private readonly ISqlDataAccess _sql;

    public ProductsData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<ProductsModel>> GetAll()
    {
        return _sql.Loaddata<ProductsModel>("dbo.spProducts_GetAll", "Default");
    }
    public async Task<ProductsModel?> GetOne(int productId)
    {
        var result = await _sql.Loaddata<ProductsModel, dynamic>("dbo.spProducts_GetOne", new { product_id = productId }, "Default");

        return result.FirstOrDefault();
    }

    public async Task<ProductsModel?> Create(string name, decimal price, int quantity, string? img_url, string description,  int? coupon_id , decimal discounted_price,decimal original_price)
    {
        var result = await _sql.Loaddata<ProductsModel, dynamic>("dbo.spProducts_Create", new { name , price, quantity, img_url, description  ,coupon_id , discounted_price , original_price }, "Default");

        return result.FirstOrDefault();
    }

    public Task Update(int product_id, string name, decimal price, int quantity, string? img_url, string description,  int? coupon_id, decimal discounted_price, decimal original_price)
    {
        return _sql.SaveData<dynamic>("dbo.spProducts_Update", new { product_id, name, price, quantity, img_url, description,   coupon_id, discounted_price , original_price }, "Default");
    }
    public Task Delete(int product_id)
    {
        return _sql.SaveData<dynamic>("dbo.spProducts_Delete", new { product_id}, "Default");
    }

    public async Task<IEnumerable<ProductsModel>?> SearchProducts(string searchText)
    {
        var result = await _sql.Loaddata<ProductsModel,dynamic>("dbo.spProducts_SearchProducts",new { name = searchText }, "Default");
        return  result.Where(opts => opts.name.Contains(searchText) );
    }
}
