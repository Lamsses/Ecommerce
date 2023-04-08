using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models;
public class CustomerLogsModel
{
    public int order_id { get; set; }

    public string first_name { get; set; }

    public int receipt { get; set; }

    public int product_id { get; set; }
    public int amount { get; set; }



    public decimal total { get; set; }
}
