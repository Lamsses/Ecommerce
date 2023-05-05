using EcommerceLibrary.Constants;
using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]

public class ProductCategoryController : ControllerBase
{
    private readonly IProdcutCategoryData _product;

    public ProductCategoryController(IProdcutCategoryData product)
    {
        _product = product;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdcutCategoryData>>> Get()
    {

        var output = await _product.GetAll();

        return Ok(output);

    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ProductCategoryModel>> Post([FromBody] ProductCategoryModel products)
    {
        
            var output = await _product.Create( products.category_id,products.product_id);
            return Ok(output);



    }
    [HttpDelete("{category_id}/{product_id}")]
    [AllowAnonymous]
    public async Task<ActionResult> Delete(int category_id,int product_id)
    {

          await _product.Delete(category_id, product_id);
        return Ok();



    }
}
