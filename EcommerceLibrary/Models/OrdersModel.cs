
namespace EcommerceLibrary.Models;

public class OrdersModel
{
    public int order_id { get; set; }
    public DateTime order_date { get; set; } 
    public int customer_id { get; set; }

    public string receipt { get; set; }
    public List<ProductsModel>? Products { get; set; }
}
