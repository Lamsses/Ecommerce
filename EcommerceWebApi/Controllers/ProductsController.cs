using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace EcommerceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]


public class ProductsController : ControllerBase
{
    private readonly IProductsData _products;
    private readonly ILogger<ProductsController> _logger;
    private readonly IAdminLog _adminLog;
    private readonly ICustomersData _customers;

    public ProductsController
        (IProductsData products, ILogger<ProductsController> logger, IAdminLog adminLog, ICustomersData customers)
    {
        _products = products;
        _logger = logger;
        _adminLog = adminLog;
        _customers = customers;
    }
    [HttpGet]

    public async Task<ActionResult<IEnumerable<ProductsModel>>> Get()
    {
       
       var output =await  _products.GetAll();
    
        return Ok(output);

    }

    [HttpGet("{id}")]

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
  

    public async Task<ActionResult<ProductsModel>> Post([FromBody] ProductsModel products)
    {
        var output = await _products.Create
            (products.name, decimal.Parse(products.price), products.quantity, products.img_url, products.description, products.category_id);


        return Ok(output);

    }

    [HttpPut("{id}")]

    public async Task<ActionResult<ProductsModel>> PutAsync(int id,[FromBody] ProductsModel products)
    {
        
         await _products.Update
            (id, products.name, decimal.Parse(products.price), 
            products.quantity,  products.img_url,  products.description, products.category_id);

        return Ok();
    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteAsync(int id)
    {
         await _products.Delete(id);

        return Ok();
    }
}
