﻿namespace BlazorEcommerce.Models
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
    }


}
