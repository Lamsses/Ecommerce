using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace EcommerceApi.Controllers;

[Route("api/[controller]")]
[ApiController]


public class ProductsController : ControllerBase
{
    private readonly IProductsData _data;

    public ProductsController(IProductsData data)
    {
        _data = data;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductsModel>>> Get()
    {
       var output =await  _data.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductsModel>> Get(int id)
    {
        var output = await _data.GetOne(id);
        return Ok(output);

    }

    [HttpPost]
    public async Task<ActionResult<ProductsModel>> Post(string name, decimal price, int quantity, string imgUrl, string description ,int category_id)
    {
        var output = await _data.Create(name, price, quantity, imgUrl, description, category_id);
        return Ok(output);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductsModel>> PutAsync(int id, string name, decimal price, int quantity, string img_url, string description, int catagory_id)
    {
         await _data.Update( id,  name,  price,  quantity,  img_url,  description,  catagory_id);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
         await _data.Delete(id);

        return Ok();
    }
}
