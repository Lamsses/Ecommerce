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
    public async Task<ActionResult<ProductCategoryModel>> Post([FromBody] ProductCategoryModel products)
    {
        try
        {
            var output = await _product.Create(products.product_id, products.category_id);
            return Ok(output);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
               "Error retrieving data from the database");
        }


    }
}
