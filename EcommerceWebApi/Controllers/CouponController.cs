using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
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
        return Ok(output);
    }
    [HttpPost]


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

    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _coupons.Delete(id);

        return Ok();
    }
}
