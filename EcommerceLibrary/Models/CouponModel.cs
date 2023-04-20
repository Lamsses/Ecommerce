using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models;
public class CouponModel
{
    public int coupon_id { get; set; }
    public string coupon_name { get; set; }
    public int coupon_use { get; set; } 
    public int coupon_discount { get;set; }
    public DateTime coupon_expire { get; set; }


}
