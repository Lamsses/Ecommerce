using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AdminLogs : ControllerBase
{
    private readonly IAdminLog _adminLog;

    public AdminLogs(IAdminLog adminLog)
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
