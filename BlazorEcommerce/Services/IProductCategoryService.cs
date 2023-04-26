using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;
public interface IProductCategoryService
{
    Task<List<ProductCategoryModel>> GetProductCategory();
}