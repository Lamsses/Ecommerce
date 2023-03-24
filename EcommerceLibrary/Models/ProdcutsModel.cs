using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models
{
    public class ProductsModel
    {
        public int product_id { get; set; }
        public string? name { get; set; }
        public string? price { get; set; }
        public int quantity { get; set; }
        public string? img_url { get; set; }

        public string? description { get; set; }
        public int category_id { get; set; }

        public int ProductAmount { get; set; } = 1;
    }
}
