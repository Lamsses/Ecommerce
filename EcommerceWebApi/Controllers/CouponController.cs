using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]

public class CouponController : ControllerBase
{
    private readonly ICouponData _coupons;

    public CouponController(ICouponData coupons)
    {
        _coupons = coupons;
    }
    [HttpGet]


    public async Task<ActionResult<IEnumerable<CouponModel>>> Get()
    {
        var output = await _coupons.GetAll();
        return Ok(output);
    }
    [HttpGet("{name}")]

    public async Task<ActionResult<CouponModel>> GetByName(string name)
    {
        var output = await _coupons.GetCouponByName(name);
        if (output is null)
        {
            return BadRequest();
        }
        return Ok(output);
    }
    [HttpGet("GetById/{id}")]

    public async Task<ActionResult<CouponModel>> GetById(int id)
    {
        var output = await _coupons.GetCouponById(id);
        if (output is null)
        {
            return BadRequest();
        }
        return Ok(output);
    }
    [HttpPost("Apply/{cname}/{customerId}")]
    [AllowAnonymous]
    public async Task<ActionResult<CouponModel>> ApplyCoupon(string cname, [FromBody] List<ProductsModel> CartItems, int customerId)
    {

        var output = await _coupons.ApplyCoupon(cname,CartItems, customerId);
        return Ok(output);

    }
    [HttpPost]
    [Authorize(Policy = PolicyConstants.Admin)]


    public async Task<ActionResult<CouponModel>> Post([FromBody] CouponModel coupons)
    {
       
        var output = await _coupons.Create(coupons.coupon_name,coupons.coupon_use,coupons.coupon_discount,coupons.coupon_expire);
        return Ok(output);

    }
    [HttpPut("{id}")]

    public async Task<ActionResult<CouponModel>> PutAsync(int id, [FromBody] CouponModel coupons)
    {

        await _coupons.Update(id, coupons.coupon_name, coupons.coupon_use, coupons.coupon_discount, coupons.coupon_expire);

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _coupons.Delete(id);
        return Ok();
    }
}
