using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public interface ICustomersLogData
{
    Task<CustomerLogsModel?> Create(int customer_id, string log_msg);
    Task<List<CustomerLogsModel>> GetAll();
}