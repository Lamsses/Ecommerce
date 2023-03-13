using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]


public class CategoriesController : ControllerBase
{
    private readonly ICategoriesData _data;

    public CategoriesController(ICategoriesData data)
    {
        _data = data;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriesModel>>> Get()
    {
        var output = await _data.GetAll();
        return Ok(output);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriesModel>> Get(int id)
    {
        var output = await _data.GetOne(id);
        return Ok(output);

    }

    [HttpPost]
    public async Task<ActionResult<CategoriesModel>> Post(string name)
    {
        var output = await _data.Create(name);
        return Ok(output);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoriesModel>> PutAsync(int id, string name)
    {
        await _data.Update(id, name);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _data.Delete(id);

        return Ok();
    }
}
