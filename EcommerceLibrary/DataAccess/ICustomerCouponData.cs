using EcommerceLibrary.Models;

namespace EcommerceLibrary.DataAccess;
public interface ICustomerCouponData
{
    Task<CustomerCouponModel?> Create(int customer_id, int coupon_id);
    Task<List<CustomerCouponModel>> GetAll(int customer_id, int coupon_id);

}