using EcommerceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.DataAccess;
public class CustomersLogData : ICustomersLogData
{
    private readonly ISqlDataAccess _sql;

    public CustomersLogData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<List<CustomerLogsModel>> GetAll()
    {
        return _sql.Loaddata<CustomerLogsModel>("dbo.spCustomerLog_GetAll", "Default");
    }
    public async Task<CustomerLogsModel?> Create(int customer_id, string log_msg)
    {
        var result = await _sql.Loaddata<CustomerLogsModel, dynamic>("dbo.spCustomersLog_Create", new { customer_id, log_msg }, "Default");

        return result.FirstOrDefault();
    }

}
