using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class CustomerCouponController : ControllerBase
{
    private readonly ICustomerCouponData _customerCouponData;

    public CustomerCouponController(ICustomerCouponData customerCouponData)
    {
        _customerCouponData = customerCouponData;
    }

    [HttpGet("{customer_id}/{coupon_id}")]
    public async Task<ActionResult<IEnumerable<CustomerCouponModel>>> Get(int customer_id, int coupon_id )
    {
        var output = await _customerCouponData.GetAll(customer_id, coupon_id );
        
        if ( output.Count == 0 )
        {
            return BadRequest();
        }
        return Ok(output);
    }
        [HttpPost]

    public async Task<ActionResult<CustomerCouponModel>> Post([FromBody] CustomerCouponModel customerCouponData)
    {
        var output = await _customerCouponData.Create(customerCouponData.customer_id, customerCouponData.coupon_id);
        return Ok(output);
    }
    [HttpPut("{customer_id}/{coupon_id}")]
    
    public async Task<ActionResult<CustomerCouponModel>> PutAsync([FromBody]bool IsUsed, int customer_id, int coupon_id)
    {

        await _customerCouponData.Update(customer_id, coupon_id, IsUsed);
        return Ok();
    }

    [HttpDelete("{customer_id}/{coupon_id}")]

    public async Task<IActionResult> DeleteAsync(int customer_id, int coupon_id)
    {
        await _customerCouponData.Delete(customer_id, coupon_id);

        return Ok();
    }
}
