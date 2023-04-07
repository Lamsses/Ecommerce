using EcommerceLibrary.Models;

namespace BlazorEcommerce.Services.Interface;
public interface IAdminLogService
{
    Task DeleteLog(int productId);
}