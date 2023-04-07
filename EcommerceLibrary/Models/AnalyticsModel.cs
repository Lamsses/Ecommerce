using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models;
public class AnalyticsModel
{
    public string name { get; set; }
    public int quantity { get; set; }
    public int sold { get; set; }
    public decimal total_profit { get; set; }
}
