using EcommerceLibrary.DataAccess;
using EcommerceLibrary.Models;
using EcommerceLibrary.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers;
[Route("api/[controller]")]
[ApiController]


public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsData _analyticsData;

    public AnalyticsController(IAnalyticsData analyticsData)
    {
        _analyticsData = analyticsData;
    }
    [HttpGet]

    public async Task<ActionResult<IEnumerable<AnalyticsModel>>> Get()
    {
        var output = await _analyticsData.GetAll();
        return Ok(output);
    }
    [HttpGet("/today")]

    public async Task<ActionResult<IEnumerable<AnalyticsModel>>> GetToday()
    {
        var output = await _analyticsData.GetToday();
        return Ok(output);
    }
    [HttpGet("/30day")]

    public async Task<ActionResult<IEnumerable<AnalyticsModel>>> GetLast30Days()
    {
        var output = await _analyticsData.getLast30Days();
        return Ok(output);
    }
    [HttpGet("/7day")]

    public async Task<ActionResult<IEnumerable<AnalyticsModel>>> GetLast7Days()
    {
        var output = await _analyticsData.GetLast7days();
        return Ok(output);
    }
}
