using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models;
public class CustomerCouponModel
{
    public int coupon_id { get; set; }

    public int customer_id { get; set; }
    public bool IsUsed { get; set; }
}
