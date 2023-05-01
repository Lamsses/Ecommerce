using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public interface ICustomerCouponData
{
    Task<CustomerCouponModel?> Create(int customer_id, int coupon_id);
    Task Delete(int customer_id, int coupon_id);
    Task<List<CustomerCouponModel>> GetAll(int customer_id, int coupon_id);
    Task<List<CustomerCouponModel>> GetByCustomerId(int customer_id);
    Task<CustomerCouponModel?> Update(int customer_id, int coupon_id, bool IsUsed);
}