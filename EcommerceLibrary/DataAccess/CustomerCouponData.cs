using EcommerceLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.DataAccess;
public class CustomerCouponData : ICustomerCouponData
{

    private readonly ISqlDataAccess _sql;

    public CustomerCouponData(ISqlDataAccess sql)
    {
        _sql = sql;
    }
    public Task<List<CustomerCouponModel>> GetAll(int customer_id, int coupon_id)
    {
        return _sql.Loaddata<CustomerCouponModel, dynamic>("dbo.spCustomerCoupon_GetAll", new { customer_id, coupon_id }, "Default");
    }
    public async Task<CustomerCouponModel?> Create(int customer_id, int coupon_id)
    {
        var result = await _sql.Loaddata<CustomerCouponModel, dynamic>("dbo.spCustomerCoupon_Create", new { customer_id, coupon_id }, "Default");

        return result.FirstOrDefault();
    }

}
