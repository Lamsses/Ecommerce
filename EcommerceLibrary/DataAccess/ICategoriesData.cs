using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess
{
    public interface ICategoriesData
    {
        Task<CategoriesModel?> Create(string name);
        Task Delete(int category_id);
        Task<List<CategoriesModel>> GetAll();
        Task<CategoriesModel?> GetOne(int category_id);
        Task Update(int category_id, string name);
    }
}