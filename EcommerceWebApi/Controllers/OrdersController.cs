using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Sentry;

namespace EcommerceWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrdersData _orders;

    public OrdersController(IOrdersData orders)
    {
        _orders = orders;
    }

    [HttpGet]
    [Authorize(Policy = PolicyConstants.Admin)]
    public async Task<ActionResult<IEnumerable<OrdersModel>>> Get()
    {
        var output = await _orders.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]
    public async Task<ActionResult<OrdersModel>> Get(int id)
    {
        var output = await _orders.GetOne(id);
        return Ok(output);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<OrdersModel>> Post([FromBody] OrdersModel orders)
    {
        var output = await _orders.Create(orders.order_date, orders.customer_id, orders.receipt,orders.Products);
        return Ok(output);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]
    public async Task<ActionResult<OrdersModel>> PutAsync(int id, DateTime order_date, int customer_id, string receipt)
    {
        await _orders.Update(id, order_date, customer_id = GetCustomerId(), receipt);

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _orders.Delete(id);

        return Ok();
    }

    private int GetCustomerId()
    {
        var userId = User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userId);
    }
}