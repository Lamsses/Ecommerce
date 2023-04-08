using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;
public interface IAdminLogService
{
    Task DeleteLog(ProductsModel product);
    Task AddLog(ProductsModel product);
    Task UpdateLog(int id);
    Task AddCategeoryLog(string name);
}