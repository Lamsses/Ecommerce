using AutoMapper;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static TodoApi.Controllers.AuthenticationController;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using EcommerceLibrary.Dto;

namespace EcommerceWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CustomersController : ControllerBase
{
    private readonly ICustomersData _data;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public CustomersController(ICustomersData data, IMapper mapper, IConfiguration config)
    {
        _data = data;
        _mapper = mapper;
        _config = config;
    }

    private string GenerateToken(CustomersModel user)
    {
        var secretKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(
                _config.GetValue<string>("Authentication:SecretKey")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.customer_id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.GivenName, user.first_name));
        claims.Add(new(JwtRegisteredClaimNames.FamilyName, user.last_name));

        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(1),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] LoginInput userInput)
    {
        var user = await _data.GetUserByEmail(userInput.email);
        if (user == null) return BadRequest(ModelState);
        if (userInput.email == string.Empty && userInput.password == string.Empty)
        {
            return BadRequest("please enter user name and password");
        }
        else if (user.email != userInput.email)
            return BadRequest("Wrong Username");
        else if (!_data.VerifyPasswordHash(userInput.password, user.passwordHash!, user.passwordSalt!))
            return BadRequest("Wrong Password");

        string token = GenerateToken(user);
        return Ok(token);
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomersModel>>> Get()
    {
        var output = await _data.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomersModel>> Get(int id)
    {
        var output = await _data.GetOne(id);
        return Ok(output);

    }

    [HttpPost]
    [AllowAnonymous]

    public async Task<ActionResult<CustomersModel>> Post([FromBody] AuthenticationModel customer)
    {
        var customerModel = _mapper.Map<CustomersModel>(customer);
        _data.CreatePassWordHash( customer.password, out byte[] passwordHash, out byte[] passwordSalt);
        var output = await _data.Create(customerModel.first_name, customerModel.last_name, passwordHash,
                            passwordSalt, customerModel.phone_number, customerModel.email, customerModel.city);

        return Ok(output);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomersModel>> PutAsync(int id, string first_name, string last_name, string password, string phone_number, string email, string city)
    {
        await _data.Update(id, first_name, last_name, password, phone_number, email, city);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _data.Delete(id);

        return Ok();
    }

}