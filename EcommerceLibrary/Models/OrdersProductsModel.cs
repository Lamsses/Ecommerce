

namespace EcommerceLibrary.Models;

public class OrdersProductsModel
{
    public int order_id { get; set; }

    public int product_id { get; set;}

    public int amount { get; set; }

    public decimal price { get; set; }
}
