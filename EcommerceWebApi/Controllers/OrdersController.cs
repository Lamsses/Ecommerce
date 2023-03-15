using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]

    
public class OrdersController : ControllerBase
{

    private readonly IOrdersData _orders;

    public OrdersController(IOrdersData orders)
    {
        _orders = orders;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdersModel>>> Get()
    {
        var output = await _orders.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrdersModel>> Get(int id)
    {
        var output = await _orders.GetOne(id);
        return Ok(output);

    }

    [HttpPost]
    public async Task<ActionResult<OrdersModel>> Post(DateTime order_date , int customer_id)
    {
        var output = await _orders.Create(order_date, customer_id);
        return Ok(output);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrdersModel>> PutAsync(int id, DateTime order_date, int customer_id)
    {
        await _orders.Update(id, order_date, customer_id);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _orders.Delete(id);

        return Ok();
    }
}
