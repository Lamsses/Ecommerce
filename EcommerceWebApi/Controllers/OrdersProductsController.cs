using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]



public class OrdersProductsController : ControllerBase
{
    private readonly IOrdersProductsData _ordersProducts;

    public OrdersProductsController(IOrdersProductsData ordersProducts)
    {
        _ordersProducts = ordersProducts;
    }
    [HttpGet]
    [Authorize(Policy = PolicyConstants.Admin)]

    public async Task<ActionResult<IEnumerable<OrdersProductsModel>>> Get()
    {
        var output = await _ordersProducts.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]

    public async Task<ActionResult<OrdersProductsModel>> Get(int id)
    {
        var output = await _ordersProducts.GetOne(id);
        return Ok(output);

    }

    [HttpPost]

    public async Task<ActionResult<OrdersProductsModel>> Post(OrdersProductsModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var output = await _ordersProducts.Create(model.order_id, model.product_id, model.amount, model.price);
        return Ok(output);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]
    public async Task<ActionResult<OrdersProductsModel>> PutAsync(OrdersProductsModel orders)
    {
        await _ordersProducts.Update(orders.order_id ,orders.product_id,orders.amount,orders.price);

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperAdmin")]

    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _ordersProducts.Delete(id);

        return Ok();
    }
}