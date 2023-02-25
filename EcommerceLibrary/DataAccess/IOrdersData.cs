using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess
{
    public interface IOrdersData
    {
        Task<CategoriesModel?> Create(DateTime order_date, int customer_id);
        Task Delete(int order_id);
        Task<List<CategoriesModel>> GetAll();
        Task<CategoriesModel?> GetOne(int order_id);
        Task Update(int order_id, DateTime order_date, int customer_id);
    }
}