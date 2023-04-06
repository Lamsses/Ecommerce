namespace BlazorEcommerce.Services.Interface;

public interface ICustomerService
{
    int GetUserIdFromToken(string token);
}