using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
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




    [HttpPost]
    public async Task<ActionResult<CustomerLogsModel>> Post([FromBody] AdminLogsModel adminLog)
    {
        var output = await _customersLog.Create(adminLog.customer_id, adminLog.log_msg);
        return Ok(output);
    }


}

