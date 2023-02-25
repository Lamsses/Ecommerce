

using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;

public class CategoriesData : ICategoriesData
{
    private readonly ISqlDataAccess _sql;

    public CategoriesData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<CategoriesModel>> GetAll()
    {
        return _sql.Loaddata<CategoriesModel>("dbo.spCategories_GetAll", "Default");
    }
    public async Task<CategoriesModel?> GetOne(int category_id)
    {
        var result = await _sql.Loaddata<CategoriesModel, dynamic>("dbo.spCategories_GetOne", new { category_id }, "Default");

        return result.FirstOrDefault();
    }

    public async Task<CategoriesModel?> Create(string name)
    {
        var result = await _sql.Loaddata<CategoriesModel, dynamic>("dbo.spCategories_Create", new { name }, "Default");

        return result.FirstOrDefault();
    }

    public Task Update(int category_id, string name)
    {
        return _sql.SaveData<dynamic>("dbo.spCategories_Update", new { category_id, name }, "Default");
    }
    public Task Delete(int category_id)
    {
        return _sql.SaveData<dynamic>("dbo.spCategories_Delete", new { category_id }, "Default");
    }
}
