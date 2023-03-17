
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcommerceLibrary.Models;

public class CustomersModel
{
    public int customer_id { get; set; }
    [Required]
    [MinLength(2)]
    [MaxLength(10)]
    public string first_name { get; set; }
    [MinLength(2)]
    [MaxLength(10)]
    public string last_name { get; set; }
    public byte[] passwordHash { get; set; }
    public byte[] passwordSalt { get; set; }
    [Phone]
    public string  phone_number { get; set; }
    
    [EmailAddress]
    public string email { get; set; }
    
    public string city { get; set; }
    public int? role_id { get; set; }

}
