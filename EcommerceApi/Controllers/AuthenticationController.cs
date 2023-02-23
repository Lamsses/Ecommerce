using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthenticationController(IConfiguration config)
        {
            _config = config;
        }
        public record AuthenticationData(string? UesrName, string? Password);
        public record UesrData(int UserId, string UesrName,string FirstName, string LastName);


        [HttpPost ("token123")]
        [AllowAnonymous]
        public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
        {
            var user = ValidateCredentials(data);
            if (user is null)
            {
                return Unauthorized();
            }
            var token = GenrateToken(user);
            return Ok(token);
        }

        private string GenrateToken(UesrData uesr)
        {
            var secretKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey")));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new();
            claims.Add(new(JwtRegisteredClaimNames.Sub, uesr.UserId.ToString()));
            claims.Add(new(JwtRegisteredClaimNames.UniqueName, uesr.UesrName));
            claims.Add(new(JwtRegisteredClaimNames.GivenName, uesr.FirstName));
            claims.Add(new(JwtRegisteredClaimNames.FamilyName, uesr.LastName));

            var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);



        }
        private UesrData? ValidateCredentials(AuthenticationData data)
        {
            //change Later
            if (CompareValues(data.UesrName, "test") &&
                    CompareValues(data.Password, "test"))
            {
                return new UesrData( 1 ,"test" ,"test", data.UesrName!);
            }
            return null;
        }
        private bool CompareValues(string? acutal, string expected)
        {
            if (acutal is not null)
            {
                if (acutal.Equals(expected)) return true;
            }
            return false;
        }

    }
}
