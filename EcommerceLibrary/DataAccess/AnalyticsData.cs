using EcommerceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.DataAccess;
public class AnalyticsData : IAnalyticsData

{
    private readonly ISqlDataAccess _sql;

    public AnalyticsData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<List<AnalyticsModel>> GetAll()
    {
        return _sql.Loaddata<AnalyticsModel>("dbo.spAnalytics_GetAll", "Default");
    }

    public Task<List<AnalyticsModel>> GetToday()
    {
        return _sql.Loaddata<AnalyticsModel>("dbo.spAnalytics_GetToday", "Default");

    }

    public Task<List<AnalyticsModel>> getLast30Days()
    {
        return _sql.Loaddata<AnalyticsModel>("dbo.spAnalytics_GetLast30day", "Default");

    }

    public Task<List<AnalyticsModel>> GetLast7days()
    {
        return _sql.Loaddata<AnalyticsModel>("dbo.spAnalytics_GetLast7day", "Default");

    }
}
