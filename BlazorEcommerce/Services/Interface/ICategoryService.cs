using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;

public interface ICategoryService
{
    Task<List<CategoriesModel>> GetCategories();
}
