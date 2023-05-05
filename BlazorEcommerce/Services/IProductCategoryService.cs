using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services;
public interface IProductCategoryService
{
    Task<List<ProductCategoryModel>> GetProductCategory();
    Task<HttpResponseMessage> AddProductCategory(ProductCategoryModel model);
    Task<HttpResponseMessage> DeleteProductCategory(ProductCategoryModel model);
}