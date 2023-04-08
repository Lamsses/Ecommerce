namespace BlazorEcommerce.Services.Interface;

public interface ICustomerService
{
    Task<int> GetUserIdFromToken();
    Task<string> GetUserNameFromToken();
}