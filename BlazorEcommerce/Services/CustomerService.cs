using System.IdentityModel.Tokens.Jwt;
using BlazorEcommerce.Services.Interface;

namespace BlazorEcommerce.Services;

public class CustomerService : ICustomerService
{

    public int GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);

        var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "sub");

        return int.Parse(userIdClaim?.Value);
    }

}
