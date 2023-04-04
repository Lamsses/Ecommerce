using EcommerceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.DataAccess;
public class AdminLogData : IAdminLog
{
    private readonly ISqlDataAccess _sql;

    public AdminLogData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<List<AdminLogsModel>> GetAll()
    {
        return _sql.Loaddata<AdminLogsModel>("dbo.spAdminLogs_GetAll", "Default");
    }
    public async Task<AdminLogsModel?> Create( int customer_id, string log_msg)
    {
        var result = await _sql.Loaddata<AdminLogsModel, dynamic>("dbo.spAdminLogs_Create", new { customer_id, log_msg }, "Default");

        return result.FirstOrDefault();
    }

}
