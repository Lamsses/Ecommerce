using AutoMapper;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using EcommerceLibrary.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using EcommerceLibrary.Constants;

namespace EcommerceWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]



public class CustomersController : ControllerBase
{
    private readonly ICustomersData _customers;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public CustomersController(ICustomersData customers, IMapper mapper, IConfiguration config) 
    {
        _customers = customers;
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
        claims.Add(new("role_id", user.role_id.ToString()));

        


        var token = new JwtSecurityToken(
            _config.GetValue<string>("Authentication:Issuer"),
            _config.GetValue<string>("Authentication:Audience"),
            claims,
            DateTime.Now,
            DateTime.Now.AddMonths(1),
            signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] LoginInput userInput)
    {
        var user = await _customers.GetUserByEmail(userInput.email);
        if (user == null) return BadRequest("Wrong username or password");
        if (userInput.email == string.Empty && userInput.password == string.Empty)
        {
            return BadRequest("please enter user name and password");
        }

        if (user.email != userInput.email)
            return BadRequest("Wrong username or password");
        if (!_customers.VerifyPasswordHash(userInput.password, user.passwordHash!, user.passwordSalt!))
            return BadRequest("Wrong username or password");

        var token = GenerateToken(user);

        return Ok(token);
    }

    [HttpGet("Search/{CustomerEmail}")]
    [AllowAnonymous]
    public async Task<ActionResult<CustomersModel>> GetUsersByEmail(string CustomerEmail)
    {
        var output = await _customers.GetUsersByEmail(CustomerEmail);
        return Ok(output);
    }
    [HttpGet]
    [Authorize(Policy = PolicyConstants.Admin)]

    public async Task<ActionResult<IEnumerable<CustomersModel>>> Get()
    {
        var output = await _customers.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]

    public async Task<ActionResult<CustomersModel>> Get(int id)
    {
        var output = await _customers.GetOne(id);
        return Ok(output);

    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<CustomersModel>> Post([FromBody] AuthenticationModel customer)
    {
        var customerModel = _mapper.Map<CustomersModel>(customer);

        var existingCustomer = await _customers.GetUserByEmail(customer.email);
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (existingCustomer != null)
        {
            return Conflict("A customer with this email address already exists.");
        }
        
            _customers.CreatePassWordHash(customer.password, out byte[] passwordHash, out byte[] passwordSalt);
            var output = await _customers.Create(customerModel.first_name, customerModel.last_name, passwordHash,
                                passwordSalt, customerModel.phone_number, customerModel.email, customerModel.city, customerModel.role_id);

            return Ok(output);

        
        

    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]

    public async Task<ActionResult<CustomersModel>> PutAsync(int id, [FromBody] AuthenticationModel customer)
    {
        var customerModel = _mapper.Map<CustomersModel>(customer);
        customerModel.customer_id = id;
        _customers.CreatePassWordHash(customer.password, out byte[] passwordHash, out byte[] passwordSalt);
        var output =   _customers.Update(customerModel.customer_id,customerModel.first_name, customerModel.last_name, passwordHash,
                            passwordSalt, customerModel.phone_number, customerModel.email, customerModel.city, customerModel.role_id);

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]

    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _customers.Delete(id);

        return Ok();
    }

    [HttpPatch("{email}")]
    [Authorize(Policy = PolicyConstants.SuperAdmin)]


    public async Task<IActionResult> UpdateUserRole(string email, [FromBody] int? roleId)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required.");
        }

        var customer = await _customers.GetUserByEmail(email);

        if (customer == null)
        {
            return NotFound($"Customer with email {email} not found.");
        }

        await _customers.Update(customer.customer_id, customer.first_name, customer.last_name,
            customer.passwordHash, customer.passwordSalt, customer.phone_number, customer.email,
            customer.city, roleId);

        return NoContent();
    }
}

