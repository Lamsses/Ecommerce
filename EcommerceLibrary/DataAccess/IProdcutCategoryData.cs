using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public interface IProdcutCategoryData
{
    Task<ProductCategoryModel?> Create(int category_id, int product_id);
    Task Delete(int category_id, int product_id);
    Task<List<ProductCategoryModel>> GetAll();
}