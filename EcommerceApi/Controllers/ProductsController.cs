using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductsController : ControllerBase
    {
        private readonly IProdcutsData _data;

        public ProductsController(IProdcutsData data)
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
        public async Task<ActionResult<ProductsModel>> Post([FromBody] string name, string price, int quantity, string img_url, string description, int catagoryId)
        {
            var output = await _data.Create(name, price, quantity, img_url, description, catagoryId);
            return Ok(output);

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
