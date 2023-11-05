using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models
{
    public class ProductsModel
    {
        public int product_id { get; set; }
        public string? name { get; set; }
        public string price { get; set; }
        public decimal discounted_price { get; set; }

        public int quantity { get; set; }
        public string? img_url { get; set; }

        public string? description { get; set; }

        public int ProductAmount { get; set; } = 1;
        
        public int? coupon_id { get; set; }
        public decimal original_price { get; set; }
        public List<CategoriesModel>? Categories { get; set; }
    }
}
