using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public interface IAdminLog
{
    Task<AdminLogsModel?> Create( int custoemr_id, string log_msg);
    Task<List<AdminLogsModel>> GetAll();
}