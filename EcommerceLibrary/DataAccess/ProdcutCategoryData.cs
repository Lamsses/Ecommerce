using EcommerceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.DataAccess;
public class ProdcutCategoryData : IProdcutCategoryData
{
    private readonly ISqlDataAccess _sql;

    public ProdcutCategoryData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<List<ProductCategoryModel>> GetAll()
    {
        return _sql.Loaddata<ProductCategoryModel>("dbo.spProductCategory_GetAll", "Default");
    }


    public async Task<ProductCategoryModel?> Create(int category_id, int product_id)
    {
        var result = await _sql.Loaddata<ProductCategoryModel, dynamic>
            ("dbo.spProductCategory_Create", new { category_id, product_id }, "Default");
        return result.FirstOrDefault();
    }
    public  Task Delete(int category_id, int product_id)
    {
 var s =          _sql.SaveData<dynamic>
            ("dbo.spProductCategory_Delete", new { category_id, product_id }, "Default");
 return s;
    }
}
