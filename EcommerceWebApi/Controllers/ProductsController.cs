using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace EcommerceApi.Controllers;

[Route("api/[controller]")]
[ApiController]


public class ProductsController : ControllerBase 
{
    private readonly IProductsData _products;

    public ProductsController(IProductsData products)
    {
        _products = products;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProductsModel>>> Get()
    {
       
       var output =await  _products.GetAll();
    
        return Ok(output);

    }

    [HttpGet("{id}")]
    [AllowAnonymous]

    public async Task<ActionResult<ProductsModel>> Get(int id)
    {
        var output = await _products.GetOne(id);
        return Ok(output);

    }

    [HttpGet("Search/{searchText}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProductsModel>> Get(string searchText)
    {
        var output = await _products.SearchProducts(searchText);
        return Ok(output);

    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<ProductsModel>> Post([FromBody] ProductsModel products)
    {
        try
        {
            var output = await _products.Create
                (products.name, decimal.Parse(products.price), products.quantity, products.img_url, products.description
                , products.coupon_id, products.discounted_price, products.original_price);
            return Ok(output);
        }
        catch (Exception e)
        {

            throw e;
        }

    }

    [HttpPut("{id}")]

    public async Task<ActionResult<ProductsModel>> PutAsync(int id,[FromBody] ProductsModel products)
    {
        
         await _products.Update
            (id,products.name, decimal.Parse(products.price), products.quantity, products.img_url, products.description
                , products.coupon_id, products.discounted_price, products.original_price);

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]


    public async Task<IActionResult> DeleteAsync(int id)
    {
         await _products.Delete(id);

        return Ok();
    }
}
