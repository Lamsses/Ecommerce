using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]


public class AdminLogsController : ControllerBase
{
    private readonly IAdminLog _adminLog;

    public AdminLogsController(IAdminLog adminLog)
    {
        _adminLog = adminLog;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdminLogsModel>>> Get()
    {
       var output = await _adminLog.GetAll();
        return Ok(output);
    }


 

    [HttpPost]
    public async Task<ActionResult<AdminLogsModel>> Post([FromBody] AdminLogsModel adminLog)
    {
        var output = await _adminLog.Create(adminLog.customer_id, adminLog.log_msg);
        return Ok(output);
    }

 
}
