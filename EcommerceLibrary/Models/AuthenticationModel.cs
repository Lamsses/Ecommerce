
using System.ComponentModel.DataAnnotations;

namespace EcommerceLibrary.Models;

public class AuthenticationModel
{
 
    public string first_name { get; set; }
    [MinLength(2)]
    [MaxLength(10)]
    public string last_name { get; set; }
    public string password { get; set; }
    [Phone]
    public string  phone_number { get; set; }
    
    [EmailAddress]
    public string email { get; set; }
    
    public string city { get; set; }

    public int? role_id { get; set; }

}
