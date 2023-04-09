using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = PolicyConstants.Admin)]


public class CustomerLogsController : ControllerBase
{
    private readonly ICustomersLogData _customersLog;

    public CustomerLogsController(ICustomersLogData customersLog)
    {
 
        _customersLog = customersLog;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerLogsModel>>> Get()
    {
        var output = await _customersLog.GetAll();
        return Ok(output);
    }

}

