using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess
{
    public interface ICustomersData
    {
        Task<CustomersModel?> Create(string first_name, string last_name, byte[] passwordHash
            , byte[] passwordSalt, string phone_number, string email, string city, int? role_id);
        void CreatePassWordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task Delete(int customer_id);
        Task<List<CustomersModel>> GetAll();
        Task<CustomersModel?> GetOne(int customer_id);
        Task<CustomersModel?> GetUserByEmail(string email);

        Task<IEnumerable<CustomersModel?>> GetUsersByEmail(string email);

        Task Update(int customer_id, string first_name, string last_name, byte[] passwordHash, byte[] passwordSalt,
            string phone_number, string email, string city, int? role_id);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}