

using EcommerceLibrary.Models;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceLibrary.DataAccess;

public class CustomersData : ICustomersData
{
    private readonly ISqlDataAccess _sql;

    public CustomersData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<List<CustomersModel>> GetAll()
    {
        return _sql.Loaddata<CustomersModel>("dbo.spCustomers_GetAll", "Default");
    }
    public async Task<CustomersModel?> GetOne(int customer_id)
    {
        var result = await _sql.Loaddata<CustomersModel, dynamic>("dbo.spCustomers_GetOne", new { customer_id }, "Default");

        return result.FirstOrDefault();
    }
    public async Task<CustomersModel?> GetUserByEmail(string email)
    {
        var result = await _sql.Loaddata<CustomersModel, dynamic>("dbo.spCustomers_GetUserByEmail", new { email }, "Default");

        return result.FirstOrDefault();
    }
    public async Task<IEnumerable<CustomersModel?>> GetUsersByEmail(string email)
    {
        var result = await _sql.Loaddata<CustomersModel, dynamic>("dbo.spCustomers_GetUsersByEmail", new { email }, "Default");

        return result.Where(opts => opts.email.Contains(email));
    }

    public async Task<CustomersModel?> Create(string first_name, string last_name, byte[] passwordHash, byte[] passwordSalt, string phone_number, string email, string city,int? role_id)
    {
        var result = await _sql.Loaddata<CustomersModel, dynamic>("dbo.spCustomers_Create", new { first_name, last_name,  passwordHash, passwordSalt, phone_number, email, city, role_id }, "Default");

        return result.FirstOrDefault();
    }

    public Task Update(int customer_id, string first_name, string last_name, byte[] passwordHash, byte[] passwordSalt, string phone_number, string email, string city, int? role_id)
    {
        return _sql.SaveData<dynamic>("dbo.spCustomers_Update", new { customer_id, first_name,  last_name,  passwordHash,  passwordSalt,  phone_number,  email,  city, role_id }, "Default");
    }
    public Task Delete(int customer_id)
    {
        return _sql.SaveData<dynamic>("dbo.spCustomers_Delete", new { customer_id }, "Default");
    }
    public void CreatePassWordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }





}
