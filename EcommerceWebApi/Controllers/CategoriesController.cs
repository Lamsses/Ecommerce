using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]



public class CategoriesController : ControllerBase
{
    private readonly ICategoriesData _categories;

    public CategoriesController(ICategoriesData categories)
    {
        _categories = categories;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoriesModel>>> Get()
    {
        var output = await _categories.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<CategoriesModel>> Get(int id)
    {
        var output = await _categories.GetOne(id);
        return Ok(output);

    }

    [HttpPost]
    [Authorize(Policy = PolicyConstants.Admin)]
    public async Task<ActionResult<CategoriesModel>> Post([FromBody]string name)
    {
        var output = await _categories.Create(name);
        return Ok(output);

    }

    [HttpPut("{id}")]
    [Authorize(Policy = PolicyConstants.Admin)]


    public async Task<ActionResult<CategoriesModel>> PutAsync(int id, [FromBody] CategoriesModel category)
    {
        await _categories.Update(id, category.Name);

        return Ok(category);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy =PolicyConstants.Admin)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _categories.Delete(id);

        return Ok();
    }
}
